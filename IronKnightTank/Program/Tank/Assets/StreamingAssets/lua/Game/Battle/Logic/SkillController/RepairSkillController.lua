---------------------------------------------
--- RepairSkillController
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
---@class RepairSkillController
local RepairSkillController = {}


local this = RepairSkillController
RepairSkillController.callback = nil
RepairSkillController.skillScript = nil
RepairSkillController.area = 0
RepairSkillController.realeaseCall = nil
RepairSkillController.disposeCall = nil

local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local TankModel = require("Game.Battle.Model.TankModel")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickTank"),
	require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
}


function RepairSkillController.Init(skillScript)
	this.skillScript = skillScript
	this._registerCommandHandler()
	this._highLightSkillTarget()
end

function RepairSkillController.Dispose()
	this._cancelHighLightSkillTarget()
	this._unregisterCommandHandler()
end


function RepairSkillController._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,RepairSkillController)
end

function RepairSkillController._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,RepairSkillController)
end

-----------------------------------------
---event handler
-----------------------------------------
---@param msg MSGClickTank
function RepairSkillController.OnMSGClickTank(msg)
	local tankData = TankModel.GetTankData(msg.TankId)
	if tankData.IsSkillTarget then
		GameTools.Alert("是否释放技能" , function (yes)
			if yes then
				this.realeaseCall(this.skillScript.SkillId ,{TankId = msg.TankId , Camp = BattleRuntimeData.CurCamp}  )
			end
		end)
	end
end

---MSGClickRightMouseBtn
----@param msg MSGClickRightMouseBtn
function RepairSkillController.OnMSGClickRightMouseBtn(msg)
	if this.isEnsure then
		GameTools.CloseAlert()
		this.isEnsure = false
	else
		this.disposeCall()
	end
end

-----------------------------------------
function RepairSkillController._highLightSkillTarget()
	local tankList = TankModel.GetCampTanks(BattleRuntimeData.CurCamp)
	for i, v in pairs(tankList) do
		if v:IsBreak() then
			v.IsSkillTarget = true
		end
	end
	ViewFacade.UpdateCampTank(BattleRuntimeData.CurCamp)
end

function RepairSkillController._cancelHighLightSkillTarget()
	local tankList = TankModel.GetCampTanks(BattleRuntimeData.CurCamp)
	for i, v in pairs(tankList) do
		v.IsSkillTarget = false
	end
	ViewFacade.UpdateCampTank(BattleRuntimeData.CurCamp)
end

return RepairSkillController