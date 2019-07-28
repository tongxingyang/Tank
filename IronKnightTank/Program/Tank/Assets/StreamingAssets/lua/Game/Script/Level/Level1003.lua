---------------------------------------------
--- Level1003
--- Created by jie
--- DateTime: 2018/8/29
---------------------------------------------
---@class Level1003 : BaseLevelScript
local Level1003 = {}
local LevelTools = require("Game.Battle.Level.LevelTools")
---坦克胜利条件描述,对应语言表ID
Level1003.SuccesCondition = 0
---坦克失败条件描述，对应语言表ID
Level1003.FailCondition = 0

---自定义变量----
local Tigger01 = 0
local EnemyRoundNum_1 = 0 ---敌方回合数
local PlyaerRoundNum_1 = 0 ---我方回合数
local EnemyDeadNum_1 = 0  ---敌人死亡数量
local PlayerDeadNum = 0  ---我方死亡数量

------阵营顺序
Level1003.Player = false


---技能
Level1003.SKills = {
						{id = 1001 , times = 2} ,
						---{id = 1002 , times = 2} ,
						---{id = 1008 , times = 2}
					}

---战斗开始前回调
Level1003.BeforeLevelStart = function()
	clog("Level1003.BeforeLevelStart")
end

---阵营回合结束时回调
Level1003.OnCampRoundEnd = function(isPlayer)
	if isPlayer == true then
		PlyaerRoundNum_1 = PlyaerRoundNum_1 + 1
	else
		EnemyRoundNum_1 = EnemyRoundNum_1 + 1
	end

	---if  EnemyRoundNum_1 == 3 then
		---LevelTools.TipsSkillEffectArea(9 , 7 , 2)
	---end

	----if PlyaerRoundNum_1 == 3 then
		---LevelTools.CancelTipsSkillEffectArea(9 , 7 , 2)
		---LevelTools.ReleaseDmgSkill(1002, 9 , 7 )

	---end
	
end

---回合开始回调
Level1003.OnRoundStart = function(roundNumber)
	clog("Level001.OnRoundEnd")
	---if roundNumber == 1 then
		---LevelTools.TipsSkillEffectArea(10 , 11 , 3)
	---end
	---if roundNumber == 2 then
		---LevelTools.CancelTipsSkillEffectArea(10 , 11 , 3)
		---LevelTools.ReleaseDmgSkill(1002, 10 , 11 )

	---end
	if  roundNumber == 15 then
		---clog("援军")
		LevelTools.Fail()
	end
end

---坦克摧毁回调
Level1003.OnTankDestory = function(isPlayer , soliderId , tankId)
	clog("Level1003.OnTankDestory")
	----统计敌方死亡数字
	if isPlayer == false then
		EnemyDeadNum_1 = EnemyDeadNum_1 + 1
		clog("Level001.EnemyDeadNum_1"..tostring(EnemyDeadNum_1))
	else
	    PlayerDeadNum = PlayerDeadNum + 1
		clog("Level001.PlayerDeadNum"..tostring(PlayerDeadNum))
	end


	------敌方全部死亡，胜利
	if  isPlayer == false and EnemyDeadNum_1 >= 7 then
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
Level1003.OnTankEnterPos = function(x, y, soliderId , tankId , isPlayer)
	clog("Level1003.OnTankEnterPos")
end

---释放技能回调
Level1003.OnReleaseSkill = function(skillId , isPlayer)
	clog("Level1003.OnReleaseSkill")
end

return Level1003