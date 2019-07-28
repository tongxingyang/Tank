---------------------------------------------
--- CoroutineTimeLine
--- Created by Tianc.
--- DateTime: 2018/04/28
---------------------------------------------
--TODO:使用树结构，深度优先替换Stack+Queue完成协程恢复
---@class CoroutineTimeLine
local CoroutineTimeLine = {}
CoroutineTimeLine.__index=CoroutineTimeLine

function CoroutineTimeLine.New()
    local o = { _interruptStack = Stack.New(),_waitQueue = Queue.New(),_suspendedThreads = {}}
    ReferenceCounter.MarkWithTraceback("CoroutineTimeLine", o)
    setmetatable(o,CoroutineTimeLine)
    return o
end

---@type Stack
CoroutineTimeLine._interruptStack = nil

---@type Queue
CoroutineTimeLine._waitQueue = nil

---@type table
CoroutineTimeLine._suspendedThreads = nil

---@type table
CoroutineTimeLine._current = nil

function CoroutineTimeLine:Request(task,co,atomic)
    co = co or coroutine.running()
    local threadInfo = self:_getThreadInfo(task,co,atomic)
    --clog2(25,"-----Request-----"..tostring(task),self)
    --当前没有正在运行的协程
    if self._current == nil then
        self:_run(threadInfo)
        --clog2(25,task,"run-1")
        return AsyncState.DoneState
    end

    --已经被阻断了
    if self._suspendedThreads[task] ~= nil then
        --clog2(25,task,"suspended")
        return self._suspendedThreads[task].runState
    end

    --当前正在运行请求的协程
    if self._current.co == co then
        --clog2(25,task,"self!")
        return AsyncState.DoneState
    end

    --当前正有一个原子级协程在运行，请求的协程加入等待运行队列
    if self._current.atomic then
        self._suspendedThreads[task] = threadInfo
        self._waitQueue:Offer(threadInfo)
        --table.insert(self._waitQueue,threadInfo)
        --clog2(25,task,"to queue")
        --coroutine.yield()
        threadInfo.runState:Reset()
        return threadInfo.runState
    end

    --当前正在运行的协程允许被中断，中断当前后，优先运行请求的协程
    self._suspendedThreads[self._current.task] = self._current
    self._interruptStack:Push(self._current)
    self._current.runState:Reset()
    --clog2(25,self._current.task,"to stack","run",task)
    self:_run(threadInfo)
    return AsyncState.DoneState
end

function CoroutineTimeLine:_getThreadInfo(task,co,atomic)
    return self._suspendedThreads[task] or {co = co,atomic = atomic,task = task,runState = AsyncState.New()}
end

function CoroutineTimeLine:_run(threadInfo)
    self._current = threadInfo

    if self._endHandler ~= nil then
        stopRepeatCall(self._endHandler)
    end
    self._endHandler = function()
        if coroutine.status(self._current.co) == "dead" then
            stopRepeatCall(self._endHandler)
            self:_onCurrentEnd()
        end
    end
    repeatCall(self._endHandler,0)
end

function CoroutineTimeLine:_onCurrentEnd()
    local next
    if self._waitQueue:Size() > 0 then
        next =  self._waitQueue:Poll()
    elseif self._interruptStack:Size() > 0 then
        next = self._interruptStack:Pop()
    end
    self._current = nil

    --恢复运行
    if next then
        self._suspendedThreads[next.task] = nil
        self:Request(next.task,next.co,next.atomic)
        next.runState:End()
    end
end

return CoroutineTimeLine