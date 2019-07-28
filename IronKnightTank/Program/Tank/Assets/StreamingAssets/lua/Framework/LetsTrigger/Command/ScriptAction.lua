---------------------------------------------
--- ScriptAction
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------

---@class ScriptAction : ScriptCommand
local ScriptAction = class("ScriptAction")
local AsyncState = require("Framework.Util.Structure.AsyncState")

---@type AsyncState
---@private
ScriptAction._runState = nil

---------------------------------------------------------
--- interface
---------------------------------------------------------

function ScriptAction:OnExecute()end

function ScriptAction:OnEnd()end

---------------------------------------------------------
--- function
---------------------------------------------------------

function ScriptAction:Execute()
    self._runState = AsyncState.New()
    self:OnExecute()
    return self._runState
end

function ScriptAction:End()
    self:OnEnd()
    self._runState:End()
end


return ScriptAction