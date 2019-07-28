---------------------------------------------
--- SkillPlayer
---技能播放器
--- Created by thrt520.
--- DateTime: 2018/8/7
---------------------------------------------
---@class SkillPlayer
local SkillPlayer = {}

---@type table<ECommanderSkillType , BaseSkill>
local skillDic = {}
skillDic[ECommanderSkillType.Dmg] = require("Game.Battle.Logic.Skill.DmgSkill")
skillDic[ECommanderSkillType.Repair] = require("Game.Battle.Logic.Skill.RepairSkill")
skillDic[ECommanderSkillType.Smoke] = require("Game.Battle.Logic.Skill.SmokeSkill")
skillDic[ECommanderSkillType.FakeTank] = require("Game.Battle.Logic.Skill.FakeTankSkill")
skillDic[ECommanderSkillType.TempBunker] = require("Game.Battle.Logic.Skill.TempBunkerSkill")
skillDic[ECommanderSkillType.ClearFOW] = require("Game.Battle.Logic.Skill.ClearFowSkill")

local MSGSkillRelaseFinish = require("Game.Event.Message.Battle.MSGSkillRelaseFinish")

function SkillPlayer.PlaySkill(skillId , param , camp)
	local skillData = JsonDataMgr.GetInitiativeSkillData(skillId)
	local skillScript = require("Game.Script.CommanderSkill."..skillData.Skill_Script)
	local skillType = skillScript.Type
	local  skill = skillDic[skillType]
	if skill then
		skill.Play(skillScript , param)
	else
		clog("not skill "..tostring(skillId).."   "..tostring(skillType))
	end
	EventBus:Brocast(MSGSkillRelaseFinish:Build({SkillID = skillId , Camp = camp}))
end

function SkillPlayer.Dispose()
	skillDic = {}
end


return SkillPlayer