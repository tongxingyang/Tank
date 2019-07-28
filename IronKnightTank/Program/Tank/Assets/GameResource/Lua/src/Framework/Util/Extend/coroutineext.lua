---------------------------------------------
--- coroutineext
--- Created by Tianc.
--- DateTime: 2018/04/15
---------------------------------------------

---@class coroutineext

local coroutineext = {}

local unpacklist = table.unpacklist

--- 调用并等待一个异步回调返回形式的函数返回
---@param co coroutin
---@param func fun():any
---@param callbackIndex number @回调在func参数列表中的序号
function coroutine.callwait(co,func,callbackIndex,...)
    local co = co or coroutine.running()
    local args = {...}
    local yield = nil
    local result = nil

    args[callbackIndex] = function(...)
        if yield then
            coroutine.resume(co,...)
        else
            result = {...}
        end
    end

    func(unpacklist(args,10))

    if result then
        return unpacklist(result,10)
    else
        yield = true
        return coroutine.yield()
    end
end

--- 等待一个异步状态
---@param co thread 
---@param state AsyncState
function coroutine.waitstate(co,state)
    if state == nil or state:IsDone() then
        return
    end

    local co = co or coroutine.running()
    local waitEnd

    waitEnd = function()
        if state:IsDone() then
            stopRepeatCall(waitEnd)
            coroutine.resume(co)
        end
    end

    repeatCall(waitEnd,0)
    coroutine.yield()
end

---@param co thread 
---@param thread thread
function coroutine.waitthread(co,thread)
    if thread == nil or coroutine.status(thread) == "dead" then
        return
    end
    local co = co or coroutine.running()
    local waitEnd

    waitEnd = function()
        clog(23,coroutine.status(thread))
        if coroutine.status(thread) == "dead" then
            stopRepeatCall(waitEnd)
            coroutine.resume(co)
        end
    end

    repeatCall(waitEnd,0)
    coroutine.yield()
end

--- 创建并运行一个协程
function coroutine.createAndRun(func,...)
    local params = {...}
    local co = coroutine.create(function()
        xpcall(func,logException,unpack(params))
    end)
    coroutine.resume(co)
    return co
end

return coroutineext