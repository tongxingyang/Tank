---------------------------------------------
--- MSGRecruitTankRequest
--- Created by thrt520.
--- DateTime: 2018/10/18
---------------------------------------------
---@class MSGRecruitTankRequest : EventArgsBasics
local MSGRecruitTankRequest = require("Framework.Event.EventArgsBasics").Define("MSGRecruitTankRequest")
MSGRecruitTankRequest.TankData = nil
MSGRecruitTankRequest.Count = 0
return MSGRecruitTankRequest