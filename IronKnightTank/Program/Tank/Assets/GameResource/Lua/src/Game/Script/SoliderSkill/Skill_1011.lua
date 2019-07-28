local Skill_1011 =  class("Skill_1011" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1011
local Coefficient = 1 ----------技能等级加成数
local BasicNum = 5   ---------基础系数


----炮塔破坏者
----对炮塔伤害的几率增强
---默认值 0
function Skill_1011:GetRateStrengthForTurret(level)
    local Skill_1011Num  = (level - 1) * Coefficient + BasicNum ------参数公数
    return 0
end

return Skill_1011

