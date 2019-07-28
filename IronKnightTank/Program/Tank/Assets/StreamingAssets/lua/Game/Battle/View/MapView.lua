---------------------------------------------
--- MapView
--- Created by thrt520.
--- DateTime: 2018/5/4
---------------------------------------------
---@class MapView
local MapView = class("MapView")
local this = MapView

local BlockViewClass = require("Game.Battle.View.BlockView")

this.gameObject = nil

this.transform = nil

---@type BlockView[]
this._blockViewDic = nil

this._mapObj = nil
local receiveCommands = {
	require("Game.Event.Message.Battle.MSGInitMap"),
}

function MapView:ctor()
	local gameObject  = GameObject.New()
	gameObject.name = "Map"
	self.gameObject = gameObject
	self.transform = gameObject.transform
	self._blockViewDic = {}
	EventBus:RegisterSelfReceiver(receiveCommands , self )
end


function MapView:_registerCommandHandler()
	EventBus:RegisterSelfReceiver(receiveCommands,self )
end

function MapView:_unregisterCommandHandler()
	EventBus:UnregisterSelfReceiver(receiveCommands,self)
end
---------------------------------------------------------
---event handler
---------------------------------------------------------
--- MSGInitMap 事件处理
---@param msg MSGInitMap
function MapView:OnMSGInitMap(msg)
	self:LoadMap(msg.MapData)
end
---------------------------------------------------------

---@param mapData MapData
function MapView:LoadMap(mapData)
	if mapData.MapRes ~= "" then
		self._mapObj = ResMgr.InstantiatePrefabYield(nil , "Prefab/Map/"..mapData.MapRes..".prefab")
		GameObjectUtilities.SetTransformParentAndNormalization(self._mapObj.transform  , self.transform)
	else
		for pos, blockData in pairs(mapData.BlockDataDic) do
			local blockView = ViewManager.GetPoolViewYield(nil , BlockViewClass)
			GameObjectUtilities.SetTransformParentAndNormalization(blockView.transform , self.transform)
			blockView:Load(blockData)
			self._blockViewDic[pos] = blockView
		end
	end
end

function MapView:Dispose()
	self:_unregisterCommandHandler()
	for i, v in pairs(self._blockViewDic) do
		v:Dispose()
		ViewManager.ReturnPoolView(v)
	end
	self._blockViewDic = nil
	self._mapObj = nil
end

return MapView