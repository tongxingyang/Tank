---------------------------------------------
--- BattleCamMgr
---	战斗镜头管理器
--- Created by thrt520.
--- DateTime: 2018/6/26
---------------------------------------------
---@class BattleCamMgr
local BattleCamMgr = {}
local this = BattleCamMgr

---@type 战场摄像机
BattleCamMgr._battleCamera = nil

---@type TankAimCamera  坦克瞄准摄像机
BattleCamMgr._tankAimCamrea = nil

local receiveCommands = {
}

function BattleCamMgr.Init()
	this._tankAimCamrea = ViewManager.GetPoolViewYield(nil , require("Game.Battle.View.TankAimCamera"))
	this._tankAimCamrea.gameObject:SetActive(false)
	this._battleCamera = GameObject.Find("CameMover"):GetComponent("CameraMover")
	this._registerCommandHandler()
end


---------------------------------------------------
---evet handler
---------------------------------------------------


------------------------------------------------------------------------------------
--- function
------------------------------------------------------------------------------------
function BattleCamMgr._registerCommandHandler()
	EventBus:RegisterReceiver(receiveCommands,BattleCamMgr)
end

function BattleCamMgr._unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,BattleCamMgr)
end

function BattleCamMgr.ShowAimTank(angle)
	this._tankAimCamrea.gameObject:SetActive(true)
end

function BattleCamMgr.ShowAimCamera(tankView , msg)
	this._tankAimCamrea:SetTank(tankView , msg)
	this._tankAimCamrea.gameObject:SetActive(true)
end

function BattleCamMgr.CloseAimTank()
	this._tankAimCamrea:Close()
end

function BattleCamMgr.CloseBattleCam()
	this._battleCamera.gameObject:SetActive(false)
end

function BattleCamMgr.OpenBattleCam()
	this._battleCamera.gameObject:SetActive(true)
end

function BattleCamMgr.CameraFollow(tran)
	this._battleCamera:Follow(tran)
end

function BattleCamMgr.StopFollow()
	this._battleCamera:StopFollow()
end

function BattleCamMgr.Follow(tran)
	this._battleCamera:Follow(tran)
end

function BattleCamMgr.Focus(pos, time , callBack)
	this._battleCamera:Focus(pos , time , callBack)
end

------------------------------------------------------------------------------------
function BattleCamMgr.Dispose()
	this._unregisterCommandHandler()
	--this._battleCamera:Dispose()
	ViewManager.ReturnPoolView(this._tankAimCamrea)
	this._tankAimCamrea = nil
	this._battleCamera = nil
end

return BattleCamMgr