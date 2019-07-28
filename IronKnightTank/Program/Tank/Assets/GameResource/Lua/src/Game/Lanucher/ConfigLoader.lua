---------------------------------------------
--- ConfigLoader
--- Created by Tianc.
--- DateTime: 2018/04/13
---------------------------------------------

---@class ConfigLoader

local ConfigLoader = {}

require("Game.Config.AppConfig")
require("Game.Config.GameConfig")
require("Game.Config.ViewConfig")
require("Game.Config.PanelConfig")
require("Game.Config.ScriptConfig")

ConfigLoader.Weight = 5
---@type LuaModuleTaskProxy
ConfigLoader.Proxy = nil

function ConfigLoader.StartTask()
    ConfigLoader.Proxy:SetProgress(1,"");
end

return ConfigLoader