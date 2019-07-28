---
---编队相关
---Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by JCY.
--- DateTime: 2018/10/16 19:18
---
---@class BattleTeamData
local BattleTeamData = class("BattleTeamData")

---@type number 士兵ID
BattleTeamData.SoliderID = 0;
---@type number 坦克ID
BattleTeamData.TankID = 0;

function BattleTeamData:ctor()
    self.SoliderID = 0;
    self.TankID = 0;
end

function BattleTeamData:UpdateInfo(soliderID,tankID)
    self.SoliderID =soliderID;
    self.TankID = tankID;
end

return BattleTeamData;