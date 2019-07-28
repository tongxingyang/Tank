
local Skill_2001 =  class("Skill_2001" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 2001
local Coefficient = 0.01 ----------技能等级加成数
local BasicNum =1.05   ---------基础系数
----伤害增强系数：
----distance  距离
---默认值 1
function Skill_2001:GetDamageStrengthenCoefficient(distance , level)
    local Skill_2001Num = 0
    if distance > 0 then
        Skill_2001Num =  (level - 1) * Coefficient + BasicNum ------参数公式
        return Skill_2001Num
    end
    return 1
end

return Skill_2001