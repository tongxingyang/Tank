---------------------------------------------
--- MSGShowTankTrajectory
--- Created by thrt520.
--- DateTime: 2018/5/17
---------------------------------------------
---@class MSGShowTankTrajectory : EventArgsBasics
local MSGShowTankTrajectory = require("Framework.Event.EventArgsBasics").Define("MSGShowTankTrajectory")
MSGShowTankTrajectory.StartPos = nil
MSGShowTankTrajectory.EndPos = nil
return MSGShowTankTrajectory