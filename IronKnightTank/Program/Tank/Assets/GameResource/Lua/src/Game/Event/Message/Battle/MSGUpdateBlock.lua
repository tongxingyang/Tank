---------------------------------------------
--- MSGUpdateBlock
--- Created by thrt520.
--- DateTime: 2018/6/11
---------------------------------------------
---@class MSGUpdateBlock : EventArgsBasics
local MSGUpdateBlock = require("Framework.Event.EventArgsBasics").Define("MSGUpdateBlock")
---@type table<GridPos , BlockData>
MSGUpdateBlock.BlockDataDic = nil
return MSGUpdateBlock