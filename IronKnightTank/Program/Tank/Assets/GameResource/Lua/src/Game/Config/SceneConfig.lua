---------------------------------------------
--- GameScene
--- Created by Tianc.
--- DateTime: 2018/04/15
---------------------------------------------

---@class SceneConfig
local SceneConfig = {}

SceneConfig.Welcome = {
    Name = "Welcome",
    Modules = {
        require("Framework.UI.ViewManager"),
        require("Framework.UI.PanelManager"),
        require("Game.Welcome.WelcomeSceneManager"),
        require("Game.User.Model.PlayerSettingModel"),
        require("Game.User.Model.PlayerModel"),
    },
}

SceneConfig.WorldMap = {
    Name = "WorldMap",
    Modules = {

    },
}

SceneConfig.Battle = {
    Name = "WorldMap",
    Modules = {

    },
}

return SceneConfig