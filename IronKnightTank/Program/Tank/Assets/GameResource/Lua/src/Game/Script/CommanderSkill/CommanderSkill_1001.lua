---------------------------------------------
--- BaseCommanderSkill
--- Created by thrt520.
--- DateTime: 2018/8/7
---------------------------------------------
local  BaseCommanderSkill = {}

BaseCommanderSkill.SkillId = 1001

-------技能属性范例之伤害技能
BaseCommanderSkill.Type = ECommanderSkillType.Dmg
BaseCommanderSkill.Area = 3									---范围
BaseCommanderSkill.Hit = 50									---命中
BaseCommanderSkill.DmgType = 1                              ---高爆弹

return BaseCommanderSkill
