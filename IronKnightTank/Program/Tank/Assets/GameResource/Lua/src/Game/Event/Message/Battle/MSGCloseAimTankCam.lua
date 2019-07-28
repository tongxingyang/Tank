---------------------------------------------
--- MSGCloseAimTankCam
--- Created by thrt520.
--- DateTime: 2018/6/26
---------------------------------------------
---@class MSGCloseAimTankCam : EventArgsBasics
local MSGCloseAimTankCam = require("Framework.Event.EventArgsBasics").Define("MSGCloseAimTankCam")
MSGCloseAimTankCam.TankId = 0
MSGCloseAimTankCam.IsPlayer = 0
return MSGCloseAimTankCam