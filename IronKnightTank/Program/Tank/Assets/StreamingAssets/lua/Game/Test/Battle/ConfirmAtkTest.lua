---------------------------------------------
--- ConfirmAtkTest
--- Created by thrt520.
--- DateTime: 2018/8/1
---------------------------------------------
local ConfirmAtkTest = {}

local TankModel  = require("Game.Battle.Model.TankModel")

ConfirmAtkTest.Test = function()
	local atkTank = TankModel.GetCampTanks(BattleConfig.PlayerCamp)
	local atk
	for i, v in pairs(atkTank) do
		atk = v
		break
	end
	local defTank = TankModel.GetCampTanks(BattleConfig.NpcCamp)
	local def
	for i, v in pairs(defTank) do
		def = v
		break
	end
	clog("def id"..tostring(def.Id))
	local EnsureAtkState = require("Game.Battle.Logic.State.EnsureAtkState")
	--EnsureAtkState.OpenConfirmAtkPanel(atk.Id , def.Id , false)
end



return ConfirmAtkTest