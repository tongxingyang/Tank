---------------------------------------------
--- FakeTankSkill
---伪装坦克技能
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
local FakeTankSkill = {}

local TankModel = require("Game.Battle.Model.TankModel")
local FakeTank = require("Game.Battle.Model.Data.Tank.FakeTank")
local ShadowModel = require("Game.Battle.Model.ShadowModel")

local MSGCreateTank = require("Game.Event.Message.Battle.MSGCreateTank")

function FakeTankSkill.Play(skillScript , param)
	local camp = param.Camp
	local targetPos = param.TargetPos
	local tankDic = TankModel.GetCampTanks(camp)
	local tankList = {}
	for i, v in pairs(tankDic) do
		table.insert(tankList , v)
	end
	local tank = tankList[math.random(#tankList)]
	local fakeTank = FakeTank.new()
	fakeTank:Init(tank)
	if param.Toward then
		fakeTank.Toward = param.Toward
	end
	TankModel.AddTank(fakeTank)
	TankModel.ArrangeTank(fakeTank.Id , targetPos)
	ShadowModel.UpdateFOW()
	EventBus:Brocast(MSGCreateTank:Build({BaseFightTankDataList ={fakeTank}}))
end


return FakeTankSkill