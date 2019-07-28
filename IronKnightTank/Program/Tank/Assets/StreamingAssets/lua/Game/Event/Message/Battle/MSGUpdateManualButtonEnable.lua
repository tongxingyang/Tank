---------------------------------------------
--- MSGUpdateManualButtonEnable
--- Created by thrt520.
--- DateTime: 2018/7/24
---------------------------------------------
---@class MSGUpdateManualButtonEnable : EventArgsBasics
local MSGUpdateManualButtonEnable = require("Framework.Event.EventArgsBasics").Define("MSGUpdateManualButtonEnable")
MSGUpdateManualButtonEnable.Power = 0
MSGUpdateManualButtonEnable.CanAtk = false
MSGUpdateManualButtonEnable.FocusAtkButtonVisiable = false
MSGUpdateManualButtonEnable.AtkButtonVisiable = false
MSGUpdateManualButtonEnable.MoveButtonVisiable  = false
MSGUpdateManualButtonEnable.MoveButtonEnable = false
MSGUpdateManualButtonEnable.AtkButtonEnable = false
MSGUpdateManualButtonEnable.FocusAtkButtonEnable = false
MSGUpdateManualButtonEnable.SkipButtonEnable = false
return MSGUpdateManualButtonEnable