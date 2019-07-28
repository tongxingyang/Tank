---------------------------------------------
--- LevelDemo
--- Created by thrt520.
--- DateTime: 2018/8/27
------------------------------------------------
---@class BaseLevelScript
local LevelDemo = {}

---坦克胜利条件描述
LevelDemo.SuccesCondition = 1001
---坦克失败条件描述
LevelDemo.FailCondition = 1001
---阵营顺序
LevelDemo.Player = false
---技能
LevelDemo.SKills = {{id = 111 , times = 3}}
---战斗开始前回调
LevelDemo.BeforeLevelStart = function()

end

---回合结束回调
LevelDemo.OnRoundStart = function(roundNumber)

end

LevelDemo.OnCampRoundEnd = function(isPlayer)

end

---坦克摧毁回调
LevelDemo.OnTankDestory = function(isPlayer , soliderId , tankId)

end

---坦克进入指定地点回调
LevelDemo.OnTankEnterPos = function(x , y , soliderId , tankId , isPlayer)

end

---释放技能回调
LevelDemo.OnReleaseSkill = function(skillId  , isPlayer)

end



LevelDemo.Tools = {}


LevelDemo.Tools.CreatTank = function(npcId , x , y , toward)

end

LevelDemo.Tools.ChangeGlobalViewCost = function(number)

end

LevelDemo.Tools.AddSkill = function(skilId , times)

end

LevelDemo.Tools.BanSkill = function(skillId , roundNumber)

end

LevelDemo.Tools.DelSkill = function(skillId )

end

LevelDemo.Tools.Dialog = function(type ,content , icon )

end


LevelDemo.Tools.ChangeSkillTimes = function(skillId , val)

end


LevelDemo.Tools.Success = function( )

end




LevelDemo.Tools.Fail = function()

end

return LevelDemo