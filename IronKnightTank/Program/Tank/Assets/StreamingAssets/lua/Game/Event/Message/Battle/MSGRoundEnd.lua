---------------------------------------------
--- MSGRoundEnd
--- Created by thrt520.
--- DateTime: 2018/5/22
---------------------------------------------
---@class MSGRoundEnd : EventArgsBasics
local MSGRoundEnd = require("Framework.Event.EventArgsBasics").Define("MSGRoundEnd")
MSGRoundEnd.CurRound = 0
return MSGRoundEnd