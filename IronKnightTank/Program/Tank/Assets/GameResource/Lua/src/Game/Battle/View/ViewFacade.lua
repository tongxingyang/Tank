---------------------------------------------
--- ControllerTools
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class ViewFacade
local ViewFacade = {}
local TankModel = require("Game.Battle.Model.TankModel")
local ShadowModel = require("Game.Battle.Model.ShadowModel")
local MapModel = require("Game.Battle.Model.MapModel")
local MapConfig = require("Game.Battle.Config.MapConfig")

local MSGUpdateTank = require("Game.Event.Message.Battle.MSGUpdateTank")
local MSGUpdateSingleTank = require("Game.Event.Message.Battle.MSGUpdateSingleTank")
local MSGUpdateShadow = require("Game.Event.Message.Battle.MSGUpdateShadow")
local MSGUpdateBlock = require("Game.Event.Message.Battle.MSGUpdateBlock")
local MSGCamFocusTank = require("Game.Event.Message.Battle.MSGCamFocusTank")
local MSGOpenTankCollider  = require("Game.Event.Message.Battle.MSGOpenTankCollider")
local MSGCloseTankCollider  = require("Game.Event.Message.Battle.MSGCloseTankCollider")
local MSGShowChoseTankTips = require("Game.Event.Message.Battle.MSGShowChoseTankTips")
local MSGUpdateManualState = require("Game.Event.Message.Battle.MSGUpdateManualState")
local MSGCancelShowChoseTankTips = require("Game.Event.Message.Battle.MSGCancelShowChoseTankTips")
local MSGUpdateSkill = require("Game.Event.Message.Battle.MSGUpdateSkill")
local MSGHighLightBornBlock = require("Game.Event.Message.Battle.MSGHighLightBornBlock")
local MSGCancelBornHighLight = require("Game.Event.Message.Battle.MSGCancelBornHighLight")

----更新单一坦克
---@param tankId number
function ViewFacade.UpdateSingleTank(tankId)
	EventBus:Brocast(MSGUpdateSingleTank:Build({TankData = TankModel.GetTankData(tankId)}))
end

----更新玩家坦克
function ViewFacade.UpdateAllPlayerTank()
	EventBus:Brocast(MSGUpdateTank:Build({TankDataArray = TankModel.GetCampTanks(BattleConfig.PlayerCamp)}))
end

----更新同一阵营坦克
function ViewFacade.UpdateCampTank(camp)
	EventBus:Brocast(MSGUpdateTank:Build({ TankDataArray = TankModel.GetCampTanks(camp) }))
end

----更新敌方阵营坦克
function ViewFacade.UpdateEnemyCampTank(camp)
	EventBus:Brocast(MSGUpdateTank:Build({ TankDataArray = TankModel.GetEnemyCampTank(camp) }))
end

----更新所有坦克
function ViewFacade.UpdateAllTank()
	EventBus:Brocast(MSGUpdateTank:Build({TankDataArray = TankModel.GetAllTankData()}))
end


----打开坦克属性面板 玩家
---@param tankId number
function ViewFacade.OpenTankAttributePanel(tankId , isLeft)
	PanelManager.Open(PanelManager.PanelEnum.TankAttributePanel , {fightTankData = TankModel.GetTankData(tankId) , IsLeft = true })
end

----关闭坦克属性面板
function ViewFacade.CloseTankAttributePanel()
	PanelManager.Close(PanelManager.PanelEnum.TankAttributePanel)
end

----打开坦克属性面板 npc
---@param tankId number
function ViewFacade.OpenNpcTankAttributePanel(tankId)
	PanelManager.Open(PanelManager.PanelEnum.TankAttributePanel , {fightTankData = TankModel.GetTankData(tankId) , IsLeft = false })
end

----关闭坦克属性面板 npc
function ViewFacade.CloseNpcTankAttributePanel()
	PanelManager.Close(PanelManager.PanelEnum.TankAttributePanel )
end

----更新战争阴影
function ViewFacade.UpdateShadowView()
	EventBus:Brocast(MSGUpdateShadow:Build({ShadowDataList = ShadowModel.GetShadowData() }))
end

----更新地块
function ViewFacade.UpdateBlocks()
	EventBus:Brocast(MSGUpdateBlock:Build({BlockDataDic =MapModel.GetAllBlockData() }))
end

----关闭当前选中坦克提示
function ViewFacade.CloseTankSingView()
	EventBus:Brocast(MSGCancelShowChoseTankTips.new())
end

----打开当前选中坦克提示
function ViewFacade.ShowTankSignView(tankId , isMine)
	local pos = TankModel.GetTankData(tankId).Pos
	EventBus:Brocast(MSGShowChoseTankTips:Build({Pos = MapConfig.GetWorldPos(pos) , IsMine = isMine }))
end

---取消显示坦克可移动范围
function ViewFacade.CancelShowTankMoveableArea()
	MapModel.CancelMoveableHighLight()
	ViewFacade.UpdateBlocks()
end

---提示可攻击坦克  NPC
function ViewFacade.TipsCanAttackTank(tankId)
	TankModel.TipsCanAttackTank(tankId)
	ViewFacade.UpdateCampTank(BattleConfig.NpcCamp)
end

---取消提示可攻击坦克  NPC
function ViewFacade.CancelTipsCanAttackTank()
	TankModel.CancelTipsCanAttackTank()
	ViewFacade.UpdateCampTank(BattleConfig.NpcCamp)
end

---提示可攻击坦克
function ViewFacade.UpdateCtrlState(manualState)
	EventBus:Brocast(MSGUpdateManualState:Build({State = manualState }))
end

---提示攻擊坦克和防守坦克
function ViewFacade.AtkHighLight(atkTankId, defTankId)
	local MSGCombatHighLight  =require("Game.Event.Message.Battle.MSGCombatHighLight")
	EventBus:Brocast(MSGCombatHighLight:Build({AtkTankId = atkTankId, DefTankId = defTankId }))
end

---取消提示攻擊坦克和防守坦克
function ViewFacade.CancelAtkHighLight(atkTankId, defTankId)
	local MSGCancelCombatHighLight  =require("Game.Event.Message.Battle.MSGCancelCombatHighLight")
	EventBus:Brocast(MSGCancelCombatHighLight:Build({AtkTankId = atkTankId, DefTankId = defTankId }))
end

---关闭坦克碰撞盒
function ViewFacade.CloseTankCollider()
	EventBus:Brocast(MSGCloseTankCollider.new())
end

---打开坦克碰撞盒
function ViewFacade.OpenTankCollider()
	EventBus:Brocast(MSGOpenTankCollider.new())
end

---更新技能
function ViewFacade.UpdateSkill()
	local SkillModel = require("Game.Battle.Model.SkillModel")
	EventBus:Brocast(MSGUpdateSkill:Build({SKillDataList = SkillModel.GetAllSKillData() }))
end

---镜头跟踪坦克
function ViewFacade.CamFollowTank(tankId)
	local MSGCamFollowTank  =require("Game.Event.Message.Battle.MSGCamFollowTank")
	EventBus:Brocast(MSGCamFollowTank:Build({TankId = tankId}))
end

---停止镜头跟踪
function ViewFacade.StopCamFollow()
	local MSGStopCamFollow = require("Game.Event.Message.Battle.MSGStopCamFollow")
	EventBus:Brocast(MSGStopCamFollow.new())
end


----镜头聚焦坦克
function ViewFacade.CamFocusTank(tankId , time)
	return EventBus:Brocast(MSGCamFocusTank:Build({ TankId = tankId , Time = time}))
end

function ViewFacade.HighLightBornBlock()
	EventBus:Brocast(MSGHighLightBornBlock.new())
end

function ViewFacade.CancelHighLightBornBlock()
	EventBus:Brocast(MSGCancelBornHighLight.new())
end

return ViewFacade