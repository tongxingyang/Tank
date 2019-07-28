---------------------------------------------
--- MSGUpdateSkill
--- Created by thrt520.
--- DateTime: 2018/8/9
---------------------------------------------
---@class MSGUpdateSkill : EventArgsBasics
local MSGUpdateSkill = require("Framework.Event.EventArgsBasics").Define("MSGUpdateSkill")
---@type table<string , SkillData>
MSGUpdateSkill.SKillDataList = nil
return MSGUpdateSkill