---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by JCY.
--- DateTime: 2018/10/16 23:01
---

---@class TankDataCfg
local TankDataCfg = class("TankDataCfg")

---@type number 坦克ID
TankDataCfg.TankID = 0;
---@type number 坦克消耗
TankDataCfg.TankUse = 0;
---@type number 坦克在当前战役下最大的招募数量
TankDataCfg.TankMaxCount = 0;

function TankDataCfg:ctor()
    self.TankID = 0;
    self.TankUse = 0;
    self.TankMaxCount = 0;
end

function TankDataCfg:UpdateInfo(info)
    self.TankID = info.TankID;
    self.TankUse = info.TankUse;
    self.TankMaxCount = info.TankMaxCount;
end

return TankDataCfg;