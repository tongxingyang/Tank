---------------------------------------------
--- TankTypeFeaturesConfig
---	坦克特性的配置脚本
---	可以配置不同类型的坦克的行为与消耗，但是现在攻击已经修改成消耗所有能量，攻击的配置已经失效
--- Created by thrt520.
--- DateTime: 2018/8/15
---------------------------------------------

TankTypeFeaturesConfig = {}
TankTypeFeaturesConfig[ETankType.Small] = {Move = 1, Atk = 1 , FocusAtk = 2 , DmgAdd = 1 , HitAdd = 1 , ArmoredAdd = 1}
TankTypeFeaturesConfig[ETankType.Middle] = {Move = 1, Atk = 1 , FocusAtk = 2 , DmgAdd = 1 , HitAdd = 1 , ArmoredAdd = 1}
TankTypeFeaturesConfig[ETankType.Heavy] = {Move = 1, Atk = 1 , FocusAtk = 2 , DmgAdd = 1 , HitAdd = 1 , ArmoredAdd = 1}
TankTypeFeaturesConfig[ETankType.Fighter] = {Move = 1, FocusAtk = 2 , DmgAdd = 1 , HitAdd = 1 , ArmoredAdd = 1}

function TankTypeFeaturesConfig.GetMoveCost(tankType)
	return TankTypeFeaturesConfig[tankType].Move
end

function TankTypeFeaturesConfig.GetAtkCost(tankType)
	return TankTypeFeaturesConfig[tankType].Atk
end

function TankTypeFeaturesConfig.GetFocusAtkCost(tankType)
	return TankTypeFeaturesConfig[tankType].FocusAtk
end
