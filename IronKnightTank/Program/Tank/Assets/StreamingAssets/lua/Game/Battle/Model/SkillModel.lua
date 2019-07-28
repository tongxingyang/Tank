---------------------------------------------
--- SkillModel
--- Created by thrt520.
--- DateTime: 2018/8/9
---------------------------------------------
---@class SkillModel : SceneActor
local SkillModel = {}
local this = SkillModel

---@type table<int , SkillData>
local skillDataDic = {}

local SkillData = require("Game.Battle.Model.Data.Skill.SkillData")
local ViewFacade = require("Game.Battle.View.ViewFacade")

local receiveCommand = {
	require("Game.Event.Message.Battle.MSGAddSkill"),
	require("Game.Event.Message.Battle.MSGBanSkill"),
	require("Game.Event.Message.Battle.MSGDelSkill"),
	require("Game.Event.Message.Battle.MSGChangeSkillTimes"),
	require("Game.Event.Message.Battle.MSGRoundStart"),
	require("Game.Event.Message.Battle.MSGBeforeCombatStart"),
}

function  SkillModel.OnIntoScene()
	local tLevel = JsonDataMgr.GetLevelData(BattleConfig.LevelId)
	---@type BaseLevelScript
	local levelScript = require("Game.Script.Level."..tLevel.Level_Script)
	local skillData = levelScript.SKills
	for i, v in ipairs(skillData) do
		this.AddSKill(v.id , v.times)
	end
	this._registerCommandHandler()
end

function SkillModel.OnLeaveScene()
	this.Dispose()
	this._unregisterCommandHandler()
end

---增加技能
---@param skillId number
---@param times number
function SkillModel.AddSKill(skillId , times)
	local skillData = SkillData.New(skillId , times)
	skillDataDic[skillId] = skillData
end

---更改技能次数
---@param skillId number
---@param times number
function SkillModel.ChangeSkillTimes(skillId , times)
	local skillData = skillDataDic[skillId]
	if skillData then
		skillData.CurTime = skillData.CurTime + times
		if skillData.CurTime < 0 then
			skillData.CurTime = 0
		end
	else
		clog("not skill data"..tostring(skillId))
	end
end

---删除技能
---@param skillId number
function SkillModel.DelSkill(skillId)
	skillDataDic[skillId] = nil
	for i, v in pairs(skillDataDic) do
		clog(tostring(i))
	end
end

---禁用技能
---@param skillId number 技能id
---@param round number 回合数
function SkillModel.BanSkill(skillId , round)
	local skillData = skillDataDic[skillId]
	if skillData then
		skillData.BanRound = round
	else
		clog("no skill Data ".. tostring(skillId))
	end
end

---释放技能
---@param skillId number
function SkillModel.ReleaseSkill(skillId)
	local skillData = this.GetSkillData(skillId)
	skillData:DreaseSkillTimes()
end

---技能是否可释放
---@return boolean
function SkillModel.CanSKillRelease(skillId)
	local skillData = this.GetSkillData(skillId)
	return skillData.CurTime > 0
end

---获取所有技能数据
function SkillModel.GetAllSKillData()
	return skillDataDic
end

function SkillModel.GetSkillData(skillId)
	local skillData =  skillDataDic[skillId]
	if skillData then
		return skillData
	else
		clog("no skill data "..tostring(skillId))
	end
end

function SkillModel._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommand,SkillModel)
end

function SkillModel._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommand,SkillModel)
end

----------------------------------------------------------
---evet handler
----------------------------------------------------------
----MSGAddSkill
---@param msg MSGAddSkill
function SkillModel.OnMSGAddSkill(msg)
	this.AddSKill(msg.SkillId , msg.Times)
	ViewFacade.UpdateSkill()
end

----MSGDelSkill
---@param msg MSGDelSkill
function SkillModel.OnMSGDelSkill(msg)
	this.DelSkill(msg.SKillId)
	ViewFacade.UpdateSkill()
end


----MSGChangeSkillTimes
---@param msg MSGChangeSkillTimes
function SkillModel.OnMSGChangeSkillTimes(msg)
	this.ChangeSkillTimes(msg.SkillID, msg.Val)
	ViewFacade.UpdateSkill()
end


----MSGBanSkill
---@param msg MSGBanSkill
function SkillModel.OnMSGBanSkill(msg)
	this.BanSkill(msg.SKillID, msg.Round)
	ViewFacade.UpdateSkill()
end


----MSGRoundStart
---@param msg MSGRoundStart
function SkillModel.OnMSGRoundStart(msg)
	for i, v in pairs(skillDataDic) do
		v:DreaseBanRound()
	end
end


----MSGBeforeCombatStart
---@param msg MSGBeforeCombatStart
function SkillModel.OnMSGBeforeCombatStart(msg)
	ViewFacade.UpdateSkill()
end
----------------------------------------------------------

function SkillModel.Dispose()
	skillDataDic = {}
end
return SkillModel