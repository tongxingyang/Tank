---------------------------------------------
--- MSGDisplayFight
--- Created by thrt520.
--- DateTime: 2018/5/18
---------------------------------------------
---@class MSGDisplayFight : EventArgsBasics
local MSGDisplayFight = require("Framework.Event.EventArgsBasics").Define("MSGDisplayFight")
---@type FightResult
MSGDisplayFight.FightRes = nil
MSGDisplayFight.AtkSliderValue = nil
MSGDisplayFight.DefSliderValue = nil
MSGDisplayFight.Des = nil
MSGDisplayFight.Time = nil
return MSGDisplayFight