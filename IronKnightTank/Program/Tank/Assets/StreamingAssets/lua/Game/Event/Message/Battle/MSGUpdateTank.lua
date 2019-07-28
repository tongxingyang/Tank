---------------------------------------------
--- MSGUpdateTank
--- Created by thrt520.
--- DateTime: 2018/5/16
---------------------------------------------
---@class MSGUpdateTank : EventArgsBasics
local MSGUpdateTank = require("Framework.Event.EventArgsBasics").Define("MSGUpdateTank")
MSGUpdateTank.TankDataArray = nil
MSGUpdateTank.IsMine = false
return MSGUpdateTank