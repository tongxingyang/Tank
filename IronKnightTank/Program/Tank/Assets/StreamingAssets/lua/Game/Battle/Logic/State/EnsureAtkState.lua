---------------------------------------------
--- EnsureAtkState
---	确认攻击状态
--- Created by thrt520.
--- DateTime: 2018/6/21
---------------------------------------------
---@class EnsureAtkState : IState
local EnsureAtkState = {}
local this = EnsureAtkState

EnsureAtkState.StateEnum = EManualState.EnsureAtkState
EnsureAtkState.curTankId = 0
EnsureAtkState.isFocus = false
EnsureAtkState.enemyId = 0
EnsureAtkState.isShowPanel = false

local TankModel = require("Game.Battle.Model.TankModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickTank"),
	require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
	require("Game.Event.Message.Battle.MSGMouseHoverOutTank"),
	require("Game.Event.Message.Battle.MSGMouseHoverTank"),
	require("Game.Event.Message.Battle.MSGClickCancelButton"),
	require("Game.Event.Message.Battle.MSGConfirmAtk"),
}

function EnsureAtkState.OnEnter(param)
	this.curTankId = param.TankId
	this.isFocus = param.IsFocus
	this.isShowPanel = false
	ViewFacade.UpdateCtrlState(this.StateEnum)
	ViewFacade.TipsCanAttackTank(this.curTankId , BattleRuntimeData.CurCamp)
	ViewFacade.ShowTankSignView(this.curTankId , true)
	this._registerCommandHandler()
end

function EnsureAtkState.OnExit()
	ViewFacade.CloseTankSingView()
	ViewFacade.CancelTipsCanAttackTank()
	ViewFacade.CloseNpcTankAttributePanel()
	this._unregisterCommandHandler()
end

function EnsureAtkState._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,EnsureAtkState)
end

function EnsureAtkState._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,EnsureAtkState)
end

-------------------------------------------
---event handler
-------------------------------------------
--- MSGClickTank 事件处理
---@param msg MSGClickTank
function EnsureAtkState.OnMSGClickTank(msg)
	if  BattleRuntimeData.CurCamp == msg.Camp then
		return
	end
	local tankData = TankModel.GetTankData(msg.TankId)
	if tankData.CanBeAtk then
		this.enemyId = msg.TankId
		local atkTank = TankModel.GetTankData(this.curTankId)
		local destoryRate , hitTrue = atkTank:GetAtkDestoryRateAndHitRate(tankData , this.isFocus )
		local param = {
			AtkTank = atkTank,
			DefTank = tankData,
			HitRate = hitTrue,
			DestoryRate = destoryRate
		}
		this._openConfirmAtkPanel(param)
	end
end

--- MSGConfirmAtk 事件处理
---@param msg MSGConfirmAtk
function EnsureAtkState.OnMSGConfirmAtk(msg)
	this._closeConfirmAtkPanel()
	this.FSM:ChangeState(EManualState.TankAtkState , {TankId = this.curTankId , EnemyId = this.enemyId , IsFocus = this.isFocus})
end

--- MSGClickRightMouseBtn 事件处理
---@param msg MSGClickRightMouseBtn
function EnsureAtkState.OnMSGClickRightMouseBtn(msg)
	if this.isShowPanel then
		this._closeConfirmAtkPanel()
	else
		this.FSM:ChangeState(EManualState.MineTankState , {TankId = this.curTankId})
	end

end

--- MSGMouseHoverOutTank 事件处理
---@param msg MSGMouseHoverOutTank
function EnsureAtkState.OnMSGMouseHoverOutTank(msg)
	ViewFacade.CloseNpcTankAttributePanel()
end

--- MSGMouseHoverTank 事件处理
---@param msg MSGMouseHoverTank
function EnsureAtkState.OnMSGMouseHoverTank(msg)
	ViewFacade.OpenNpcTankAttributePanel(msg.TankId)
end

--- MSGClickCancelButton 事件处理
---@param msg MSGClickCancelButton
function EnsureAtkState.OnMSGClickCancelButton(msg)
	if this.isShowPanel then
		this._closeConfirmAtkPanel()
	end
	this.FSM:ChangeState(EManualState.MineTankState , {TankId = this.curTankId})
end
--------------------------------------

function EnsureAtkState._closeConfirmAtkPanel()
	this.isShowPanel = false
	PanelManager.Close(PanelManager.PanelEnum.ConfirmAtkPanel)
end

function EnsureAtkState._openConfirmAtkPanel(param)
	this.isShowPanel = true
	PanelManager.Open(PanelManager.PanelEnum.ConfirmAtkPanel , param )

end

return EnsureAtkState