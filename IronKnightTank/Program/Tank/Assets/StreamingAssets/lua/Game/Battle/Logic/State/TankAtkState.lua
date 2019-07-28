---------------------------------------------
--- TankAtkState
--- Created by thrt520.
--- DateTime: 2018/6/21
---------------------------------------------
---@class TankAtkState : IState
local TankAtkState = {}
local this = TankAtkState

---@type EManualState
TankAtkState.StateEnum = EManualState.TankAtkState
---@type FSM
TankAtkState.FSM = nil

local MSGTankAtkRequeset = require("Game.Event.Message.Battle.MSGTankAtkRequeset")
local ViewFacade = require("Game.Battle.View.ViewFacade")

function TankAtkState.OnEnter(param)
	ViewFacade.UpdateCtrlState(this.StateEnum)
	coroutine.createAndRun(this.AtkTank , param)
end

function TankAtkState.OnExit()

end

function TankAtkState.AtkTank(param)
	EventBus:Brocast(MSGTankAtkRequeset:Build({AtkTankId = param.TankId , DefTankId = param.EnemyId , IsFocus = param.IsFocus})):Yield()
	this.FSM:ChangeState(EManualState.FinishCmdState , {TankId = param.TankId})
end

return TankAtkState