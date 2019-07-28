local Skill_1014 =  class("Skill_1014" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1014
----紧急治疗
----减少负伤造成的体力消耗
---默认值 0
function Skill_1014:GetTreatment()
    return 0
end
