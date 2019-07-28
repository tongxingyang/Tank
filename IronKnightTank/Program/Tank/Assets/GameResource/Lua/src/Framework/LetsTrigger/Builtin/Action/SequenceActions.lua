---------------------------------------------
--- SequenceActions
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------
---@class SequenceActions : ScriptAction
local SequenceActions = class("SequenceActions",require("Framework.LetsTrigger.Command.ScriptAction"))
local LScriptUtil = require("Framework.LetsTrigger.Util.LScriptUtil")

---@type ScriptAction[]
SequenceActions.Actions = nil

function SequenceActions:OnExecute()
    LScriptUtil.RunActionsInSequence(self.Actions)
    self:End()
end

return SequenceActions