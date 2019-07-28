---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by JCY.
--- DateTime: 2018/10/12 9:52
---

---@class SettingData
local SettingData = class("SettingData")

---@type number 玩家保存音量设置
SettingData.MusicPriority = 0;
---@type number 玩家保存的音乐音质 感觉没卵用
SettingData.MusicQuality = 0;
---@type table 当前玩家启用的语言
SettingData.CurrLanguage = {};
---@type table 当前玩家保存的画质质量，暂定6个固定档位
SettingData.RenderQuality = {};

function SettingData:ctor()
    self.MusicPriority = 100;
    self.MusicQuality = 100;
    --TODO 根据地理位置判断
    self.CurrLanguage = DataConst.RenderQuality.CurrLanguage;
    --TODO
    self.RenderQuality = DataConst.RenderQuality.Medium;
end

function SettingData:UpdateInfo(info)
    self.MusicPriority = info.MusicPriority;
    self.MusicQuality = info.MusicQuality;
    --TODO 根据地理位置判断
    self.CurrLanguage = info.CurrLanguage;
    --TODO
    self.RenderQuality = info.RenderQuality;
end

function SettingData:SetMusicPriority(index)
    self.MusicPriority = index;
end

function SettingData:SetMusicQuality(index)
    self.MusicQuality = index;
end

function SettingData:SetCurLanguage(index)
    self.CurLanguage = index;
end

function SettingData:SetRenderQuality(index)
    self.RenderQuality = index;
end

return SettingData;