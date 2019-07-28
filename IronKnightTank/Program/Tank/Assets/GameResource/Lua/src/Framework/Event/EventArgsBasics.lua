--- @class EventArgsBasics
local EventArgsBasics = class("EventArgsBasics")

local AsyncState = require("Framework.Util.Structure.AsyncState")

---@type string 消息事件名
EventArgsBasics.EventName = nil

---@type AsyncState 被挂起后的异步状态
---@private
EventArgsBasics._asyncState = nil

---@type number 被请求挂起的计数
---@private
EventArgsBasics._holdCount = nil

---@type boolean 消息已经成功转发到所有接收者
---@private
EventArgsBasics._isBrocastSucceed = nil

--- 定义一个消息
---@param eventName string @消息名字
---@return class
function EventArgsBasics.Define(eventName)
    local eventClass = class(eventName,EventArgsBasics)
    eventClass.EventName = eventName
    return eventClass
end

--TODO:替换所有的EventArgsBasics.Define
MSGClass = EventArgsBasics.Define

--- 一种快捷写法
---@param class class @class of EventArgsBasics
---@param msg table @定义初始属
---@return EventArgsBasics
function EventArgsBasics.Build(class,msg)
    local c = class.new()
    msg.__index = c
    setmetatable(msg,msg)
    return msg
end

--- 构造函数
function EventArgsBasics:ctor(...)
    self._holdCount = 0
    self._isBrocastSucceed = false
    self._asyncState = AsyncState.New()
end

--- 挂起事件
function EventArgsBasics:Pend()
    self._holdCount = self._holdCount + 1
end

--- 恢复事件
function EventArgsBasics:Restore()
    self._holdCount = self._holdCount - 1
    if self:_isWaitForFinal() then
        self:_final()
    end
end

--- 终结
---@return AsyncState
function EventArgsBasics:EndBrocast()
    self._isBrocastSucceed = true
    if self:_isWaitForFinal() then
        self:_final()
    end
    return self._asyncState
end

--- 是否处于等待结束状态
---@private
function EventArgsBasics:_isWaitForFinal()
    return self._isBrocastSucceed and self._holdCount <= 0
end

--- 进入结束状态
---@private
function EventArgsBasics:_final()
    self._holdCount = 0
    self._isBrocastSucceed = false
    self._asyncState:End()
end

return EventArgsBasics