---------------------------------------------
--- MSGTankFireRequest
--- Created by thrt520.
--- DateTime: 2018/6/21
---------------------------------------------
---@class MSGTankFireRequest : EventArgsBasics
local MSGTankFireRequest = require("Framework.Event.EventArgsBasics").Define("MSGTankFireRequest")
MSGTankFireRequest.AimPos = nil
MSGTankFireRequest.Val = 0
MSGTankFireRequest.HitRes = 0  ----0 miss 1 hit 2 weakness
return MSGTankFireRequest