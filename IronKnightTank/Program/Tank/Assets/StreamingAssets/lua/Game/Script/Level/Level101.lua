---------------------------------------------
--- Level101
--- Created by thrt520.
--- DateTime: 2018/8/28
---------------------------------------------
---@class Level101 : BaseLevelScript
local Level101 = {}
local LevelTools = require("Game.Battle.Level.LevelTools")
---坦克胜利条件描述
Level101.SuccesCondition = 0
---坦克失败条件描述
Level101.FailCondition = 0
---阵营顺序
Level101.Player = true
---技能
Level101.SKills = {
	{id = 1001 , times = 3} ,
	{id = 1002 , times = 3} ,
	{id = 1003 , times = 3} ,
	{id = 1005 , times = 3} ,
	{id = 1006 , times = 3} ,
	{id = 1007 , times = 3} ,
	{id = 1008 , times = 3} ,
}

---战斗开始前回调
Level101.BeforeLevelStart = function()
	--clog("Level101.BeforeLevelStart")
	clog("Level1001.BeforeLevelStart          11111111")
end

Level101.OnCampRoundEnd = function(isPlayer)
	--clog(" isplayert "..tostring(isPlayer))
	if isPlayer then
		LevelTools.TipsSkillEffectArea(1 , 1 , 1, )
		LevelTools.ReleaseDmgSkill(1001 , 1 , 1 )
		LevelTools.FakeTank(1001 , 1 , 1  , EToward.Down,  true )
	end
end

---回合开始回调
Level101.OnRoundStart = function(roundNumber)
	--clog("Level101.OnRoundEnd")
end

---坦克摧毁回调
Level101.OnTankDestory = function(isPlayer , soliderId , tankId)
	--clog("Level101.OnTankDestory")
end


---坦克进入指定地点回调
Level101.OnTankEnterPos = function(pos , soliderId , tankId , isPlayer)
	--clog("Level101.OnTankEnterPos")
end

---释放技能回调
Level101.OnReleaseSkill = function(skillId , isPlayer)
	--clog("Level101.OnReleaseSkill")
end

return Level101