---------------------------------------------
--- LevelScriptRunTime
--- 关卡脚本运行时
---	驱动关卡脚本运行
--- Created by thrt520.
--- DateTime: 2018/8/28
---------------------------------------------
---@class LevelScriptRunTime
local LevelScriptRunTime = {}
local this = LevelScriptRunTime
---@type BaseLevelScript
LevelScriptRunTime._levelScript = nil
LevelScriptRunTime.PlayerMoveFirst = false

local receiveCommand = {
	require("Game.Event.Message.Battle.MSGCampRoundEnd"),
	require("Game.Event.Message.Battle.MSGRoundStart"),
	require("Game.Event.Message.Battle.MSGTankEnterPos"),
	require("Game.Event.Message.Battle.MSGSkillRelaseFinish"),
	require("Game.Event.Message.Battle.MSGTankDestory"),
	require("Game.Event.Message.Battle.MSGBeforeCombatStart"),
}

local MSGShowWinCondition = require("Game.Event.Message.Battle.MSGShowWinCondition")

---@param levelScript BaseLevelScript
function LevelScriptRunTime.Init()
	local tLevel = JsonDataMgr.GetLevelData(BattleConfig.LevelId)
	local levelScript = require("Game.Script.Level."..tLevel.Level_Script)
	LevelScriptRunTime._registerCommandHandler()
	LevelScriptRunTime._levelScript = levelScript
	LevelScriptRunTime.PlayerMoveFirst = levelScript.Player
end

function LevelScriptRunTime.Dispose()
	LevelScriptRunTime._unregisterCommandHandler()
end

function LevelScriptRunTime._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommand,LevelScriptRunTime)
end

function LevelScriptRunTime._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommand,LevelScriptRunTime)
end

----------------------------------------------------------------
---even handler
----------------------------------------------------------------
----MSGCampRoundEnd
---@param msg MSGCampRoundEnd
function LevelScriptRunTime.OnMSGCampRoundEnd(msg)
	msg:Pend()
	if 	this._levelScript.OnCampRoundEnd then
		this._levelScript.OnCampRoundEnd(msg.Camp == BattleConfig.PlayerCamp)
	end
	msg:Restore()
end

----MSGRoundStart
---@param msg MSGRoundStart
function LevelScriptRunTime.OnMSGRoundStart(msg)
	msg:Pend()
	if this._levelScript.OnRoundStart then
		this._levelScript.OnRoundStart(msg.CurRound)
	end
	msg:Restore()
end

----MSGTankEnterPos
---@param msg MSGTankEnterPos
function LevelScriptRunTime.OnMSGTankEnterPos(msg)
	msg:Pend()
	if this._levelScript.OnTankEnterPos then
		this._levelScript.OnTankEnterPos(msg.Pos.x , msg.Pos.y , msg.TankData.Solider.Id , msg.TankData.TankData.Id  , msg.TankData.Camp == BattleConfig.PlayerCamp)
	end
	msg:Restore()
end

----MSGSkillRelaseFinish
---@param msg MSGSkillRelaseFinish
function LevelScriptRunTime.OnMSGSkillRelaseFinish(msg)
	msg:Pend()
	if this._levelScript.OnReleaseSkill then
		this._levelScript.OnReleaseSkill(msg.SKillId , msg.Camp == BattleConfig.PlayerCamp)
	end
	msg:Restore()
end

----MSGTankDestory
---@param msg MSGTankDestory
function LevelScriptRunTime.OnMSGTankDestory(msg)
	msg:Pend()
	if this._levelScript.OnTankDestory then
		this._levelScript.OnTankDestory(msg.TankData.Camp == BattleConfig.PlayerCamp , msg.TankData.Solider.Id , msg.TankData.TankData.Id)
	end
	msg:Restore()
end

----MSGBeforeCombatStart
---@param msg MSGBeforeCombatStart
function LevelScriptRunTime.OnMSGBeforeCombatStart(msg)
	msg:Pend()
	this.BeforeBattleStart()
	EventBus:Brocast(MSGShowWinCondition:Build({Des = this._levelScript.SuccesCondition , Time = 1})):Yield()
	msg:Restore()
end
--------------------------------------
function LevelScriptRunTime.BeforeBattleStart()
	if this._levelScript.BeforeLevelStart then
		this._levelScript.BeforeLevelStart()
	end
end

---@return number
function LevelScriptRunTime.GetWinCon()
	return this._levelScript.SuccesCondition
end

---@return number
function LevelScriptRunTime.GetFailCon()
	return this._levelScript.FailCondition
end

return LevelScriptRunTime
