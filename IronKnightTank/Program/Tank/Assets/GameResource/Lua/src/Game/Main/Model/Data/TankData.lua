---------------------------------------------
--- TankData
---    坦克数据
--- Created by thrt520.
--- DateTime: 2018/5/3
---------------------------------------------
---@class TankData
local TankData = class("TankData")

---@type number
TankData.Id = 0
---@type number
TankData.Name = 0
---@type number 坦克类型
TankData.TankType = 0
---@type number 炮塔类型
TankData.TurretType = 0
---@type number 炮塔名字
TankData.TurretName = 0
---@type number 速度
TankData.Speed = 0
---@type number 视野
TankData.View = 0
---@type number 攻击
TankData.Atk = 0
---@type number 护甲
TankData.Armored = 0
---@type TankUnitData
TankData.TTank = nil
---@type number 装填速度
TankData.ReloadSpeed = nil
---@type number 投影量
TankData.Projection = nil
---@type string 头像路径
TankData.IconPath = nil

function TankData:ctor(tankId)
    local TTank = JsonDataMgr.GetTankUnitData(tankId)
    if TTank ~= nil then
        self.Id = tankId
        self.Name = TTank.Name
        self.TankType = TTank.Tank_Type
        self.TurretName = TTank.Turret_Name
        self.TurretType = TTank.Turret_Type
        self.View = TTank.View
        self.Speed = TTank.Speed
        self.TTank = TTank
        self.Atk = TTank.Gun_Damage
        self.Armored = TTank.Armored
        self.Projection = TTank.Projection
        self.TankType = TTank.Tank_Type
    end
end

function TankData:GetTankIconPath()
    if not self.IconPath then
        local tTank = JsonDataMgr.GetTankUnitData(self.Id)
        self.IconPath = "Sprite/Icon/Tank_Icon/" .. tTank.Tank_Icon
    end
    return self.IconPath
end

---@param eHitRes EHitRes
---@param eHitTankPos EHitTankPos
function TankData:GetEquipmentArmored(eHitRes , eHitTankPos)
	local type = ""
	if eHitRes == EHitRes.HitBody then
		type = "Armored"
	else
		type = "Turrets"
	end
	local pos = "_"..EHitTankPos.GetDes(eHitTankPos)
	local armored = self.TTank[type .. pos]
	if not armored then
		armored = 0
		clog("no armored" ..tostring(eHitTankPos).."  "..tostring(eHitRes))
	end
	return armored
end

---@param dis number
function TankData:GetDmg(dis)
	local dmg = self.TTank["Gun_Damage"..dis]
	if not dmg then
		dmg = 0
	end
	return dmg
end

return TankData