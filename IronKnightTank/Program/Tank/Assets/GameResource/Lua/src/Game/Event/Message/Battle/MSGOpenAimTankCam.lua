---------------------------------------------
--- MSGOpenAimTankCam
--- Created by thrt520.
--- DateTime: 2018/6/26
---------------------------------------------
---@class MSGOpenAimTankCam : EventArgsBasics
local MSGOpenAimTankCam = require("Framework.Event.EventArgsBasics").Define("MSGOpenAimTankCam")
MSGOpenAimTankCam.TankId = 0
MSGOpenAimTankCam.IsPlayer = false
MSGOpenAimTankCam.TankAngles = nil
MSGOpenAimTankCam.AimCofficient = 0
MSGOpenAimTankCam.RotateTime = 0


return MSGOpenAimTankCam