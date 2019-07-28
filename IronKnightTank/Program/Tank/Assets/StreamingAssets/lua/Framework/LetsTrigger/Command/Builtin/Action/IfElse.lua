---------------------------------------------
--- IfElse
--- Created by Tianc.
--- DateTime: 2018/04/26
---------------------------------------------
---@class IfElse : ScriptAction
local IfElse = class("IfElse",require("Framework.LetsTrigger.Command.ScriptAction"))

---@type ScriptAction[]
IfElse.True = nil

---@type ScriptAction[]
IfElse.False = nil

---@type boolean
IfElse.Condition = nil

function IfElse:OnExecute()
    if self.Condition then
        coroutine.createAndRun(self._runActions,self,self.True)
    else
        coroutine.createAndRun(self._runActions,self,self.False)
    end
end

function IfElse:_runActions(actions)
    for i, v in ipairs(actions) do
        v:Execute():Yield()
    end
    self:End()
end


return IfElse