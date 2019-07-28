---------------------------------------------
--- ScriptCommand
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------

---@class ScriptCommand
local ScriptCommand = class("ScriptCommand")

---@type table @命令原始配置信息
ScriptCommand.Config = nil

---@type LScriptRuntimeInstance @所属的运行实例
ScriptCommand.RuntimeInstance = nil

return ScriptCommand