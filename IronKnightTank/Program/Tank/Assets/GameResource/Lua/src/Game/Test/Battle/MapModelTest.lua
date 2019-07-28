---------------------------------------------
--- MapModelTest
--- Created by thrt520.
--- DateTime: 2018/8/29
---------------------------------------------
local MapModelTest = {}

local MapModel = require("Game.Battle.Model.MapModel")
local GridPos = require("Game.Battle.Model.Data.Map.GridPos")

MapModelTest.TestGetAvailablePos = function()
	local gridPos = GridPos.New(0 , 0)
	local linePos = gridPos:GetQuadLinePos(1)
	for i, v in ipairs(linePos) do
		MapModel._moverDic[v] = true
	end
	MapModel._moverDic[gridPos] = true
	local pos = MapModel.GetAvailablePos(gridPos)
	clog(tostring(pos))
end

return MapModelTest