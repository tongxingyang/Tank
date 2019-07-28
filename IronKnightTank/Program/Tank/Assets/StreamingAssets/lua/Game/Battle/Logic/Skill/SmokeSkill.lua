---------------------------------------------
--- SmokeSkill
---烟雾弹技能
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
local SmokeSkill = {}


local SmokeBuff = require("Game.Battle.Logic.Buff.SmokeBuff")
local ShadowModel  =require("Game.Battle.Model.ShadowModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")

function SmokeSkill.Play(skillScript , param)
	local area = skillScript.Area
	local skillTargetPos = param.TargetPos
	local val = skillScript.Val
	local round = param.Round
	local skillReleaseArea = skillTargetPos:GetMultiQuadPos(area)

	for i, v in pairs(skillReleaseArea) do
		local shadowData = ShadowModel.GetShadowNodeData(v)
		if shadowData then
			local smokeBuff = SmokeBuff.New(val , round , shadowData )
			shadowData:AddBuff(smokeBuff)
		end
	end
	ShadowModel:UpdateFOW()
	ViewFacade.UpdateShadowView()
end


return SmokeSkill