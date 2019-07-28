--主入口函数。从这里开始lua逻辑
local Game = {}
require("Common.define")
require("Game.Tools.StringTools")
---------------------------------------------------------------
--- 游戏场景定义
---------------------------------------------------------------
Game.Battle = {
    Name = "Battle",
    Actors = {
        require("Game.Input.InputMgr"),
        require("Framework.UI.ViewManager"),
        require("Framework.UI.PanelManager"),
        require("Game.Battle.BattleSceneManager"),
        require("Game.Battle.Model.MapModel"),
        require("Game.Battle.Model.TankModel"),
        require("Game.Battle.Model.ShadowModel"),
        require("Game.Battle.Model.SkillModel"),
        require("Game.Battle.View.BattleViewMgr"),
        require("Game.Battle.Logic.BattleFieldMgr"),
    },
}

Game.Welcome = {
    Name = "Welcome",
    Actors = {
        require("Framework.UI.ViewManager"),
        require("Framework.UI.PanelManager"),
        require("Game.Welcome.Logic.WelcomeManager"),
    },
}

---@type SceneManager 场景管理器
Game.SceneManager = require("Framework.Scene.SceneManager")

math.randomseed(os.time())

---------------------------------------------------------------
--- 游戏入口
---------------------------------------------------------------
function Game.Lanuch(lanucher)
    --配置加载
    local configLoader = require("Game.Lanucher.ConfigLoader")
    --基础模块建设
    local infrastructureTask = require("Game.Lanucher.InfrastructureTask")
    --游戏场景启动
    local gameStarter = require("Game.Lanucher.GameStarter")

    local jsonDataLoader = require("Game.Lanucher.JsonDataLoader")
    gameStarter.EntryScene = Game.Welcome
    lanucher:AddTask(configLoader)
    lanucher:AddTask(jsonDataLoader)
    lanucher:AddTask(infrastructureTask)
    lanucher:AddTask(gameStarter)
    lanucher:Lanuch()

end

return Game