---------------------------------------------
--- MSGTankDestory
--- Created by thrt520.
--- DateTime: 2018/8/28
---------------------------------------------
---@class MSGTankDestory : EventArgsBasics
local MSGTankDestory = require("Framework.Event.EventArgsBasics").Define("MSGTankDestory")
---@type BaseFightTank
MSGTankDestory.TankData = nil
return MSGTankDestory