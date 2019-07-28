---------------------------------------------
--- MSGRotateTank
--- Created by thrt520.
--- DateTime: 2018/5/29
---------------------------------------------
---@class MSGRotateTank : EventArgsBasics
local MSGRotateTank = require("Framework.Event.EventArgsBasics").Define("MSGRotateTank")
MSGRotateTank.TankId = 0
---@type Vector3
MSGRotateTank.Angel = 0
MSGRotateTank.Toward = 0
MSGRotateTank.IsMine = false
MSGRotateTank.Time = 0.1
return MSGRotateTank