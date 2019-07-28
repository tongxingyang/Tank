---------------------------------------------
--- TankMoveData
---	坦克移动数据 包含移动的坦克id和移动路径
------只是作为数据的参考并不参与计算
--- Created by thrt520.
--- DateTime: 2018/5/15
---------------------------------------------
----@class TankMoveData
local TankMoveData = {}
---@type GridPos[]
TankMoveData.GridPosList = nil
---@type number
TankMoveData.TankId = nil

return TankMoveData