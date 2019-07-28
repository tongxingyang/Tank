---------------------------------------------
--- MSGInitBattle
--- Created by thrt520.
--- DateTime: 2018/5/4
---------------------------------------------
---@class MSGInitBattle : EventArgsBasics
local MSGInitBattle = require("Framework.Event.EventArgsBasics").Define("MSGInitBattle")

MSGInitBattle.PlayerTankData = nil
MSGInitBattle.NpcTankData = nil
---@type MapData
MSGInitBattle.MapData = nil
---@type ShadowNodeData[]
MSGInitBattle.ShadowDataList = nil
---@type BaseFightTank[]
MSGInitBattle.TankData = nil

return MSGInitBattle