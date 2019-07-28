---------------------------------------------
--- CombatController
---战斗阶段控制器
---负责处理阵营间的回合流转和攻击移动等表现请求
--- Created by thrt520.
--- DateTime: 2018/6/13
---------------------------------------------
---@class CombatController :BaseStageController
local CombatController = class("CombatController" )
local this = CombatController

local TankModel = require("Game.Battle.Model.TankModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local AttackActionClass = require("Game.Battle.Action.AttackAction")
local ManualCommander = require("Game.Battle.Logic.Commander.ManualCommander")
local AICommander =require("Game.Battle.Logic.Commander.AICommander")
local TankMoveAction =require("Game.Battle.Action.TankMoveAction")
local ShadowModel = require("Game.Battle.Model.ShadowModel")
local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local MapModel = require("Game.Battle.Model.MapModel")
local LevelScriptRunTime = require("Game.Battle.Level.LevelScriptRunTime")
local SkillPlayer = require("Game.Battle.Logic.Skill.SkillPlayer")
local SkillModel = require("Game.Battle.Model.SkillModel")

local MSGActiveNextCamp = require("Game.Event.Message.Battle.MSGActiveNextCamp")
local MSGCampRoundEnd = require("Game.Event.Message.Battle.MSGCampRoundEnd")
local MSGRoundStart = require("Game.Event.Message.Battle.MSGRoundStart")
local MSGBeforeCombatStart = require("Game.Event.Message.Battle.MSGBeforeCombatStart")
local MSGPlayActiveCampView = require("Game.Event.Message.Battle.MSGPlayActiveCampView")


local receiveCommand = {
	require("Game.Event.Message.Battle.MSGMoveTankRequest"),
	require("Game.Event.Message.Battle.MSGTankAtkRequeset"),
	require("Game.Event.Message.Battle.MSGCampCtrlEndRequest"),
	require("Game.Event.Message.Battle.MSGReleaseSkill"),
}

---ai控制commander
---@type AICommander
this.aiCommander = nil
---手动控制commander
---@type ManualCommander
this.manualCommander = nil
----@type ECampp[]
this._campList = {}

function CombatController.OnEnterStage()

	this.manualCommander = ManualCommander.new(BattleConfig.PlayerCamp)

	this.aiCommander = AICommander.new(BattleConfig.NpcCamp)

	this._registerCommandHandler()

	coroutine.createAndRun(this._init)

end

function CombatController.OnLeaveStage()
	this.Dispose()
end

function CombatController._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommand,CombatController)
end

function CombatController._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommand,CombatController)
end

--------------------------------
---even handler
--------------------------------
--- MSGMoveTankRequest 事件处理
---@param msg MSGMoveTankRequest
function CombatController.OnMSGMoveTankRequest(msg)
	coroutine.createAndRun(this.MoveTank ,msg.TankId , msg.TargetPos , msg)
end

--- MSGTankAtkRequeset 事件处理
---@param msg MSGTankAtkRequeset
function CombatController.OnMSGTankAtkRequeset(msg)
	coroutine.createAndRun(this.AtkTank , msg.AtkTankId , msg.DefTankId , msg.IsFocus , msg)
end

--- MSGEndRound 事件处理
---@param msg MSGCampCtrlEndRequest
function CombatController.OnMSGCampCtrlEndRequest(msg)
	clog("OnMSGCampCtrlEndRequest")
	coroutine.createAndRun(this._broadCastCampRoundEndAndEndCtrl , msg)
end

--- MSGReleaseSkill 事件处理
---@param msg MSGReleaseSkill
function CombatController.OnMSGReleaseSkill(msg)
	coroutine.createAndRun(this._releaseSkill , msg)
end
--------------------------------
---function
--------------------------------
function CombatController._init()



	PanelManager.OpenYield(PanelManager.PanelEnum.BattleMainPanel)

	this.BeforBattleStart()

	this.UpdateRound()

	this._broadCastNextActiveCamp()

end

---坦克移动
function CombatController.MoveTank(tankId , targetPos , holder)
	local path = TankModel.GetTankMovePath(tankId , targetPos)
	local cost = MapModel.GetBlockData(targetPos).CostPower
	local moveAction = TankMoveAction.new({GridPosList = path , TankId = tankId  , CostPower = cost})
	moveAction:Action(holder)
end

---坦克攻击
function CombatController.AtkTank(atkTankId , defTankId , isFocus , holder)
	local atkAction = AttackActionClass.new(atkTankId , defTankId , isFocus)
	atkAction:Action(holder)
end

----释放技能
---@param msg MSGReleaseSkill
function CombatController._releaseSkill(msg)
	if msg.IsPlayer then
		SkillModel.ReleaseSkill(msg.SkillId)
		ViewFacade.UpdateSkill()
	end
	msg:Pend()
	SkillPlayer.PlaySkill(msg.SkillId , msg.param , msg.Camp)
	msg:Restore()
end

---发送大回合结束消息  刷新游戏
function CombatController.UpdateRound()
	EventBus:Brocast(MSGRoundStart:Build({CurRound = BattleRuntimeData.CurRound})):Yield()
	ShadowModel.UpdateFOW()
	ViewFacade.UpdateShadowView()
	ViewFacade.UpdateAllTank()
end

---战斗开始前表演
function CombatController.BeforBattleStart()

	this._registerAllCombatCamp()

	BattleRuntimeData.Refresh()

	EventBus:Brocast(MSGBeforeCombatStart.New()):Yield()
end

---发送阵营小回合结束消息并发送下个阵营行动消息
function CombatController._broadCastCampRoundEndAndEndCtrl(msg)
	EventBus:Brocast(MSGCampRoundEnd:Build({Camp = msg.Camp})):Yield()
	this._broadCastNextActiveCamp()
end

---注册出战阵营
function CombatController._registerAllCombatCamp()
	if LevelScriptRunTime.PlayerMoveFirst then
		this._registerCamp(BattleConfig.PlayerCamp)
		this._registerCamp(BattleConfig.NpcCamp)
	else
		this._registerCamp(BattleConfig.NpcCamp)
		this._registerCamp(BattleConfig.PlayerCamp)
	end
end

function CombatController._registerCamp(camp)
	table.insert(this._campList , camp)
end

---发送阵营行动消息
function CombatController._broadCastNextActiveCamp()
	local curIndex = BattleRuntimeData.CurCampIndex
	curIndex = curIndex + 1
	if curIndex > #this._campList then
		BattleRuntimeData.CurRound = BattleRuntimeData.CurRound + 1
		this.UpdateRound()
		curIndex = 1
	end
	BattleRuntimeData.CurCamp = this._campList[curIndex]
	BattleRuntimeData.CurCampIndex = curIndex
	EventBus:Brocast(MSGPlayActiveCampView:Build({Camp = BattleRuntimeData.CurCamp , Round = BattleRuntimeData.CurRound})):Yield()
	EventBus:Brocast(MSGActiveNextCamp:Build({Camp = BattleRuntimeData.CurCamp})):Yield()
end

function CombatController.Dispose()
	this._unregisterCommandHandler()
	this.manualCommander:Dispose()
	this.aiCommander:Dispose()
	this._campList ={}
end

return CombatController