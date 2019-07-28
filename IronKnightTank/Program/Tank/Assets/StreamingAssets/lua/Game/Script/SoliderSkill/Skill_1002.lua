local Skill_1002 =  class("Skill_1002" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1002
local Coefficient = 0.01 ----------技能等级加成数
local BasicNum =1.05   ---------基础系数
----伤害增强系数：
----distance  距离
---默认值 1
function Skill_1002:GetDamageStrengthenCoefficient(distance , level)
    local Skill_1002Num = 0
    if distance > 0 and distance <= 2 then
        Skill_1002Num =  (level - 1) * Coefficient + BasicNum ------参数公式
        return Skill_1002Num
    end
    return 1
end

return Skill_1002