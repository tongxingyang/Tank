---------------------------------------------
--- DmgSkillController
--- Created by thrt520.
--- DateTime: 2018/8/7
---------------------------------------------
---@class DmgSkillController
local DmgSkillController = {}
local this = DmgSkillController
DmgSkillController.realeaseCall = nil
DmgSkillController.disposeCall = nil
DmgSkillController.skillScript = nil
DmgSkillController.area = 0
DmgSkillController.isEnsure = false

local BattleRuntimeData = require("Game.Battle.Model.Data.Runtime.BattleRuntimeData")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local MapModel =  require("Game.Battle.Model.MapModel")
local ShadowModel = require("Game.Battle.Model.ShadowModel")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGMouseEnterBlock"),
	require("Game.Event.Message.Battle.MSGClickBlock"),
	require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
}


function DmgSkillController.Init(skillScript)
	this.skillScript = skillScript
	this._registerCommandHandler()
	this.area = this.skillScript.Area
	ViewFacade.CloseTankCollider()
	this._highLightSkillTargetArea()
end

function DmgSkillController.Dispose()
	this._cancelHighLightSkillArea()
	this._cancelHighLightSkillTargetArea()
	ViewFacade.OpenTankCollider()
	this._unregisterCommandHandler()
end


function DmgSkillController._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,DmgSkillController)
end

function DmgSkillController._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,DmgSkillController)
end

-----------------------------------------
---event handler
-----------------------------------------
----@param msg MSGMouseEnterBlock
function DmgSkillController.OnMSGMouseEnterBlock(msg)
	if this.isEnsure then
		return
	end
	if ShadowModel.IsPosVisiable(msg.GridPos) then
		this._hightLightSkillArea(msg.GridPos , this.skillScript.Area)
	end
end

----@param msg MSGClickBlock
function DmgSkillController.OnMSGClickBlock(msg)
	local blockData =MapModel.GetBlockData(msg.gridPos)
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
function DmgSkillController.OnMSGClickRightMouseBtn(msg)
	if this.isEnsure then
		GameTools.CloseAlert()
		this.isEnsure = false
	else
		this.disposeCall()
	end
end
-----------------------------------------
function DmgSkillController._highLightSkillTargetArea()
	local blockDic = MapModel.GetAllBlockData()
	for i, v in pairs(blockDic) do
		v.SkillReleaseArea = ShadowModel.IsPosVisiable(v.Pos)
	end
	ViewFacade.UpdateBlocks()
end

function DmgSkillController._cancelHighLightSkillTargetArea()
	local blockDic = MapModel.GetAllBlockData()
	for i, v in pairs(blockDic) do
		v.SkillReleaseArea = false
	end
	ViewFacade.UpdateBlocks()
end

function DmgSkillController._hightLightSkillArea(pos , area)
	MapModel.CancelHighLight("SkillEffectArea")
	MapModel.HighLightArea("SkillEffectArea" , pos , area)
	ViewFacade.UpdateBlocks()
end

function DmgSkillController._cancelHighLightSkillArea()
	MapModel.CancelHighLight("SkillEffectArea")
	ViewFacade.UpdateBlocks()
end


return DmgSkillController