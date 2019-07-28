---------------------------------------------
--- BattleRuntimeData
---	战斗临时数据存放
--- Created by thrt520.
--- DateTime: 2018/7/24
---------------------------------------------
---@class BattleRuntimeData
local BattleRuntimeData = {}
BattleRuntimeData.CurCamp = 0
BattleRuntimeData.CurRound = 1
BattleRuntimeData.WinCamp = 0
BattleRuntimeData.CurCampIndex = 0

function BattleRuntimeData.Refresh()
	BattleRuntimeData.CurCamp = 0
	BattleRuntimeData.CurRound = 1
	BattleRuntimeData.CurCampIndex = 0
	BattleRuntimeData.WinCamp = 0
end

return BattleRuntimeData