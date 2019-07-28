---------------------------------------------
--- MSGRevertTankTurret
--- Created by thrt520.
--- DateTime: 2018/6/1
---------------------------------------------
---@class MSGRevertTankTurret : EventArgsBasics
local MSGRevertTankTurret = require("Framework.Event.EventArgsBasics").Define("MSGRevertTankTurret")
MSGRevertTankTurret.TankId = 0
MSGRevertTankTurret.IsMine = false
MSGRevertTankTurret.Time = false

return MSGRevertTankTurret