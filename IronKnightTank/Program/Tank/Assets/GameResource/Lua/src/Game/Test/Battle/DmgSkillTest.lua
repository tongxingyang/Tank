---------------------------------------------
--- DmgSkillTest
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
local DmgSkillTest = {}

DmgSkillTest.Test  = function()
	local MSGReleaseSkill = require("Game.Event.Message.Battle.MSGReleaseSkill")
	EventBus:Brocast(MSGReleaseSkill:Build({SkillId = 1001 , param = {TargetPos = GridPos.New(28 , 28 ) , Camp = BattleConfig.PlayerCamp} }))
end

return DmgSkillTest