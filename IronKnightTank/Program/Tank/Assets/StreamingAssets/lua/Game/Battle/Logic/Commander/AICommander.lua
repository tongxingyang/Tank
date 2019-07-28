---------------------------------------------
--- AICommander
---AI指挥官，存储和调用AI
--- Created by thrt520.
--- DateTime: 2018/6/20
---------------------------------------------
---@class AICommander
local AICommander = class("AICommander")

---@type ECamp 阵营
AICommander.Camp = nil
---@type table<number , BaseAIClient> 坦克AI
AICommander.tankAIList = nil
---@type boolean 是否活动
AICommander.isActive = false
local AIClient = require("Game.Battle.AI.AIClient")
local TankModel = require("Game.Battle.Model.TankModel")
local MSGCampCtrlEndRequest =require("Game.Event.Message.Battle.MSGCampCtrlEndRequest")

local receiveCommand = {
	require("Game.Event.Message.Battle.MSGActiveNextCamp"),
}

function AICommander:ctor(camp)
	self.Camp = camp
	self.isActive = true
	self:_registerCommandHandler()
end


function AICommander:_registerCommandHandler()
	EventBus:RegisterSelfReceiver(receiveCommand,self )
end

function AICommander:_unregisterCommandHandler()
	EventBus:UnregisterSelfReceiver(receiveCommand,self )
end

--------------------------------
---even handler
--------------------------------
---@param msg MSGActiveNextCamp
function AICommander:OnMSGActiveNextCamp(msg)
	if self.Camp == msg.Camp then
		self.co = coroutine.createAndRun(AICommander._aiAction  , self)
	end
end


function AICommander:_aiAction()
	self.tankAIList = {}
	local tankDatas = TankModel.GetCampTanks(self.Camp)
	for i, v in pairs(tankDatas) do
		if not self.tankAIList[v.Id] then
			self.tankAIList[v.Id] =AIClient.new(v)
		end
	end
	for i, v in pairs(self.tankAIList) do
		while self.isActive and v:CanAction()  do
			v:MakeDecision()
		end
	end
	EventBus:Brocast(MSGCampCtrlEndRequest:Build({Camp = self.Camp}))
end

function AICommander:Dispose()
	self.isActive = false
	if self.tankAIList then
		for i, v in pairs(self.tankAIList) do
			v:Dispose()
		end
	end

	self.tankAIList = nil
	self:_unregisterCommandHandler()
end

return AICommander