---------------------------------------------
--- GridPos
---	坦克中最基本的坐标单位，封装了一些常用的数学库
--- Created by thrt520.
--- DateTime: 2018/5/11
---------------------------------------------
---@class GridPos
GridPos = {}
local _posPool = {}
GridPos.x = 0
GridPos.y = 0

GridPos.__index = function(t,k)
	local var = rawget(GridPos, k)
	return var
end

function GridPos.New(x , y)
	local key = GridPos._getKey(x , y)
	if _posPool[key] then
		return _posPool[key]
	else
		local t = {x = x or 0, y = y or 0}
		setmetatable(t, GridPos)
		_posPool[key] = t
		return t
	end
end

function GridPos._getKey(x , y)
	return x*100000 + y
end

local new = GridPos.New

GridPos.__call = function(t , x , y)
	local t = {x = x or 0, y = y or 0}
	setmetatable(t, GridPos)
	return t
end

GridPos.__tostring = function(t)
	return t.x ..","..t.y
end

GridPos.__add = function(a , b)
	return setmetatable({x = a.x+b.x , y = a.y + b.y} , GridPos)
end

GridPos.__div = function(va , d)

	return setmetatable({x = va.x / d, y = va.y / d}, GridPos)
end

GridPos.__mul = function(a, d)
	if type(d) == "number" then
		return setmetatable({x = a.x * d, y = a.y * d}, GridPos)
	else
		return setmetatable({x = a * d.x, y = a * d.y}, GridPos)
	end
end

GridPos.__sub = function(a, b)
	return new(a.x - b.x, a.y - b.y)
end


GridPos.__unm = function(v)
	return setmetatable({x = -v.x, y = -v.y}, GridPos)
end

------isRound true 返回角度在0~360 false 返回角度在0~180
---@param a GridPos
---@param b GridPos
function GridPos.GetAngleBetweenTwoVec(a , b , isRound)
	if not isRound then
		local radians = (a.x * b.x + a.y * b.y) / (a:Magnitude() * b:Magnitude())
		return math.deg(radians)
	else
		local a1 = math.deg( math.atan2(a.y , a.x))
		local a2 = math.deg( math.atan2(b.y , b.x))
		local angle = a1 - a2
		if angle <0 then
			angle = 360 + angle
		end
		return angle
	end
end

function GridPos:Set(x , y)
	self.x = x
	self.y = y
end

function GridPos:Get()
	return self.x , self.y
end


function GridPos:GetAngle()
	local radians = math.atan2(  self.y , self.x)
	return radians * 180 / math.pi
end

----获取上下左右四个位置
function GridPos:GetQuadPos()
	return {
		GridPos.New(self.x -1 , self.y), 	---left
		GridPos.New(self.x +1 , self.y), 	---right
		GridPos.New(self.x , self.y +1), 	---up
		GridPos.New(self.x , self.y -1), 	---down
	}
end

---获取以自身位置为中心的菱形区域内的所有地块
function GridPos:GetMultiQuadPos(area)
	local posList = {}
	for i = -1 * area, area do
		local x = area - math.abs(i)
		for j = -1 * x, x do
			table.insert(posList , GridPos.New(i + self.x , j + self.y))
		end
	end
	return posList
end

---获取以自身位置为中心的菱形区域边缘地块
function GridPos:GetQuadLinePos(area)
	local posList = {}
	for i = -1 * area, area do
		local y = area - math.abs(i)
		table.insert(posList , GridPos.New(i + self.x , y + self.y))
		if y ~= 0 then
			table.insert(posList , GridPos.New(i + self.x , -y + self.y))
		end
	end
	return posList
end


---归一化 ， x ,y 如果不为0就除以他的绝对值
function GridPos.Normalize(p)
	local a , b
	if p.x == 0 then
		a = 0
	elseif p.x > 0 then
		a = 1
	else
		a = -1
	end

	if p.y == 0 then
		b = 0
	elseif p.y > 0 then
		b = 1
	else
		b = -1
	end
	return setmetatable({x = a or 0 , y = b or 0} , GridPos)
end

function GridPos:SqrMagnitude()
	return self.x * self.x + self.y * self.y
end

function GridPos:Magnitude()
	return math.sqrt(self.x * self.x + self.y * self.y)
end

----获取向量代表的朝向
function GridPos:GetToward()
	local pi = math.pi
	local radians = math.atan2(self.y , self.x)
	if radians >= pi * -1 / 8 and radians <pi * 1 / 8 then
		return EToward.Right
	elseif radians >= pi * 1 / 8 and radians <  pi * 3 / 8 then
		return EToward.RightUp
	elseif radians >= pi * 3 / 8 and radians <  pi * 5 / 8 then
		return EToward.Up
	elseif radians >= pi * 5 / 8 and radians <  pi * 7 / 8 then
		return EToward.LeftUp
	elseif radians < pi * -7 / 8 or radians >=  pi * 7 / 8 then
		return EToward.Left
	elseif radians >= pi * -7 / 8 and radians <  pi * -5 / 8 then
		return EToward.LeftDown
	elseif radians >= pi * -5 / 8 and radians <  pi * -3 / 8 then
		return EToward.Down
	elseif radians >= pi * -3 / 8 and radians <  pi * -1 / 8 then
		return EToward.RightDown
	else
		return 0
	end
end

---获得切向量
function GridPos:GetTangentVec()
	if self.x == 0 then
		return GridPos.New(1 , 0)
	elseif self.y == 0 then
		return GridPos.New(0 , 1)
	else
		return GridPos.New(-1 * self.x , self.y)
	end
end

return GridPos