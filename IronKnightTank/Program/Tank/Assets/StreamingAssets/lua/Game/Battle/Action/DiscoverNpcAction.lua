---------------------------------------------
--- DiscoverNpcAction
--- 发现坦克动作
---	镜头聚焦坦克
---	打开坦克对话面板
--- Created by thrt520.
--- DateTime: 2018/5/31
---------------------------------------------
---@class DiscoverNpcAction
local DiscoverNpcAction = class("DiscoverNpcAction")

---@type IViewer
DiscoverNpcAction.Viewer = 0
---@type IViewer
DiscoverNpcAction.BeSeenViewer = 0
---@type boolean
DiscoverNpcAction.HasBeenSeen = 0

local MSGUpdateViewer = require("Game.Event.Message.Battle.MSGUpdateViewer")
local ViewFacade = require("Game.Battle.View.ViewFacade")

function DiscoverNpcAction:ctor(viewer , beSeenViewer , hasBeenSeeen)
	self.Viewer = viewer
	self.BeSeenViewer = beSeenViewer
	self.HasBeenSeen = hasBeenSeeen
end

function DiscoverNpcAction:Action()
	if not self.HasBeenSeen then
		local co = coroutine.running()
		ViewFacade.UpdateShadowView()
		EventBus:Brocast(MSGUpdateViewer:Build({Viewer = self.BeSeenViewer }))
		local content = self.Viewer:GetViewerName().."发现"..self.BeSeenViewer:GetViewerName()
		ViewFacade.CamFocusTank(self.BeSeenViewer.Id , 1):Yield()
		PanelManager.Open(PanelManager.PanelEnum.SoliderTalkPanel , {Content = content , SolideName = self.Viewer:GetViewerName()  , IconPath= self.Viewer:GetViewerIcon() , CloseCallBack = function()
			coroutine.resume(co)
		end })
		coroutine.yield()
		coroutine.wait(0.5)
	end
end

return DiscoverNpcAction