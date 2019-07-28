---------------------------------------------
--- Level1001
--- Created by jie
--- DateTime: 2018/8/29
---------------------------------------------
---@class Level1001 : BaseLevelScript
local Level1001 = {}
local LevelTools = require("Game.Battle.Level.LevelTools")
---坦克胜利条件描述,对应语言表ID
Level1001.SuccesCondition = 104001
---坦克失败条件描述，对应语言表ID
Level1001.FailCondition = 0

---自定义变量----
local Tigger01 = 0
local EnemyRoundNum_1 = 0 ---敌方回合数
local EnemyDeadNum_1 = 0  ---敌人死亡数量
local PlayerDeadNum = 0  ---我方死亡数量

------阵营顺序
Level1001.Player = true


---技能
Level1001.SKills = {
						{id = 1001 , times = 2} ,
						{id = 1002 , times = 1} ,
						{id = 1003 , times = 3} ,
						{id = 1004 , times = 3} ,
						{id = 1005 , times = 3} ,
						{id = 1006 , times = 3} ,
						{id = 1007 , times = 3} ,
						{id = 1008 , times = 3} ,
					}

---战斗开始前回调
Level1001.BeforeLevelStart = function()
	clog("Level1001.BeforeLevelStart")
end

---阵营回合结束时回调
Level1001.OnCampRoundEnd = function(isPlayer)



	--LevelTools.Success()
	if  isPlayer == false then

	EnemyRoundNum_1 = EnemyRoundNum_1 + 1
		clog("Level1001.EnemyRoundNum_1"..tostring(EnemyRoundNum_1))
	end

	if  isPlayer == false and EnemyRoundNum_1 == 5 and Tigger01 ==  0 then
		Tigger01 = 1
		---创建坦克(npcId , x , y , toward)
			clog("援军")
			LevelTools.CreatTank(10011,19,14,EToward.LeftDown)
			LevelTools.CreatTank(10012,20,13,EToward.LeftDown)
			LevelTools.CreatTank(10013,21,14,EToward.LeftDown)
	end

end

---回合开始回调
Level1001.OnRoundStart = function(roundNumber)
	clog("Level001.OnRoundStart  "..tostring(roundNumber))
	if  roundNumber == 10 then
		---clog("援军")
		LevelTools.Fail()
	end
	--if roundNumber == 1 then
	--	LevelTools.TipsSkillEffectArea(5 , 5 , 2)
	--end
	--if roundNumber == 2  then
	--	LevelTools.CancelTipsSkillEffectArea( 5 , 5, 2)
	--end
end

---坦克摧毁回调
Level1001.OnTankDestory = function(isPlayer , soliderId , tankId)
	clog("Level1001.OnTankDestory")
	----统计敌方死亡数字
	if isPlayer == false then
		EnemyDeadNum_1 = EnemyDeadNum_1 + 1
		clog("Level001.EnemyDeadNum_1"..tostring(EnemyDeadNum_1))
	else
	    PlayerDeadNum = PlayerDeadNum + 1
		clog("Level001.PlayerDeadNum"..tostring(PlayerDeadNum))
	end

	----敌方死亡了5辆并且还没有到指定回合数触发援军。强制触发
	if isPlayer == false and EnemyDeadNum_1 == 5 and Tigger01 == 0 then
		Tigger01 = 2
		clog("Level1001.EnemyRoundNum_1"..tostring(EnemyDeadNum_1))
		LevelTools.CreatTank(10011,19,14,EToward.LeftDown)
		LevelTools.CreatTank(10012,20,13,EToward.LeftDown)
		LevelTools.CreatTank(10013,21,14,EToward.LeftDown)
	end

	------敌方全部死亡，胜利
	if  isPlayer == false and EnemyDeadNum_1 >= 8 then
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
Level1001.OnTankEnterPos = function(x, y, soliderId , tankId , isPlayer)
	clog("Level1001.OnTankEnterPos")
end

---释放技能回调
Level1001.OnReleaseSkill = function(skillId , isPlayer)
	clog("Level1001.OnReleaseSkill")
end

return Level1001