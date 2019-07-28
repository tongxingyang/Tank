---------------------------------------------
--- MapModel
--- Created by thrt520.
--- DateTime: 2018/5/2
---------------------------------------------
---@class MapModel : SceneActor
local MapModel = {}
local this = MapModel

local MapDataClass = require("Game.Battle.Model.Data.Map.MapData")
local MapConfig = require("Game.Battle.Config.MapConfig")

---@type table<GridPos , BlockData> 地块数据
this._blockDataDic = {}
---@type MapData 地图数据
this._mapData  = nil
---@type table<GridPos , IMover> 所有mover
this._moverDic = {}
---@type table 地图路径数据
this._mapPathData = {}

function MapModel.OnIntoScene()
	this._init(require(BattleConfig.LevelFolderPath..BattleConfig.MapScriptName))
end

function MapModel.OnLeaveScene()
	this.Dispose()
end

---@param levelData EditorLevelData
function MapModel._init(levelData)
	local tLevel = JsonDataMgr.GetLevelData(BattleConfig.LevelId)
	---@type LevelData
	local mapRes = tLevel.Map_Resource
	this._mapData = MapDataClass.new(levelData  , mapRes)
	this._blockDataDic = this._mapData.BlockDataDic
	MapConfig.SetMapWidthAndHeight(levelData.Height , levelData.Width , levelData.GridHeight , levelData.GridWidth)
	MapConfig.DefaultToward = levelData.DefaultToward
	this._initPathData()
end

function MapModel._initPathData()
	for i, v in pairs(this._blockDataDic) do
		local t = {}
		this._mapPathData[i] = t
		local around = this.GetAroundBlock(i)
		for pos, v in pairs(around) do
			t[v.Pos] = v.Cost
		end
	end
end

---获取地图数据
---@return MapData
function MapModel.GetMapData()
	return this._mapData
end

---获取地块数据
---@param pos GridPos
---@return BlockData
function MapModel.GetBlockData(pos)
	if this._blockDataDic[pos] then
		return this._blockDataDic[pos]
	end
end

---获取所有地块数据
---@param pos GridPos
---@return table<GridPos , BlockData>
function MapModel.GetAllBlockData()
	return this._blockDataDic
end


---更新地图数据 计算之前要刷一遍地图信息
---@param mover IMover
function MapModel.UpdateMapPathData(mover)
	for i, blockData in pairs(this._blockDataDic) do
		blockData.CostSpeed = -1
		blockData.CostPower = -1
		blockData.parentNode = nil
	end
	---@type table<pos , BlockData>
	local book = {} 			----已完成计算记录
	---@type table<BlockData , number>
	local costRecord = {}		----未完成计算节点
	local startNode = this._blockDataDic[mover.Pos]
	startNode.CostSpeed = 0
	costRecord[startNode] = 0
	local index = startNode.Pos
	while index do
		local min = 100000
		local isGet = false
		local parent
		for i, v in pairs(costRecord) do
			if not book[i.Pos] and v < min and v ~= -1 then
				min = v
				index = i.Pos
				parent = i
				isGet = true
			end
		end
		if not isGet then
			break
		end
		if not book[index] then
			book[index] = 1
			for i, v in pairs(costRecord) do
				if this._mapPathData[index][i.Pos] then
					local moveCost = mover:GetMoveCost(i)
					if v > moveCost + min then
						costRecord[i] = moveCost + min
						i.CostSpeed = moveCost + min
						i.parentNode = parent
					end
				end
			end
			local around = this.GetAroundBlock(index)
			for i, v in pairs(around) do
				if not costRecord[v] and v.CanPass then
					local m = mover:GetMoveCost(v)
					local val = min + m
					costRecord[v] = val
					v.CostSpeed = val
					v.parentNode = parent
					--if this._isMoverBlock(mover , v.Pos) then
					--	v.CostSpeed = -1
					--end
				end
			end
		end
	end
	local speed = mover:GetMoveSpeed()
	for i, v in pairs(costRecord) do
		i.CostPower = math.ceil( i.CostSpeed / speed)
	end
end

function MapModel.GetPath(targetPos)
	local blockData = this.GetBlockData(targetPos)
	local path = {}
	table.insert(path , blockData.Pos)
	while blockData.parentNode  do
		 table.insert(path , blockData.parentNode.Pos)
		blockData = blockData.parentNode
	end
	return path
end


---获取可移动区域
---@param power number 消耗的能量点数
---@return GridPos[]
function MapModel.GetCanMoveArea(power)
	local t = {}
	for i, v in pairs(this._blockDataDic) do
		if v.CostPower <= power and not  v.IsHold and  v.CostPower > 0 then
			table.insert(t  , v.Pos)
		end
	end
	return t
end

---在指定区域指定类型高亮
function MapModel.HighLightArea(type , pos , area)
	local around = this.GetAroundBlock(pos , area)
	for i, v in pairs(around) do
		v[type] = true
	end
end

---取消区域指定类型高亮
function MapModel.CancelHighLightArea(type , pos , area)
	local around = this.GetAroundBlock(pos , area)
	for i, v in pairs(around) do
		v[type] = false
	end
end

---取消所有指定类型的高亮
function MapModel.CancelHighLight(type)
	for i, v in pairs(this._blockDataDic) do
		v[type] = false
	end
end

---高亮指定区域为指定类型
---@param posList GridPos[]
function MapModel.HighLightTargetArea(type , posList)
	for i, v in pairs(posList) do
		local blockData = this.GetBlockData(v)
		if blockData then
			blockData.SkillReleaseArea = true
		end
	end
end

----高亮可移动区域
---@param mover IMover
function MapModel.HighLightCanMoveArea(power)
	this.CancelMoveableHighLight()
	local closeList = this.GetCanMoveArea(power)
	for i, v in pairs(closeList) do
		local blockData = this._blockDataDic[v]
		blockData.IsMoveableArea = true
	end
	return closeList
end

----取消可移动区域高亮
function MapModel.CancelMoveableHighLight()
	for i, v in pairs(this._blockDataDic) do
		v.IsMoveableArea = false
	end
end


----布置mover
---@param mover IMover
function MapModel.ArrangeMover(mover , pos)
	if table.contains(this._moverDic , mover) then
		this.RemoveMover(mover)
	end
	local block = this.GetBlockData(pos)
	if block then
		this._moverDic[pos] = mover
		block.IsHold = true
	else
		clog("MapModel.ArrangeMover block nil wrong"..tostring(pos))
	end
end


----移除mover
function MapModel.RemoveMover(mover)
	local pos = mover.Pos
	local block = this.GetBlockData(pos)
	if block then
		block.IsHold = false
		this._moverDic[pos] = nil
	else
		clog("block  not exit "..tostring(pos))
	end
end

----获取指定位置的mover
---@return IMover
function MapModel.GetMover(pos)
	return this._moverDic[pos]
end


---高亮移动路径  已弃用
---@param pos GridPos
---@param targetPos GridPos
---@param mover IMover
function MapModel.HighLightMovePath(pos , targetPos , mover )
	this.CancelHighLightMovePath()
	local path = this.GetMovePath(pos , targetPos , mover)
	for i, v in pairs(path) do
		this._blockDataDic[v].IsMovePath = true
	end
	this._blockDataDic[targetPos].IsMovePathEnd = true
	return path
end

---取消高亮移动路径 已弃用
function MapModel.CancelHighLightMovePath()
	for i, v in pairs(this._blockDataDic) do
		v.IsMovePath = false
		v.IsMovePathEnd = false
	end
end


---移动路径
---@param pos GridPos
---@param targetPos GridPos
---@param mover IMover
function MapModel.GetMovePath(pos , targetPos , mover)
	if pos == targetPos then
		clog("target pos same to pos return nil ")
		return nil
	end
	local path = this._getPath(pos , targetPos , mover)
	if not path then
		clog("paht nil wrong : ".."  tankPos:"..tostring(pos).."  targetPos:"..tostring(targetPos).."}")
	end
	return path
end


---指定位置是否有mover挡路
---@param mover IMover
function MapModel._isMoverBlock(mover , pos)
	local holder = this.GetMover(pos)
	if holder then
		return holder:IsBlockMove(mover)
	end
	return false
end


-------------------------------------------------------------------------------------
------获取移动路径   A*
---@param startPos GridPos
---@param endPos GridPos
---@param mover IMover
---@return BlockData[]
function MapModel._getPath(startPos , endPos , mover )
	if not this._assertBlock(startPos) or not this._assertBlock(endPos) then
		return
	end
	local startBlock = this._blockDataDic[startPos]
	local endBlock = this._blockDataDic[endPos]
	---@type BlockData
	local curBlock
	---@type BlockData[]
	local openList = { startBlock }
	local closeList = {}
	while #openList > 0 do
		curBlock = openList[1]
		for i = 2, #openList do
			if openList[i]:GetFCost() <= curBlock:GetFCost() and openList[i].hCost > curBlock.hCost then
				curBlock = openList[i]
			end
		end
		table.removeValue(openList , curBlock)
		table.insert(closeList , curBlock)
		if curBlock == endBlock then
			return this._genPath(startBlock, endBlock )
		end
		local aroundBlock = this.GetAroundBlockEightToward(curBlock.Pos , 1)
		for i, blockData in pairs(aroundBlock) do
			if not blockData.CanPass  or table.contains(closeList , blockData) or this._isMoverBlock(mover , blockData.Pos) then
			else
				local newcost = curBlock.gCost + this._getDis(curBlock , blockData)
				if newcost < blockData.gCost or (not table.contains(openList , blockData)) then
					blockData.gCost = newcost
					blockData.hCost = this._getDis(blockData, endBlock)
					blockData.parent = curBlock
					if not table.contains(openList , blockData) then
						table.insert(openList , blockData)
					end
				end
			end
		end
	end
end

---@param block1 BlockData
---@param block2 BlockData
function MapModel._getDis(block1 , block2)
	local xDis = math.abs( block2.Pos.x -block1.Pos.x)
	local yDis = math.abs( block2.Pos.y -block1.Pos.y)
	if xDis > yDis then
		return 14* yDis + 10 *(xDis - yDis)
	else
		return 14* xDis + 10 *(yDis - xDis)
	end
end

---@param startBlock BlockData
---@param endBlock BlockData
---@return GridPos[]
function MapModel._genPath(startBlock , endBlock )
	local path = {}
	local temp = endBlock
	while temp ~= startBlock do
		table.insert(path , temp.Pos)
		temp = temp.parent
	end

	table.insert(path , startBlock.Pos)

	return table.reverse(path)
end
-------------------------------------------------------------------------------------
----block断言
---@return boolean
function MapModel._assertBlock(pos)
	if not pos then
		return
	end
	return this._blockDataDic[pos] ~= nil
end

----获取范围为area的菱形区域的block
---@param pos GridPos
---@param area number
---@return BlockData[]
function MapModel.GetAroundBlock(pos , area)
	local t
	if not area then
		t = pos:GetQuadPos()
	else
		t = pos:GetMultiQuadPos(area)
	end
	local blockList = {}
	for i, pos in pairs(t) do
		if this._assertBlock(pos) then
			table.insert(blockList, this._blockDataDic[pos])
		end
	end
	return blockList
end

---获取八方向范围内的地块
function MapModel.GetAroundBlockEightToward(pos)
	local t = {}
	for i = -1, 1 do
		for j = -1, 1 do
			if i ~= 0 or j ~= 0 then
				table.insert(t , GridPos.New(pos.x + i , pos.y + j))
			end
		end
	end
	local blockList = {}
	for i, pos in pairs(t) do
		if this._assertBlock(pos) then
			table.insert(blockList, this._blockDataDic[pos])
		end
	end
	return blockList
end

----获取可用的地块  條件： 没有mover  可以移动
---@param pos GridPos
function MapModel.GetAvailablePos(pos)
	local area = 0
	local realpos
	while not realpos do
		local posList = pos:GetQuadLinePos(area)
		for i, v in ipairs(posList) do
			clog("check pos "..tostring(v))
			local blockData = this.GetBlockData(v)
			if blockData and not this.GetMover(v) and  blockData.CanPass then
				return v
			else
				clog("pos 不合格"..tostring(pos))
			end
		end
		area = area + 1
	end
end

function MapModel.Dispose()
	this._blockDataDic ={}
	this._mapPathData = {}
	this._mapData = nil
	this._moverDic = {}
end

return MapModel