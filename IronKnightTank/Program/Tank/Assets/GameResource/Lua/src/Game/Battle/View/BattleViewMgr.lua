---------------------------------------------
--- BattleViewMgr
---	主要负责View生成销毁管理
--- Created by thrt520.
--- DateTime: 2018/5/4
---------------------------------------------
local BattleViewMgr = {}
local this = BattleViewMgr


---@type TankViewContainer
this._tankViewContainer = nil			----坦克

---@type TankDirChoseView
this._tankDirChoseView = nil			----选择坦克方向

---@type TankTrajectoryView
this._tankTrajectoryView= nil			----显示坦克弹道

---@type TankSignView
this._tankSignView= nil 				----坦克标记

---@type ThreeDTankDirChoseView
this._3dTankDirChoseView= nil 			----坦克标记

---@type BattleCamMgr
this._battleCamMgr = nil 				----镜头管理器

---@type LogicMapView
this._logicMapView = nil 				----逻辑地图管理器

---@type MapView
this._mapView = nil						----地圖

local MapViewClass = require("Game.Battle.View.MapView")
local LogicMapViewClass = require("Game.Battle.View.LogicMapView")
local TankViewContainerClass =require("Game.Battle.View.TankViewContainer")
local TankTrajectoryView = require("Game.Battle.View.TankTrajectoryView")
local ShadowViewContainer  =require("Game.Battle.View.ShadowViewContainer")
local TankSignView = require("Game.Battle.View.TankSignView")

local receiveCommand = {
	require("Game.Event.Message.Battle.MSGInitBattle"),
	require("Game.Event.Message.Battle.MSGShowTankTrajectory"),
	require("Game.Event.Message.Battle.MSGShowTankChoseDirView"),
	require("Game.Event.Message.Battle.MSGCloseTankChoseDirView"),
	require("Game.Event.Message.Battle.MSGShowChoseTankTips"),
	require("Game.Event.Message.Battle.MSGCancelShowChoseTankTips"),
	require("Game.Event.Message.Battle.MSGOpenAimTankCam"),
	require("Game.Event.Message.Battle.MSGCloseAimTankCam"),
	require("Game.Event.Message.Battle.MSGShowChoseTankTips"),
	require("Game.Event.Message.Battle.MSGCamFollowTank"),
	require("Game.Event.Message.Battle.MSGStopCamFollow"),
	require("Game.Event.Message.Battle.MSGCamFocusTank"),
}

function BattleViewMgr.OnIntoScene()
	this._registerCommandHandler()
	this._mapView = MapViewClass.new()
	this._logicMapView = LogicMapViewClass.new()
	this._tankViewContainer = TankViewContainerClass.new()
	this._shadowViewContainer = ShadowViewContainer.new()
	this._battleCamMgr = require("Game.Battle.View.BattleCamMgr")
end

function BattleViewMgr.OnLeaveScene()
	if this._3dTankDirChoseView then
		ViewManager.ReturnPoolView( this._3dTankDirChoseView)
		this._3dTankDirChoseView = nil
	end
	if this._tankTrajectoryView then
		ViewManager.ReturnPoolView( this._tankTrajectoryView)
		this._tankTrajectoryView = nil
	end
	this._tankViewContainer:Dispose()
	this._tankViewContainer = nil
	this._mapView:Dispose()
	this._mapView = nil
	this._shadowViewContainer:Dispose()
	this._shadowViewContainer = nil
	this._logicMapView:Dispose()
	this._logicMapView = nil
	this._battleCamMgr.Dispose()
	this._unregisterCommandHandler()
end
------------------------------------------------------------------------------------
--- function
------------------------------------------------------------------------------------
function BattleViewMgr._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommand,BattleViewMgr)
end

function BattleViewMgr._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommand,BattleViewMgr)
end
--------------------------------------------------------------
--- event handler
--------------------------------------------------------------
--- MSGInitBattle 事件处理
---@param msg MSGInitBattle
function BattleViewMgr.OnMSGInitBattle(msg)
	coroutine.createAndRun(this._initBattle , msg)
end

--- MSGShowTankTrajectory 事件处理
---@param msg MSGShowTankTrajectory
function BattleViewMgr.OnMSGShowTankTrajectory(msg)
	this._tankTrajectoryView:Show(msg.StartPos, msg.EndPos)
end

--- MSGCancelShowTankTrajectory 事件处理
---@param msg MSGCancelShowTankTrajectory
function BattleViewMgr.OnMSGCancelShowTankTrajectory(msg)
	this._tankTrajectoryView:Close()
end

--- MSGCamFocusTank 事件处理
---@param msg MSGCamFocusTank
function BattleViewMgr.OnMSGCamFocusTank(msg)
	local tankContainer = msg.IsMine and this._tankViewContainer or this._enemyTankViewContainer
	local pos = tankContainer:GetTankPos(msg.TankId)
	this._battleCamera:MoveTo(pos ,msg.Time , msg)
end


--- MSGOpenAimTankCam 事件处理
---@param msg MSGOpenAimTankCam
function BattleViewMgr.OnMSGOpenAimTankCam(msg)
	local tankView = this._tankViewContainer:GetTankView(msg.TankId)
	this._battleCamMgr.ShowAimCamera(tankView , msg)
end

--- MSGCloseAimTankCam 事件处理
---@param msg MSGCloseAimTankCam
function BattleViewMgr.OnMSGCloseAimTankCam(msg)
	this._battleCamMgr.CloseAimTank()
	this._battleCamMgr.OpenBattleCam()
	local tankView = this._tankViewContainer:GetTankView(msg.TankId)
	GameObjectUtilities.SetLayerWidthChildren(tankView.gameObject ,8 )
	tankView:ResetMaterial()
	tankView:OpenMoveEffect()
	tankView.transform:SetParent(this._tankViewContainer.transform)
	tankView.transform.localEulerAngles = Vector3.New(0 , 0 , 0)
end

--- MSGShowTankChoseDirView 事件处理
---@param msg MSGShowTankChoseDirView
function BattleViewMgr.OnMSGShowTankChoseDirView(msg)
	this._3dTankDirChoseView.gameObject:SetActive(true)
	this._3dTankDirChoseView:SetPos(msg.WroldPos)
end

--- MSGCloseTankChoseDirView 事件处理
---@param msg MSGCloseTankChoseDirView
function BattleViewMgr.OnMSGCloseTankChoseDirView(msg)
	this._3dTankDirChoseView.gameObject:SetActive(false)
end


--- MSGShowChoseTankTips 事件处理
---@param msg MSGShowChoseTankTips
function BattleViewMgr.OnMSGShowChoseTankTips(msg)
	this._tankSignView:SetPos(msg.Pos)
	this._tankSignView:SetMode(msg.IsMine)
end

--- MSGCancelShowChoseTankTips 事件处理
---@param msg MSGCancelShowChoseTankTips
function BattleViewMgr.OnMSGCancelShowChoseTankTips(msg)
	this._tankSignView:Close()
end


--- MSGCamFocusTank 事件处理
---@param msg MSGCamFocusTank
function BattleViewMgr.OnMSGCamFocusTank(msg)
	local tankView = this._tankViewContainer:GetTankView(msg.TankId)
	msg:Pend()
	this._battleCamMgr.Focus(tankView.transform.position , msg.Time , function ()
		msg:Restore()
	end)
end

--- MSGCamFollowTank 事件处理
---@param msg MSGCamFollowTank
function BattleViewMgr.OnMSGCamFollowTank(msg)
	local tankView = this._tankViewContainer:GetTankView(msg.TankId)
	this._battleCamMgr.Follow(tankView.transform)
end

--- MSGStopCamFollow 事件处理
---@param msg MSGStopCamFollow
function BattleViewMgr.OnMSGStopCamFollow(msg)
	this._battleCamMgr.StopFollow()
end
--------------------------------------------------------------
--- function
--------------------------------------------------------------

---@param msg MSGInitBattle
function BattleViewMgr._initBattle(msg)
	msg:Pend()
	this._tankTrajectoryView = ViewManager.GetPoolViewYield(nil , TankTrajectoryView)
	this._tankTrajectoryView:Close()
	this._battleCamMgr.Init()
	this._3dTankDirChoseView = ViewManager.GetPoolViewYield(nil , require("Game.Battle.View.ThreeDTankDirChoseView"))
	this._3dTankDirChoseView.gameObject:SetActive(false)
	this._tankSignView = ViewManager.GetPoolViewYield(nil , TankSignView)
	this._tankSignView:Close()
	msg:Restore()
end


function BattleViewMgr._getTankViewContianer(isMine)
	return isMine and this._tankViewContainer or this._enemyTankViewContainer
end

return BattleViewMgr