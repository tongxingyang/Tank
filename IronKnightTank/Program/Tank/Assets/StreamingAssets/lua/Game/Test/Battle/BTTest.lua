---------------------------------------------
--- BTTest
--- Created by thrt520.
--- DateTime: 2018/7/18
---------------------------------------------
local BTTest = {}

local BehaviourTree = require("Game.Battle.behaviour_tree.core.BehaviourTree")
local BTSelector = require("Game.Battle.behaviour_tree.composites.BTSelector")
local BTAction = require("Game.Battle.behaviour_tree.core.BTActionNode")
local BTCondition = require("Game.Battle.behaviour_tree.core.BTConditionNode")
local BTSequence = require("Game.Battle.behaviour_tree.composites.BTSequence")

function BTTest.Test()
	local bt = BehaviourTree.New()
	local root  = BTSelector.New()
	bt.root = root
	local action1 = BTAction.New()
	action1.property = {"log1"}
	local action2 = BTAction.New()
	action2.property = {"log1"  , "log2"}
	local blackBoard = {}
	blackBoard["log1"] = function(blackBoard)
		clog("log1")
	end
	blackBoard["log2"] = function(blackBoard)
		clog("log2")
	end
	local condition = BTCondition.New()
	condition.property = {"c1"}
	blackBoard["c1"] = function(blackBoard)
		return false
	end
	local sequence1 = BTSequence.New()
	sequence1.children = {condition , action1}
	root.children = {sequence1  , action2}
	bt:tick(blackBoard)
end

function BTTest.Test2()
	require("Game.Battle.behaviour_tree.CheckType")
	local bt = BehaviourTree.New()
	local data = {
		name = "moveAndCombat",
		nodes = {
			{id = 1 , category = "Selector" , des = "root" , children = {2}},
			{id = 2 , category = "Sequence" , des = "atk" , children = {3 , 4}},
			{id = 3 , category = "Action" , des = "GetCanAtkTank" , tasks = {{name = "GetCanAtkTank" , TankId = "CurTankId" , SaveAs = "CanAtkTankIds" }} },
			{id = 4 , category = "Iterator" , des = "找到攻击目标" , iterKey = "CanAtkTanks"  , curKey = "curEnemyTank" , child = 5 },
			{id = 5 , category = "Sequence" , des = "判断坦克是否符合标准" , children = {6  , 7} },
			{id = 6 , category = "Condition" , des = "判断坦克是否符合标准" ,  tasks = {{names = "CheckNumber" , keyA = {name = "curEnemyTank" , valName = "View" }  , keyB = {name = "maxView"} , checkType = CheckType.GreaterThan}}},
		},
	}

	local blackBoard = {}
	blackBoard.GetCanAtkTank = function() return {1 , 2} end
	blackBoard.maxView = 0
	bt:Load(data)
	bt:tick(blackBoard)

end

function BTTest.Test3()
	local bt = BehaviourTree.New()

	local blackBoard = {
		curTankData = {Power = 1 , Id = 1}
	}
	require("Game.Battle.behaviour_tree.CheckType")
	local data = {
		name = "aiTankAction",
		nodes = {
			{id = 1 , category = "Sequence" , children = {2  , 4}},

			{id = 2 , category = "Condition" , tasks = {{name = "CheckNumber" ,data = { keyA = {name = "curTankData" , val = {name = "Power"}} , keyB = 0 , checkType = CheckType.GreaterThan}}}},
			{id = 3 , category = "Selector" , child = {4 }},
			{id = 4 , category = "Sequence" , children = {5 , 6  , 7}},
			{id = 5 , category = "Action" ,
			 tasks = {{
				 name = "GetCanAtkTanks" ,
				 data = {SaveAs = "CanAtkTanks" ,
						 ankId = {name = "curTankData"} ,
						 val = {name = "Id"} } }} },
			{id = 6 , category = "Condition" , tasks = {{name = "CheckArrayLength" , data = {arrayKey = {name = "CanAtkTanks"} ,lengthKeyOrVal = 0 , checkType = CheckType.GreaterThan }}} , child = 7},
			{id = 7 , category = "Iterator" ,iterKey = "CanAtkTanks"  , curKey = "curEnemyTank" , child = 8},
			{id = 8 , category = "Action" , tasks = {{name = "TankAtk" , data = {AtkTankId = {name = "curTankData" , val = {name = "Id"}} , DefTankId = {name = "curEnemyTank"}}}}}
		}
	}
	bt:Load(data)
	bt:tick(blackBoard)
end

function GetBloackBoardVal(bloackBoard , keyTable)
	--clog(type(keyTable))
	if type(keyTable) == "number" then
		clog("key "..tostring(keyTable))
		return keyTable
	end
	local val = bloackBoard
	local nextKey = keyTable
	while nextKey and val do
		local name = nextKey.name
		val = val[name]
		nextKey = nextKey.val
	end
	return val
end


return BTTest