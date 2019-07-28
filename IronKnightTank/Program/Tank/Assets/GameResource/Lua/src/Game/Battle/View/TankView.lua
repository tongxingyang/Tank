---@class TankView
local TankView = class('TankView',uView)
TankView.ViewPrefabPath = 'Prefab/Battle/View/TankView.prefab'

---@type GameObject
TankView.gameObject = nil

---@type Transform
TankView.transform = nil

---@type GameObject
TankView.attackTipsObject = nil

---@type Transform
TankView.tankAnchorTransform = nil

---@type GameObject
TankView.turretBreakSignObject = nil

---@type GameObject
TankView.bodyBreakSignObject = nil

---@type BoxCollider
TankView.tankAnchorCollider = nil

---@type GameObject
TankView.fakeTankSignObject = nil


--==userCode==--
---@type BaseFightTank
TankView._tankData = nil
TankView.tankActionViewPlayer = nil
TankView.tankMesh = nil

local MSGClickTank = require("Game.Event.Message.Battle.MSGClickTank")
local MapConfig = require("Game.Battle.Config.MapConfig")

--==userCode==--

function TankView:Init()

end

function TankView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.attackTipsObject = luaUiFields[1]
    self.tankAnchorTransform = luaUiFields[2]
    self.turretBreakSignObject = luaUiFields[3]
    self.bodyBreakSignObject = luaUiFields[4]
    self.tankAnchorCollider = luaUiFields[5]
    self.fakeTankSignObject = luaUiFields[6]

end

function TankView:Dispose()
	self.gameObject  = nil
	self.transform = nil
	self._tankData = nil
end



function TankView:OnOnMouseUpAsButtontankAnchor()
	EventBus:Brocast(MSGClickTank:Build({Camp = self._tankData.Camp , TankId = self._tankData.Id}))
end


--==userCode==--


function TankView:Init()

end

---@param tankData PlayerFightTank
function TankView:LoadTank(tankData)
	local tTank = JsonDataMgr.GetTankUnitData( tankData.TankData.Id)
	local tankModelPath = tTank.Tank_Model
	tankModelPath = "Prefab/Tank/"..tankModelPath..".prefab"
	local tank = ResMgr.InstantiatePrefabYield(nil , tankModelPath , self.tankAnchorTransform)
	self.tankMesh = tank:GetComponent("TankMesh")
	local matPath = tankData.IsPlayer and tTank.Style or tTank.Enemy_Style
	matPath = "Material/Tank/"..matPath..".mat"
	self._isPlayer = tankData.IsPlayer
	ResMgr.LoadMaterial(matPath , function (mat)
		self.tankMesh:SetDefaultMaterila(mat)
	end)
	self.tankActionViewPlayer = tank:GetComponent("ActionViewPlayer")
end


function TankView:ResetMaterial()
	self.tankMesh:ResetMaterial()
end


function TankView:SetMaterial(mat)
	self.tankMesh:SetMaterial(mat)
end

---@param tankData BaseFightTank
function TankView:Update(tankData)
	self._tankData = tankData
	if self._tankData.Visiable then
		self:OpenView()
	elseif BattleConfig.IsTankHideInShadow then
		self:CloseView()
	end

	self:SetToward(tankData.Toward)
	if tankData.Pos then
		self.gameObject:SetActive(true)
		self.transform.position =  MapConfig.GetWorldPos(tankData.Pos)
	else
		self.gameObject:SetActive(false)
	end

	if self._tankData.CanBeAtk then
		self:_attackTips()
	elseif self._tankData.IsSkillTarget then
		self:_attackTips()
	else
		self:_cancelAttackTips()
	end

	self.bodyBreakSignObject:SetActive(tankData.BodytHurtRound >0)
	self.turretBreakSignObject:SetActive(tankData.TurretHurtRound >0)
	self.fakeTankSignObject:SetActive(tankData.IsFakeTank)
	if not self._tankData.IsAlive then
		self:DestoryTank()
	end

end

---@param iViewer IViewer
function TankView:UpdateVisiable(iViewer)
	if iViewer.Visiable then
		self:OpenView()
	else
		self:CloseView()
	end
end

function TankView:Move(gridPos , time , holder)
	local pos = MapConfig.GetWorldPos(gridPos)
	if holder then
		holder:Pend()
	end
	self.transform:DTLocalMove(pos, time , function ()
		if holder then
			holder:Restore()
		end
	end)
end


function TankView:CloseMoveEffect()
	self.tankMesh:CloseMoveEffect()
end

function TankView:OpenMoveEffect()
	self.tankMesh:OpenMoveEffect()
end

function TankView:SetTankMoveActionActive(isActive)
	if not self.tankActionViewPlayer then
		clog("no tankActionViewPlayer"..tostring(self._tankData.Id))
		return
	end

	if isActive then
		self.tankActionViewPlayer:PlayLoopView("walk")
	else
		self.tankActionViewPlayer:StopLoopView("walk")
	end
end

function TankView:TankFire(msg)
	msg:Pend()
	if self._tankData.Visiable then
		self.tankActionViewPlayer:PlayOnceView("atk" , function ()
			msg:Restore()
		end)
	else
		msg:Restore()
	end
end

function TankView:Rotate( angles , time , holder)
	holder:Pend()
	local angle = self.tankAnchorTransform.localEulerAngles
	local interval = math.calAngle(angle.y , angles.y) --math.fmod (math.abs(angle.y - angles.y) , 180)
	time = time * interval / 180
	self.tankAnchorTransform:DTLocalRotate(angles , time , function ()
		holder:Restore()
	end)
end

------坦克瞄准目标位置
---@param pos GridPos
---@param holder MSGTankAImAtPos
function TankView:AimAtPos(pos , time , holder)
	holder:Pend()

	if self._tankData.Visiable then
		local hitVec = pos - self._tankData.Pos
		local angleY = GridPos.GetAngleBetweenTwoVec(GridPos.New(0 , 1 ) , hitVec , true)

		self.tankMesh:RotateTurret(angleY , time , true , function ()
			holder:Restore()
		end)
	else
		--TODO 隐藏坦克提示
		clog("隐藏坦克提示")
		holder:Restore()
	end
end


----恢复炮塔
function TankView:RevertTurret(  time , holder)
	holder:Pend()
	if self._tankData.Visiable then
		self.tankMesh:RevertTurret(time , function ()
			holder:Restore()
		end)
	else
		--TODO 隐藏坦克提示
		holder:Restore()
		clog("关闭隐藏坦克提示")
	end
end

---设置朝向
function TankView:SetToward(toward)
	if toward ~= 0 then
		self.tankAnchorTransform.transform.localEulerAngles = EToward.GetAngle(toward)
	end
end

---显示可被攻击标记
function TankView:_attackTips()
	self.attackTipsObject:SetActive(true)
end

---取消可被攻击标记
function TankView:_cancelAttackTips()
	self.attackTipsObject:SetActive(false)
end


function TankView:CloseCollider()
	self.tankAnchorCollider.enabled = false
end

function TankView:OpenCollider()
	self.tankAnchorCollider.enabled = true
end


function TankView:CloseView()
	self.tankAnchorTransform.gameObject:SetActive(false)
end

function TankView:OpenView()
	self.tankAnchorTransform.gameObject:SetActive(true)
end

function TankView:DestoryTank(msg)
	self.tankActionViewPlayer:PlayOnceView("boom" , function ()
		self.gameObject:SetActive(false)

	end)
end

function TankView:DefHighLight()
	self.attackTipsObject:SetActive(true)
end

function TankView:CancelDefHighLight()
	self.attackTipsObject:SetActive(false)
end
--==userCode==--

return TankView
