---------------------------------------------
--- ScriptConfig
--- Created by Tianc.
--- DateTime: 2018/04/26
---------------------------------------------

---@class ScriptConfig
local ScriptConfig = {}

------------------------------------------------------------------------------
--- 加载命令
------------------------------------------------------------------------------

local scriptCommands = {
    --action-builtin
    --require("Framework.LetsTrigger.Builtin.Action.IfElse"),
    --require("Framework.LetsTrigger.Builtin.Action.Delay"),
    --require("Framework.LetsTrigger.Builtin.Action.IfThen"),
    --require("Framework.LetsTrigger.Builtin.Action.ParallelActions"),
    --require("Framework.LetsTrigger.Builtin.Action.Print"),
    --require("Framework.LetsTrigger.Builtin.Action.RunScript"),
    --require("Framework.LetsTrigger.Builtin.Action.SequenceActions"),
    --require("Framework.LetsTrigger.Builtin.Action.SetVariable"),
    --require("Framework.LetsTrigger.Builtin.Action.WhileCondition"),
    --require("Framework.LetsTrigger.Builtin.Action.WhileTimes"),
	--
    ----action
    --require("Game.Script.Command.Action.SendMessage"),
	--
    ----variable-builtin
    --require("Framework.LetsTrigger.Builtin.Variable.Variable"),
    --require("Framework.LetsTrigger.Builtin.Variable.DigitArithmetic"),
    --require("Framework.LetsTrigger.Builtin.Variable.Compare"),

    --variable
}


------------------------------------------------------------------------------
--- 初始化命令配置
------------------------------------------------------------------------------

local ScriptCommandFactory = require("Framework.LetsTrigger.Command.ScriptCommandFactory")
ScriptCommandFactory.Config = {}
for i, v in ipairs(scriptCommands) do
    assert(ScriptCommandFactory.Config[v.__cname] == nil,string.format("There are two command named '%s'.",v.__cname))
    ScriptCommandFactory.Config[v.__cname] = v
end
scriptCommands = nil

return ScriptConfig