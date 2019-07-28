---------------------------------------------
--- EnsureDirState
---确认方向状态
--- Created by thrt520.
--- DateTime: 2018/6/21
---------------------------------------------
---@class EnsureDirState : IState
local EnsureDirState = {}
local this = EnsureDirState

---@type EManualState
EnsureDirState.StateEnum = EManualState.EnsureDirState
---@type FSM
EnsureDirState.FSM = nil
---@type number
EnsureDirState.curTankId = nil

local TankModel = require("Game.Battle.Model.TankModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local MapConfig = require("Game.Battle.Config.MapConfig")

local MSGShowTankChoseDirView = require("Game.Event.Message.Battle.MSGShowTankChoseDirView")
local MSGCloseTankChoseDirView = require("Game.Event.Message.Battle.MSGCloseTankChoseDirView")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickTankChoseDir"),
}

function EnsureDirState.OnEnter(param)
	this._registerCommandHandler()
	this.curTankId = param.TankId
	ViewFacade.UpdateCtrlState(this.StateEnum)
	this.ShowTankChoseDirView(this.curTankId)
end

function EnsureDirState.OnExit()
	this.CancelShowTankChoseDirView()
	this._unregisterCommandHandler()
end

function EnsureDirState._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,EnsureDirState)
end

function EnsureDirState._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,EnsureDirState)
end

-------------------------------------------
---event handler
-------------------------------------------
---选择坦克方向
--- MSGClickTankChoseDir 事件处理
---@param msg MSGClickTankChoseDir
function EnsureDirState.OnMSGClickTankChoseDir(msg)
	TankModel.RotateTank(this.curTankId , msg.Toward)
	ViewFacade.UpdateSingleTank(this.curTankId)
	this.FSM:ChangeState(EManualState.DefaultState)
end
-------------------------------------------
---打开坦克方向选择view
function EnsureDirState.ShowTankChoseDirView(tankId)
	local tankData = TankModel.GetTankData(tankId)
	local pos = MapConfig.GetWorldPos(tankData.Pos)
	EventBus:Brocast(MSGShowTankChoseDirView:Build({WroldPos = pos}))
end

---关闭坦克方向选择view
function EnsureDirState.CancelShowTankChoseDirView()
	EventBus:Brocast(MSGCloseTankChoseDirView.new())
end

return EnsureDirState