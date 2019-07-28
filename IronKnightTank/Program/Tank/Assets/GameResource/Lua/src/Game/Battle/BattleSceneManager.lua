---------------------------------------------
--- BattleSceneManager
--- Created by thrt520.
--- DateTime: 2018/5/2
---------------------------------------------
---@class BattleSceneManager
local BattleSceneManager  = {}
require("Game.Battle.Config.BattleDefine")
require("Game.Tools.GameTools")
--------------------------------------------------------------
--- SceneActor
--------------------------------------------------------------\
--------
function BattleSceneManager.OnIntoScene()
	BattleSceneManager.Progress = 0
	local tLevel = JsonDataMgr.GetLevelData(BattleConfig.LevelId)
	BattleConfig.MapScriptName = tLevel.Level_Map
end

function BattleSceneManager.OnLeaveScene()

end

return BattleSceneManager