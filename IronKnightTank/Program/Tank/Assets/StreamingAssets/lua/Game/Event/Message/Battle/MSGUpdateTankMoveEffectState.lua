---------------------------------------------
--- MSGUpdateTankMoveEffectState
--- Created by thrt520.
--- DateTime: 2018/8/22
---------------------------------------------
---@class MSGUpdateTankMoveEffectState : EventArgsBasics
local MSGUpdateTankMoveEffectState = require("Framework.Event.EventArgsBasics").Define("MSGUpdateTankMoveEffectState")
MSGUpdateTankMoveEffectState.IsActive = false
MSGUpdateTankMoveEffectState.TankId = 0
return MSGUpdateTankMoveEffectState