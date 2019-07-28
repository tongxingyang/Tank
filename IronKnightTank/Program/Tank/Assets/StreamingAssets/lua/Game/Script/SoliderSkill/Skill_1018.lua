local Skill_1018 =  class("Skill_1018" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1018
local Coefficient = 1 ----------技能等级加成数


-------地形速度惩罚
------在战场上是否受到指定地形类型的速度惩罚影响
--------默认值 0 ，代表会受到惩罚
function Skill_1018:TopographyInfluence(TerrainEleement_Type)
    if TerrainEleement_Type == 6  then-----如果地形类型是6，即山地
        return 1  ----不受山地速度惩罚影响

    else

        return 0

    end
end

return Skill_1018

