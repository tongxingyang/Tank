---------------------------------------------
--- Command
--- Created by Tianc.
--- DateTime: 2018/04/24
---------------------------------------------

---@class Command
local Command = class("Command")

local AsyncState = require("Framework.Util.Structure.AsyncState")

---@type AsyncState 执行状态
---@private
Command._state = nil

--- 定义一个Command
---@param commandName string
---@return class
function CommandClass(commandName)
    --继承Command生成一个类
    local commandClass = class("Command",Command)

    --代理子类中的Execute方法
    local metatable = getmetatable(commandClass)
    metatable.__newindex = function(t,n,v)
        if n == "Execute" then
            metatable.__newindex = nil
            commandClass.__execute = v
            commandClass.Execute = Command._execute
        end
    end

    --
    return commandClass
end

--- 声明为异步状态，返回一个状态查询接口
---@return AsyncState
---@protected
function Command:Async()
    return self:_getAsyncState()
end

--- 异步执行结束
---@protected
function Command:EndCommand()
    self:_getAsyncState():End()
end

--- 子类中的Execute代理方法
---@private
function Command._execute(c,...)
    local command = c.new()
    local result,result2 = command:__execute(...)
    assert((result == nil or result == command._state) and result2 == nil,"Can not return any result on 'Execute'.")
    return result or AsyncState.DoneState
end

---@private
function Command:_getAsyncState()
    if self._state == nil then
        self._state = AsyncState.New()
    end
    return self._state
end

return Command