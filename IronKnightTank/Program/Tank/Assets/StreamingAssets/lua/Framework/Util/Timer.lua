local LuaCallUtil = {}

local tremove = table.remove
local tinsert = table.insert

local function coremove(colist,func)
    if colist == nil then
        return
    end
    
    for i,v in ipairs(colist) do
        if v.func == func then
            tremove(colist, i)
            break
        end
    end
end

local function hasco(colist,func)
    if colist == nil then
        return
    end
    
    for i,v in ipairs(colist) do
        if v.func == func then
            return true
        end
    end
    return false
end

local newcolist = {}

--------------------------------------------------------
-- 延时调用
--------------------------------------------------------
local delayCalls = {}

function delayCall(func,delay,attachTag,userData)
    local co = {func = func,delay = delay,arg = userData,time = 0,attachTag = tostring(attachTag),typelist = delayCalls}
    tinsert(newcolist,co)
end

function stopDelayCall(func,attachTag)
    attachTag = tostring(attachTag)
    
    if func==nil then
        delayCalls[attachTag] = nil
    else
        coremove(delayCalls[attachTag],func)
        coremove(newcolist,func)
    end
end

function stopAllDelayCall()
    delayCalls = {}
end

--------------------------------------------------------
-- 帧调用
--------------------------------------------------------
local delayFrames = {}

function delayFrame(func,delay,attachTag,userData)
    local co = {func = func,delay = delay,arg = userData,time = 0,attachTag = tostring(attachTag),typelist = delayFrames}
    tinsert(newcolist,co)
end

function stopDelayFrame(func,attachTag)
    attachTag = tostring(attachTag)
    
    if func==nil then
        delayFrames[attachTag] = nil
    else
        coremove(delayFrames[attachTag],func)
        coremove(newcolist,func)
    end
end


function stopAllDelayFrame()
    delayFrames = {}
end




--------------------------------------------------------
-- 循环调用
--------------------------------------------------------
local repeatCalls = {}

function repeatCall(func,delay,attachTag,userData)
    attachTag = tostring(attachTag)
    if hasco(repeatCalls[attachTag],func) or hasco(newcolist,func) then
        return
    end
    
    local co = {func = func,delay = delay,arg = userData,time = 0,attachTag = attachTag,typelist = repeatCalls}
    tinsert(newcolist,co)
end

function stopRepeatCall(func,attachTag)
    attachTag = tostring(attachTag)
    
    if func==nil then
        repeatCalls[attachTag] = nil
    else
        coremove(repeatCalls[attachTag],func)
        coremove(newcolist,func)
    end
end

function stopAllRepeatCall()
    repeatCalls = {}
end
----------------------------------------------------------

function LuaCallUtil._update()
    --add new co
    for i,co in ipairs(newcolist) do
        newcolist[i] = nil
        
        local colist = co.typelist[co.attachTag]
        if colist==nil then
            colist = {}
            co.typelist[co.attachTag] = colist
        end
        tinsert(colist,co)
    end
    
    --dofunc
    local deltaTime = Time.deltaTime
    --delaycall
    for attachTag,colist in pairs(delayCalls) do
        local hasCo = false
        local count = #colist
        for i = count, 1,-1 do
            local co = colist[i]
            co.time = co.time + deltaTime
            if co.time >= co.delay then
                xpcall(co.func,logException,co.arg)
--                co.func(co.arg)
                tremove(colist, i)
            end
            hasCo = true
        end
        if not hasCo then
            delayCalls[attachTag] = nil
        end
    end
    --delayframe
    for attachTag,colist in pairs(delayFrames) do
        local hasCo = false
        local count = #colist
        for i = count, 1,-1 do
            local co = colist[i]
            co.time = co.time + 1
            if co.time >= co.delay then
--                co.func(co.arg)
                xpcall(co.func,logException,co.arg)
                tremove(colist, i)
            end
            hasCo = true
        end
        if not hasCo then
            delayFrames[attachTag] = nil
        end
    end
    --repeatcall
    for attachTag,colist in pairs(repeatCalls) do
        for i,co in ipairs(colist) do
            co.time = co.time + deltaTime
            if co.time >= co.delay then
                co.time = 0
                xpcall(co.func,logException,co.arg)
--                co.func()
            end
        end
    end
end

UpdateBeat:Add(LuaCallUtil._update,LuaCallUtil)

return LuaCallUtil