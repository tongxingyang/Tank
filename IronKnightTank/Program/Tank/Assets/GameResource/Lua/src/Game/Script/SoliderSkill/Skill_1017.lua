local Skill_1017 =  class("Skill_1017" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1017
local Coefficient = 1 ----------技能等级加成数


-------快速旋转---------
-----在战斗中是否受旋转炮塔的速度惩罚
-----默认值 0，代表会受惩罚
function Skill_1017:RotationPenalty()
    return Coefficient
end

return Skill_1017

