---------------------------------------------
--- DefaultState
---	默认状态
---
--- Created by thrt520.
--- DateTime: 2018/7/24
---------------------------------------------
---@class DefaultState : IState
local DefaultState = {}
local this = DefaultState
DefaultState.StateEnum = EManualState.DefaultState
---@type FSM
DefaultState.FSM = nil
---@type ECamp
DefaultState.Camp = nil

local ViewFacade = require("Game.Battle.View.ViewFacade")
local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local TankModel = require("Game.Battle.Model.TankModel")
local SkillModel  = require("Game.Battle.Model.SkillModel")

local MSGCampCtrlEndRequest = require("Game.Event.Message.Battle.MSGCampCtrlEndRequest")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickTank"),
	require("Game.Event.Message.Battle.MSGClickEndRoundBtn"),
	require("Game.Event.Message.Battle.MSGMouseHoverOutTank"),
	require("Game.Event.Message.Battle.MSGMouseHoverTank"),
	require("Game.Event.Message.Battle.MSGReleaseSkillRequest"),
}

function DefaultState.OnEnter(param)
	this.Camp = BattleRuntimeData.CurCamp
	ViewFacade.UpdateCtrlState(this.StateEnum)
	this._registerCommandHandler()
end

function DefaultState.OnExit()
	this._unregisterCommandHandler()
end

function DefaultState._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,DefaultState)
end

function DefaultState._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,DefaultState)
end

-------------------------------------------
---event handler
-------------------------------------------
--- MSGClickTank 事件处理
---@param msg MSGClickTank
function DefaultState.OnMSGClickTank(msg)
	if msg.Camp == this.Camp then
		this.FSM:ChangeState(EManualState.MineTankState, { TankId = msg.TankId })
	else
		this.FSM:ChangeState(EManualState.EnemyTankState, { TankId = msg.TankId})
	end
end

--- MSGClickEndRoundBtn 事件处理
---@param msg MSGClickEndRoundBtn
function DefaultState:OnMSGClickEndRoundBtn(msg)
	EventBus:Brocast(MSGCampCtrlEndRequest:Build({Camp = BattleRuntimeData.CurCamp}))
	this.FSM:ChangeState(EManualState.NoneState)
end

--- MSGMouseHoverOutTank 事件处理
---@param msg MSGMouseHoverOutTank
function DefaultState.OnMSGMouseHoverOutTank(msg)
	ViewFacade.CloseNpcTankAttributePanel()
end

--- MSGMouseHoverTank 事件处理
---@param msg MSGMouseHoverTank
function DefaultState.OnMSGMouseHoverTank(msg)
	local tankData = TankModel.GetTankData(msg.TankId)
	if tankData.Camp == this.Camp then
		return
	end
	ViewFacade.OpenNpcTankAttributePanel(msg.TankId)
end

--- MSGReleaseSkillRequest 事件处理
---@param msg MSGReleaseSkillRequest
function DefaultState.OnMSGReleaseSkillRequest(msg)
	local skillId = msg.SkillId
	local skillData =SkillModel.GetSkillData(skillId)
	if skillData:Enable() then
		this.FSM:ChangeState(EManualState.EnsureSkillState , {SkillId = skillId})
	end
end

-------------------------------------------
return DefaultState