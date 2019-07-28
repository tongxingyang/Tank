---------------------------------------------
--- NPCSolider
---	npc士兵
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class NPCSolider : BaseSolider
local NPCSolider = class("NPCSolider" , require("Game.Main.Model.Data.BaseSolider"))

function NPCSolider:ctor(npcId)
	local tNpcData = JsonDataMgr.GetEnemyUnitData( npcId)
	self.Id = npcId
	self.level = tNpcData.Level
	self.Name = tNpcData.Name
	self.Hit = tNpcData.Hit
	self.MaxPower = tNpcData.Power
	self.Power = self.MaxPower
	self.SkillID = tNpcData.Skill_ID
	self.SkillLevel = tNpcData.Skill_Grade
	self:_initSkill()
end

function NPCSolider:GetSoliderIconPath()
	local tSolider = JsonDataMgr.GetEnemyUnitData(self.Id)
	local iconPath = "Sprite/Icon/Unit_Head/"..tSolider.Enemy_Icon
	return iconPath
end

return NPCSolider