---------------------------------------------
--- DmgSkill
--- 伤害技能
--- Created by thrt520.
--- DateTime: 2018/8/7
---------------------------------------------
local DmgSkill = {}

local TankModel = require("Game.Battle.Model.TankModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")

local skillDmgFunc = require("Game.Script.Combat.CombatFormula").InitiativeSkillDamage

---BaseCommanderSkill.Type = ECommanderSkillType.Dmg
---BaseCommanderSkill.Area = 1									---范围
---BaseCommanderSkill.Hit = 0									---命中
---BaseCommanderSkill.HitType = 1 								---命中类型  1为地图 2位单位
---BaseCommanderSkill.DmgType = 1 								---伤害类型  1 --高爆弹  2 -穿甲弹和轰炸

function DmgSkill.Play(skillScript , param)
	local area = skillScript.Area
	---@type GridPos
	local skillRelesePos = param.TargetPos
	local atkPosList = skillRelesePos:GetMultiQuadPos(area)
	for i, v in pairs(atkPosList) do
		local tank = TankModel.GetTankByPos(v)
		if tank then
			---@type BaseFightTank
			local res = skillDmgFunc(skillScript.Hit , skillScript.DmgType)
			if res == 1 then			----未命中
			elseif res == 2 then  		----车体损坏
				tank:Hurt(EHitRes.HitBody)
			elseif res == 3 then		----炮塔损坏
				tank:Hurt(EHitRes.HitTurret)
			elseif res == 4 then		----摧毁
				tank:OnDead()
			else
				clog("未知的结果"..tostring(res))
			end
			ViewFacade.UpdateSingleTank(tank.Id)
			if res == 4 then
				TankModel.RemoveTank(tank.Id)
			end
		end
	end
end

return DmgSkill