local Skill_1012 =  class("Skill_1012" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1012
local Coefficient = -1 ----------技能等级加成数
local BasicNum = -5   ---------基础系数


----炮塔破坏者
----对炮塔伤害的几率增强
---默认值 0
function Skill_1012:GetRateStrengthForTurret(level)
    local Skill_1012Num  = (level - 1) * Coefficient + BasicNum ------参数公数
    return 0
end

return Skill_1012

