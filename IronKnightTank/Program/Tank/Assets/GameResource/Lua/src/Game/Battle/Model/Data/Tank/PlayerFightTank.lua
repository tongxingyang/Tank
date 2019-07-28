---------------------------------------------
--- PlayerFightTank
---	玩家坦克
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class PlayerFightTank :BaseFightTank
local PlayerFightTank = class("PlayerFightTank"  , require("Game.Battle.Model.Data.Tank.BaseFightTank"))
PlayerFightTank.IsArrange = false
PlayerFightTank.IsChose = false
local ChoseTank = nil
local PlayerSolider = require("Game.Battle.Model.Data.Tank.PlayerSolider")
local TankData = require("Game.Main.Model.Data.TankData")

function PlayerFightTank:ctor(soliderId, soliderLv , tankId , soliderSkillLv)
	self.Solider = PlayerSolider.new(soliderId , soliderLv , soliderSkillLv)
	self.TankData = TankData.new(tankId)
	self.IsPlayer = true
	self.canAtk = true
	self.Camp = BattleConfig.PlayerCamp
	self:_init()
	self.Compatility = self.Solider:GetCompatility(self.TankData.TankType , self.TankData.Id)
	self.CompatilityCofficient = ETankCompatibility.GetCompatibilityCofficient(self.Compatility)
end

function PlayerFightTank:Chose()
	if ChoseTank == nil then
		self.IsChose = true
		ChoseTank = self
	elseif ChoseTank == self then
		self.IsChose = false
		ChoseTank = nil
	else
		self.IsChose = true
		ChoseTank.IsChose = false
		ChoseTank = self
	end
end

function PlayerFightTank.CancelChose()
	if ChoseTank then
		ChoseTank.IsChose = false
	end
	ChoseTank = nil
end


function PlayerFightTank:NoHurtDialog()
	return  "未能击穿我们的装甲！"
end

function PlayerFightTank:DestoryDialog()
	return  "无法继续战斗弃车！"
end

function PlayerFightTank:MissDialog()
	return "还好敌人，打偏了！"
end

function PlayerFightTank:HurtDialog(hitRes)
	if hitRes == EHitRes.HitBody  then
		return  "我们的车身被破坏了！"
	elseif  hitRes == EHitRes.HitTurret then
		return  "我们的炮塔被破坏了！"
	else
		return  "我们未知部位被破坏"
	end
end

function PlayerFightTank:LuckDialog()
	return  "God bless ，跳弹了！"
end

function PlayerFightTank:BeforeAtkDialog()
	return "攻击开始"
end




return PlayerFightTank