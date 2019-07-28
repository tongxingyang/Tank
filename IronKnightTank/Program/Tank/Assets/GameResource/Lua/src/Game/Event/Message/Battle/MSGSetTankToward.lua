---------------------------------------------
--- MSGSetTankToward
--- Created by thrt520.
--- DateTime: 2018/6/21
---------------------------------------------
---@class MSGSetTankToward : EventArgsBasics
local MSGSetTankToward = require("Framework.Event.EventArgsBasics").Define("MSGSetTankToward")
MSGSetTankToward.TankId = 0
MSGSetTankToward.Toward = 0
MSGSetTankToward.IsPlayer = false
return MSGSetTankToward