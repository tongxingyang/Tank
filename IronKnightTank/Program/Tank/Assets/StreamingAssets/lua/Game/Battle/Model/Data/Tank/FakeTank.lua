---------------------------------------------
--- FakeTank
---	伪装坦克
--- Created by thrt520.
--- DateTime: 2018/8/10
---------------------------------------------
---@class FakeTank:BaseFightTank
local FakeTank = class("FakeTank" , require("Game.Battle.Model.Data.Tank.BaseFightTank"))

FakeTank._view = 0

--FakeTank.IsFakeTank = true
FakeTank.Type = EFightTankType.Fake

---@param tank BaseFightTank
function FakeTank:ctor(tank)

end

---@param tank BaseFightTank
function FakeTank:Init(tank)
	self.Solider =  setmetatable( table.copy(tank.Solider) , getmetatable(tank.Solider))
	self.TankData =  setmetatable( table.copy(tank.TankData) , getmetatable(tank.TankData))
	self.Camp = tank.Camp
	self.Solider.Power = 0
	self.Solider.MaxPower = 0
	self._view = 0
	self.ObserversCamp = {}
	self.Toward = tank.Toward
	self.TankTypeFeatures = tank.TankTypeFeatures
end

--function FakeTank:CanCreateFakeTank()
--	return false
--end

function FakeTank:GetView()
	return 0
end

---是否可移动
function FakeTank:CanMove()
	return false
end

function FakeTank:GetEquipmentArmored(eHitRes, ehitTankPos)
	return 0
end

function FakeTank:GetEquipmentRawArmor(eHitRes, eHitTankPos)
	return 0
end

---是否可攻击
function FakeTank:CanAtk()
	return false
end

function FakeTank:CanAction()
	return true
end

function FakeTank:DestoryDialog()
	return "摧毁伪装坦克"
end

return FakeTank