---------------------------------------------
--- AsyncState
--- Created by Tianc.
--- DateTime: 2018/04/24
---------------------------------------------

---@class AsyncState
AsyncState = {}
---@private
AsyncState.__index = AsyncState

---@type boolean 已经结束
---@private
AsyncState._isDone = nil

---@return AsyncState
function AsyncState.New(isDone)
    local o = { _isDone = isDone }
    ReferenceCounter.MarkWithTraceback("AsyncState", o)
    setmetatable(o,AsyncState)
    return o
end

--- 查询异步状态是否结束
---@return boolean
function AsyncState:IsDone()
    return self._isDone
end

--- 结束该状态
function AsyncState:End()
    self._isDone = true
    if self._co then
        local co = self._co
        self._co = nil
        for i, v in ipairs(co) do
            coroutine.resume(v)
        end
    end
end

--- 重置该状态
function AsyncState:Reset()
    self._isDone = false
    self._co = nil
end

--- 协程等待该状态结束
---@param co thread|nil
function AsyncState:Yield(co)
    if not self._isDone then
        if self._co == nil then
            self._co = {}
        end
        table.insert(self._co,co or coroutine.running())
        --self._co = co or coroutine.running()
        coroutine.yield()
    end
end

---@type AsyncState 表示一个结束状态
AsyncState.DoneState = AsyncState.New(true)

return AsyncState