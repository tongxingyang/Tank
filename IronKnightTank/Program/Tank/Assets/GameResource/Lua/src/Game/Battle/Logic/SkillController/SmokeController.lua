---------------------------------------------
--- SmokeController
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
---@class SmokeController
local SmokeController = {}


local this = SmokeController
SmokeController.realeaseCall = nil
SmokeController.skillScript = nil
SmokeController.area = 0
SmokeController.disposeCall = nil

local ViewFacade = require("Game.Battle.View.ViewFacade")
local MapModel =  require("Game.Battle.Model.MapModel")
local ShadowModel = require("Game.Battle.Model.ShadowModel")


local receiveCommands = {
	require("Game.Event.Message.Battle.MSGClickBlock"),
	require("Game.Event.Message.Battle.MSGMouseEnterBlock"),
	require("Game.Event.Message.Input.MSGClickRightMouseBtn"),
}

function SmokeController.Init(skillScript)
	this.skillScript = skillScript
	this._registerCommandHandler()
	ViewFacade.CloseTankCollider()
	this._highLightSkillTargetArea()
end

function SmokeController.Dispose()
	ViewFacade.OpenTankCollider()
	this._cancelHighLightSkillArea()
	this._cancelHighLightSkillTargetArea()
	this._unregisterCommandHandler()
end


function SmokeController._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,SmokeController)
end

function SmokeController._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,SmokeController)
end

-----------------------------------------
---event handler
-----------------------------------------
----@param msg MSGMouseEnterBlock
function SmokeController.OnMSGMouseEnterBlock(msg)
	if ShadowModel.IsPosVisiable(msg.GridPos) then
		this._highLightSkillArea(msg.GridPos , this.skillScript.Area)
	end
end

----@param msg MSGClickBlock
function SmokeController.OnMSGClickBlock(msg)
	local blockData =MapModel.GetBlockData(msg.gridPos)
	if blockData.SkillReleaseArea then
		GameTools.Alert("是否释放技能" , function (yes)
			if yes then
				this.realeaseCall(this.skillScript.SkillId , {TargetPos = msg.gridPos })
			end
		end)
	end
end

---MSGClickRightMouseBtn
----@param msg MSGClickRightMouseBtn
function SmokeController.OnMSGClickRightMouseBtn(msg)
	if this.isEnsure then
		GameTools.CloseAlert()
		this.isEnsure = false
	else
		this.disposeCall()
	end
end


-----------------------------------------
function SmokeController._highLightSkillArea(pos , area)
	MapModel.CancelHighLight("SkillEffectArea")
	MapModel.HighLightArea("SkillEffectArea" , pos , area)
	ViewFacade.UpdateBlocks()
end

function SmokeController._cancelHighLightSkillArea()
	--MapModel.CancelSkillEffectAreaHighLight()
	MapModel.CancelHighLight("SkillEffectArea")
	ViewFacade.UpdateBlocks()
end


function SmokeController._highLightSkillTargetArea()
	local blockDic = MapModel.GetAllBlockData()
	for i, v in pairs(blockDic) do
		v.SkillReleaseArea = ShadowModel.IsPosVisiable(v.Pos)
	end
	ViewFacade.UpdateBlocks()
end

function SmokeController._cancelHighLightSkillTargetArea()
	--MapModel.CancelSkillReleaseHighLight()
	MapModel.CancelHighLight("SkillReleaseArea")
	ViewFacade.UpdateBlocks()
end

return SmokeController