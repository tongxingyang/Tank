---------------------------------------------
--- NoneState
--- Created by thrt520.
--- DateTime: 2018/6/21
---------------------------------------------
---@class NoneState : IState
local NoneState = {}
---@type EManualState
NoneState.StateEnum = EManualState.NoneState
local ViewFacade = require("Game.Battle.View.ViewFacade")

function NoneState.OnEnter(param)
	ViewFacade.UpdateCtrlState(NoneState.StateEnum)
end

function NoneState.OnExit()

end

return NoneState