ReferenceCounter = {}
local globalToString = tostring
local mark = Assets.Scripts.Tools.Lua.LuaReferenceCounter.Mark

local weakReference = {}
local weakReferenceIndex = {}
local weakReferenceTypes = {}
local snapshoots = {}

setmetatable(weakReference,{__mode = "k"})
setmetatable(weakReferenceIndex,{__mode = "v"})
setmetatable(weakReferenceTypes,{__mode = "v"})

local index = 0

local function tostring(obj)
    local str
    xpcall(function(o)
        str = globalToString(o)
    end,function(o)
        str = "unknowstr"
    end,obj)
    return str
end

--- 监视对象引用情况
-- @param typeName @class string 被监视对象类型
-- @param tab @class table 被监视对象
-- @param tabName @class string 被监视对象名字
-- @return
function ReferenceCounter.Mark(typeName,tab,tabName,traceback)
    if weakReference[tab] ~= nil or tab == nil then
        return
    end
    index = index + 1
    local tabId = string.format("%s(%s)",tostring(tab),tostring(index))
    local refName = string.format("%s_ref_%s",tabName or typeName or "unknowtable",tabId)
    
    weakReference[tab] = index
    weakReferenceIndex[tabId] = tab
    weakReferenceTypes[refName] = tab
    mark(typeName,tabId,tabName or traceback)
end

function ReferenceCounter.MarkWithTraceback(typeName,tab,tracebackIndex)
    tracebackIndex = tracebackIndex or 1
    --    ReferenceCounter.Mark(typeName, tab, debug.traceback(""))
    ReferenceCounter.Mark(typeName, tab, nil, string.split(debug.traceback(""),"\n")[tracebackIndex + 3])
end

function ReferenceCounter.Has(tabId)
    --    clog("has",indexStr,"?",weakReferenceIndex[indexStr]~=nil)
    return weakReferenceIndex[tabId] ~= nil
end

function ReferenceCounter.MarkAll()
    ReferenceCounter.ForeachViableTables(function(path,tab)
        ReferenceCounter.Mark("Reference",tab,path)
    end)
end

function ReferenceCounter.Snapshoot()
    local snapshoot = {}
    setmetatable(snapshoot,{__mode = "k"})
    table.insert(snapshoots, snapshoot)
    ReferenceCounter.ForeachViableTables(function(path,tab)
        snapshoot[tab] = path
    end)
end

function ReferenceCounter.AnalyzeSnapshoot(index1,index2)
    local snapshoot1 = snapshoots[index1]
    local snapshoot2 = snapshoots[index2]
    if snapshoot1 == nil or snapshoot2 == nil then
        return
    end
    local typeName = string.format("Snapshoot %s-%s",index1,index2)
    for k,v in pairs(snapshoot2) do
        if snapshoot1[k] == nil then
            index = index + 1
            local tabId = string.format("%s(%s)",tostring(k),tostring(index))
            weakReferenceIndex[tabId] = k
            mark(typeName,tabId,v)
        end
    end
end

function ReferenceCounter.PrintTabReferenceWithTabId(tabId)
    ReferenceCounter.PrintTabReference(weakReferenceIndex[tabId])
end

function ReferenceCounter.PrintTabReference(t)
    ReferenceCounter.ForeachViableTables(function(path,tab)
        if t == tab then
            print("Reference@" .. tostring(path))
        end
    end)
    print("###############################################################")
end

function ReferenceCounter.ForeachViableTables(dofunc)
    local finedTab = {}
    local finedFuncTab = {}
    
    local function findFunc(path,func,findTabFunc)
        if finedFuncTab[func] then
            return
        end
        finedFuncTab[func] = true
        
        for i = 1, 100 do
            local uk, uv = debug.getupvalue(func, i)
            --            local uk, uv = xpcall(debug.getupvalue,logException,func, i)
            if uk ~= nil or uv ~= nil then
                if uv ~= nil and type(uv) == "table" then
                    xpcall(findTabFunc,logException,string.format("%s.%s",tostring(uk),path),uv)
                end
                if uv ~= nil and type(uv) == "function" then
                    xpcall(findFunc,logException,string.format("%s.%s",tostring(uk),path),uv,findTabFunc)
                end
            end
        end
        
        --        while true do
        --            local uk, uv = debug.getupvalue(func, i)
        --            if uk then
        --                if type(uv) == "table" then
        --                    findTabFunc(string.format("%s.%s",tostring(uk),path),uv)
        --                end
        --                if type(uv) == "function" then
        --                    findFunc(string.format("%s.%s",tostring(uk),path),uv,findTabFunc)
        --                end
        --                i = i + 1
        --            else
        --                break
        --            end
        --        end
        --        local funcEnv = getfenv(func)
        --        for k,v in pairs(funcEnv) do
        --            if v ~= nil and type(v) == "table" then
        --                findTabFunc(string.format("%s.%s",tostring(k),path),v)
        --            end
        --            if k ~= nil and type(k) == "table" then
        --                findTabFunc(string.format("%s.%s",tostring(k),path),k)
        --            end
        --            if v ~= nil and type(v) == "function" then
        --                findFunc(string.format("%s.%s",tostring(k),path),v,findTabFunc)
        --            end
        --            if k ~= nil and type(k) == "function" then
        --                findFunc(string.format("%s.%s",tostring(k),path),k,findTabFunc)
        --            end
        --        end
    end
    
    local function findTab(path,tab)
        if tab == nil or tab == ReferenceCounter or tab == weakReferenceIndex or tab == weakReference or tab == snapshoots then
            return
        end
        
        xpcall(dofunc,logException,path,tab)
        
        if finedTab[tab] then
            return
        end
        finedTab[tab] = true
        
        for k,v in pairs(tab) do
            if v ~= nil and type(v) == "table" then
                xpcall(findTab,logException,string.format("%s.%s",tostring(k),path),v)
            end
            if k ~= nil and type(k) == "table" then
                xpcall(findTab,logException,string.format("%s.%s",tostring(k),path),k)
            end
            if v ~= nil and type(v) == "function" then
                xpcall(findFunc,logException,string.format("%s.%s",tostring(k),path),v,findTab)
            end
            if k ~= nil and type(k) == "function" then
                xpcall(findFunc,logException,string.format("%s.%s",tostring(k),path),k,findTab)
            end
        end
        xpcall(findTab,logException,string.format("metatable.%s",path),getmetatable(tab))
    end
    
    xpcall(findTab,logException,"_G",_G)
    xpcall(findTab,logException,"Registry",debug.getregistry())
end

--if not iseditor then
--    function ReferenceCounter.Mark() end
--    function ReferenceCounter.MarkWithTraceback() end
--end