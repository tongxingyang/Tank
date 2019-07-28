local Skill_1010 =  class("Skill_1010" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1010
local Coefficient = 0.01 ----------技能等级加成数
local BasicNum =1.05   ---------基础系数


---$坦杀手
---对指定坦克类型的伤害增强
---默认值 1
----tankType 1为轻型，2为中型，3为重型，4为坦克歼击车
function Skill_1010:GetDmgStrengthForTargetTank(tankType,level)
    local Skill_1010Num = 0
    if tankType == 1 then
        Skill_1010Num  = (level - 1) * Coefficient + BasicNum ------参数公式
        return Skill_1010Num
    else
        return 1
    end
end

return Skill_1010