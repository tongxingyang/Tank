local Skill_2002 =  class("Skill_2002" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 2002
local Coefficient = 0.01 ----------技能等级加成数
local BasicNum =1.05   ---------基础系数
----伤害增强系数：
----distance  距离
---默认值 1
function Skill_2002:GetDamageStrengthenCoefficient(distance , level)
    local Skill_2002Num = 0
    if distance > 0 and distance <= 2 then
        Skill_2002Num =  (level - 1) * Coefficient + BasicNum ------参数公式
        return Skill_2002Num
    end
    return 1
end

return Skill_2002