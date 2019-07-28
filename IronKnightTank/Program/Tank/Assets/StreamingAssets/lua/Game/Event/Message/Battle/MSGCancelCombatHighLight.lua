---------------------------------------------
--- MSGCancelCombatHighLight
--- Created by thrt520.
--- DateTime: 2018/5/19
---------------------------------------------
---@class MSGCancelCombatHighLight : EventArgsBasics
local MSGCancelCombatHighLight = require("Framework.Event.EventArgsBasics").Define("MSGCancelCombatHighLight")
MSGCancelCombatHighLight.DefTankId = 0
MSGCancelCombatHighLight.AtkTankId = 0
return MSGCancelCombatHighLight