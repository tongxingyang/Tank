---------------------------------------------
--- TempBunkerSkill
--- 临时掩体技能
--- Created by thrt520.
--- DateTime: 2018/8/8
---------------------------------------------
local TempBunkerSkill = {}

local MapModel = require("Game.Battle.Model.MapModel")

function TempBunkerSkill.Play(skillScript , param)
	---@type GridPos
	local pos = param.TargetPos
	local area = pos:GetMultiQuadPos(skillScript.Area)
	local val = skillScript.Val
	for i, v in pairs(area) do
		local blockData = MapModel.GetBlockData(pos)
		blockData.CovertAddition = blockData.CovertAddition < val and blockData.CovertAddition or val
		clog("临时掩体"..tostring(pos))
		------------等待美术提供资源  模型啊 特效啊  巴拉巴拉
	end
end

return TempBunkerSkill