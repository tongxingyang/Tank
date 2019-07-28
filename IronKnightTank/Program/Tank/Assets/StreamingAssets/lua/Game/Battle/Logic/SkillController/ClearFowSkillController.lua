---------------------------------------------
--- ClearFowSkillController
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
---@class ClearFowSkillController
local ClearFowSkillController = {}
local this = ClearFowSkillController
ClearFowSkillController.realeaseCall = nil
ClearFowSkillController.disposeCall = nil
ClearFowSkillController.skillScript = nil
ClearFowSkillController.area = 0
ClearFowSkillController.isEnsure = false

local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local MapModel =  require("Game.Battle.Model.MapModel")
local ShadowModel = require("Game.Battle.Model.ShadowModel")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGMouseEnterBlock"),
	require("Game.Event.Message.Battle.MSGClickBlock"),
	require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
}


function ClearFowSkillController.Init(skillScript)
	this.skillScript = skillScript
	this.area = this.skillScript.Area
	this._registerCommandHandler()
	this._highLightSkillTargetArea()
end

function ClearFowSkillController.Dispose()
	this._cancelHighLightSkillEffectArea()
	this._cancelHighLightSkillTargetArea()
	this._unregisterCommandHandler()
end

function ClearFowSkillController._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,ClearFowSkillController)
end

function ClearFowSkillController._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,ClearFowSkillController)
end

-------------------
-------------------------
---event handler
-----------------------------------------
----@param msg MSGMouseEnterBlock
function ClearFowSkillController.OnMSGMouseEnterBlock(msg)
	if this.isEnsure then
		return
	end
	if not ShadowModel.IsPosVisiable(msg.GridPos) then
		MapModel.CancelHighLight("SkillEffectArea")
		MapModel.HighLightArea("SkillEffectArea" , msg.GridPos , this.skillScript.Area)
		ViewFacade.UpdateBlocks()
	end
end

----@param msg MSGClickBlock
function ClearFowSkillController.OnMSGClickBlock(msg)
	local blockData = MapModel.GetBlockData(msg.gridPos)
	if blockData.SkillReleaseArea then
		this.isEnsure = true
		GameTools.Alert("是否释放技能" , function (yes)
			this.isEnsure = false
			if yes then
				this.realeaseCall(this.skillScript.SkillId ,{TargetPos = msg.gridPos , Camp = BattleRuntimeData.CurCamp} )
			end
		end)
	end
end

---MSGClickRightMouseBtn
----@param msg MSGClickRightMouseBtn
function ClearFowSkillController.OnMSGClickRightMouseBtn(msg)
	if this.isEnsure then
		GameTools.CloseAlert()
		this.isEnsure = false
	else
		this.disposeCall()
	end
end
-----------------------------------------
function ClearFowSkillController._highLightSkillTargetArea()
	local blockDic = MapModel.GetAllBlockData()
	for i, v in pairs(blockDic) do
		v.SkillReleaseArea = not ShadowModel.IsPosVisiable(v.Pos)
	end
	ViewFacade.UpdateBlocks()
end

function ClearFowSkillController._cancelHighLightSkillTargetArea()
	local blockDic = MapModel.GetAllBlockData()
	for i, v in pairs(blockDic) do
		v.SkillReleaseArea = false
	end
	ViewFacade.UpdateBlocks()
end

function ClearFowSkillController._cancelHighLightSkillEffectArea()
	--MapModel.CancelSkillEffectAreaHighLight()
	MapModel.CancelHighLight("SkillEffectArea")
	ViewFacade.UpdateBlocks()
end

return ClearFowSkillController