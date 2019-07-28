---@class EventDomain
local EventDomain = class("EventDomain")

---@private
EventDomain._friendDomain = nil

---@private
EventDomain._handlers = nil

function EventDomain:ctor()
    self._handlers = {}
    self._friendDomain = {}
end

---发送消息
---@param eventArg EventArgsBasics 消息体
---@return AsyncState
function EventDomain:Brocast(eventArg)
    self:_brocastToInternal(eventArg.EventName,eventArg)
    self:_brocastToFriend(eventArg.EventName,eventArg)
    return eventArg:EndBrocast()
end

---移除监听
---@param eventName string|EventArgsBasics 事件名字|消息体的定义
---@param handler fun(msg:EventArgsBasics,...) 监听
function EventDomain:RemoveListener(eventName,handler)
    eventName = EventDomain._parseEventName(eventName)
    local events = self._handlers[eventName]
    if events then
        events[handler] = nil
    end
end

---添加监听
---@param eventName string|EventArgsBasics @事件名字|消息体的定义
---@param handler fun(msg:EventArgsBasics,...) @监听
---@param ... @缓存的数据，传回监听函数处理
function EventDomain:AddListener(eventName,handler,...)
    eventName = EventDomain._parseEventName(eventName)
    local events = self._handlers[eventName]
    if not events then
        events = {}
        self._handlers[eventName] = events
    end
    local userData = {...}

    events[handler] = #userData > 0 and userData or true
end

--- 为一组消息添加注册接收器，接收方能使用形如 OnMSGXXXX 函数相应事件
---@param eventList string[]|EventArgsBasics[] 需要注册接收的消息列表
---@param receiver table 消息处理方
function EventDomain:RegisterReceiver(eventList,receiver,...)
    for i, eventName in ipairs(eventList) do
        eventName = EventDomain._parseEventName(eventName)
        local handler = receiver[string.format("On%s",eventName)]
        if handler then
            self:AddListener(eventName,handler,...)
        else
            clog("无指定 handler"..eventName)
        end
    end
end

--- 移除接收器上的一组事件监听
---@param eventList string[]|EventArgsBasics[] 需要移除接收的消息列表
---@param receiver table 消息处理方
function EventDomain:UnregisterReceiver(eventList,receiver)
    for i, eventName in ipairs(eventList) do
        eventName = EventDomain._parseEventName(eventName)
        local handler = receiver[string.format("On%s",eventName)]
        if handler then
            self:RemoveListener(eventName,handler)
        end
    end
end


--- 为一组消息添加注册接收器（该接收器为class的具体对象），接收方能使用形如 OnMSGXXXX 函数相应事件
---@param eventList string[]|EventArgsBasics[] 需要注册接收的消息列表
---@param receiver table 消息处理方
function EventDomain:RegisterSelfReceiver(eventList,receiver,...)
    for i, eventName in ipairs(eventList) do
        eventName = EventDomain._parseEventName(eventName)
        local classHandler = receiver[string.format("On%s",eventName)]
        if classHandler then
            local selfHandler = function(...)
                classHandler(receiver,...)
            end
            receiver[string.format("_rsr_On%s",eventName)] = selfHandler
            self:AddListener(eventName,selfHandler,...)
        end
    end
end

--- 移除接收器（该接收器为class的具体对象）上的一组事件监听
---@param eventList string[]|EventArgsBasics[] 需要移除接收的消息列表
---@param receiver table 消息处理方
function EventDomain:UnregisterSelfReceiver(eventList,receiver)
    for i, eventName in ipairs(eventList) do
        eventName = EventDomain._parseEventName(eventName)
        local selfHandler = receiver[string.format("_rsr_On%s",eventName)]
        if selfHandler then
            self:RemoveListener(eventName, selfHandler)
        end
    end
end



--- 监听另一个事件域中的事件
---@param eventDomain EventDomain
function EventDomain:ListenToDomain(eventDomain)
    eventDomain:_addFriendDomain(self)
end

--- 清除指定事件所有监听
---@param eventName string|EventArgsBasics @事件名字|消息体的定义
function EventDomain:ClearListener(eventName)
    eventName = EventDomain._parseEventName(eventName)
    self._handlers[eventName] = nil
end

--- 清除所有事件
function EventDomain:ClearAllListener()
    self._handlers = {}
end

--- 清除关心的其他事件域
function EventDomain:ClearFriendDomain()
    self._friendDomain = {}
end

---@private
function EventDomain._parseEventName(eventName)
    return type(eventName) == "string" and eventName or eventName.EventName
end

---@private
function EventDomain:_brocastToInternal(eventName,eventArg)
    local events = self._handlers[eventName]
    if events then
        for k,v in pairs(events) do
            if v == true then
                xpcall(k,logException,eventArg)
                --k(eventArg)
            else
                xpcall(k,logException,unpack(v),eventArg)
                --k(unpack(v),eventArg)
            end
        end
    end
end

---@private
function EventDomain:_brocastToFriend(eventName,eventArg)
    for i,v in ipairs(self._friendDomain) do
        v:_brocastToInternal(eventName,eventArg)
    end
end

---@private
function EventDomain:_addFriendDomain(eventDomain)
    for i,v in ipairs(self._friendDomain) do
        if v == eventDomain then
            return
        end
    end
    table.insert(self._friendDomain, eventDomain)
end


return EventDomain

