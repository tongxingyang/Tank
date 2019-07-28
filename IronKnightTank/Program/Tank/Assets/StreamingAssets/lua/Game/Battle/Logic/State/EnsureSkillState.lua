---------------------------------------------
--- EnsureSkillState
---	确认技能状态
---	控制权限转交给SkillController
--- Created by thrt520.
--- DateTime: 2018/8/7
---------------------------------------------
---@class EnsureSkillState : IState
local EnsureSkillState = {}
local this = EnsureSkillState
---@type EManualState
EnsureSkillState.StateEnum = EManualState.EnsureSkillState
---@type FSM
EnsureSkillState.FSM = nil
---@type number
EnsureSkillState._skillId = nil
---@type number
EnsureSkillState.curTankId = nil
EnsureSkillState.curController = nil

local skillContorller = {}
skillContorller[ECommanderSkillType.Dmg] = require("Game.Battle.Logic.SkillController.DmgSkillController")
skillContorller[ECommanderSkillType.Repair] = require("Game.Battle.Logic.SkillController.RepairSkillController")
skillContorller[ECommanderSkillType.Smoke] = require("Game.Battle.Logic.SkillController.SmokeController")
skillContorller[ECommanderSkillType.FakeTank] = require("Game.Battle.Logic.SkillController.FakeTankController")
skillContorller[ECommanderSkillType.TempBunker] = require("Game.Battle.Logic.SkillController.TempBunkerController")
skillContorller[ECommanderSkillType.ClearFOW] = require("Game.Battle.Logic.SkillController.ClearFowSkillController")

local ViewFacade = require("Game.Battle.View.ViewFacade")
local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")

local MSGCloseSkillDes = require("Game.Event.Message.Battle.MSGCloseSkillDes")
local MSGShowSkillDes = require("Game.Event.Message.Battle.MSGShowSkillDes")
local MSGReleaseSkill = require("Game.Event.Message.Battle.MSGReleaseSkill")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickCancelButton"),
	--require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
}

function EnsureSkillState.OnEnter(param)
	local skillId = param.SkillId
	local tSkillData = JsonDataMgr.GetInitiativeSkillData(skillId)
	local skillScript = require("Game.Script.CommanderSkill."..tSkillData.Skill_Script)
	local type = skillScript.Type
	local controller = skillContorller[type]
	if controller then
		controller.Init(skillScript)
	else
		clog("no skill controller type "..tostring(type).."  skill id"..tostring(skillId))
	end
	controller.realeaseCall = this.OnReleaseSkill
	controller.disposeCall = this.OnDispose
	this.curController = controller
	this.UpdateSKillDes(tSkillData.Skill_Description)
	ViewFacade.UpdateCtrlState(this.StateEnum)
	this._registerCommandHandler()
end

function EnsureSkillState.OnExit()
	if this.curController then
		this.curController.Dispose()
	end
	EventBus:Brocast(MSGCloseSkillDes.New())
	this._unregisterCommandHandler()
end

function EnsureSkillState._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,EnsureSkillState)
end

function EnsureSkillState._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,EnsureSkillState)
end
-------------------------------------------
---event handler
-------------------------------------------
-----MSGClickRightMouseBtn 事件
----@param msg MSGClickRightMouseBtn
function EnsureSkillState.OnMSGClickRightMouseBtn(msg)
	this.FSM:ChangeState(EManualState.DefaultState)
end

-----MSGClickCancelButton 事件
----@param msg MSGClickCancelButton
function EnsureSkillState.OnMSGClickCancelButton(msg)
	this.FSM:ChangeState(EManualState.DefaultState)
end
-------------------------------------------

function EnsureSkillState.UpdateSKillDes(content)
	EventBus:Brocast(MSGShowSkillDes:Build({Des = content}))
end

function EnsureSkillState.OnReleaseSkill(skillId , param)
	coroutine.createAndRun(function ()
		this.FSM:ChangeState(EManualState.NoneState)
		EventBus:Brocast(MSGReleaseSkill:Build({SkillId = skillId , param = param , Camp = BattleRuntimeData.CurCamp , IsPlayer = true })):Yield()
		this.FSM:ChangeState(EManualState.DefaultState)
	end)
end

function EnsureSkillState.OnDispose()
	this.FSM:ChangeState(EManualState.DefaultState)
end

return EnsureSkillState