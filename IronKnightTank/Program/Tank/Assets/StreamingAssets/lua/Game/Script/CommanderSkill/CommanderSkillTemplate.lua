---------------------------------------------
--- BaseCommanderSkill
--- Created by thrt520.
--- DateTime: 2018/8/7
---------------------------------------------
local  BaseCommanderSkill = {}

BaseCommanderSkill.SkillId = 0

-------技能属性范例之伤害技能
BaseCommanderSkill.Type = ECommanderSkillType.Dmg
BaseCommanderSkill.Area = 1									---范围
BaseCommanderSkill.Hit = 0									---命中
BaseCommanderSkill.HitType = 1 								---命中类型  1为地图 2位单位
BaseCommanderSkill.DmgType = 1 								---伤害类型  1 --高爆弹  2 -穿甲弹和轰炸

-------技能属性范例之修复技能
BaseCommanderSkill.Type = ECommanderSkillType.Repair
BaseCommanderSkill.Area = 1									---范围

-------技能属性范例之临时掩体
BaseCommanderSkill.Type = ECommanderSkillType.TempBunker
BaseCommanderSkill.Area = 1									---范围
BaseCommanderSkill.Val = 1									---数值
BaseCommanderSkill.Round = 1								---回合数

-------技能属性范例之烟雾弹
BaseCommanderSkill.Type = ECommanderSkillType.Smoke
BaseCommanderSkill.Area = 1									---范围
BaseCommanderSkill.Val = 1									---数值
BaseCommanderSkill.Round = 1								---回合数

-------技能属性范例之伪装坦克
BaseCommanderSkill.Type = ECommanderSkillType.FakeTank
BaseCommanderSkill.Round = 1								---回合数

-------技能属性范例之飞机侦查
BaseCommanderSkill.Type = ECommanderSkillType.ClearFOW
BaseCommanderSkill.Area = 1									---范围
BaseCommanderSkill.Round = 1								---回合数

-------技能属性范例之公共属性
BaseCommanderSkill.ActionRange = ESkillActionRange.AllMap
return BaseCommanderSkill
