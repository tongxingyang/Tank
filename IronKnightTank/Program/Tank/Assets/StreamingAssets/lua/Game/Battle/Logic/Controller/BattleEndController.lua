---------------------------------------------
--- BattleEndController
---战斗结束控制器
--- Created by thrt520.
--- DateTime: 2018/6/29
---------------------------------------------
---@class BattleEndController : BaseStageController
local BattleEndController = {}

local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local MSGDisplayBattleEndAim = require("Game.Event.Message.Battle.MSGDisplayBattleEndAim")


local receiveCommand = {
	require("Game.Event.Message.Battle.MSGClickBattleResultContinueButton"),
}

function BattleEndController.OnEnterStage()
	BattleEndController._registerCommandHandler()
	coroutine.createAndRun(function ()
		local isWin = BattleRuntimeData.WinCamp == BattleConfig.PlayerCamp
		EventBus:Brocast(MSGDisplayBattleEndAim:Build({Win = isWin})):Yield()
		if isWin then
			PanelManager.Open(PanelManager.PanelEnum.BattleResultPanel )
		end
	end)
end

function BattleEndController.OnLeaveStage()
	BattleEndController._unregisterCommandHandler()
end


function BattleEndController._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommand,BattleEndController)
end

function BattleEndController._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommand,BattleEndController)
end


function BattleEndController.Dispose()

end


--------------------------------
---even handler
--------------------------------
---@param msg MSGClickBattleResultContinueButton
function BattleEndController.OnMSGClickBattleResultContinueButton(msg)
	PanelManager.Open(PanelManager.PanelEnum.SolideResultPanel)
end

return BattleEndController