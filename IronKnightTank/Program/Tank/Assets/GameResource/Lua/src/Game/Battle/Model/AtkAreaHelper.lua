---------------------------------------------
--- AtkAreaHelper
--- Created by thrt520.
--- DateTime: 2018/9/12
---------------------------------------------
---@class AtkAreaHelper
local AtkAreaHelper = {}

local MapModel = require("Game.Battle.Model.MapModel")

---获取可攻击范围  固定炮塔
---@param pos GridPos
---@param atkDis number
---@param toward EToward
---@param atkType ETurretType
function AtkAreaHelper.GetFixedAtkArea(pos , atkDis , toward )
	local startPos = {}
	local towardVec = EToward.GetVector(toward)
	local tangentVec = towardVec:GetTangentVec()
	for i = -1, 1 do
		local t =  towardVec + i * tangentVec
		local sum = pos + GridPos.Normalize(t)
		local gridPos = GridPos.New(sum.x , sum.y)
		local blockData = MapModel.GetBlockData(gridPos)
		if blockData and  not blockData.Blocking then
			table.insert(startPos , gridPos)
		end
	end
	local atkArea = {}
	for i, v in pairs(startPos) do
		table.insert(atkArea , v)
	end
	local cullingList = {}
	for i = 1, atkDis - 1 do
		for j = #startPos, 1 , -1 do
			if not table.contains(cullingList , j) then
				local pos = GridPos.New(startPos[j].x + towardVec.x * i , startPos[j].y + towardVec.y * i)
				--local blockData = MapModel.GetBlockData(pos)
				--if blockData then
				table.insert(atkArea , pos)
					--if blockData.Blocking then
					--	table.insert(cullingList , j);
				--end
				--end
			end
		end
	end

	return atkArea
end


----判斷两点中是否有阻挡
---@param pos1 GridPos
---@param pos2 GridPos
---@param tank BaseFightTank
function AtkAreaHelper.IsAtkBlock( tank , pos2)
	local pos1 = tank.Pos
	local xLower , xHigher , yLower , yHigher
	if pos1.x > pos2.x then
		xLower , xHigher = pos2.x , pos1.x
	else
		xLower , xHigher = pos1.x , pos2.x
	end
	if pos1.y > pos2.y then
		yLower , yHigher = pos2.y , pos1.y
	else
		yLower , yHigher = pos1.y , pos2.y
	end

	for i = xLower , xHigher  do
		for j = yLower , yHigher  do
			local pos = GridPos.New(i , j)
			if pos == pos1 or pos == pos2 then
			else
				local blockData = MapModel.GetBlockData(pos)
				if blockData then
					local blocking = blockData.Blocking
					local mover = MapModel.GetMover(pos)
					if mover then
						blocking = blocking or mover:IsBlockAtk(tank)
					end
					if blocking and AtkAreaHelper._judgeLineCrossTangle(pos  , pos1 , pos2) then
						return true
					end
				end
			end
		end
	end
	return false
end


---判断直线是否经过四边形
function AtkAreaHelper._judgeLineCrossTangle(girdPos , pos1 , pos2)
	local posX , posY= girdPos.x , girdPos.y
	local leftUp = {x = posX - 0.5 , y = posY + 0.5}
	local rightUp = {x = posX + 0.5 , y = posY + 0.5}
	local leftDown = {x = posX - 0.5 , y = posY - 0.5}
	local rightDown = {x = posX + 0.5 , y = posY - 0.5}
	local posList = {}
	table.insert(posList , leftUp)
	table.insert(posList , leftDown)
	table.insert(posList , rightUp)
	table.insert(posList , rightDown)
	local dx = pos2.x - pos1.x
	local dy = pos2.y - pos1.y
	local isLeft = ((pos1.y + dy * (posX - pos1.x) / dx)> posY)
	for i, v in pairs(posList) do
		local val = (pos1.y + dy * (v.x - pos1.x) / dx)- v.y
		if val == 0 then   ---如果点在直线上 跳过 不阻挡
		else
			local bool = val > 0
			if bool ~= isLeft then
				return  true
			end
		end
	end
	return false
end

return AtkAreaHelper