---------------------------------------------
--- MSGMoveTankSingle
--- Created by thrt520.
--- DateTime: 2018/5/29
---------------------------------------------
---@class MSGMoveTankSingle : EventArgsBasics
local MSGMoveTankSingle = require("Framework.Event.EventArgsBasics").Define("MSGMoveTankSingle")
MSGMoveTankSingle.TankId = 0
MSGMoveTankSingle.TargetPos = nil
MSGMoveTankSingle.IsMine = false
MSGMoveTankSingle.Time = 0.2
return MSGMoveTankSingle