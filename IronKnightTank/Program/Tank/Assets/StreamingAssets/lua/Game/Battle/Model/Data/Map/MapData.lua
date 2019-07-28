---------------------------------------------
--- MapData
---	所有的地图数据
--- Created by thrt520.
--- DateTime: 2018/5/10
---------------------------------------------
---@class MapData
local MapData = class("MapData")
---@type number
MapData.Height = 0
---@type number
MapData.Width = 0
---@type number
MapData.GridHeight = 0
---@type number
MapData.GridWidth = 0
---@type string
MapData.Name = 0
---@type number
MapData.Id = 0
---@type table<GridPos , BlockData>
MapData.BlockDataDic = nil
---@type EToward
MapData.DefaultToward = 0
---@type string
MapData.MapRes = 0
local BlockDataClass = require("Game.Battle.Model.Data.Map.BlockData")

---@param editorLevelData EditorLevelData
function MapData:ctor(editorLevelData , MapRes)
	self.Width = editorLevelData.Width
	self.Height = editorLevelData.Height
	self.Name = editorLevelData.Name
	self.GridHeight = editorLevelData.GridHeight
	self.GridWidth = editorLevelData.GridWidth
	self.DefaultToward = editorLevelData.DefaultToward
	self.BlockDataDic = { }
	local editorBlockDataList = editorLevelData.BlockDataList
	for i, v in pairs(editorBlockDataList) do
		local blockData = BlockDataClass.new(v)
		local key = blockData.Pos
		self.BlockDataDic[key] = blockData
	end
	self.MapRes = MapRes
end

return MapData