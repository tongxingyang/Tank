---------------------------------------------
--- MSGCreateNpc
--- Created by thrt520.
--- DateTime: 2018/8/28
---------------------------------------------
---@class MSGCreateNpc : EventArgsBasics
local MSGCreateNpc = require("Framework.Event.EventArgsBasics").Define("MSGCreateNpc")
MSGCreateNpc.NpcId = 0
MSGCreateNpc.Pos = 0
MSGCreateNpc.Toward = 0
return MSGCreateNpc