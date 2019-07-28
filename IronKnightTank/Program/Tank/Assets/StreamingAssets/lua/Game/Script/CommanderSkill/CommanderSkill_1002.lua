---------------------------------------------
--- BaseCommanderSkill
--- Created by thrt520.
--- DateTime: 2018/8/7
---------------------------------------------
local  BaseCommanderSkill = {}

BaseCommanderSkill.SkillId = 1002

-------技能属性范例之伤害技能
BaseCommanderSkill.Type = ECommanderSkillType.Dmg
BaseCommanderSkill.Area = 2									---范围
BaseCommanderSkill.Hit = 30									---命中
BaseCommanderSkill.DmgType = 2                              ---穿甲弹


return BaseCommanderSkill
