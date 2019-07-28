---------------------------------------------
--- AIClient
---决策顺序：
---计算所有可以攻击坦克的击毁概率，如果满足要求则攻击，如果不满足则计算移动区域权重，选择最佳移动位置
---移动权重计算：计算所有可移动区域每个位置的击毁敌方坦克概率和我方被击毁的概率以及距离敌方坦克的距离
---公式为 val = 最大击毁概率*100 - 被击毁概率 * 100 - 距离所有敌方坦克的距离 * 0.1
--- Created by thrt520.
--- DateTime: 2018/8/16
---------------------------------------------
---@class AIClient : BaseAIClient
local AIClient = class("AIClient" , require("Game.Battle.AI.BaseAIClient"))
local TankModel = require("Game.Battle.Model.TankModel")
local MapModel = require("Game.Battle.Model.MapModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local ShadowModel = require("Game.Battle.Model.ShadowModel")
local AtkAreaHelper = require("Game.Battle.Model.AtkAreaHelper")

---@type BaseFightTank
AIClient._fightTankData = nil


local MSGTankAtkRequeset = require("Game.Event.Message.Battle.MSGTankAtkRequeset")
local MSGMoveTankRequest = require("Game.Event.Message.Battle.MSGMoveTankRequest")

function AIClient:ctor(tankData)
	self._fightTankData = tankData
end

---是否可动作
function AIClient:CanAction()
	return self._fightTankData:GetPower()> 0 and self._fightTankData.IsAlive
end


function AIClient:MakeDecision()
	local atkTarget , targetPos
	if self._fightTankData:CanAtk() then
		local canAtkTank = TankModel.GetCanAttackTank(self._fightTankData.Id)
		local maxHitRate = 0
		local isFocus = (self._fightTankData:GetPower() >= BattleConfig.FocusAtkCost)
		local hitRateLimit = isFocus and 0.5 or 0.1
		for i, v in pairs(canAtkTank) do
			local destroyRate, hitRate = self._fightTankData:GetAtkDestoryRateAndHitRate(v )
			if destroyRate > 0.1 and hitRate > hitRateLimit and hitRate > maxHitRate then
				atkTarget = v
			end
		end
	end
	if atkTarget then
		EventBus:Brocast(MSGTankAtkRequeset:Build({AtkTankId = self._fightTankData.Id , DefTankId = atkTarget.Id , IsFocus = isFocus})):Yield()
	else
		if self._fightTankData:CanMove() then
			MapModel.UpdateMapPathData(self._fightTankData)
			local oldPos = self._fightTankData.Pos
			local moveableArea = MapModel.GetCanMoveArea(1)
			table.insert(moveableArea , oldPos)
			--------计算移动到哪个地点比较好 ，使用权重来计算 这个区域有可以攻击到的敌人
			local maxVal
			maxVal = -10000
			for i, v in ipairs(moveableArea) do
				local val = self:_calPosVal(v)
				if val > maxVal then
					maxVal = val
					targetPos = v
				end
			end
			self._fightTankData.Pos = oldPos
			if targetPos == oldPos then
				targetPos = nil
			end
		end
	end
	if targetPos then
		EventBus:Brocast(MSGMoveTankRequest:Build({TankId = self._fightTankData.Id , TargetPos = targetPos})):Yield()
	else
		self._fightTankData:CostAllPower()
		self:SetToward()
	end
	if self._fightTankData:GetPower() == 0 then
		self:SetToward()
	end
end

function AIClient:_calPosVal(pos)
	local val = 0
	self._fightTankData.Pos = pos
	local totalDesrate  , totalBeDesrate , viewCount , totalDis
	totalDis = 0
	totalBeDesrate = 0
	totalDesrate = 0
	viewCount = 0
	local viewArea = ShadowModel.GetViewerViewArea(self._fightTankData)
	MapModel.UpdateMapPathData(self._fightTankData)
	for i, v in pairs(TankModel.GetEnemyCampTank(self._fightTankData.Camp)) do
		local destroyRate, hitRate= self:GetDestoryAndHitRate(self._fightTankData , v)
		local rate1 = destroyRate * hitRate
		totalDesrate = rate1 + totalDesrate
		local beDestoryRate , beHitRate = self:GetDestoryAndHitRate(v , self._fightTankData )
		local rate2 = beDestoryRate * beHitRate
		totalBeDesrate = rate2 + totalBeDesrate
		if table.contains(viewArea , v.Pos) then
			viewCount = viewCount + 1
		end
		totalDis = totalDis + #MapModel.GetPath(v.Pos)
	end
	val = val + totalDesrate - totalBeDesrate + viewCount
	val = val - totalDis * 0.1

	return val
end

function AIClient:SetToward()
	local enemyTanks = TankModel.GetEnemyCampTank(self._fightTankData.Camp)
	local maxDmg , targetTank
	maxDmg = 0
	for i, v in pairs(enemyTanks) do
		local dis = self._fightTankData:GetTankDis(v)
		local dmg = v:GetRawDmg(dis)
		dmg = v:GetHit() * dmg
		if dmg > maxDmg then
			maxDmg = dmg
			targetTank = v
		end
	end
	local toward
	if targetTank then
		toward = (targetTank.Pos - self._fightTankData.Pos):GetToward()
		self._fightTankData.Toward = toward
		ViewFacade.UpdateSingleTank(self._fightTankData.Id)
	end
end

---@param atkTank BaseFightTank
---@param defTank BaseFightTank
---@return number , number
function AIClient:GetDestoryAndHitRate(atkTank , defTank , isFocus)
	local dis = atkTank:GetTankDis(defTank)
	if dis > atkTank:GetFireRangeAdvanced() + BattleConfig.TankAtkDistance then
		return 0 , 0
	end
	if AtkAreaHelper.IsAtkBlock(atkTank , defTank.Pos) then
		return 0 , 0
	end
	return atkTank:GetAtkDestoryRateAndHitRate(defTank , isFocus)
end

function AIClient:Dispose()
	self._fightTankData = nil
end

return AIClient