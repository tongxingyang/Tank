---------------------------------------------
--- RepairSkill
---修复坦克技能
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
local RepairSkill = {}

local TankModel = require("Game.Battle.Model.TankModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")
function RepairSkill.Play(skillScript , param)
	local tankId = param.TankId
	local TankData = TankModel.GetTankData(tankId)
	TankData.TurretHurtRound = 0
	TankData.BodytHurtRound = 0
	ViewFacade.UpdateSingleTank(tankId)
end

return RepairSkill