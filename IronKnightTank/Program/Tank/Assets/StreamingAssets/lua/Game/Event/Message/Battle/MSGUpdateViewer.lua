---------------------------------------------
--- MSGUpdateViewer
--- Created by thrt520.
--- DateTime: 2018/8/14
---------------------------------------------
---@class MSGUpdateViewer : EventArgsBasics
local MSGUpdateViewer = require("Framework.Event.EventArgsBasics").Define("MSGUpdateViewer")
---@type IViewer
MSGUpdateViewer.Viewer = nil
return MSGUpdateViewer