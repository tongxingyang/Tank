local Skill_1006 =  class("Skill_1006" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1006
local Coefficient = 1 ----------技能加成

----视野增强：
----坦克的视野增加n%
---默认值 0
function Skill_1006:GetViewStrengthen( level)

    return Coefficient
end

return Skill_1006