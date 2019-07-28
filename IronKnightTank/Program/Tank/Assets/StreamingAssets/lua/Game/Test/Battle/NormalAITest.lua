---------------------------------------------
--- NormalAITest
--- Created by thrt520.
--- DateTime: 2018/8/16
---------------------------------------------
local NormalAITest = {}

local TankModel = require("Game.Battle.Model.TankModel")
local NormalAIClient = require("Game.Battle.AI.AIClient")
function NormalAITest.Test()
	local tankData =  TankModel.GetCampTanks(BattleConfig.NpcCamp)
	for i, v in pairs(tankData) do
		local ai = NormalAIClient.new(v)
		ai:MakeDecision()
		return
	end
end

return NormalAITest