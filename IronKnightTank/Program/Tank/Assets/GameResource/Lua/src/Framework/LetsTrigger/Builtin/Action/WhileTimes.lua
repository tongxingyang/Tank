---------------------------------------------
--- WhileTimes
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------
---@class WhileTimes : ScriptAction
local WhileTimes = class("WhileTimes",require("Framework.LetsTrigger.Command.ScriptAction"))
local LScriptUtil = require("Framework.LetsTrigger.Util.LScriptUtil")

---@type number
WhileTimes.Times = nil

---@type ScriptAction[]
WhileTimes.Actions = nil

function WhileTimes:OnExecute()
    local times = self.Times
    for i = 1, times do
        LScriptUtil.RunActionsInSequence(self.Actions)
    end
    self:End()
end

return WhileTimes