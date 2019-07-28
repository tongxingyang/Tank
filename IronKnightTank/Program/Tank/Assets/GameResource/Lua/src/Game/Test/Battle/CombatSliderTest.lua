---------------------------------------------
--- CombatSliderTest
--- Created by thrt520.
--- DateTime: 2018/8/17
---------------------------------------------
local CombatSliderTest = {}
local TankModel = require("Game.Battle.Model.TankModel")
local MSGDisplayFight = require("Game.Event.Message.Battle.MSGDisplayFight")


function CombatSliderTest.Test()
	coroutine.createAndRun(function ()

		local atkTankList = TankModel.GetCampTanks(BattleConfig.PlayerCamp)
		local defTankList = TankModel.GetCampTanks(BattleConfig.NpcCamp)
		local atkTank  , defTank
		for i, v in pairs(atkTankList) do
			atkTank = v
		end
		for i, v in pairs(defTankList) do
			defTank = v
		end

		PanelManager.OpenYield(PanelManager.PanelEnum.CombatDisplayPanel , {atkTank = atkTank, defTank = defTank })
		EventBus:Brocast(MSGDisplayFight:Build({AtkSliderValue = 0.5 , DefSliderValue = 0 , Des = "Test" })):Yield()
		coroutine.wait(1)
		EventBus:Brocast(MSGDisplayFight:Build({AtkSliderValue = -1 , DefSliderValue = 0.5 , Des = "Test" })):Yield()
		coroutine.wait(1)
		EventBus:Brocast(MSGDisplayFight:Build({AtkSliderValue = 1 , DefSliderValue = 1 , Des = "Test" })):Yield()
	end)
end

return CombatSliderTest