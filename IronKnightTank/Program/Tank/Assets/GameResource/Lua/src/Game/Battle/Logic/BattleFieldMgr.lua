---------------------------------------------
--- BattleFieldMgr
--- Created by thrt520.
--- DateTime: 2018/5/3
---战场控制器
---------------------------------------------
---@class BattleFieldMgr
local BattleFieldMgr = {}
local this = BattleFieldMgr

local MapModel = require("Game.Battle.Model.MapModel")
local TankModel = require("Game.Battle.Model.TankModel")
local ShadowModel = require("Game.Battle.Model.ShadowModel")
local LevelScriptRunTime = require("Game.Battle.Level.LevelScriptRunTime")
--------------------------------------------------------------
--- MSG
--------------------------------------------------------------
local MSGInitBattle = require("Game.Event.Message.Battle.MSGInitBattle")
local MSGInitMap  =require("Game.Event.Message.Battle.MSGInitMap")
local MSGInitFOW = require("Game.Event.Message.Battle.MSGInitFOW")
local MSGCreateTank =  require("Game.Event.Message.Battle.MSGCreateTank")

local receiveCommand = {
	require("Game.Event.Message.Battle.MSGStartCombat"),
	require("Game.Event.Message.Battle.MSGBattleEnd"),
	require("Game.Event.Message.Battle.MSGClickSetButton"),
	require("Game.Event.Message.Battle.MSGClickWinConButton"),
	require("Game.Event.Message.Battle.MSGBattleEndContinue"),
}

----战场阶段控制器
---@type table<EBattleStage ,  BaseStageController>
local Controllers = {}
---当前战场阶段控制器
---@type BaseStageController
this.CurController = nil
--------------------------------------------------------------
--- SceneActor
--------------------------------------------------------------
function BattleFieldMgr.OnIntoScene()
	this._registerControllers()
	this._registerCommandHandler()
	coroutine.createAndRun(this._init)
end

function BattleFieldMgr.OnLeaveScene()
	for i, v in pairs(Controllers) do
		v:Dispose()
	end
	LevelScriptRunTime.Dispose()
	this._registerCommandHandler()
end

--------------------------------------------------------------
--- function
--------------------------------------------------------------
---初始化
function BattleFieldMgr._init()

	local mapHandler =  EventBus:Brocast(MSGInitMap:Build({MapData = MapModel.GetMapData()}))
	local fowHandler = EventBus:Brocast(MSGInitFOW:Build({ShadowNodeDataList = ShadowModel.GetShadowData()}))
	local tankHandler = EventBus:Brocast(MSGCreateTank:Build({BaseFightTankDataList = TankModel.GetAllTankData()}))
	local battleHandler = EventBus:Brocast(MSGInitBattle.New())

	mapHandler:Yield()
	fowHandler:Yield()
	tankHandler:Yield()
	battleHandler:Yield()

	LevelScriptRunTime.Init()

	this.StartStage(EBattleStage.Arrange)
end

function BattleFieldMgr._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommand,BattleFieldMgr)
end

function BattleFieldMgr._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommand,BattleFieldMgr)
end

function BattleFieldMgr._registerControllers()
	Controllers[EBattleStage.Arrange] = require("Game.Battle.Logic.Controller.ArrangeController")
	Controllers[EBattleStage.Combat] = require("Game.Battle.Logic.Controller.CombatController")
	Controllers[EBattleStage.End] = require("Game.Battle.Logic.Controller.BattleEndController")
end

function BattleFieldMgr.StartStage(stageEnum)
	if this.CurController then
		this.CurController.OnLeaveStage()
		this.CurController = nil
	end
	local nextStage = Controllers[stageEnum]
	if nextStage then
		nextStage.OnEnterStage()
		this.CurController = nextStage
	end
end

------------------------------------------------------------------------------------
--- event handler
------------------------------------------------------------------------------------
--- MSGStartCombat 事件处理
---@param msg MSGStartCombat
function BattleFieldMgr.OnMSGStartCombat(msg)
	this.StartStage(EBattleStage.Combat)
end


--- MSGBattleEnd 事件处理
---@param msg MSGBattleEnd
function BattleFieldMgr.OnMSGBattleEnd(msg)
	local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
	BattleRuntimeData.WinCamp =msg.WinCamp
	this.StartStage(EBattleStage.End)
end

--- MSGClickSetButton 事件处理
---@param msg MSGClickSetButton
function BattleFieldMgr.OnMSGClickSetButton(msg)
	PanelManager.Open(PanelManager.PanelEnum.BattleSetPanel)
end


--- MSGClickWinConButton 事件处理
---@param msg MSGClickWinConButton
function BattleFieldMgr.OnMSGClickWinConButton(msg)
	PanelManager.Open(PanelManager.PanelEnum.BattleEndConditionPanel , {WinDes = LevelScriptRunTime.GetWinCon() , FailDes = LevelScriptRunTime.GetFailCon()})
end


--- MSGBattleEndContinue 事件处理
---@param msg MSGBattleEndContinue
function BattleFieldMgr.OnMSGBattleEndContinue(msg)
	local Game = require("Game.Game")
	local SceneManager = require("Framework.Scene.SceneManager")
	SceneManager.SwicthScene(Game.Welcome , function (p)

	end)
end
------------------------------------------------------------------------------------
return BattleFieldMgr