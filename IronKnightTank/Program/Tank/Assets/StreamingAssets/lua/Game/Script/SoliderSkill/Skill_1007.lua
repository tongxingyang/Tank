local Skill_1007 =  class("Skill_1007" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1007
local Coefficient = 1 ----------技能加成

----速度增强：
----坦克的速度增加n%
---默认值 0
function Skill_1007:GetSpeedStrengthen( level)
    return Coefficient
end

return Skill_1007