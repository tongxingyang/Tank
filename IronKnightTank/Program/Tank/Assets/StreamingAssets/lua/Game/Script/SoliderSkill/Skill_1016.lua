local Skill_1016 =  class("Skill_1016" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1016
---缴获增强
---自己击毁的坦克战斗后回收几率上升
---默认值 1
function Skill_1016:RecycleEnemyTank()
    return 1
end

