---------------------------------------------
--- PlayerSolider
---	玩家solider
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class PlayerSolider : BaseSolider
local PlayerSolider = class("PlayerSolider" , require("Game.Main.Model.Data.BaseSolider"))

function PlayerSolider:ctor(soliderId , level , soliderSkillLv)
	self.Id = soliderId
	local tUnitData = JsonDataMgr.GetUnitData( soliderId)
	self.Level = level
	self.Name = tUnitData.Name
	self.Hit = tUnitData.Hit
	self.MaxPower = tUnitData.Power
	self.Power = self.MaxPower
	self.SkillID = tUnitData.Skill_ID
	self.SkillLevel = soliderSkillLv
	self:_initSkill()
end

function PlayerSolider:GetCompatility(tankType , tankId)
	local tSolider = JsonDataMgr.GetUnitData( self.Id)
	if tankId == tSolider.Compatibility_Model then
		return ETankCompatibility.Target
	end
	if tankType == ETankType.Small then
		return tSolider.Compatibility_Small
	elseif tankType == ETankType.Middle then
		return tSolider.Compatibility_Middle
	elseif tankType == ETankType.Heavy then
		return tSolider.Compatibility_Heavy
	elseif tankType == ETankType.Fighter then
		return tSolider.Compatibility_Fighter
	else
		clog("未知的坦克类型")
		return 0
	end
end

function PlayerSolider:GetSoliderIconPath()
	local tSolider = JsonDataMgr.GetUnitData(self.Id)
	local iconPath = "Sprite/Icon/Unit_Head/"..tSolider.Card_Art_HeadResources
	return iconPath
end
return PlayerSolider