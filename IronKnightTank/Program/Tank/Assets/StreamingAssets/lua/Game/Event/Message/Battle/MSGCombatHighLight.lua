---------------------------------------------
--- MSGCombatHight
--- Created by thrt520.
--- DateTime: 2018/5/19
---------------------------------------------
---@class MSGCombatHighLight : EventArgsBasics
local MSGCombatHighLight = require("Framework.Event.EventArgsBasics").Define("MSGCombatHighLight")
---@type BaseFightTank
MSGCombatHighLight.AtkTankId = nil
---@type BaseFightTank
MSGCombatHighLight.DefTankId = nil
return MSGCombatHighLight