local Skill_1004 =  class("Skill_1004" , require("Game.Script.SoliderSkill.BaseSoliderSkill"))
local SkillId = 1004
local Coefficient = 0.01 ----------技能等级加成数
local BasicNum =1.05   ---------基础系数

----护甲增强系数：
----armoredSite 护甲类型  1为炮塔 2为车身
----armoredPosType 护甲位置类型 1为背面 2为侧面  3为正面
---默认值 1
function Skill_1004:GetArmoredStrengthenCoefficient(armoredSite , armoredPosType ,  level)
    local Skill_1004Num = 0
    if armoredSite  == 1  then  -----------炮塔全方位加强
        Skill_1004Num =  (level - 1) * Coefficient + BasicNum ------参数公式
        return Skill_1004Num
    end
    return 1
end

return Skill_1004