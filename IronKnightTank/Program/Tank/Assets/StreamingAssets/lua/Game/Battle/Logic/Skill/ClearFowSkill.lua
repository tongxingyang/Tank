---------------------------------------------
--- ClearFowSkill
---清除战争阴影技能
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
local ClearFowSkill = {}

local ClearFowBuff = require("Game.Battle.Logic.Buff.ClearFowBuff")

function ClearFowSkill.Play(skillScript , param)
	ClearFowBuff.new(skillScript.Round  , param.Camp , skillScript.Area , param.TargetPos)
end

return ClearFowSkill