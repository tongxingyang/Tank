---------------------------------------------
--- MSGUpdateRecruitPanel
--- Created by thrt520.
--- DateTime: 2018/10/17
---------------------------------------------
---@class MSGUpdateRecruitPanel : EventArgsBasics
local MSGUpdateRecruitPanel = require("Framework.Event.EventArgsBasics").Define("MSGUpdateRecruitPanel")
MSGUpdateRecruitPanel.MaxSoliderCount = 0
MSGUpdateRecruitPanel.CurSoliderCount = 0
MSGUpdateRecruitPanel.MaxTankCount = 0
MSGUpdateRecruitPanel.CurTankCount = 0
MSGUpdateRecruitPanel.CurGold = 0
return MSGUpdateRecruitPanel