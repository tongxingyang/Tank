local Skill_1019 =  class("Skill_1019" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1019
local Coefficient = 0.01 ----------技能等级加成数
local BasicNum =1.05   ---------基础系数
----装填增强：
----坦克的装填速度上升为原数值的n%
---默认值 1
function Skill_1019:GetReload_Speed( level )
    local Skill_1019Num = 0
    Skill_1019Num = (level - 1) * Coefficient + BasicNum
    return Skill_1019Num
end

return Skill_1019