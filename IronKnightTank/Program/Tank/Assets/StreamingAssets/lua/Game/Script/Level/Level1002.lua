---------------------------------------------
--- Level1002
--- Created by jie
--- DateTime: 2018/8/29
---------------------------------------------
---@class Level1002 : BaseLevelScript
local Level1002 = {}
local LevelTools = require("Game.Battle.Level.LevelTools")
---坦克胜利条件描述,对应语言表ID
Level1002.SuccesCondition = 0
---坦克失败条件描述，对应语言表ID
Level1002.FailCondition = 0

---自定义变量----
local Tigger01 = 0
local EnemyRoundNum_1 = 0 ---敌方回合数
local EnemyDeadNum_1 = 0  ---敌人死亡数量
local PlayerDeadNum = 0  ---我方死亡数量

------阵营顺序
Level1002.Player = true


---技能
Level1002.SKills = {
						{id = 1002 , times = 1} ,
						{id = 1008 , times = 1}
					}

---战斗开始前回调
Level1002.BeforeLevelStart = function()
	clog("Level1002.BeforeLevelStart")
end

---阵营回合结束时回调
Level1002.OnCampRoundEnd = function(isPlayer)



end

---回合开始回调
Level1002.OnRoundStart = function(roundNumber)
	clog("Level001.OnRoundEnd")
	if  roundNumber == 15 then
		---clog("援军")
		LevelTools.Fail()
	end
end

---坦克摧毁回调
Level1002.OnTankDestory = function(isPlayer , soliderId , tankId)
	clog("Level1002.OnTankDestory")
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
Level1002.OnTankEnterPos = function(x, y , soliderId , tankId , isPlayer)
	clog("Level1002.OnTankEnterPos")
end

---释放技能回调
Level1002.OnReleaseSkill = function(skillId , isPlayer)
	clog("Level1002.OnReleaseSkill")
end

return Level1002