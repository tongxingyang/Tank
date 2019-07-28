---------------------------------------------
--- IfThen
--- Created by Tianc.
--- DateTime: 2018/04/26
---------------------------------------------
---@class IfThen : ScriptAction
local IfThen = class("IfThen",require("Framework.LetsTrigger.Command.ScriptAction"))
local LScriptUtil = require("Framework.LetsTrigger.Util.LScriptUtil")

---@type ScriptAction[]
IfThen.True = nil

---@type boolean
IfThen.Condition = nil

function IfThen:OnExecute()
    if self.Condition then
        LScriptUtil.RunActionsInSequence(self.True)
    end
    self:End()
end

function IfThen:_runActions(actions)
    for i, v in ipairs(actions) do
        v:Execute():Yield()
    end
    self:End()
end


return IfThen