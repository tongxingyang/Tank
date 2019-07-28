---------------------------------------------
--- UIViewContainer
--- Created by thrt520.
--- DateTime: 2018/9/3
--------------------------------------------
---@class UIViewContainer
local UIViewContainer = class("UIViewContainer")

UIViewContainer.ViewClass = nil
---@type View[]
UIViewContainer._closeViewList = nil
---@type View[]
UIViewContainer._openViewList = nil
---@type table
UIViewContainer._layoutGroup = nil
---@type Transform
UIViewContainer._viewParentTran = nil
---@type function
UIViewContainer._event = nil
---@type table<table , View>
UIViewContainer._viewDic = nil

function UIViewContainer:ctor(classPath  , parent, gridLayoutGroup , event)
	self.ViewClass = require(classPath)
	self._viewParentTran = parent
	self._layoutGroup = gridLayoutGroup
	self._openViewList = {}
	self._closeViewList = {}
	self._viewDic = {}
	self._event = event
end


function UIViewContainer:Update(viewDataList)
	for i, v in pairs(self._openViewList) do
		v.gameObject:SetActive(false)
		table.insert(self._closeViewList , v)
	end
	self._openViewList = {}
	self._viewDic = {}
	local index = 0
	for i, v in pairs(viewDataList) do
		local view = self:_getView()
		table.insert(self._openViewList , view)
		view:Update(v)
		self._viewDic[v] = view
		view.gameObject:SetActive(true)
		if self._layoutGroup then
			view.transform.localPosition = self._layoutGroup:GetGridPos(index)
		end
		index = index + 1
	end
end

function UIViewContainer:_getView()
	local view = self._closeViewList[1]
	if not view then
		view = ViewManager.GetPoolViewYield(nil  , self.ViewClass)
		if self._event and view.SetEvent then
			view:SetEvent(self._event)
		end
		GameObjectUtilities.SetTransformParentAndNormalization(view.transform , self._viewParentTran)
	else
		table.remove(self._closeViewList , 1)
	end
	return view
end


function UIViewContainer:GetView(data)
	local view = self._viewDic[data]
	if view then
		return view
	else
		clog("view nil "..tostring(data))
		return nil
	end
end

function UIViewContainer:Dispose()
	for i, v in ipairs(self._closeViewList) do
		v:Dispose()
	end
	for i, v in ipairs(self._openViewList) do
		v:Dispose()
	end
	self._closeViewList = nil
	self._openViewList = nil
	self._viewParentTran = nil
	self._viewDic = nil
	self.ViewClass = nil
end

return UIViewContainer