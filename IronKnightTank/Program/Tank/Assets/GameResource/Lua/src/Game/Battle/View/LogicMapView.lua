---------------------------------------------
--- LogicMapView
--- Created by thrt520.
--- DateTime: 2018/9/10
---------------------------------------------
---@class LogicMapView
local LogicMapView = class("LogicMapView")


local this = LogicMapView

local LogicBlockViewClass = require("Game.Battle.View.LogicBlockView")
this.gameObject = nil
this.transform = nil
---@type table< GridPos , LogicBlockView>
this._blockViewDic = nil

local receiveCommands = {
	require("Game.Event.Message.Battle.MSGUpdateBlock"),
	require("Game.Event.Message.Battle.MSGCancelBornHighLight"),
	require("Game.Event.Message.Battle.MSGHighLightBornBlock"),
	require("Game.Event.Message.Battle.MSGInitMap"),
}

function LogicMapView:ctor()
	local gameObject  = GameObject.New()
	gameObject.name = "LogicMap"
	self.gameObject = gameObject
	self.transform = gameObject.transform
	self._blockViewDic = {}
	self:_registerCommandHandler()
end


function LogicMapView:_registerCommandHandler()
	EventBus:RegisterSelfReceiver(receiveCommands,self )
end

function LogicMapView:_unregisterCommandHandler()
	EventBus:UnregisterSelfReceiver(receiveCommands,self)
end

---------------------------------------------------------
---event handler
---------------------------------------------------------
--- MSGUpdateBlock 事件处理
---@param msg MSGUpdateBlock
function LogicMapView:OnMSGUpdateBlock(msg)
	self:UpdateBlock(msg.BlockDataDic)
end

--- MSGHighLightBornBlock 事件处理
function LogicMapView:OnMSGHighLightBornBlock(msg)
	self:HightLightBornPos()
end

--- MSGCancelBornHighLight 事件处理
function LogicMapView:OnMSGCancelBornHighLight(msg)
	self:CancelBornHighLight()
end

--- MSGInitMap 事件处理
---@param msg MSGInitMap
function LogicMapView:OnMSGInitMap(msg)
	self:LoadMap(msg.MapData)
end
---------------------------------------------------------

---@param mapData MapData
function LogicMapView:LoadMap(mapData)
	for pos, blockData in pairs(mapData.BlockDataDic) do
		local blockView = ViewManager.GetPoolViewYield(nil , LogicBlockViewClass)
		LuaTools.SetTranParent(blockView.transform , self.transform)
		blockView:Update(blockData)
		self._blockViewDic[pos] = blockView
	end
end

---@param blockDataArray BlockData[]
function LogicMapView:UpdateBlock(blockDataArray)
	for pos, blockData in pairs(blockDataArray) do
		local blockView = self._blockViewDic[pos]
		blockView:Update(blockData)
	end
end

function LogicMapView:HightLightBornPos()
	for i, v in pairs(self._blockViewDic) do
		if v.blockData.IsBornBlock then
			v:_bornHighLight()
		end
	end
end

function LogicMapView:CancelBornHighLight()
	for i, v in pairs(self._blockViewDic) do
		if v.blockData.IsBornBlock then
			v:_cancelBornHighLight()
		end
	end
end


function LogicMapView:Dispose()
	self:_unregisterCommandHandler()
	for i, v in pairs(self._blockViewDic) do
		v:Dispose()
		ViewManager.ReturnPoolView(v)
	end
	self.gameObject = nil
	self.transform = nil
	self._blockViewDic = nil
end


return LogicMapView
