---------------------------------------------
--- TempBunkerController
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
---@class TempBunkerController
local TempBunkerController = {}
local this = TempBunkerController
TempBunkerController.realeaseCall = nil
TempBunkerController.disposeCall = nil
TempBunkerController.skillScript = nil
TempBunkerController.area = 0

local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local TankModel = require("Game.Battle.Model.TankModel")
local MapModel =  require("Game.Battle.Model.MapModel")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickBlock"),
	require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
}

function TempBunkerController.Init(skillScript)
	this.skillScript = skillScript
	this.area = this.skillScript.Area
	this._registerCommandHandler()
	this._highLightSkillTargetArea()
	ViewFacade.CloseTankCollider()
end

function TempBunkerController.Dispose()
	ViewFacade.OpenTankCollider()
	this._cancelHighLightSkillTargetArea()
	this._unregisterCommandHandler()
end

function TempBunkerController._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,TempBunkerController)
end

function TempBunkerController._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,TempBunkerController)
end
-----------------------------------------
---event handler
-----------------------------------------
---@param msg MSGClickBlock
function TempBunkerController.OnMSGClickBlock(msg)
	local blockData = MapModel.GetBlockData(msg.gridPos)
	if blockData.SkillReleaseArea then
		GameTools.Alert("是否释放技能" , function (yes)
			if yes then
				this.realeaseCall(this.skillScript.SkillId , {TargetPos = msg.gridPos })
			end
		end)

	end
end


---MSGClickRightMouseBtn
----@param msg MSGClickRightMouseBtn
function TempBunkerController.OnMSGClickRightMouseBtn(msg)
	if this.isEnsure then
		GameTools.CloseAlert()
	else
		this.disposeCall()
	end
end
-----------------------------------------
function TempBunkerController._highLightSkillTargetArea()
	local tankList = TankModel.GetCampTanks(BattleRuntimeData.CurCamp)
	local posList = {}
	for i, v in pairs(tankList) do
		table.insert(posList , v.Pos)
	end
	--MapModel.HighLightDesignatedSkillReleaseArea(posList)

	MapModel.HighLightTargetArea("SkillReleaseArea" , posList)
	ViewFacade.UpdateBlocks()
end

function TempBunkerController._cancelHighLightSkillTargetArea()
	MapModel.CancelHighLight("SkillReleaseArea")
	--MapModel.CancelSkillReleaseHighLight()
	ViewFacade.UpdateBlocks()
end

return TempBunkerController