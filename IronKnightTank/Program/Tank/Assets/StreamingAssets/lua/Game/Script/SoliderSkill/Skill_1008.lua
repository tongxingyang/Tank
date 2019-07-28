local Skill_1008 =  class("Skill_1008" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1008
local Coefficient = 0.01 ----------技能加成
local BasicNum = 0.95   ---------基础系数

----隐蔽增强：
----坦克的投影量减少为原数值的n%
---默认值 1
function Skill_1008:GetHideStrengthen( level)
    local Skill_1008Num =  BasicNum - (level - 1) * Coefficient  ------参数公式
    return Skill_1008Num
end

return Skill_1008