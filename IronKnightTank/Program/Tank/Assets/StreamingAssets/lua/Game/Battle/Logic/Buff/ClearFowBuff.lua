---------------------------------------------
--- ClearFowBuff
--- Created by thrt520.
--- DateTime: 2018/8/9
---清楚战争阴影buff  用于飞机侦查
---------------------------------------------
---------------------------------------------
---@class ClearFowBuff : BaseBuff
local ClearFowBuff = class("ClearFowBuff")
local this = ClearFowBuff

this.BuffId = 1002

---@type number
this._round = 0


---@type PlaneViewer
this._viewer = nil
local receiveCommands = {
	require("Game.Event.Message.Battle.MSGRoundStart") ,
}

local ShadowModel = require("Game.Battle.Model.ShadowModel")
local ViewFacade = require("Game.Battle.View.ViewFacade")
local PlaneViewer = require("Game.Battle.Logic.Viewer.PlaneViewer")
function ClearFowBuff:ctor(round  , camp , area , pos)
	self._round = round
	self._viewer = PlaneViewer.new(camp , area , pos)
	self:_registerCommandHandler()
	self:OnStart()
end


function ClearFowBuff:OnStart()
	-----播放特效等巴拉巴拉
	ShadowModel.AddViewer(self._viewer)
	ShadowModel.UpdateFOW()
	ViewFacade.UpdateShadowView()
end

function ClearFowBuff:OnEnd()
	-----关闭特效等巴拉巴拉的
	ShadowModel.RemoveViewer(self._viewer)
	ShadowModel.UpdateFOW()
	ViewFacade.UpdateAllTank()
	ViewFacade.UpdateShadowView()
end

function ClearFowBuff:Refresh()

end

function ClearFowBuff:Run(type, val)

end


function ClearFowBuff:_registerCommandHandler()
	EventBus:RegisterSelfReceiver(receiveCommands,self)
end

function ClearFowBuff:_unregisterCommandHandler()
	EventBus:UnregisterReceiver(receiveCommands,self)
end

function ClearFowBuff:Dispose()
	self:_unregisterCommandHandler()
	self:OnEnd()
	self._viewer = nil
end

-----------------------------------------
---event handler
-----------------------------------------
----@param msg MSGRoundStart
function ClearFowBuff:OnMSGRoundStart(msg)
	self._round = self._round - 1
	if self._round<=0 then
		self:Dispose()
	end
end


return ClearFowBuff