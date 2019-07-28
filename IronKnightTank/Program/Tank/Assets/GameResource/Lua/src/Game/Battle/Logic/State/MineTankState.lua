---------------------------------------------
--- MineTankState
--- 选取我方坦克状态
--- Created by thrt520.
--- DateTime: 2018/7/24
---------------------------------------------
---@class MineTankState : IState
local MineTankState = {}
local this = MineTankState
MineTankState.StateEnum = EManualState.MineTankState
---@type FSM
MineTankState.FSM = nil
---@type number 當前坦克ID
MineTankState.curTankId = 0
---@type BaseFightTank 當前坦克數據
MineTankState.curTankData = 0
---@type ECamp 当前阵营
MineTankState.Camp = nil
MineTankState.curShowMoveAreaCost = 0

local ViewFacade = require("Game.Battle.View.ViewFacade")
local BattleRuntimeData =  require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local MapModel = require("Game.Battle.Model.MapModel")
local TankModel = require("Game.Battle.Model.TankModel")

local MSGUpdateManualButtonEnable = require("Game.Event.Message.Battle.MSGUpdateManualButtonEnable")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickTank"),
	require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
	require("Game.Event.Message.Battle.MSGClickBlock"),
	require("Game.Event.Message.Battle.MSGClickAtkButton"),
	require("Game.Event.Message.Battle.MSGClickFocusAtkBtn"),
	require("Game.Event.Message.Battle.MSGClickSkipButton"),
	require("Game.Event.Message.Battle.MSGReleaseSkillRequest"),
	require("Game.Event.Message.Battle.MSGClickCancelButton"),
}

function MineTankState.OnEnter(param)
	this.curTankId = param.TankId
	this.curTankData = TankModel.GetTankData(this.curTankId)
	ViewFacade.UpdateCtrlState(this.StateEnum)
	ViewFacade.OpenTankAttributePanel(param.TankId)
	ViewFacade.CamFocusTank(this.curTankId , 0.3 )
	this.UpdateManualBtnEnable()
	ViewFacade.ShowTankSignView(this.curTankId , true)
	MapModel.UpdateMapPathData(this.curTankData)
	if this.curTankData:CanMove() then
		this.ShowTankMoveableArea(this.curTankData:GetPower())
	end
	this._registerCommandHandler()
end

function MineTankState.OnExit()
	this.curShowMoveAreaCost = 0
	ViewFacade.CloseTankSingView()
	ViewFacade.CloseTankAttributePanel()
	ViewFacade.CancelShowTankMoveableArea()
	this._unregisterCommandHandler()
end

function MineTankState._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,MineTankState)
end

function MineTankState._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,MineTankState)
end

-------------------------------------------
---event handler
-------------------------------------------
--- MSGClickTank 事件处理
---@param msg MSGClickTank
function MineTankState.OnMSGClickTank(msg)
	if msg.TankId == this.curTankId then
		return
	end
	if msg.Camp == BattleRuntimeData.CurCamp then
		this.FSM:ChangeState(EManualState.MineTankState, { TankId = msg.TankId })
	else
		this.FSM:ChangeState(EManualState.EnemyTankState, { TankId = msg.TankId })
	end
end

--- MSGClickRightMouseBtn 事件处理
---@param msg MSGClickRightMouseBtn
function MineTankState.OnMSGClickRightMouseBtn(msg)
	this.FSM:ChangeState(EManualState.DefaultState)
end

--- MSGClickBlock 事件处理
---@param msg MSGClickBlock
function MineTankState.OnMSGClickBlock(msg)
	--MapModel.CancelHighLight("SkillReleaseArea")
	--local paht = MapModel.TempGetPath(msg.gridPos)
	--for i, v in ipairs(paht) do
	--	local block = MapModel.GetBlockData(v)
	--	block.SkillReleaseArea = true
	--end
	--ViewFacade.UpdateBlocks()
	if this.curTankData:CanMove() then
		local pos = msg.gridPos
		local blockData = MapModel.GetBlockData(pos)
		if blockData.IsMoveableArea then
			this.FSM:ChangeState(EManualState.TankMoveState , {TankId = this.curTankId , TargetPos = pos})
		end
	end
end

--- MSGClickAtkButton 事件处理
---@param msg MSGClickAtkButton
function MineTankState.OnMSGClickAtkButton(msg)
	this.FSM:ChangeState(EManualState.EnsureAtkState , {TankId = this.curTankId , IsFocus = false})
end

--- MSGClickFocusAtkBtn 事件处理
---@param msg MSGClickFocusAtkBtn
function MineTankState.OnMSGClickFocusAtkBtn(msg)
	this.FSM:ChangeState(EManualState.EnsureAtkState , {TankId = this.curTankId , IsFocus = true})
end


--- MSGClickSkipButton 事件处理
---@param msg MSGClickSkipButton
function MineTankState.OnMSGClickSkipButton(msg)
	local tankData = TankModel.GetTankData(this.curTankId)
	tankData:CostAllPower()
	this.FSM:ChangeState(EManualState.EnsureDirState , {TankId = this.curTankId})
end

--- MSGMouseEnterBlock 事件处理
---@param msg MSGMouseEnterBlock
function MineTankState.OnMSGMouseEnterBlock(msg)
	if not this.curTankData:CanMove() then
		return
	end
	local blockData = MapModel.GetBlockData(msg.GridPos)
	if blockData.CostPower <=0 then
		return
	end
	this.ShowTankMoveableArea(blockData.CostPower)
end

--- MSGReleaseSkillRequest 事件处理
---@param msg MSGReleaseSkillRequest
function MineTankState.OnMSGReleaseSkillRequest(msg)
	local skillId = msg.SkillId
	this.FSM:ChangeState(EManualState.EnsureSkillState , {SkillId = skillId})
end

--- MSGClickCancelButton 事件处理
---@param msg MSGClickCancelButton
function MineTankState.OnMSGClickCancelButton(msg)
	this.FSM:ChangeState(EManualState.DefaultState)
end
-------------------------------------------

----显示可移动区域
function MineTankState.ShowTankMoveableArea(power)
	if power ~= this.curShowMoveAreaCost and this.curTankData:GetPower() >= power then
		MapModel.HighLightCanMoveArea(power)
		ViewFacade.UpdateBlocks()
		this.curShowMoveAreaCost = power
	end
end


----更新按钮状态
function MineTankState.UpdateManualBtnEnable()
	local tankData = this.curTankData
	local moveButtonVisibale = tankData.TankTypeFeatures.Move ~= nil
	local atkButtonVisibale = tankData.TankTypeFeatures.Atk ~= nil
	local focusAtkButtonVisiable = tankData.TankTypeFeatures.FocusAtk ~= nil
	local moveButtonEnable , atkButtonEnable , focusAtkButtonEnable
	local curPower = tankData:GetPower()
	local canAtk = (#TankModel.GetCanAttackTank(tankData.Id) > 0)

	if moveButtonVisibale then
		moveButtonEnable = curPower >= tankData.TankTypeFeatures.Move
	else
		moveButtonEnable = false
	end

	if atkButtonVisibale then
		atkButtonEnable = curPower >= tankData.TankTypeFeatures.Atk and canAtk
	else
		atkButtonEnable = false
	end

	if focusAtkButtonVisiable then
		focusAtkButtonEnable = curPower >= tankData.TankTypeFeatures.FocusAtk and canAtk
	else
		focusAtkButtonEnable = false
	end

	local  skipButtonEnable = tankData:GetPower() > 0
	EventBus:Brocast(MSGUpdateManualButtonEnable:Build({
		MoveButtonEnable = moveButtonEnable,
		AtkButtonEnable = atkButtonEnable,
		FocusAtkButtonEnable = focusAtkButtonEnable,
		MoveButtonVisiable = moveButtonVisibale,
		AtkButtonVisiable = atkButtonVisibale,
		FocusAtkButtonVisiable = focusAtkButtonVisiable,
		SkipButtonEnable = skipButtonEnable
	}))
end



return MineTankState