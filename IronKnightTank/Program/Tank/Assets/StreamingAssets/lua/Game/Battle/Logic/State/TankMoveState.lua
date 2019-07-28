---------------------------------------------
--- TankMoveState
--- Created by thrt520.
--- DateTime: 2018/7/24
---------------------------------------------
---@class TankMoveState : IState
local TankMoveState = {}
local this = TankMoveState

TankMoveState.StateEnum = EManualState.TankMoveState
TankMoveState.FSM = nil

local MSGMoveTankRequest = require("Game.Event.Message.Battle.MSGMoveTankRequest")
local ViewFacade = require("Game.Battle.View.ViewFacade")

function TankMoveState.OnEnter(param)
	ViewFacade.UpdateCtrlState(this.StateEnum)
	coroutine.createAndRun(this.MoveTank , param)
end

function TankMoveState.OnExit()

end

function TankMoveState.MoveTank(param)
	EventBus:Brocast(MSGMoveTankRequest:Build({TankId = param.TankId , TargetPos = param.TargetPos})):Yield()
	this.FSM:ChangeState(EManualState.FinishCmdState , {TankId = param.TankId})
end

return TankMoveState