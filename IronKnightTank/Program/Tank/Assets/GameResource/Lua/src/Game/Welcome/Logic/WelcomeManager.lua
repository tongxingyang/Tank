---------------------------------------------
--- WelcomeManager
--- Created by thrt520.
--- DateTime: 2018/8/28
---------------------------------------------
---@class WelcomeManager : SceneActor
local WelcomeManager = {}
require("Game.Tools.GameTools")
local SceneManager = require("Framework.Scene.SceneManager")
local DataMgr = require("Game.Main.Model.DataMgr")
local MSGUpdateRecruitPanel  = require("Game.Event.Message.Welcome.MSGUpdateRecruitPanel")

function WelcomeManager.OnIntoScene()
	WelcomeManager._registerCommandHandler()
	PanelManager.OpenYield(PanelManager.PanelEnum.LoadLevelPanel)
end

function WelcomeManager.OnLeaveScene()
	WelcomeManager._unregisterCommandHandler()
end

local receiveCommand = {
	require("Game.Event.Message.Welcome.MSGLoadLevelRequest"),
	require("Game.Event.Message.Welcome.MSGOpenRecruit"),
	require("Game.Event.Message.Welcome.MSGClickRecruitSolideButton"),
	require("Game.Event.Message.Welcome.MSGRecruitSolider"),
	require("Game.Event.Message.Welcome.MSGClickRecruitTankButton"),
	require("Game.Event.Message.Main.MSGOpenRecruitEnsureWinRequest"),
	require("Game.Event.Message.Main.MSGRecruitTankRequest"),
	require("Game.Event.Message.Main.MSGShowAllRecruitTankRequest"),
	require("Game.Event.Message.Main.MSGShowAllRecruitSoliderRequest"),
}


function WelcomeManager._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommand,WelcomeManager)
end

function WelcomeManager._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommand,WelcomeManager)
end


function WelcomeManager.LoadLevel(levelId)
	local levelData = JsonDataMgr.GetLevelData(levelId)
	if levelData then
		BattleConfig.LevelId = levelId
		local Game = require("Game.Game")
		SceneManager.SwicthScene(Game.Battle , function (p)

		end)
	else
		GameTools.Alert("关卡id不存在")
	end
end

----------------------------------------------------------
---evet handler
----------------------------------------------------------
----MSGLoadLevelRequest
---@param msg MSGLoadLevelRequest
function WelcomeManager.OnMSGLoadLevelRequest(msg)
	coroutine.createAndRun(WelcomeManager.LoadLevel , msg.LevelId)
end

----MSGOpenRecruit
---@param msg MSGOpenRecruit 打开招募面板
function WelcomeManager.OnMSGOpenRecruit(msg)
	PanelManager.Close(PanelManager.PanelEnum.LoadLevelPanel)
	local soliderLen =  DataMgr.GetHaveSoliderListCount()
	local tankLen = DataMgr.GetHaveTankListCount()
	PanelManager.Open(PanelManager.PanelEnum.RecruitPanel , {SoliderMax = DataMgr.ReturnSoliderMaxCount() , SoliderCount = soliderLen , TankMax = DataMgr.ReturnTankMaxCount() , TankCount = tankLen , })
end

----MSGClickRecruitSolideButton
---@param msg MSGClickRecruitSolideButton  打开招募士兵窗口
function WelcomeManager.OnMSGClickRecruitSolideButton(msg)
	local soliderMax = DataMgr.ReturnSoliderMaxCount()
	local soliderCount = DataMgr.GetHaveSoliderListCount()
	if soliderMax <= soliderCount then
		GameTools.Alert("士兵已满")
	else
		PanelManager.Open(PanelManager.PanelEnum.RecruitEnsureWin , {cost = 100 , isOld = msg.IsOld , maxCount = soliderMax - soliderCount})
	end
end

----MSGRecruitSolider
---@param msg MSGRecruitSolider  打开显示招募士兵结果面板
function WelcomeManager.OnMSGRecruitSolider(msg)
	local soliders = msg.IsOld and DataMgr.RecruitExpSoliderList(msg.Count) or  DataMgr.RecruitYounSoliderList(msg.Count)
	local soliderList = {}
	for i, v in pairs(soliders) do
		table.insert(soliderList , v.RandomSoliderData)
	end
	WelcomeManager.UpdateSolideAndTankData()
	PanelManager.Open(PanelManager.PanelEnum.RecruitSoliderInfoPanel  , {SoliderList = soliderList , IsAll = false  })
end


----MSGShowAllRecruitSoliderRequest
---@param msg MSGShowAllRecruitSoliderRequest 打开显示所有士兵信息面板
function WelcomeManager.OnMSGShowAllRecruitSoliderRequest(msg)
	local soliders = DataMgr.GetHaveSoliderList()
	local solideList = {}
	for i, v in pairs(soliders) do
		table.insert(solideList , v.RandomSoliderData)
	end
	PanelManager.Open(PanelManager.PanelEnum.RecruitSoliderInfoPanel  , {SoliderList = solideList , IsAll = true  })
end


----MSGShowAllRecruitTankRequest
---@param msg MSGShowAllRecruitTankRequest 打开显示所有坦克信息面板
function WelcomeManager.OnMSGShowAllRecruitTankRequest(msg)
	local tanks = DataMgr.TankListInfo()
	PanelManager.Open(PanelManager.PanelEnum.RecruitTankInfoPanel , {TankList = tanks})
end

----MSGClickRecruitTankButton
---@param msg MSGClickRecruitTankButton 打开招募坦克面板
function WelcomeManager.OnMSGClickRecruitTankButton(msg)
	local tankInfos = DataMgr.GetTankListInfo()
	PanelManager.Open(PanelManager.PanelEnum.RecruitTankPanel , {TankDataList = tankInfos})
end

----MSGOpenRecruitEnsureWinRequest
---@param msg MSGOpenRecruitEnsureWinRequest 打开招募坦克确认面板
function WelcomeManager.OnMSGOpenRecruitEnsureWinRequest(msg)
	--local count = msg.TankData.TankMaxCount
	--if count <= 0 then
	--	clog("  count " + msg.TankData.TankMaxCount)
	--	return
	--end
	PanelManager.Open(PanelManager.PanelEnum.RecruitTankEnsureWin , {TankData = msg.TankData })
end

----MSGRecruitTankRequest
---@param msg MSGRecruitTankRequest 招募坦克请求
function WelcomeManager.OnMSGRecruitTankRequest(msg)
	local suc = DataMgr.RecruitTankList(msg.TankData.TankData.Id , msg.Count)
	WelcomeManager.UpdateSolideAndTankData()
	if suc then
		GameTools.Alert("招募成功")
		PanelManager.Close(PanelManager.PanelEnum.RecruitTankEnsureWin)
	else
		GameTools.Alert("招募失败")
	end
end
-------------------------------------------------------------------------------------------------------------

---@return void 发送更新士兵和坦克数据消息
function WelcomeManager.UpdateSolideAndTankData()
	EventBus:Brocast(MSGUpdateRecruitPanel:Build({MaxSoliderCount = DataMgr.ReturnSoliderMaxCount() ,
												  MaxTankCount = DataMgr.ReturnTankMaxCount(),
												  CurSoliderCount = DataMgr.GetHaveSoliderListCount(),
												  CurTankCount = DataMgr.GetHaveTankListCount(),
	}))
end
return WelcomeManager