---------------------------------------------
--- CombGridTest
--- Created by thrt520.
--- DateTime: 2018/8/27
---------------------------------------------
local CombGridTest = {}

local CombGrid = require("Game.Tools.CombaGrid.CombGrid")
require("Game.Tools.MathTools")
function CombGridTest.Test()
	local grid = CombGrid.new(3 , 100 , 100 , 10 , 10)
	for i = 0, 10 do
		local pos = grid:GetGridPos(i)
		clog("index "..tostring(i).."pos   "..tostring(pos.x).." "..tostring(pos.y))
	end


end

return CombGridTest