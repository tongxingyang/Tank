---------------------------------------------
--- AttackActionTest
--- Created by thrt520.
--- DateTime: 2018/6/19
---------------------------------------------

local AttackActionTest = {}
local TankModel = require("Game.Battle.Model.TankModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local AttackAction = require("Game.Battle.Action.AttackAction")

function AttackActionTest.Test()
	--
	local atkTankList = TankModel.GetCampTanks(BattleConfig.PlayerCamp)
	local defTankList = TankModel.GetCampTanks(BattleConfig.NpcCamp)
	local atkTank  , defTank
	for i, v in pairs(atkTankList) do
		atkTank = v
	end
	for i, v in pairs(defTankList) do
		defTank = v
	end
	defTank.Visiable = true
	defTank.Toward = EToward.Down
	atkTank.Pos = GridPos.New(0 , 3 )
	defTank.Pos = GridPos.New( 3,  0)
	ViewFacade.UpdateAllTank()
	local action = AttackAction.new(   atkTank.Id , defTank.Id , true)
	action:Action()
	clog(" att ack  finish")
end


function AttackActionTest.TestCamera()

	local MSGOpenAimTankCam  = require("Game.Event.Message.Battle.MSGOpenAimTankCam")
	local atkTankList = TankModel.GetCampTanks(BattleConfig.PlayerCamp)
	local defTankList = TankModel.GetCampTanks(BattleConfig.NpcCamp)
	local atkTank  , defTank

	for i, v in pairs(atkTankList) do
		atkTank = v
	end

	for i, v in pairs(defTankList) do
		defTank = v
	end

	defTank.Toward = EToward.Down
	defTank.Visiable = true

	atkTank.Pos = GridPos.New(0 , 0 )
	atkTank.Visiable =  true
	defTank.Pos = GridPos.New( 3,  3)
	ViewFacade.UpdateAllTank()
	coroutine.wait(1)
	local vec =  defTank.Pos - atkTank.Pos

	local angle3 = GridPos.GetAngleBetweenTwoVec(vec , EToward.GetVector(defTank.Toward) , true)
	local angle2 = Vector3.New(0 , angle3 , 0)
	clog(" angle" .. tostring(angle2))
	EventBus:Brocast(MSGOpenAimTankCam:Build({TankId = defTank.Id  , IsPlayer = defTank.IsPlayer, TankAngles = angle2}))

end

return AttackActionTest