---------------------------------------------
--- MapConfig
---	地图数据脚本，提供游戏中位置信息从数据空间到真实游戏空间的转换
--- Created by thrt520.
--- DateTime: 2018/7/5
---------------------------------------------
---@class MapConfig
local MapConfig = {}
local this = MapConfig
this._centerX = 0
this._centerY = 0
this._gridHeight = 0
this._gridWith = 0
this.DefaultToward = 0

function MapConfig.SetMapWidthAndHeight(mapHeight , mapWidth , gridHeight , gridWidth)
	this._centerX = (mapWidth - 1) / 2
	this._centerY = (mapHeight - 1) / 2
	this._gridHeight = gridHeight
	this._gridWith = gridWidth
end

---@param pos GridPos
MapConfig.GetWorldPos = function(pos)
	return Vector3.New((pos.x - this._centerX) * this._gridWith , 0 , (pos.y - this._centerY) * this._gridHeight )
end

return MapConfig