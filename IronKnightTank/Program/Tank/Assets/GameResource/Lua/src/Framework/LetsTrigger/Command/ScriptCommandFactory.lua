---------------------------------------------
--- ScriptCommandFactory
--- Created by Tianc.
--- DateTime: 2018/04/26
---------------------------------------------

---@class ScriptCommandFactory
local ScriptCommandFactory = {}

---@type table<string,class>
ScriptCommandFactory.Config = nil

--- 获取一个命令对象
---@param runtimeInstance LScriptRuntimeInstance
---@param script LScript
---@return ScriptCommand
function ScriptCommandFactory.GetCommand(runtimeInstance, script, config)
    local commandName = ScriptCommandFactory.GetCommandName(config)
    local commandClass = ScriptCommandFactory.Config[commandName]
    assert(commandClass,string.format("Can not find command '%s'.",commandName))

    ---@type ScriptCommand
    local command = commandClass.new()
    command.Config = config
    command.RuntimeInstance = runtimeInstance

    return command
end

--- 判断一个对象是否是ScriptCommand或者是ScriptCommand配置
---@param data table
function ScriptCommandFactory.IsCommand(data)
    return ScriptCommandFactory.GetCommandName(data) ~= nil
end

--- 判断一个对象是否是ScriptVariable或者是ScriptVariable配置
---@param data table
function ScriptCommandFactory.IsVariable(data)
    return data ~= nil and type(data) == "table" and data.VariableName ~= nil
end

--- 获取命令名字
---@param data table
---@return string
function ScriptCommandFactory.GetCommandName(data)
    if data ~= nil and type(data) == "table" then
        return data.VariableName or data.ActionName
    end
end

return ScriptCommandFactory