---------------------------------------------
--- GameStarter
--- Created by Tianc.
--- DateTime: 2018/04/15
---------------------------------------------

---@class GameStarter
local GameStarter = {}
local Game = require("Game.Game")

---@type Scene 游戏入口场景
GameStarter.EntryScene = nil
---@type number 
GameStarter.Weight = 5
---@type LuaModuleTaskProxy
GameStarter.Proxy = nil

function GameStarter.StartTask()
    Game.SceneManager.SwicthScene(GameStarter.EntryScene, GameStarter._progress)
end

function GameStarter._progress(progress)
    GameStarter.Proxy:SetProgress(progress,"正在加载欢迎界面");
end

return GameStarter