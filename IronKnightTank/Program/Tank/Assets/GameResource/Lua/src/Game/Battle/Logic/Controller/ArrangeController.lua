---------------------------------------------
--- ArrangeController
---布置阶段控制器
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class ArrangeController :BaseStageController
local ArrangeController = class("ArrangeController" )
local this = ArrangeController

----当前选中tankId
this._curTankId = nil

local TankModel = require("Game.Battle.Model.TankModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local MapModel = require("Game.Battle.Model.MapModel")
local ShadowModel = require("Game.Battle.Model.ShadowModel")


local receiveCommand = {
	require("Game.Event.Message.Battle.MSGClickTankPropretyView"),
	require("Game.Event.Message.Battle.MSGClickTank"),
	require("Game.Event.Message.Battle.MSGEndArrange"),
	require("Game.Event.Message.Battle.MSGClickBlock"),
	require("Game.Event.Message.Battle.MSGQuickArrangeTank"),
}

local MSGStartCombat = require("Game.Event.Message.Battle.MSGStartCombat")

function ArrangeController.OnEnterStage()
	this._registerCommandHandler()
	this._updateArrangePanel()
	this._highLightBornBlock()
end

function ArrangeController.OnLeaveStage()
	ViewFacade.CancelHighLightBornBlock()
	this._unregisterCommandHandler()
end

function ArrangeController._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommand,ArrangeController)
end

function ArrangeController._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommand,ArrangeController)
end

function ArrangeController.Dispose()

end
--------------------------------
---even handler
--------------------------------
---@param msg MSGEndArrange
function ArrangeController.OnMSGEndArrange(msg)
	ViewFacade.CloseTankAttributePanel()
	EventBus:Brocast(MSGStartCombat.new())
end

---@param msg MSGClickTankPropretyView
function ArrangeController.OnMSGClickTankPropretyView(msg)
	this._curTankId = msg.TankId
	---@type PlayerFightTank
	local tankData = TankModel.ChosePlayerTank(this._curTankId)
	ViewFacade.UpdateAllPlayerTank()
	if tankData.IsChose then
		ViewFacade.OpenTankAttributePanel(this._curTankId , true)
	else
		ViewFacade.CloseTankAttributePanel()
	end
end

---@param msg MSGClickBlock
function ArrangeController.OnMSGClickBlock(msg)
	if not this._curTankId then
		return
	end
	local blockData = MapModel.GetBlockData(msg.gridPos)
	if blockData.IsHold or not blockData.IsBornBlock then
		return
	end
	local tankData = TankModel.GetTankData(this._curTankId)
	TankModel.ArrangeTank(this._curTankId , msg.gridPos )
	tankData.Visiable = true
	tankData.IsChose = false
	ViewFacade.CloseTankAttributePanel()
	ViewFacade.UpdateAllPlayerTank()
	this._curTankId = nil
end

---@param msg MSGQuickArrangeTank
function ArrangeController.OnMSGQuickArrangeTank(msg)
	this._quickArrangeTank()
	ViewFacade.UpdateAllPlayerTank()
end

---@param msg MSGClickTank
function ArrangeController.OnMSGClickTank(msg)
	TankModel.CancelChosePlayerTank(msg.TankId)
	TankModel.DisArrangeTank(msg.TankId)
	ViewFacade.UpdateAllPlayerTank()
	PanelManager.Close(PanelManager.PanelEnum.TankAttributePanel)
end

function ArrangeController._updateArrangePanel()
	PanelManager.Open(PanelManager.PanelEnum.ArrangeTankPanel , {FightTankData = TankModel.GetCampTanks(BattleConfig.PlayerCamp)})
end

function ArrangeController._closeArrangePanel()
	PanelManager.Close(PanelManager.PanelEnum.ArrangeTankPanel)
end

function ArrangeController._highLightBornBlock()
	local gridList = {}
	for i, v in pairs(MapModel.GetAllBlockData()) do
		if v.IsBornBlock then
			table.insert(gridList , v.Pos)
		end
	end
	ShadowModel.LightTargetPosList(gridList)
	ViewFacade.UpdateShadowView()
	ViewFacade.HighLightBornBlock()
end

function ArrangeController._quickArrangeTank()
	local tankList = {}
	local playerTanks = TankModel.GetCampTanks(BattleConfig.PlayerCamp)
	for i, v in pairs(playerTanks) do
		if not v.IsArrange then
			table.insert(tankList, v)
		end
	end
	local avaliableBlock = this._getNotChosedBornBlock()
	for i, v in ipairs(tankList) do
		if avaliableBlock[i] then
			TankModel.ArrangeTank(v.Id , avaliableBlock[i].Pos )
		end
	end
end


---获取未选择的出生点
---@return BlockData[]
function ArrangeController._getNotChosedBornBlock()
	local blockList = {}
	for i, v in pairs(MapModel.GetAllBlockData()) do
		if v.IsBornBlock and not v.IsHold then
			table.insert(blockList, v)
		end
	end
	return blockList
end

return ArrangeController