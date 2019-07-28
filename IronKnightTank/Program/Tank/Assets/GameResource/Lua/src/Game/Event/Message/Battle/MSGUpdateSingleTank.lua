---------------------------------------------
--- MSGUpdateSingleTank
--- Created by thrt520.
--- DateTime: 2018/5/15
---------------------------------------------
---@class MSGUpdateSingleTank : EventArgsBasics
local MSGUpdateSingleTank = require("Framework.Event.EventArgsBasics").Define("MSGUpdateSingleTank")
---@type BaseFightTank
MSGUpdateSingleTank.TankData = nil
return MSGUpdateSingleTank