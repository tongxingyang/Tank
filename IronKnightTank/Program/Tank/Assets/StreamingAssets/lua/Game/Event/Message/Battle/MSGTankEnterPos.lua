---------------------------------------------
--- MSGTankEnterPos
--- Created by thrt520.
--- DateTime: 2018/8/28
---------------------------------------------
---@class MSGTankEnterPos : EventArgsBasics
local MSGTankEnterPos = require("Framework.Event.EventArgsBasics").Define("MSGTankEnterPos")
---@type BaseFightTank
MSGTankEnterPos.TankData = nil
---@type GridPos
MSGTankEnterPos.Pos = nil
return MSGTankEnterPos