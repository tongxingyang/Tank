---------------------------------------------
--- MSGRotateTurret
--- Created by thrt520.
--- DateTime: 2018/6/1
---------------------------------------------
---@class MSGRotateTurret : EventArgsBasics
local MSGRotateTurret = require("Framework.Event.EventArgsBasics").Define("MSGRotateTurret")
MSGRotateTurret.TankId = 0
MSGRotateTurret.Angel = 0
MSGRotateTurret.Time = 0
MSGRotateTurret.IsMine = 0

return MSGRotateTurret