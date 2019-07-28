local Skill_1013 =  class("Skill_1013" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1013
local Coefficient = 1 ----------技能等级加成数
local BasicNum = 3   ---------基础系数


function Skill_1013:GetLuck(level)
    local  Skill_1013Num = 0
    Skill_1013Num = (level - 1) * Coefficient + BasicNum
    return Skill_1013Num
end

return Skill_1013

