---------------------------------------------
--- AttackAction
---	攻击表演流程：
---	坦克旋转炮塔 播放攻击表现
---	打开攻击前对话框
---	坦克旋转
---	命中计算
---	命中进度条表演
---	如果击中坦克旋转至击中位置
---	攻击计算
---	攻击进度条计算
---	攻击完成后对话框
--- Created by thrt520.
--- DateTime: 2018/5/17
---------------------------------------------
---@class AttackAction
local AttackAction = class("AttackData")
---@type BaseFightTank攻击方坦克
AttackAction.AtkTank = nil
---@type BaseFightTank受击方坦克
AttackAction.DefTank = nil
---@type number 攻击距离
AttackAction.Distance = 0
---@type FightResult 攻击结果
AttackAction._fightRes = 0
---@type bool 是否精准攻击
AttackAction.isFocus = 0
---@type GridPos 攻击向量
AttackAction.AtkVec = 0
---是否是玩家攻击
AttackAction.IsPlayerAtk = 0
AttackAction.holder = nil

local TankModel = require("Game.Battle.Model.TankModel")
local MapModel = require("Game.Battle.Model.MapModel")
local TankTalkAction =require("Game.Battle.Action.TankTalkAction")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local ShadowModel  = require("Game.Battle.Model.ShadowModel")

local MSGDisplayFight = require("Game.Event.Message.Battle.MSGDisplayFight")
local MSGRevertTankTurret = require("Game.Event.Message.Battle.MSGRevertTankTurret")
local MSGCloseAimTankCam = require("Game.Event.Message.Battle.MSGCloseAimTankCam")
local MSGOpenAimTankCam = require("Game.Event.Message.Battle.MSGOpenAimTankCam")
local MSGTankAImAtPos = require("Game.Event.Message.Battle.MSGTankAImAtPos")
local MSGShowHitPos = require("Game.Event.Message.Battle.MSGShowHitPos")
local MSGPlayTankFireView = require("Game.Event.Message.Battle.MSGPlayTankFireView")

function AttackAction:ctor( atkTankId , beatTankId , isFocus)
	self.isFocus = isFocus
	self.AtkTank = TankModel.GetTankData(atkTankId)
	self.DefTank = TankModel.GetTankData(beatTankId)
	self.Distance = self.AtkTank:GetTankDis(self.DefTank)
	self._fightRes = {}
	self.AtkVec = self.DefTank.Pos - self.AtkTank.Pos
	self.IsPlayerAtk = self.AtkTank.IsPlayer
end

function AttackAction:Action(holder)
	self.holder = holder
	self:_onStart()
	ViewFacade.AtkHighLight(self.AtkTank.Id , self.DefTank.Id)
	self:ShowPreAtkDialog()
	ViewFacade.CamFocusTank(self.AtkTank.Id , 0.3)
	self:RotateTurret()
	self:AtkTankFire()
	ViewFacade.CancelAtkHighLight(self.AtkTank.Id , self.DefTank.Id)
	PanelManager.Close(PanelManager.PanelEnum.BattleMainPanel)
	PanelManager.OpenYield(PanelManager.PanelEnum.CombatDisplayPanel , {atkTank = self.AtkTank, defTank = self.DefTank })
	self:ShowTankAimCam()
	self:DisplayHitAnim()
	self:DisplayFirAnim()
	self:CloseTankAimCam()
	PanelManager.Close(PanelManager.PanelEnum.CombatDisplayPanel)
	PanelManager.Open(PanelManager.PanelEnum.BattleMainPanel)
	ViewFacade.UpdateSingleTank(self.DefTank.Id)
	if self._fightRes.DmgRes == EDMGRes.Destroy then
		TankModel.RemoveTank(self.DefTank.Id)
	end
	self.AtkTank:CostAllPower()
	self:RevertTurret()
	self:AtkResDialog()
	ShadowModel.UpdateFOW()
	ViewFacade.UpdateShadowView()
	ViewFacade.UpdateAllTank()
	self:_onEnd()
end


function AttackAction:_onStart()
	if self.holder then
		self.holder:Pend()
	end
end

function AttackAction:_onEnd()
	if self.holder then
		self.holder:Restore()
	end
end

----播放坦克开火表演
function AttackAction:AtkTankFire()
	EventBus:Brocast(MSGPlayTankFireView:Build({TankId = self.AtkTank.Id})):Yield()
end

---打开坦克特写镜头
function AttackAction:ShowTankAimCam()
	local vec = self.DefTank.Pos - self.AtkTank.Pos
	local angle = GridPos.GetAngleBetweenTwoVec(vec , EToward.GetVector(self.DefTank.Toward) , true)
	local angle2 = Vector3.New(0 , angle , 0)
	local rotateTime = (BattleConfig.AtkSliderTweenTime + BattleConfig.AtkSliderTweenIntervalTime) * 6
	EventBus:Brocast(MSGOpenAimTankCam:Build({TankId = self.DefTank.Id  , IsPlayer = self.DefTank.IsPlayer, TankAngles = angle2 , RotateTime = rotateTime }))
end

---关闭坦克特写镜头
function AttackAction:CloseTankAimCam()
	EventBus:Brocast(MSGCloseAimTankCam:Build({TankId = self.DefTank.Id , IsPlayer = self.DefTank.IsPlayer}))
	ViewFacade.UpdateSingleTank(self.DefTank.Id)
end

---开始命中表演流程
function AttackAction:DisplayHitAnim()
	local blockCovertAddition = MapModel.GetBlockData(self.DefTank.Pos).CovertAddition
	local hitResult = self.AtkTank:Hit(self.DefTank , blockCovertAddition , self.isFocus)
	self:PlaySlider(hitResult.Project , 0 , "投影量")
	self:PlaySlider(hitResult.ProjectCorrect , nil , "投影量修正")
	self:PlaySlider(hitResult.Hit, nil , "命中")
	self:PlaySlider(hitResult.HitCorrect, nil , "命中修正")
	self:PlaySlider(hitResult.HitTrue , nil , "攻击类型修正")
	self:PlaySlider(nil, hitResult.Avoid  , "回避")
	self._fightRes.HitRes = hitResult.HitRes
	self._fightRes.HitPos = hitResult.HitPos

	if hitResult.HitPos then
		local angle =self:_getangle(hitResult.HitPos)
		local time =  (BattleConfig.AtkSliderTweenTime + BattleConfig.AtkSliderTweenIntervalTime) * (angle.y / 720) * 6
		EventBus:Brocast(MSGShowHitPos:Build({Angle = angle , Time = time})):Yield()
	end
	if hitResult.HitRes == EHitRes.HitBody then
		self:PlaySlider(nil  , nil , "击中车身")
	elseif hitResult.HitRes == EHitRes.HitTurret then
		self:PlaySlider(nil  , nil , "击中炮塔")
	else
		self:PlaySlider(nil  , nil , "未命中")
	end
end


---计算特写镜头中坦克角度
function AttackAction:_getangle(eHittankPos)
	if eHittankPos == EHitTankPos.Back then
		return Vector3.New(0 , 0 , 0)
	elseif eHittankPos == EHitTankPos.Front then
		return Vector3.New(0 , 180 , 0)
	elseif eHittankPos == EHitTankPos.LeftSide then
		return Vector3.New(0 , 90 , 0)
	elseif eHittankPos == EHitTankPos.RightSide then
		return Vector3.New(0 , 270 , 0)
	else
		return Vector3.New(0 , 0,0 )
	end
end

----开始攻击表演流程
function AttackAction:DisplayFirAnim()
	if self._fightRes.HitRes == EHitRes.None then
		self._fightRes.DmgRes = EDMGRes.None
		return
	end
	if self.DefTank:Luck() then
		self:PlaySlider(-1 , 1 , "跳弹")
		self._fightRes.DmgRes = EDMGRes.Luck
	else
		local fireResult = self.AtkTank:Fire(self.DefTank , self._fightRes.HitPos , self._fightRes.HitRes)
		local dmgRes = fireResult.Res
		self:PlaySlider(0 , 0, "开始攻击" , 0.01)
		self:PlaySlider(fireResult.Atk , nil , "理论攻击值")
		self:PlaySlider(fireResult.AtkCorrect , nil , "技能修正攻擊")
		self:PlaySlider( nil ,fireResult.Armored , "防守方護甲")
		self:PlaySlider( nil ,fireResult.ArmoredCorrect , "防守方護甲修正计算")
		local des
		if dmgRes == EDMGRes.Hurt then
			des = (self._fightRes.HitRes == EHitRes.HitBody and "履带"or"炮塔" ).."损坏"
		elseif dmgRes == EDMGRes.Destroy then
			des = "摧毁"
		elseif dmgRes == EDMGRes.NoHurt then
			des = "未擊穿護甲"
		else
			des = "炮弹飞到了异次元"
		end
		self._fightRes.DmgRes = dmgRes
		self:PlaySlider( nil ,nil , des)
	end
end

---开始slider动画
function AttackAction:PlaySlider(atkVal , defVal , des , time)
	if not time then
		time = BattleConfig.AtkSliderTweenTime
	end
	EventBus:Brocast(MSGDisplayFight:Build({AtkSliderValue = atkVal , DefSliderValue = defVal , Des = des , Time = time})):Yield()
	coroutine.wait(BattleConfig.AtkSliderTweenIntervalTime)
end

---战前提示面板
function AttackAction:ShowPreAtkDialog()
	local atkVec =  self.DefTank.Pos - self.AtkTank.Pos
	local toward = atkVec:GetToward()
	local des = EToward.GetDes(toward)
	local dis = self.AtkVec:Magnitude()
	dis = math.floor(dis) * 200
	local playerTank = self.IsPlayerAtk and self.AtkTank or self.DefTank
	local content = des.."方向  "..tostring(dis).."距离  "..(self.IsPlayerAtk and "攻击开始" or "敌方攻击")
	local tankTalkAction = TankTalkAction.new(playerTank , content)
	tankTalkAction:Action()
end


---旋转炮塔
function AttackAction:RotateTurret()
	EventBus:Brocast(MSGTankAImAtPos:Build({TankId = self.AtkTank.Id , AimPos = self.DefTank.Pos , Time = BattleConfig.TankAimTime})):Yield()
end

---回退炮塔
function AttackAction:RevertTurret()
	EventBus:Brocast(MSGRevertTankTurret:Build({ TankId = self.AtkTank.Id , IsMine = self.AtkTank.IsPlayer, Time = 1})):Yield()
end

---攻击结果面板展示
function AttackAction:AtkResDialog()
	local playerTank  = self.AtkTank.IsPlayer and self.AtkTank or self.DefTank
	local resContent
	if self._fightRes.DmgRes == EDMGRes.None then
		resContent = self.DefTank:MissDialog()
	elseif self._fightRes.DmgRes == EDMGRes.Destroy then
		resContent = self.DefTank:DestoryDialog()
	elseif self._fightRes.DmgRes == EDMGRes.Luck then
		resContent = self.DefTank:LuckDialog()
	elseif self._fightRes.DmgRes == EDMGRes.Hurt then
		resContent = self.DefTank:HurtDialog(self._fightRes.HitRes)
	elseif self._fightRes.DmgRes == EDMGRes.NoHurt then
		resContent = self.DefTank:NoHurtDialog()
	else
		resContent = "未知的战斗结果"
		clog("dmgres" .. tostring(self._fightRes.DmgRes))
	end

	local tankTalkAction = TankTalkAction.new(playerTank , resContent)
	tankTalkAction:Action()
end

return AttackAction