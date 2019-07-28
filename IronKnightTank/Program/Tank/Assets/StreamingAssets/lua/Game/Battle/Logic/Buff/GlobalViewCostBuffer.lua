---------------------------------------------
--- GlobalViewCostBuffer
--- Created by thrt520.
--- DateTime: 2018/8/28
---全局视野消耗buff
---------------------------------------------
---@class GlobalViewCostBuffer : GlobalViewCostBuffer
local GlobalViewCostBuffer = class("GlobalViewCostBuffer")
local this = GlobalViewCostBuffer
local ShadowModel = require("Game.Battle.Model.ShadowModel")
this.BuffId = 1001

this.GlobalViewCostCofficient = 1

this._round = 0


local receiveCommands = {
	require("Game.Event.Message.Battle.MSGRoundStart") ,
}

function GlobalViewCostBuffer:ctor(val , round)
	self.GlobalViewCostCofficient = val
	self._round = round
	ShadowModel.AddBuff(self)
end

function GlobalViewCostBuffer:OnStart()
	self:_registerCommandHandler()
end

function GlobalViewCostBuffer:OnEnd()
	ShadowModel.RemoveBuff(self)
	self:_unregisterCommandHandler()
end


function GlobalViewCostBuffer:Run(type , val)
	if type == "GlobalView" then
		return self.GlobalViewCostCofficient * val
	else
		return val
	end
end

---@param buff GlobalViewCostBuffer
function GlobalViewCostBuffer:Refresh(buff)

end

function GlobalViewCostBuffer:_registerCommandHandler()
	EventBus:RegisterSelfReceiver(receiveCommands,self)
end

function GlobalViewCostBuffer:_unregisterCommandHandler()
	EventBus:UnregisterSelfReceiver(receiveCommands,self)
end
-----------------------------------------
---event handler
-----------------------------------------
----@param msg MSGRoundStart
function GlobalViewCostBuffer:OnMSGRoundStart(msg)
	if self._round == -1 then
		return
	end
	self._round = self._round - 1
	if self._round <= 0 then
		self:OnEnd()
	end
end

return GlobalViewCostBuffer