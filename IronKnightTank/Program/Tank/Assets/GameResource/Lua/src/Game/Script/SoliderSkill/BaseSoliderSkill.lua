---------------------------------------------
--- BaseSoliderSkill
--- Created by thrt520.
--- DateTime: 2018/5/19
---
---
---车长技能脚本
---
---
---
---------------------------------------------
---@class BaseSoliderSkill
local BaseSoliderSkill = class("BaseSoliderSkill")
---伤害相关属性
--BaseSoliderSkill.DamageStrengthen1 = 1
--BaseSoliderSkill.DamageStrengthen2 = 1
--BaseSoliderSkill.DamageStrengthen3 = 1
--BaseSoliderSkill.DamageStrengthen4 = 1
--BaseSoliderSkill.DamageCoefficient = 1
--
-----护甲相关属性
--BaseSoliderSkill.TurretFrontArmored = 1
--BaseSoliderSkill.TurretBehindArmored = 1
--BaseSoliderSkill.TurretSideArmored = 1
--BaseSoliderSkill.BodyFrontArmored = 1
--BaseSoliderSkill.BodyBehindArmored = 1
--BaseSoliderSkill.BodySideArmored = 1
--BaseSoliderSkill.ArmoredCoefficient = 1
--
-----视野
--BaseSoliderSkill.View = 1
-----速度
--BaseSoliderSkill.Speed = 1
-----隐蔽
--BaseSoliderSkill.Hide = 1
-----射程
--BaseSoliderSkill.Range = 1
-----轻坦
--BaseSoliderSkill.TankKiller = 1
-----炮塔
--BaseSoliderSkill.TurretKiller = 1
-----幸运
--BaseSoliderSkill.Luck = 1
-----紧急治疗
--BaseSoliderSkill.EmergencyTreatment = 1
-----回收资源
--BaseSoliderSkill.RecycleResource = 1
-----缴获增强
--BaseSoliderSkill.SeizureEnhancement = 1
-----快速旋转
--BaseSoliderSkill.QuickRotate = 1
-----山地强行
--BaseSoliderSkill.MountainForced  = 1
-----装填老手
--BaseSoliderSkill.QucikFilling  = 1

----伤害增强系数：    技能名称
----distance  距离   技能参数 默认都含有level参数
---默认值 1			技能默认值
---小数				技能数值类型 小数为1。05 类型   百分制整数为以整数形式表示的百分数 ，如50% = 50  整数为正常整数



----伤害增强系数：
----distance  距离
---默认值 1
---乘法 小数
function BaseSoliderSkill:GetDamageStrengthenCoefficient(distance , level)
	return 1
end

----护甲增强系数：
----armoredSite 护甲类型  1为炮塔 2为车身
----armoredPosType 护甲位置类型 1为背面 2为右侧面  3为正面 4为左侧面
---默认值 1
---乘法 小数
function BaseSoliderSkill:GetArmoredStrengthenCoefficient(armoredSite , armoredPosType ,  level)
	return 1
end

----视野增强：
----坦克的视野增加n%
---默认值 0
---加法 整数
function BaseSoliderSkill:GetViewStrengthen( level)
	return 0
end

----速度增强：
----坦克的速度增加n%
---默认值 0
---加法 整数
function BaseSoliderSkill:GetSpeedStrengthen( level)
	return 0
end

----隐蔽增强：
----坦克的投影量减少为原数值的n%
---默认值 1
---乘法 小数
function BaseSoliderSkill:GetHideStrengthen( level)
	return 1
end

---射程增强
---坦克的射程增加n%
---默认值 0
---加法 整数
function BaseSoliderSkill:GetRangeStrengthen(level)
	return 0
end
---$坦杀手
---对指定坦克类型的伤害增强
---默认值 1
---乘法 小数
function BaseSoliderSkill:GetDmgStrengthForTargetTank(tankType)
	return 1
end

----炮塔破坏者
----对炮塔伤害的几率增强
---默认值 0
---加法 百分整数
function BaseSoliderSkill:GetRateStrengthForTurret()
	return 0
end

----幸运
----收到炮击时有n%的几率发生跳弹不受伤害
---默认值 0
---乘法 百分整数 幸运几率
function BaseSoliderSkill:GetLuck(level)
	return 0
end

----紧急治疗
----减少负伤造成的体力消耗
---默认值 0
function BaseSoliderSkill:GetTreatment()
	return 0
end

---回收资源
---自己指挥的坦克被击毁后几率回收
---默认值 1
function BaseSoliderSkill:RecycleSelfTank()
	return 1
end

---缴获增强
---自己击毁的坦克战斗后回收几率上升
---默认值 1
function BaseSoliderSkill:RecycleEnemyTank()
	return 1
end

-------快速旋转---------
-----在战斗中是否受旋转炮塔的速度惩罚
-----默认值 0，代表会受惩罚
---整数 只有0和1
function BaseSoliderSkill:RotationPenalty()
	return 0
end

-------地形速度惩罚
------在战场上是否受到指定地形类型的速度惩罚影响
--------默认值 0 ，代表会受到惩罚
---整数 只有0和1
function BaseSoliderSkill:TopographyInfluence(TerrainEleement_Type)
	return 0
end

----装填增强：
----坦克的装填速度上升为原数值的n%
---默认值 1
---乘法 小数
function BaseSoliderSkill:GetReload_Speed( level)
	return 1
end


return BaseSoliderSkill