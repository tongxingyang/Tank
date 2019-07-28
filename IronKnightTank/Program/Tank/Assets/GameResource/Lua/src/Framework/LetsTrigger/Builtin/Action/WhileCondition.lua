---------------------------------------------
--- WhileCondition
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------
---@class WhileCondition : ScriptAction
local WhileCondition = class("WhileCondition",require("Framework.LetsTrigger.Command.ScriptAction"))
local LScriptUtil = require("Framework.LetsTrigger.Util.LScriptUtil")

---@type boolean
WhileCondition.Condition = nil

---@type ScriptAction[]
WhileCondition.Actions = nil

function WhileCondition:OnExecute()
    while self.Condition do
        LScriptUtil.RunActionsInSequence(self.Actions)
    end
    self:End()
end

return WhileCondition