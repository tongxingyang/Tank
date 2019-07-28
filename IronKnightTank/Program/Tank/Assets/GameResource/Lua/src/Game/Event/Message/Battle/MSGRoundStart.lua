---------------------------------------------
--- MSGRoundStart
--- Created by thrt520.
--- DateTime: 2018/5/22
---------------------------------------------
---@class MSGRoundStart : EventArgsBasics
local MSGRoundStart = require("Framework.Event.EventArgsBasics").Define("MSGRoundStart")
MSGRoundStart.CurRound = 0
return MSGRoundStart