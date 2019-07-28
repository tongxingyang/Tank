---------------------------------------------
--- Level1004
--- Created by jie
--- DateTime: 2018/8/29
---------------------------------------------
---@class Level1004 : BaseLevelScript
local Level1004 = {}
local LevelTools = require("Game.Battle.Level.LevelTools")
---坦克胜利条件描述,对应语言表ID
Level1004.SuccesCondition = 0
---坦克失败条件描述，对应语言表ID
Level1004.FailCondition = 0

---自定义变量----
local Tigger01 = 0
local Tigger02 = 0
local EnemyRoundNum_1 = 0 ---敌方回合数
local EnemyDeadNum_1 = 0  ---敌人死亡数量
local PlayerDeadNum = 0  ---我方死亡数量

------阵营顺序
Level1004.Player = true


---技能
Level1004.SKills = {
						---{id = 1001 , times = 3} ,
						---{id = 1002 , times = 3} ,
						---{id = 1003 , times = 3} ,
						---{id = 1004 , times = 3} ,
						---{id = 1005 , times = 3} ,
						---{id = 1006 , times = 3} ,
						---{id = 1007 , times = 3} ,
						---{id = 1008 , times = 3} ,
					}

---战斗开始前回调
Level1004.BeforeLevelStart = function()
	clog("Level1004.BeforeLevelStart")
end

---阵营回合结束时回调
Level1004.OnCampRoundEnd = function(isPlayer)

	if  isPlayer == false then

	EnemyRoundNum_1 = EnemyRoundNum_1 + 1
		clog("Level1004.EnemyRoundNum_1"..tostring(EnemyRoundNum_1))
	end

	if  isPlayer == false and EnemyRoundNum_1 == 5 and Tigger01 ==  0 then
		Tigger01 = 1
		---创建坦克(npcId , x , y , toward)
			clog("援军")
		LevelTools.CreatTank(10011,10,22,EToward.Down)
		LevelTools.CreatTank(10012,9,21,EToward.Down)
		LevelTools.CreatTank(10013,11,21,EToward.Down)
		LevelTools.CreatTank(10013,12,20,EToward.Down)
		LevelTools.CreatTank(10011,10,21,EToward.Down)

		Tigger02 = 1

		Level1004.IsPlayerOnHere()
	end

end

---回合开始回调
Level1004.OnRoundStart = function(roundNumber)
	clog("Level001.OnRoundStart  "..tostring(roundNumber))
	if  roundNumber == 15 then
		---clog("援军")
		LevelTools.Fail()
	end
end

---坦克摧毁回调
Level1004.OnTankDestory = function(isPlayer , soliderId , tankId)
	clog("Level1004.OnTankDestory")
	----统计敌方死亡数字
	if isPlayer == false then
		EnemyDeadNum_1 = EnemyDeadNum_1 + 1
		clog("Level001.EnemyDeadNum_1"..tostring(EnemyDeadNum_1))
	else
	    PlayerDeadNum = PlayerDeadNum + 1
		clog("Level001.PlayerDeadNum"..tostring(PlayerDeadNum))
	end

	----敌方死亡了4辆并且还没有到指定回合数触发援军。强制触发
	if isPlayer == false and EnemyDeadNum_1 == 4 and Tigger01 == 0 then
		Tigger01 = 2
		clog("Level1004.EnemyRoundNum_1"..tostring(EnemyDeadNum_1))
		LevelTools.CreatTank(10011,10,22,EToward.Down)
		LevelTools.CreatTank(10012,9,21,EToward.Down)
		LevelTools.CreatTank(10013,11,21,EToward.Down)
		LevelTools.CreatTank(10013,12,20,EToward.Down)
		LevelTools.CreatTank(10011,10,21,EToward.Down)
		Tigger02 = 1
		Level1004.IsPlayerOnHere()
	end

	------敌方全部死亡，胜利
	if  isPlayer == false and EnemyDeadNum_1 >= 13 then
		clog("胜利")
		LevelTools.Success()
	end

	-------我方全部死亡，失败
	if  isPlayer == true and PlayerDeadNum >= 6 then
		clog("失败")
		LevelTools.Fail()
	end

end

---坦克进入指定地点回调
Level1004.OnTankEnterPos = function(x, y, soliderId , tankId , isPlayer)
	clog("Level1004.OnTankEnterPos")
        if Tigger02 == 1  and x == 9 and y == 0 and isPlayer == true then
        
                clog("胜利")
		LevelTools.Success()
        end
        
end

---释放技能回调
Level1004.OnReleaseSkill = function(skillId , isPlayer)
	clog("Level1004.OnReleaseSkill")
end

---自定义函数，，检测条件改变时是否有我方坦克已经在指定位置上
Level1004.IsPlayerOnHere = function()
	local PlayerInTarget = 	LevelTools.IsTankInTargetPos(10,0,true)

	if PlayerInTarget == true then
		LevelTools.Success()
	end

end


return Level1004