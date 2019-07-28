---------------------------------------------
--- RecruitConfig
--- Created by thrt520.
--- DateTime: 2018/10/15
---------------------------------------------
---@class RecruitConfig
RecruitConfig = {}
---@type table
RecruitConfig.NewSolider = {}
---@type table 命中值范围
RecruitConfig.NewSolider.Hit = {Min = 50 , Max = 70}
---@type number 天命型号几率
RecruitConfig.NewSolider.OwnTankRate = 0
---@type table 成长值范围
RecruitConfig.NewSolider.Growth = {Min = 1 , Max = 3}
---@type boolean 生成技能几率
RecruitConfig.NewSolider.SkillRate = 1
---@type table 向性系统
RecruitConfig.NewSolider.Compatibility = {
	A = {Rate = 0.1 , Type = 1 , MaxCount = 1} ,
	B = {Rate = 0.2 , Type = 1 , MaxCount = 1} ,
	C = {Rate = 0.7 , Type = 2 , MaxCount = 4}
}

RecruitConfig.OldSolider = {}
-----命中值范围
RecruitConfig.OldSolider.Hit = {Min = 50 , Max = 70}
-----天命型号几率
RecruitConfig.OldSolider.OwnTankRate = 0
---@type boolean 生成技能几率
RecruitConfig.OldSolider.SkillRate = 0.5
-----成长值范围
RecruitConfig.OldSolider.Growth = {Min = 1 , Max = 2}
-----向性系统
RecruitConfig.OldSolider.Compatibility = {
	A = {Rate = 0.1 , Type = 1 , MaxCount = 2} ,
	B = {Rate = 0.3 , Type = 1 , MaxCount = 2} ,
	C = {Rate = 0.6 , Type = 2 , MaxCount = 4}
}
return RecruitConfig
