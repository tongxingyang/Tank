---------------------------------------------
--- ETankCompatibility
---坦克向性
--- Created by thrt520.
--- DateTime: 2018/6/29
---------------------------------------------
---@class ETankCompatibility
ETankCompatibility ={
	LevelS = 4,
	LevelA = 3,
	LevelB = 2,
	LevelC = 1,
	Target = 10,
}

local tankCompatibilityCache = {}
tankCompatibilityCache[ETankCompatibility.LevelS] = BattleConfig.LevelS
tankCompatibilityCache[ETankCompatibility.LevelA] = BattleConfig.LevelA
tankCompatibilityCache[ETankCompatibility.LevelB] = BattleConfig.LevelB
tankCompatibilityCache[ETankCompatibility.LevelC] = BattleConfig.LevelC
tankCompatibilityCache[ETankCompatibility.Target] = BattleConfig.TargetTank

tankCompatibilityCache = setmetatable(tankCompatibilityCache , {__index = function (a)
	return BattleConfig.LevelC
end})

local tankCompatibilityDesCache= {}
tankCompatibilityDesCache[ETankCompatibility.LevelS] = "S"
tankCompatibilityDesCache[ETankCompatibility.LevelA] = "A"
tankCompatibilityDesCache[ETankCompatibility.LevelB] = "B"
tankCompatibilityDesCache[ETankCompatibility.LevelC] = "C"

function ETankCompatibility.GetCompatibilityCofficient(compatibility)
	return tankCompatibilityCache[compatibility]
end

function ETankCompatibility.GetDes(compatibility)
	if not tankCompatibilityDesCache[compatibility] then
		return compatibility
	end
	return tankCompatibilityDesCache[compatibility]
end

return ETankCompatibility