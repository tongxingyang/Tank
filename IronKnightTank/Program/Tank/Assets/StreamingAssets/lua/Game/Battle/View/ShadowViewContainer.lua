---------------------------------------------
--- ShadowViewContainer
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class ShadowViewContainer
local ShadowViewContainer = class("ShadowViewContainer")
local this = ShadowViewContainer

this.gameObject = nil
this.transform = nil
this._fowViewDic = nil
this._shadowViewDataDic = nil
---@type Transform 战争阴影父节点
this._fowParent = nil
---@type table<number , Sprite> 战争阴影贴图资源缓存
this._shadowSpriteCache = nil

local FogOfWarView = require("Game.Battle.View.FogOfWarView")
local ShadowViewData = require("Game.Battle.Model.Data.Shadow.ShadowViewData")

local receiveCommand = {
	require("Game.Event.Message.Battle.MSGUpdateShadow"),
	require("Game.Event.Message.Battle.MSGInitFOW"),
}

function ShadowViewContainer:ctor()
	self.gameObject = GameObject.New()
	self.gameObject.name = "FOW"
	self.transform = self.gameObject.transform
	self._fowParent = self.transform
	self._fowViewDic = {}
	self._shadowViewDataDic = {}
	self._shadowSpriteCache = {}
	self:_registerCommandHandler()
end

function ShadowViewContainer:_registerCommandHandler()
	EventBus:RegisterSelfReceiver(receiveCommand,self )
end

function ShadowViewContainer:_unregisterCommandHandler()
	EventBus:UnregisterSelfReceiver(receiveCommand,self)
end

---------------------------------------------------------
---event handler
---------------------------------------------------------
---更新战争阴影
---先更新战争阴影数据
---再根据数据更新表现
---@param msg MSGUpdateShadow
function ShadowViewContainer:OnMSGUpdateShadow(msg)
	self:_updateShadowViewData(msg.ShadowDataList)
	self:_updateView()
end

---@param msg MSGInitFOW
function ShadowViewContainer:OnMSGInitFOW(msg)
	self:_preLoadSprite()
	self:_updateShadowViewData(msg.ShadowNodeDataList)
	self:_createView()
end
---------------------------------------------------------
---创建FowView
---@param shadowDataList ShadowNodeData[]
function ShadowViewContainer:_createView(shadowDataList)
	for i, v in pairs(self._shadowViewDataDic) do
		local view = ViewManager.GetPoolViewYield(nil , FogOfWarView)
		view:SetGetSpriteCallBack(function (index)
			return self:_getShadowSprite(index)
		end)
		view:Update(v)
		view.transform.parent = self._fowParent
		view:SetPos(v)
		self._fowViewDic[i] = view
	end
end

---更新视图
function ShadowViewContainer:_updateView()
	for i, v in pairs(self._shadowViewDataDic) do
		local view = self._fowViewDic[i]
		if view then
			view:Update(v)
		end
	end
end

---预加载资源
function ShadowViewContainer:_preLoadSprite()
	for i = 1, 15 do
		local sprite = ResMgr.LoadSpriteYield(nil , "Sprite/FOW/"..tostring(i)..".png")
		self._shadowSpriteCache[i] = sprite
	end
end

---计算并更新ShaowViewData
function ShadowViewContainer:_updateShadowViewData(shadowDataList)
	for i, v in pairs(shadowDataList) do
		local pos = v.Pos
		local leftUpPos = GridPos.New(pos.x - 0.5 , pos.y +0.5)
		local rightUpPos = GridPos.New(pos.x + 0.5 , pos.y +0.5)
		local rightDownPos = GridPos.New(pos.x + 0.5 , pos.y - 0.5)
		local leftDownPos = GridPos.New(pos.x - 0.5 , pos.y - 0.5)
		local shadowViewData1 = self:_getShadowViewData(leftUpPos)
		local shadowViewData2 = self:_getShadowViewData(rightUpPos)
		local shadowViewData3 = self:_getShadowViewData(rightDownPos)
		local shadowViewData4 = self:_getShadowViewData(leftDownPos)
		shadowViewData1:SetShadowNodeData(v)
		shadowViewData2:SetShadowNodeData(v)
		shadowViewData3:SetShadowNodeData(v)
		shadowViewData4:SetShadowNodeData(v)
	end
end

---@return ShadowViewData
function ShadowViewContainer:_getShadowViewData(pos)
	local data = self._shadowViewDataDic[pos]
	if not data then
		data = ShadowViewData.new(pos)
		self._shadowViewDataDic[pos] = data
	end
	return data
end

function ShadowViewContainer:_getShadowSprite(index)
	local sprite = self._shadowSpriteCache[index]
	if not sprite then
		clog(tostring(index))
	end
	return sprite
end

function ShadowViewContainer:Dispose()
	self:_unregisterCommandHandler()
end


return ShadowViewContainer