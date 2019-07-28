---------------------------------------------
--- MSGCreateTank
--- Created by thrt520.
--- DateTime: 2018/7/30
---------------------------------------------
---@class MSGCreateTank : EventArgsBasics
local MSGCreateTank = require("Framework.Event.EventArgsBasics").Define("MSGCreateTank")
MSGCreateTank.BaseFightTankDataList = nil
return MSGCreateTank