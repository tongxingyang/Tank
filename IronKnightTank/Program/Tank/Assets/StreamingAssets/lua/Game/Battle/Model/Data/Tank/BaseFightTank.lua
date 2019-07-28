---------------------------------------------
--- BaseFightTank
---	战车基类
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class BaseFightTank : IUnit
local BaseFightTank = class("BaseFightTank")
---@type number 坦克ID
BaseFightTank.Id = 0

---@type ECamp 坦克阵营
BaseFightTank.Camp = 0

---@type GridPos 坦克位置
BaseFightTank.Pos = nil

---@type BaseSolider 士兵数据
BaseFightTank.Solider = nil

---@type TankData 战车数据
BaseFightTank.TankData = nil

---@type EToward 坦克朝向
BaseFightTank.Toward = nil

---@type boolean 是否为玩家坦克
BaseFightTank.IsPlayer = true

---@type boolean 是否存活
BaseFightTank.IsAlive = true

---@type number 炮塔受损回合数
BaseFightTank.TurretHurtRound = 0

---@type number 车身受损回合数
BaseFightTank.BodytHurtRound = 0

----@type type 能否被攻击
BaseFightTank.CanBeAtk = false

----@type number 坦克向性
BaseFightTank.Compatility = 0

----@type number 坦克向性系数
BaseFightTank.CompatilityCofficient = 0

----@type ECamp[] 能被看到阵营数组
BaseFightTank.ObserversCamp = nil

----@type boolean 是否可见
BaseFightTank.Visiable = false

----@type boolean 是否被看到过
BaseFightTank.HasBeenSeen = false

----@type boolean 是否可选技能目标
BaseFightTank.IsSkillTarget = false

----@type boolean 是否伪装坦克
BaseFightTank.IsFakeTank = false

---@type bool 是否无视视野消耗
BaseFightTank.IsIgnoreViewCost = false

---@type EViewerType 视野类型
BaseFightTank.ViewerType = EViewerType.Normal

---@type table 坦克类型特性
BaseFightTank.TankTypeFeatures = nil

---@type EFightTankType 坦克类型
BaseFightTank.Type = EFightTankType.Normal


local MapModel = require("Game.Battle.Model.MapModel")
local CombatFormula = require("Game.Script.Combat.CombatFormula")
local MSGTankEnterPos = require("Game.Event.Message.Battle.MSGTankEnterPos")

local hitFunc = CombatFormula.Attack_Hit
local desFunc = CombatFormula.DestoryRate
local atkFunc = CombatFormula.Attack_Result
local luckFunc = CombatFormula.Luckly

function BaseFightTank:ctor()

end

function BaseFightTank:_init()
	self.ObserversCamp = {}
	self.TankTypeFeatures = TankTypeFeaturesConfig[self.TankData.TankType]
end
---------------------------------------
---坦克技能
---------------------------------------
---幸运
function BaseFightTank:GetLuck()
	return self.Solider.Luck
end

---视野加成
function BaseFightTank:GetViewAdvanced()
	return self.Solider.ViewAdvanced
end

---速度加成
function BaseFightTank:GetSpeedAdvanced()
	return self.Solider.SpeedAdvanced
end

---攻击距离加成
function BaseFightTank:GetFireRangeAdvanced()
	return self.Solider.FireRangeAdvanced
end

---命中炮塔几率倾向
function BaseFightTank:GetHitTurretPrefer()
	return self.Solider.HitPrefer
end

---视野
function BaseFightTank:GetView()
	return self.TankData.View + self:GetViewAdvanced()
end


---是否忽略地形
function BaseFightTank:IsInflectByTerrian(terrainType)
	return self.Solider.Skill:TopographyInfluence(terrainType) == 0
end
-------------------------------------
---坦克能量
function BaseFightTank:GetPower()
	return self.Solider.Power
end

---消耗坦克能量
function BaseFightTank:CostPower(n)
	self.Solider.Power = self.Solider.Power - n
end

---重置坦克能量
function BaseFightTank:ResetPower()
	self.Solider.Power = self.Solider.MaxPower
end

---耗尽坦克能量
function BaseFightTank:CostAllPower()
	self.Solider.Power = 0
end

---投影量
function BaseFightTank:GetProject()
	return self.TankData.Projection * self.Solider.ProjectCofficient
end

----坦克命中计算
---@return HitResult
---@param defTank BaseFightTank
function BaseFightTank:Hit(defTank , blockCovertAddition , isFocus)
	local project = defTank:GetProject()
	local hit = self:GetHit()
	local distance = self:GetTankDis(defTank)
	local hitRes  , defRandom , projectTrue , hitTrue , unitHitTrue = hitFunc(project , distance , blockCovertAddition , hit , self:GetHitTurretPrefer() , isFocus)
	---@type HitResult
	local hitResult = {}
	hitResult.Project = project / BattleConfig.MaxProject
	hitResult.ProjectCorrect = projectTrue / BattleConfig.MaxProject
	hitResult.Hit = (hit + projectTrue) / BattleConfig.MaxHit
	hitResult.HitCorrect = (unitHitTrue + projectTrue) / BattleConfig.MaxHit
	hitResult.HitTrue = hitTrue
	hitResult.Avoid = defRandom
	if hitRes ~= EHitRes.None then
		local hitVec = self.Pos - defTank.Pos
		local hitPosDic = defTank:GetHitPosData(hitVec)
		local r = math.random()
		local hitPos
		if r < hitPosDic.hitrate1 then
			hitPos = hitPosDic.hitpos1
		else
			hitPos = hitPosDic.hitpos2
		end
		hitResult.HitPos = hitPos
	end
	hitResult.HitRes = hitRes
	return hitResult
end

---坦克攻击计算
---@return FireResult
---@param defTank BaseFightTank
function BaseFightTank:Fire(defTank  , hitPos, hitRes)
	local distance = self:GetTankDis(defTank)
	local rawDmg = self:GetRawDmg(distance)
	local realDmg = self:GetRealDmg(distance , defTank.TankData.TankType)
	local rawArmored = defTank:GetEquipmentRawArmor(hitRes , hitPos)
	local realArmored = defTank:GetEquipmentArmored(hitRes , hitPos)
	local dmgRes , trueArmor = atkFunc(realDmg , realArmored )
	---@type FireResult
	local fireResult = {}
	fireResult.Res = dmgRes
	fireResult.Atk = rawDmg / BattleConfig.MaxDMG
	fireResult.AtkCorrect = realDmg / BattleConfig.MaxDMG
	fireResult.Armored = rawArmored / BattleConfig.MaxArmored
	fireResult.ArmoredCorrect = realArmored / BattleConfig.MaxArmored
	if dmgRes == EDMGRes.Hurt then
		defTank:Hurt(hitRes)
	elseif dmgRes == EDMGRes.Destroy then
		defTank:OnDead()
	end
	return fireResult
end

---坦克幸运计算
function BaseFightTank:Luck()
	local luck = self:GetLuck()
	return luckFunc(luck) == 1
end

---命中
function BaseFightTank:GetHit()
	return self.Solider.Hit * (self.TankTypeFeatures.HitAdd or 1)
end

----坦克总护甲
function BaseFightTank:GetArmored()
	return self.TankData.Armored
end

---坦克局部装甲
function BaseFightTank:GetEquipmentRawArmor(eHitRes , eHitTankPos )
	local type = ""
	if eHitRes == EHitRes.HitBody then
		type = "Armored"
	else
		type = "Turrets"
	end
	local pos = "_"..EHitTankPos.GetDes(eHitTankPos)
	local armored = self.TankData.TTank[type .. pos]
	if not armored then
		armored = 0
		clog("no armored" ..tostring(eHitTankPos).."  "..tostring(eHitRes))
	end
	return armored
end

---护甲
function BaseFightTank:GetEquipmentArmored(eHitRes , ehitTankPos )
	local armoredCofficient = self.Solider:GetArmoredCoefficient(eHitRes , ehitTankPos)
	local armored = self:GetEquipmentRawArmor(eHitRes , ehitTankPos)
	armored = (self.TankTypeFeatures.ArmoredAdd or 1) * armored
	local finalArmored = armored * self.CompatilityCofficient * armoredCofficient
	return finalArmored
end

---坦克原始伤害
function BaseFightTank:GetRawDmg(dis)
	local dmg = self.TankData.TTank["Gun_Damage"..dis]
	if not dmg then
		dmg = 0
	end
	return dmg
end

---攻击真实伤害
function BaseFightTank:GetRealDmg(dis , targetTankType)
	local dmg = self:GetRawDmg(dis)
	dmg = (self.TankTypeFeatures.DmgAdd or 1 ) * dmg
	local dmgCofficient = self.Solider:GetDamageCoefficient(dis)
	local targetTankDmgCofficient = self.Solider:GetDmgCofficientForTargetTank(targetTankType)
	local finalDmg = dmg *self.CompatilityCofficient * dmgCofficient * targetTankDmgCofficient
	return finalDmg
end

----坦克命中位置
----返回一個table ， key 为可能命中位置，val为命中几率
function BaseFightTank:GetHitPosData(hitVec)
	local towardVec =EToward.GetVector(self.Toward)
	local v = GridPos.GetAngleBetweenTwoVec(towardVec  , hitVec , true)
	local hitpos = {}
	local v2 = math.fmod(v , 90)
	local count = ( v - v2 ) / 90
	local hitpos1 , hitpos2
	if count == 0 then
		hitpos1 = EHitTankPos.Front
		hitpos2 = EHitTankPos.RightSide
	elseif count == 1 then
		hitpos1 = EHitTankPos.RightSide
		hitpos2 = EHitTankPos.Back
	elseif count == 2 then
		hitpos1 = EHitTankPos.Back
		hitpos2 = EHitTankPos.LeftSide
	elseif count == 3 then
		hitpos1 = EHitTankPos.RightSide
		hitpos2 = EHitTankPos.Front
	end
	local limitAngle = 30

	local rate1 , rate2
	rate2 = math.clamp(0.5 - (45 - v2) / limitAngle , 0 , 1)
	rate1 = 1 - rate2
	hitpos.hitpos1 = hitpos1
	hitpos.hitpos2 = hitpos2
	hitpos.hitrate1 = rate1
	hitpos.hitrate2 = rate2
	return hitpos
end

---坦克间距
----@param target BaseFightTank
function BaseFightTank:GetTankDis( target)
	local dis = self.Pos - target.Pos
	local x = math.abs(dis.x)
	local y = math.abs(dis.y)
	return x > y and x or y
end

---是否可移动
function BaseFightTank:CanMove()
	return self.BodytHurtRound <=0
end

---是否可攻击
function BaseFightTank:CanAtk()
	return self.TurretHurtRound <=0
end

----TODO xxxxx
-----能否创建虚假坦克
--function BaseFightTank:CanCreateFakeTank()
--	return true
--end

----能否活动
function BaseFightTank:CanAction()
	return self:GetPower() > 0
end

---受伤
function BaseFightTank:Hurt(eHitRes )
	if eHitRes == EHitRes.HitTurret then
		self.TurretHurtRound = BattleConfig.PassTime
	elseif eHitRes == EHitRes.HitBody then
		self.BodytHurtRound = BattleConfig.PassTime
	end
end

---死亡
function BaseFightTank:OnDead()
	self.IsAlive = false
end

---是否损坏
function BaseFightTank:IsBreak()
	return (self.BodytHurtRound >0 or self.TurretHurtRound >0 )
end

--------预测命中与击毁概率
---@param defTank BaseFightTank
---@param isFocus boolean
function BaseFightTank:GetAtkDestoryRateAndHitRate(defTank , isFocus )
	if not self:CanAtk() then
		return 0 , 0
	end
	local project = defTank:GetProject()
	local hit = self:GetHit()
	local blockCovertAddition = MapModel.GetBlockData(defTank.Pos).CovertAddition
	local distance = self:GetTankDis(defTank)
	local hitRes  , defRandom , projectTrue , hitTrue , unitHitTrue = hitFunc(project , distance , blockCovertAddition , hit , self:GetHitTurretPrefer() , isFocus)
	local realDmg = self:GetRealDmg(distance , defTank.TankData.TankType)
	local hitVec = self.Pos - defTank.Pos
	local hitPos = defTank:GetHitPosData(hitVec)
	local destoryRate1 = self:_getPosDestoryRate(defTank , realDmg , hitPos.hitpos1)
	local destoryRate2 = self:_getPosDestoryRate(defTank , realDmg , hitPos.hitpos2)
	local destoryRate = destoryRate1 * hitPos.hitrate1 + destoryRate2 * hitPos.hitrate2
	--clog("destory rate "..tostring(destoryRate).."  rate1 "..tostring(destoryRate1).."  pos1 "..tostring(hitPos.hitrate1).."  rate2 "..tostring(destoryRate2).."  pos2 "..tostring(hitPos.hitrate2))
	return  destoryRate , hitTrue
end

----获取指定位置被摧毁的概率
---@param defTank BaseFightTank
function BaseFightTank:_getPosDestoryRate(defTank , dmg , hitpos)
	local realArmored1 = defTank:GetEquipmentArmored(EHitRes.HitTurret , hitpos)
	local realArmored2 = defTank:GetEquipmentArmored(EHitRes.HitBody , hitpos)
	local realArmored = (realArmored1 + realArmored2) / 2
	local destoryRate = desFunc(dmg , realArmored)
	return destoryRate
end

---设置位置
function BaseFightTank:SetPos(pos)
	self.Pos = pos
	EventBus:Brocast(MSGTankEnterPos:Build({TankData = self , Pos = pos})):Yield()
end

---设置朝向
---@param eToward EToward
function BaseFightTank:SetToward(eToward)
	self.Toward = eToward
end

---是否阻挡其他单位攻击
---@return boolean
---@param mover IMover
function BaseFightTank:IsBlockAtk(mover)
	return self.Camp ~= mover.Camp
end

---@return number
function BaseFightTank:GetAtkDistance()
	return  self:GetFireRangeAdvanced() + BattleConfig.TankAtkDistance
end

---------------------------------------------
--- IMover接口
---------------------------------------------
function BaseFightTank:CanPassTerrian(terriantType)
	return true
end

---@return number
---@param blockData BlockData
function BaseFightTank:GetMoveCost(blockData)
	if self:IsInflectByTerrian(blockData.TerrianType) then
		return blockData.MoveCost
	else
		return 1
	end
end

---是否忽略地形
---@return boolean
function BaseFightTank:IsInflectByTerrian(terrainType)
	return self.Solider.Skill:TopographyInfluence(terrainType) == 0
end


---是否阻挡其他单位移动
---@return boolean
---@param mover IMover
function BaseFightTank:IsBlockMove(mover)
	return mover.Camp ~= self.Camp
end



---移动速度
---@return number
function BaseFightTank:GetMoveSpeed()
	return  self.TankData.Speed + self.Solider.SpeedAdvanced
end
---------------------------------------------

---------------------------------------------
--- IViewer接口
---------------------------------------------
----增加观察阵营
---@param camp ECamp
function BaseFightTank:AddObserver(camp)
	if not table.contains(self.ObserversCamp , camp) then
		table.insert(self.ObserversCamp , camp)
	end
end

----增加观察阵营 列表
function BaseFightTank:AddObservers(campList)
	for i, v in pairs(campList) do
		self:AddObserver(v)
	end
end

----重设观察阵营
function BaseFightTank:ResetObservers()
	self.ObserversCamp = {}
end

-----对该阵营是否可见
function BaseFightTank:IsVisiableForCamp(camp)
	return table.contains(self.ObserversCamp , camp)
end

-----获取观察者名称
function BaseFightTank:GetViewerName()
	return LocalizationMgr.GetDes(self.TankData.Name)
end

----获取观察者icon
function BaseFightTank:GetViewerIcon()
	return self.Solider:GetSoliderIconPath()
end
---------------------------------------------

function BaseFightTank:DestoryDialog()

end

function BaseFightTank:HurtDialog(hitRes)

end

function BaseFightTank:MissDialog()

end

function BaseFightTank:NoHurtDialog()

end

function BaseFightTank:LuckDialog()

end

function BaseFightTank:BeforeAtkDialog()

end
---------------------------------------------
---------------------------------------------
return BaseFightTank