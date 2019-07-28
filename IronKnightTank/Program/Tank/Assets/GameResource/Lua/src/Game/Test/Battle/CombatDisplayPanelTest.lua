---------------------------------------------
--- CombatDisplayPanelTest
--- Created by thrt520.
--- DateTime: 2018/6/28
---------------------------------------------

local CombatDisplayPanelTest = {}

local TankModel  =require("Game.Battle.Model.TankModel")
--local HitCalCultor =  require("Game.Battle.Logic.HitCalCultor")

function CombatDisplayPanelTest.TestSlider()
	--local combatDisplayPanelParam = {
	--		atkTank = TankModel.GetTankData(1),
	--		defTank = tank,
	--		AimCofficient = 0.3,
	--	}
	--	PanelManager.Open(PanelManager.PanelEnum.CombatDisplayPanel , combatDisplayPanelParam)
	--coroutine.createAndRun(function ()
	--	local defTank = TankModel.GetAllCampTankData(true)
	--	local tank
	--	for i, v in pairs(defTank) do
	--		tank = v
	--	end
	--	local combatDisplayPanelParam = {
	--		atkTank = TankModel.GetTankData(1),
	--		defTank = tank,
	--		AimCofficient = 0.3,
	--	}
	--	PanelManager.OpenYield(PanelManager.PanelEnum.CombatDisplayPanel , combatDisplayPanelParam)
	--	delayFrame(function ()
	--		local MSGStartLoopSlider = require("Game.Event.Message.Battle.MSGStartLoopSlider")
	--		EventBus:Brocast(MSGStartLoopSlider:Build({Time = 1000 , Speed = 3}))
	--		HitCalCultor.Init(1000 , 3  , CombatDisplayPanelTest.ShowVal )
	--		local MSGShootTankRequest  = require("Game.Event.Message.Battle.MSGShootTankRequest")
	--		EventBus:AddListener(MSGShootTankRequest , function ()
	--			HitCalCultor.End()
	--		end)
	--	end ,1 )
	--	coroutine.wait(0.3333)
	--	local MSGShootTankRequest = require("Game.Event.Message.Battle.MSGShootTankRequest")
	--	EventBus:Brocast(MSGShootTankRequest.new())
	--	coroutine.wait(0.3333)
	--	EventBus:Brocast(MSGShootTankRequest.new())
	--	coroutine.wait(0.3333)
	--	EventBus:Brocast(MSGShootTankRequest.new())
	--	coroutine.wait(0.3333)
	--	EventBus:Brocast(MSGShootTankRequest.new())
	--	coroutine.wait(0.3333)
	--	EventBus:Brocast(MSGShootTankRequest.new())
	--end)
end

function CombatDisplayPanelTest.TestShow()
	local combatDisplayPanelParam = {
		--atkTank = TankModel.GetFirstNPCTank(),
		--defTank = TankModel.GetFirstPlayerTank(),
		AimCofficient = 0.3,
	}
	PanelManager.OpenYield(PanelManager.PanelEnum.CombatDisplayPanel , combatDisplayPanelParam)
end


function CombatDisplayPanelTest.ShowVal(val)
	--clog("  tank val"..tostring(val))
	--local MSGEndLoopSlider =require("Game.Event.Message.Battle.MSGEndLoopSlider")
	--EventBus:Brocast(MSGEndLoopSlider.new())
end

return CombatDisplayPanelTest