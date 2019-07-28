---------------------------------------------
--- MSGMoveTankRequest
--- Created by thrt520.
--- DateTime: 2018/6/20
---------------------------------------------
---@class MSGMoveTankRequest : EventArgsBasics
local MSGMoveTankRequest = require("Framework.Event.EventArgsBasics").Define("MSGMoveTankRequest")
MSGMoveTankRequest.TankId = 0
MSGMoveTankRequest.TargetPos = nil
return MSGMoveTankRequest