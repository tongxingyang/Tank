---------------------------------------------
--- TankViewContainer
--- Created by thrt520.
--- DateTime: 2018/5/11
---------------------------------------------
---@class TankViewContainer
local TankViewContainer = class("TankViewContainer")
local this = TankViewContainer

---@type table<number , TankView>
this._tankViewDic = nil

this.transform = nil

local TankViewClass =require("Game.Battle.View.TankView")

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGCreateTank"),
	require("Game.Event.Message.Battle.MSGUpdateTank"),
	require("Game.Event.Message.Battle.MSGUpdateSingleTank"),
	require("Game.Event.Message.Battle.MSGMoveTankSingle"),
	require("Game.Event.Message.Battle.MSGRotateTank"),
	require("Game.Event.Message.Battle.MSGRotateTurret"),
	require("Game.Event.Message.Battle.MSGRevertTankTurret"),
	require("Game.Event.Message.Battle.MSGSetTankToward"),
	require("Game.Event.Message.Battle.MSGTankAImAtPos"),
	require("Game.Event.Message.Battle.MSGCloseTankCollider"),
	require("Game.Event.Message.Battle.MSGOpenTankCollider"),
	require("Game.Event.Message.Battle.MSGUpdateViewer"),
	require("Game.Event.Message.Battle.MSGUpdateTankMoveEffectState"),
	require("Game.Event.Message.Battle.MSGPlayTankFireView"),
	require("Game.Event.Message.Battle.MSGOpenTankMoveEffect"),
	require("Game.Event.Message.Battle.MSGPlayTankDestoryView"),
	require("Game.Event.Message.Battle.MSGCombatHighLight"),
	require("Game.Event.Message.Battle.MSGCancelCombatHighLight"),
	require("Game.Event.Message.Battle.MSGBeforeCombatStart"),
}


function TankViewContainer:ctor()
	self._tankViewDic = {}
	clog(" TankViewContainer  new "..tostring(self._tankViewDic == nil ))
	local go = GameObject.New()
	go.name = "tankContainer"
	self.transform = go.transform
	EventBus:RegisterSelfReceiver(receiveCommands , self )
end

function TankViewContainer:Dispose()
	EventBus:UnregisterSelfReceiver(receiveCommands , self)
	for i, v in ipairs(self._tankViewDic) do
		ViewManager.ReturnPoolView(v)
	end
	self._tankViewDic = nil
end

---------------------------------------------------------
---event handler
---------------------------------------------------------
----- MSGCreateTank
---@param msg MSGCreateTank
function TankViewContainer:OnMSGCreateTank(msg)
	self:LoadTank(msg.BaseFightTankDataList)
end

----- MSGUpdateTank
---@param msg MSGUpdateTank
function TankViewContainer:OnMSGUpdateTank(msg)
	for i, v in pairs(msg.TankDataArray) do
		local tankView = self._tankViewDic[i]
		if tankView then
			tankView:Update(v)
		else
			clog("not contains tankview "..tostring(i))
		end
	end
end

--- MSGMoveTankSingle 事件处理
---@param msg MSGMoveTankSingle
function TankViewContainer:OnMSGMoveTankSingle(msg)
	local tankView = self:_getTankView(msg.TankId)
	if  tankView then
		tankView:Move(msg.TargetPos , msg.Time , msg)
	end
end

--- MSGRotateTank 事件处理
---@param msg MSGRotateTank
function TankViewContainer:OnMSGRotateTank(msg)
	local tankView = self:_getTankView(msg.TankId)
	if  tankView then
		tankView:Rotate(msg.Angel , msg.Time , msg)
	end
end

----- MSGUpdateSingleTank
---@param msg MSGUpdateSingleTank
function TankViewContainer:OnMSGUpdateSingleTank(msg)
	local tankView = self:_getTankView(msg.TankData.Id)
	tankView:Update(msg.TankData)
end



--- MSGRevertTankTurret 事件处理
---@param msg MSGRevertTankTurret
function TankViewContainer:OnMSGRevertTankTurret(msg)
	local tankView = self:_getTankView(msg.TankId)
	if  tankView then
		tankView:RevertTurret( msg.Time  , msg)
	end
end

--- MSGSetTankToward 事件处理
---@param msg MSGSetTankToward
function TankViewContainer:OnMSGSetTankToward(msg)
	local tankView = self:_getTankView(msg.TankId)
	if tankView then
		tankView:SetToward(msg.Toward)
	end
end


--- MSGTankAImAtPos 事件处理
---@param msg MSGTankAImAtPos
function TankViewContainer:OnMSGTankAImAtPos(msg)
	local tank = self:_getTankView(msg.TankId)
	tank:AimAtPos(msg.AimPos , msg.Time, msg)

end

--- MSGCloseTankCollider 事件处理
---@param msg MSGCloseTankCollider
function TankViewContainer:OnMSGCloseTankCollider(msg)
	for i, v in pairs(self._tankViewDic) do
		v:CloseCollider()
	end
end

--- MSGOpenTankCollider 事件处理
---@param msg MSGOpenTankCollider
function TankViewContainer:OnMSGOpenTankCollider(msg)
	for i, v in pairs(self._tankViewDic) do
		v:OpenCollider()
	end
end


--- MSGUpdateViewer 事件处理
---@param msg MSGUpdateViewer
function TankViewContainer:OnMSGUpdateViewer(msg)
	local tank = self:GetTankView(msg.Viewer.Id)
	tank:UpdateVisiable(msg.Viewer)
end


--- MSGUpdateTankMoveEffectState 事件处理
---@param msg MSGUpdateTankMoveEffectState
function TankViewContainer:OnMSGUpdateTankMoveEffectState(msg)
	local tank = self:GetTankView(msg.TankId)
	tank:SetTankMoveActionActive(msg.IsActive)
end


--- MSGPlayTankFireView 事件处理
---@param msg MSGPlayTankFireView
function TankViewContainer:OnMSGPlayTankFireView(msg)
	local tank = self:GetTankView(msg.TankId)
	tank:TankFire(msg)
end

--- MSGPlayTankDestoryView 事件处理
---@param msg MSGPlayTankDestoryView
function TankViewContainer:OnMSGPlayTankDestoryView(msg)
	local tank = self:GetTankView(msg.TankId)
	tank:DestoryTank(msg)
end


--- MSGCombatHighLight 事件处理
---@param msg MSGCombatHighLight
function TankViewContainer:OnMSGCombatHighLight(msg)
	local defTank = self:GetTankView(msg.DefTankId)
	defTank:DefHighLight()
end

--- MSGCancelCombatHighLight 事件处理
---@param msg MSGCancelCombatHighLight
function TankViewContainer:OnMSGCancelCombatHighLight(msg)
	local defTank = self:GetTankView(msg.DefTankId)
	defTank:CancelDefHighLight()
end


--- MSGBeforeCombatStart 事件处理
---@param msg MSGBeforeCombatStart
function TankViewContainer:OnMSGBeforeCombatStart(msg)
	for i, v in pairs(self._tankViewDic) do
		v:OpenMoveEffect()
	end
end
---------------------------------------------------------

---@param baseFightTankList BaseFightTank[]
function TankViewContainer:LoadTank(baseFightTankList)
	for i, v in pairs(baseFightTankList) do
		local tankView = self:_createTankView(v)
		tankView:Update(v)
		self._tankViewDic[v.Id] = tankView
	end
end

function TankViewContainer:_getTankView(tankId)
	local tankView = self._tankViewDic[tankId]
	if not tankView then
		clog(" not contains tankview "..tostring(tankId))

	end
	return tankView
end

---@return TankView
function TankViewContainer:_createTankView(tankData )
	--local tankView = self._closedTank[1]
	--if not  tankView then
	local tankView =  ViewManager.GetPoolViewYield(nil , TankViewClass)
	tankView:Init(self._posCallBack , tankData)
	tankView:LoadTank( tankData)
	tankView.transform:SetParent(self.transform)
	return tankView
	--else
	--	return tankView
	--end
end




function TankViewContainer:GetTankView(tankId)
	local tankView =self._tankViewDic[tankId]
	if not tankView then
		clog("not contain tank "..tostring(tankId))
	else
		return tankView
	end
end

function TankViewContainer:HighLightAtkTank(tankId)
	local tankView = self._tankViewDic[tankId]
	tankView:ATkHighLight()
end

function TankViewContainer:HighLightBeatTank(tankId)
	local tankView = self._tankViewDic[tankId]
	tankView:BeatHighLight()
end

function TankViewContainer:GetTankPos(tankId)
	return self._tankViewDic[tankId].transform.position
end


return TankViewContainer