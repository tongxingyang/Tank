---------------------------------------------
--- SmokeBuff
--- Created by thrt520.
--- DateTime: 2018/8/8
---烟雾弹buff
---------------------------------------------
---@class SmokeBuff :BaseBuff
local SmokeBuff = class("SmokeBuff" , require("Game.Battle.Logic.Buff.BaseBuff"))
local this = SmokeBuff

this.BuffId = 1001

---@type number
this.round = 0

----@type number
this.ViewCost = 0

---@type GridPos[]
this._smokePosList = nil

---@type ShadowNodeData
this._shadowNodeData = nil

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGRoundStart") ,
}


---@param viewCostAdd number
---@param shadowNodeData ShadowNodeData
function SmokeBuff:ctor(viewCostAdd , round  , shaodowNodeData)
	self.ViewCost = viewCostAdd
	self.round = round
	self._shadowNodeData = shaodowNodeData
	self:OnStart()
end


function SmokeBuff:OnStart()
	-----播放特效等巴拉巴拉
	self:_registerCommandHandler()
end

function SmokeBuff:OnEnd()
	-----关闭特效等巴拉巴拉的
	self._shadowNodeData:RemoveBuff(self)
	self:_unregisterCommandHandler()
end

function SmokeBuff:_registerCommandHandler()
	EventBus:RegisterSelfReceiver(receiveCommands,self)
end

function SmokeBuff:_unregisterCommandHandler()
	EventBus:UnregisterSelfReceiver(receiveCommands,self)
end

function SmokeBuff:Run(type, val)
	if type == "ViewCost" then
		return val + self.ViewCost
	end
	return val
end

---@param buff SmokeBuff
function SmokeBuff:Refresh(buff)
	if self.BuffId ~= buff.BuffId then
		return
	end
	self.round = buff.round
	self.ViewCost = buff.ViewCost
end

-----------------------------------------
---event handler
-----------------------------------------
----@param msg MSGRoundStart
function SmokeBuff:MSGRoundStart(msg)
	self.round = self.round - 1
	if self.round <= 0 then
		self:OnEnd()
	end
end


return SmokeBuff