---------------------------------------------
--- MSGReleaseSkill
--- Created by thrt520.
--- DateTime: 2018/8/7
---------------------------------------------
---@class MSGReleaseSkill : EventArgsBasics
local MSGReleaseSkill = require("Framework.Event.EventArgsBasics").Define("MSGReleaseSkill")
MSGReleaseSkill.SkillId = 0
MSGReleaseSkill.param = nil
MSGReleaseSkill.Camp = nil
MSGReleaseSkill.IsPlayer = true
return MSGReleaseSkill