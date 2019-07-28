---------------------------------------------
--- SetVariable
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------
---@class SetVariable : ScriptAction
local SetVariable = class("SetVariable",require("Framework.LetsTrigger.Command.ScriptAction"))

---@type string
SetVariable.Name = nil

---@type any
SetVariable.Value = nil

function SetVariable:OnExecute()
    self.RuntimeInstance.Blackboard[self.Name] = self.Value
    self:End()
end

return SetVariable