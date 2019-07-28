local Skill_1009 =  class("Skill_1009" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1009
local Coefficient = 1 ----------技能加成


---射程增强
---坦克的射程增加n%
---默认值 0
function Skill_1009:GetRangeStrengthen(level)
    return Coefficient
end

return Skill_1009