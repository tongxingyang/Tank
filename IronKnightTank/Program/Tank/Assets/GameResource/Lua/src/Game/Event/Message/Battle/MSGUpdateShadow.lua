---------------------------------------------
--- MSGUpdateShadow
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class MSGUpdateShadow : EventArgsBasics
local MSGUpdateShadow = require("Framework.Event.EventArgsBasics").Define("MSGUpdateShadow")
---@type ShadowNodeData[]
MSGUpdateShadow.ShadowDataList = nil
return MSGUpdateShadow