---------------------------------------------
--- 坦克发言动作
---	镜头聚焦坦克
---	打卡坦克对话面板
--- Created by thrt520.
--- DateTime: 2018/6/1
---------------------------------------------
---@class TankTalkAction
local TankTalkAction = class("SoliderTalkAction")

local MSGCamFocusTank = require("Game.Event.Message.Battle.MSGCamFocusTank")

---@type BaseFightTank
TankTalkAction.TankData = nil
TankTalkAction.Content = ""

function TankTalkAction:ctor(tankData , talkContent)
	self.TankData = tankData
	self.Content = talkContent
end

function TankTalkAction:Action()
	EventBus:Brocast(MSGCamFocusTank:Build({ TankId = self.TankData.Id , IsMine = self.TankData.IsPlayer, Time = 1})):Yield()
	local soliderData = self.TankData.Solider

	local co = coroutine.running()
	PanelManager.Open(PanelManager.PanelEnum.SoliderTalkPanel , {Content = self.Content ,SoliderName = soliderData:GetSoliderName() ,  IconPath = soliderData:GetSoliderIconPath()  , CloseCallBack = function()
		coroutine.resume(co)
	end })
	coroutine.yield()
	coroutine.wait(0.5)
end

return TankTalkAction