---------------------------------------------
--- SkillData
---	技能数据
--- Created by thrt520.
--- DateTime: 2018/8/9
---------------------------------------------
---@class SkillData
local SkillData = class("SkillData")

SkillData.Id = 0
SkillData.MaxTime = 0
SkillData.CurTime = 0
SkillData.IsBan = 0
SkillData.BanRound = 0
SkillData.buffers = nil
SkillData.IconPath = nil

function SkillData:ctor(skillId , MaxTime)
	self.Id = skillId
	self.MaxTime = MaxTime
	self.CurTime = self.MaxTime
	self.buffers = {}
end

function SkillData:DreaseBanRound()
	if self.BanRound >0 then
		self.BanRound =  self.BanRound - 1
	end
end

function SkillData:IsBan()
	return self.BanRound > 0
end

function SkillData:Enable()
	return (not self:IsBan() and self.CurTime > 0)
end

function SkillData:DreaseSkillTimes()
	self.CurTime = self.CurTime - 1
	if self.CurTime < 0 then
		self.CurTime = 0
	end
end

function SkillData:GetSkillIconPath()
	if not self.IconPath then
		local tSkillData = JsonDataMgr.GetInitiativeSkillData(self.Id)
		self.IconPath = "Sprite/Icon/Skill_Icon/"..tSkillData.Skill_Icon..".png"
	end
	return self.IconPath
end

return SkillData