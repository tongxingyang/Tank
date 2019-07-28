local Skill_1015 =  class("Skill_1015" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1015
---回收资源
---自己指挥的坦克被击毁后几率回收
---默认值 1
function Skill_1015:RecycleSelfTank()
    return 1
end

