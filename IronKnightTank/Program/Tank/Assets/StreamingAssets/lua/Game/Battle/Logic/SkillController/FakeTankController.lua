---------------------------------------------
--- FakeTankController
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
---@class FakeTankController
local FakeTankController = {}
local this = FakeTankController
FakeTankController.realeaseCall = nil
FakeTankController.disposeCall = nil
FakeTankController.skillScript = nil
FakeTankController.area = 0

local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local TankModel = require("Game.Battle.Model.TankModel")
local MapModel =  require("Game.Battle.Model.MapModel")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickBlock"),
	require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
}


function FakeTankController.Init(skillScript)
	this.skillScript = skillScript
	this._registerCommandHandler()
	this.area = this.skillScript.Area
	this._highLightSkillTargetArea()
end

function FakeTankController.Dispose()
	this._cancelHighLighSkillTargetArea()
	this._unregisterCommandHandler()
end


function FakeTankController._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,FakeTankController)
end

function FakeTankController._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,FakeTankController)
end
-----------------------------------------
---event handler
-----------------------------------------
----@param msg MSGClickBlock
function FakeTankController.OnMSGClickBlock(msg)
	local blockData =MapModel.GetBlockData(msg.gridPos)
	if blockData.SkillReleaseArea then
		GameTools.Alert("是否释放技能" , function (yes)
			if yes then
				this.realeaseCall(this.skillScript.SkillId ,{TargetPos = msg.gridPos , Camp = BattleRuntimeData.CurCamp} )
			end
		end)

	end
end


---MSGClickRightMouseBtn
----@param msg MSGClickRightMouseBtn
function FakeTankController.OnMSGClickRightMouseBtn(msg)
	if this.isEnsure then
		GameTools.CloseAlert()
		this.isEnsure = false
	else
		this.disposeCall()
	end
end
-----------------------------------------
function FakeTankController._highLightSkillTargetArea()
	local tanks = TankModel.GetCampTanks(BattleRuntimeData.CurCamp)
	local posList = {}
	for i, v in pairs(tanks) do
		if v.Type ~= EFightTankType.Fake then
			local quadPos  = v.Pos:GetQuadPos()
			for k, pos in pairs(quadPos) do
				--posList[pos] = 1
				local blockData = MapModel.GetBlockData(pos)
				if blockData and not blockData.IsHold then
					table.insert(posList , pos)
				end
			end
		end
	end
	MapModel.HighLightTargetArea("SkillReleaseArea"  , posList)
	--for i, v in pairs(posList) do
	--	local blockData = MapModel.GetBlockData(i)
	--	if blockData and not blockData.IsHold then
	--		blockData.SkillReleaseArea = true
	--	end
	--end
	ViewFacade.UpdateBlocks()
end

function FakeTankController._cancelHighLighSkillTargetArea()
	--MapModel.CancelSkillReleaseHighLight()
	MapModel.CancelHighLight("SkillReleaseArea")
	ViewFacade.UpdateBlocks()
end

return FakeTankController