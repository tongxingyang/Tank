---------------------------------------------
--- MSGTankAtkRequeset
--- Created by thrt520.
--- DateTime: 2018/6/21
---------------------------------------------
---@class MSGTankAtkRequeset : EventArgsBasics
local MSGTankAtkRequeset = require("Framework.Event.EventArgsBasics").Define("MSGTankAtkRequeset")
MSGTankAtkRequeset.AtkTankId = 0
MSGTankAtkRequeset.DefTankId = 0
MSGTankAtkRequeset.IsFocus = false
return MSGTankAtkRequeset