---------------------------------------------
--- NPCFightTank
---	npc战车
--- Created by thrt520.
--- DateTime: 2018/5/29
---------------------------------------------
---@class NPCFightTank : BaseFightTank
local NPCFightTank = class("NPCFightTank"  , require("Game.Battle.Model.Data.Tank.BaseFightTank"))
NPCFightTank.HasSeen = false
NPCFightTank.AI = nil
local NPCSolider = require("Game.Battle.Model.Data.Tank.NPCSolider")
local TankData = require("Game.Main.Model.Data.TankData")

function NPCFightTank:ctor(npcId)
	self.Solider = NPCSolider.new(npcId)
	local tNpcData = JsonDataMgr.GetEnemyUnitData( npcId)
	local tankId = tNpcData.Tank_ID
	self.TankData = TankData.new(tankId)
	self.IsPlayer = false
	self.Camp = BattleConfig.NpcCamp
	self:_init()
	self.Compatility = ETankCompatibility.LevelC
	self.CompatilityCofficient = 1
end


function NPCFightTank :DestoryDialog()
	return  "成功摧毁目标！"
end

function NPCFightTank :MissDialog()
	return  "没能击中目标"
end

function NPCFightTank :HurtDialog(hitRes)
	if hitRes == EHitRes.HitBody  then
		return  "敌人的车身被破坏了！"
	elseif  hitRes == EHitRes.HitTurret then
		return  "敌人的炮塔被破坏了！"
	else
		return  "敌人未知部位被破坏"
	end
end

function NPCFightTank:NoHurtDialog()
	return "未能击穿敌人护甲"
end


function NPCFightTank :LuckDialog()
	return  "God bless ，跳弹了！"
end


function NPCFightTank :BeforeAtkDialog()
	return "攻击开始"
end



return NPCFightTank