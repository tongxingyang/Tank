---------------------------------------------
--- CombGrid
--- Created by thrt520.
--- DateTime: 2018/8/27
---------------------------------------------
---@class CombGrid
local CombGrid = class("CombGrid")
---@type CombGridType
CombGrid.Type = 0
---@type number
CombGrid.GridHeight = 0
---@type number
CombGrid.GridWidth = 0
---@type number
CombGrid.GridCount = 0
---@type number
CombGrid.GridHorizontalSpace = 0
---@type number
CombGrid.GridVerticalSpace = 0
---@type number   0 vectical 1 horizontal
CombGrid.StartAxis = 0
---@type number   0 UpLeft 1 UpRight 2 DownLeft 3 DownRight
CombGrid.StartCorner = 0
---@type number   0 凹型蜂巢  1 凸型蜂巢
CombGrid.CombType = 0

function CombGrid:ctor(gridCount, gridCellHight , gridCellWidth , gridHorizontalSpace , gridVerticalSpace)
	self.GridWidth = gridCellWidth
	self.GridHeight = gridCellHight
	self.GridCount = gridCount
	self.GridHorizontalSpace = gridHorizontalSpace or 0
	self.GridVerticalSpace = gridVerticalSpace or 0
end

function CombGrid:SetStartAxis(startAxis)
	self.StartAxis = startAxis
end

function CombGrid:SetStartCorner(corner)
	self.StartCorner = corner
end

function CombGrid:SetCombType(type)
	self.CombType = type
end

function CombGrid:GetGridPos(gridIndex)
	local pos = {x = 0  , y = 0 }

	local count = self.GridCount * 2 + (self.CombType == 0 and  -1 or 1)

	local x , y
	if self.StartAxis == 0 then   --- vectical
		y , x = math.mod(gridIndex , count)
		x = x * 2
		if y >= self.GridCount then
			x = x + 1
			y = y - self.GridCount + (self.CombType == 0 and 0.5 or -0.5 )
		end
	else						  --- horizontal
		x , y = math.mod(gridIndex , count)
		y = y * 2
		if x >= self.GridCount then
			y = y + 1
			x = x - self.GridCount +  (self.CombType == 0 and 0.5 or -0.5 )
		end
	end
	pos.x = x
	pos.y = y
	return self:_calGridPos(pos)
end

function CombGrid:_calGridPos(pos)
	local x , y

	if self.StartCorner == 0  or self.StartCorner == 2 then
		x = pos.x * self.GridWidth + pos.x * self.GridHorizontalSpace
	else
		x = -1 * (pos.x * self.GridWidth + pos.x * self.GridHorizontalSpace)
	end
	if self.StartCorner == 2 or self.StartCorner == 3 then
		y = pos.y * self.GridHeight + pos.y * self.GridHorizontalSpace
	else
		y = -1 * (pos.y * self.GridHeight + pos.y * self.GridHorizontalSpace)
	end
	return {x = x , y = y}
end



return CombGrid