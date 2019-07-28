---------------------------------------------
--- LevelTools
--- Created by thrt520.
--- DateTime: 2018/8/28
---------------------------------------------

local LevelTools = {}

local ViewFacade = require("Game.Battle.View.ViewFacade")
local MapModel = require("Game.Battle.Model.MapModel")
local ShadowModel = require("Game.Battle.Model.ShadowModel")
local TankModel = require("Game.Battle.Model.TankModel")

local MSGCreateNpc = require("Game.Event.Message.Battle.MSGCreateNpc")
local MSGChangeSkillTimes = require("Game.Event.Message.Battle.MSGChangeSkillTimes")
local MSGBanSkill = require("Game.Event.Message.Battle.MSGBanSkill")
local MSGBattleEnd  =require("Game.Event.Message.Battle.MSGBattleEnd")
local MSGAddSkill  =require("Game.Event.Message.Battle.MSGAddSkill")
local MSGReleaseSkill = require("Game.Event.Message.Battle.MSGReleaseSkill")

-----創建坦克
LevelTools.CreatTank = function(npcId , x , y , toward)
	if not npcId or not x or not y or not toward then
		clog("  argument nil wrong")
	end
	EventBus:Brocast(MSGCreateNpc:Build({NpcId = npcId , Pos = GridPos.New(x , y )  , Toward = toward}))
end

-----更改全局消耗視野係數
LevelTools.ChangeGlobalViewCost = function(number , round)
	local GlobalViewCostBuffer = require("Game.Battle.Logic.Buff.GlobalViewCostBuffer")
	GlobalViewCostBuffer.new(number , round)
	ShadowModel.UpdateFOW()
	ViewFacade.UpdateShadowView()
end


-----增加技能
LevelTools.AddSkill = function(skilId , times)
	if not skilId or not times then
		clog(" argument nil wrong")
	end
	EventBus:Brocast(MSGAddSkill:Build({SkillId = skilId , Times = times}))
end


---禁用技能
LevelTools.BanSkill = function(skillId , roundNumber)
	if not skillId or not roundNumber then
		clog(" argument nil wrong")
	end
	EventBus:Brocast(MSGBanSkill:Build({SKillID =  skillId , Round = roundNumber}))
end

---刪除技能
LevelTools.DelSkill = function(skillId )
	local MSGDelSkill = require("Game.Event.Message.Battle.MSGDelSkill")
	EventBus:Brocast(MSGDelSkill:Build({SKillId = skillId}))
end

---技能释放范围提示
LevelTools.TipsSkillEffectArea = function(posX , posY , area)
	MapModel.HighLightArea("LevelSkillEffectArea" , GridPos.New(posX , posY) , area)
	ViewFacade.UpdateBlocks()
end

----取消技能释放范围提示
LevelTools.CancelTipsSkillEffectArea = function(posX , posY , area)
	MapModel.CancelHighLightArea("LevelSkillEffectArea" , GridPos.New(posX , posY) , area)
	ViewFacade.UpdateBlocks()
end

---释放伤害技能
LevelTools.ReleaseDmgSkill = function(skillId , posX , posY )
	EventBus:Brocast(MSGReleaseSkill:Build({SkillId = skillId , param = {TargetPos = GridPos.New(posX , posY) , Camp = BattleConfig.NpcCamp} , Camp = BattleConfig.NpcCamp , IsPlayer = false })):Yield()
end

LevelTools.FakeTank = function(skilld , posX , posY , toward , isPlayer)
	local camp = isPlayer and BattleConfig.PlayerCamp or BattleConfig.NpcCamp
	EventBus:Brocast(MSGReleaseSkill:Build({SkillId = skilld , param = {TargetPos = GridPos.New(posX , posY) , Camp = camp , Toward = toward} , Camp = BattleConfig.NpcCamp , IsPlayer = false })):Yield()
end

-----劇情文本
LevelTools.Dialog = function(type ,content , icon )

end

-----更改技能使用次數
LevelTools.ChangeSkillTimes = function(skillId , val)
	EventBus:Brocast(MSGChangeSkillTimes:Build({SkillID = skillId , Val = val}))
end

---勝利
LevelTools.Success = function( )
	EventBus:Brocast(MSGBattleEnd:Build({WinCamp = BattleConfig.PlayerCamp}))
end

---失敗
LevelTools.Fail = function()

	EventBus:Brocast(MSGBattleEnd:Build({WinCamp = BattleConfig.NpcCamp}))
end

---指定位置是否有坦克
LevelTools.IsTankInTargetPos = function(posX , posY , isPlayer)
	local tank = TankModel.GetTankByPos(GridPos.New(posX , posY))
	if tank then
		local tankIsPlayer = tank.Camp == BattleConfig.PlayerCamp
		return tankIsPlayer == isPlayer
	else
		return false
	end
end

return LevelTools