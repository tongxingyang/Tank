---------------------------------------------
--- IfThen
--- Created by Tianc.
--- DateTime: 2018/04/26
---------------------------------------------
---@class IfThen : ScriptAction
local IfThen = class("IfThen",require("Framework.LetsTrigger.Command.ScriptAction"))

---@type ScriptAction[]
IfThen.True = nil

---@type boolean
IfThen.Condition = nil

function IfThen:OnExecute()
    if self.Condition then
        coroutine.createAndRun(self._runActions,self,self.True)
    end
end

function IfThen:_runActions(actions)
    for i, v in ipairs(actions) do
        v:Execute():Yield()
    end
    self:End()
end


return IfThen