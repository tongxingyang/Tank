---------------------------------------------
--- EnemTankState
---选取玩家坦克状态
--- Created by thrt520.
--- DateTime: 2018/7/24
---------------------------------------------
---@class EnemyTankState : IState
local EnemyTankState = {}
local this = EnemyTankState

---@type EManualState
EnemyTankState.StateEnum = EManualState.EnemyTankState
---@type FSM
EnemyTankState.FSM = nil
---@type number
EnemyTankState.curTankId = 0
---@type ECamp
EnemyTankState.Camp = nil

local ViewFacade = require("Game.Battle.View.ViewFacade")
local BattleRuntimeData =  require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local MapModel = require("Game.Battle.Model.MapModel")
local TankModel = require("Game.Battle.Model.TankModel")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickTank"),
	require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
	require("Game.Event.Message.Battle.MSGClickCancelButton"),
}

function EnemyTankState.OnEnter(param)
	this.curTankId = param.TankId
	this.Camp = BattleRuntimeData.CurCamp
	local tankData = TankModel.GetTankData(this.curTankId)
	this._registerCommandHandler()
	MapModel.UpdateMapPathData(tankData)
	MapModel.HighLightCanMoveArea(2)
	ViewFacade.UpdateBlocks()
	ViewFacade.OpenNpcTankAttributePanel(this.curTankId)
	ViewFacade.ShowTankSignView(this.curTankId , false)
	ViewFacade.UpdateCtrlState(this.StateEnum)
	ViewFacade.CamFocusTank(this.curTankId , 0.3)
end

function EnemyTankState.OnExit()
	ViewFacade.CloseTankSingView()
	ViewFacade.CancelShowTankMoveableArea()
	ViewFacade.CloseNpcTankAttributePanel()
	ViewFacade.CancelShowTankMoveableArea()
	this._unregisterCommandHandler()
end

function EnemyTankState._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands, EnemyTankState)
end

function EnemyTankState._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands, EnemyTankState)
end
-------------------------------------------
---event handler
-------------------------------------------
--- MSGClickTank 事件处理
---@param msg MSGClickTank
function EnemyTankState.OnMSGClickTank(msg)
	if msg.TankId == this.curTankId then
		return
	end
	if msg.Camp == this.Camp then
		this.FSM:ChangeState(EManualState.MineTankState, { TankId = msg.TankId })
	else
		this.FSM:ChangeState(EManualState.EnemyTankState, { TankId = msg.TankId })
	end
end

--- MSGClickRightMouseBtn 事件处理
---@param msg MSGClickRightMouseBtn
function EnemyTankState.OnMSGClickRightMouseBtn(msg)
	this.FSM:ChangeState(EManualState.DefaultState)
end

--- MSGMouseHoverOutTank 事件处理
---@param msg MSGMouseHoverOutTank
function EnemyTankState.OnMSGMouseHoverOutTank(msg)
	ViewFacade.CloseNpcTankAttributePanel()
end

--- MSGClickCancelButton 事件处理
---@param msg MSGClickCancelButton
function EnemyTankState.OnMSGClickCancelButton(msg)
	this.FSM:ChangeState(EManualState.DefaultState)
end

--- MSGMouseHoverTank 事件处理
---@param msg MSGMouseHoverTank
function EnemyTankState.OnMSGMouseHoverTank(msg)
	ViewFacade.OpenNpcTankAttributePanel(msg.TankId)
end
-------------------------------------------


return EnemyTankState