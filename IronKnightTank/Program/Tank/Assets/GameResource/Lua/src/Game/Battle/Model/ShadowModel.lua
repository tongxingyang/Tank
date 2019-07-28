---------------------------------------------
--- ShadowModel
--- Created by thrt520.
--- DateTime: 2018/7/10
---------------------------------------------
---@class ShadowModel
local ShadowModel = {}
local this = ShadowModel

---@type table<GridPos , ShadowNodeData>
this._shadowDataDic = {}

---@type IViewer[]   				默认只有两个阵营
this._viewerList = {}

---@type ECamp[]					决定视野阵营
this._shadowExpolreCampList = {}

---@type table<GridPos , number>    预处理地图视野数据
this._mapPathData = {}

---@type table<number , BaseBuff>	buff
this._buffers ={}

---@type number 全局视野消耗系数
this._viewCostCofficient = 1

local ShadowNodeData = require("Game.Battle.Model.Data.Shadow.ShadowNodeData")
local MSGFirstObeserveViewer = require("Game.Event.Message.Battle.MSGFirstObeserveViewer")
------------------------------------------------------------------------------------
--- sceneActor
------------------------------------------------------------------------------------
function ShadowModel.OnIntoScene()
	this.InitShadowNodeData( require( BattleConfig.LevelFolderPath..BattleConfig.MapScriptName))
	this._initMapPathData()
	this.SetShadowExploreViewerCamp({BattleConfig.PlayerCamp })
end

function ShadowModel.OnLeaveScene()
	this.Dispose()
end

------------------------------------------------------------------------------------
---@param editorLevelData EditorLevelData
function ShadowModel.InitShadowNodeData(editorLevelData)
	local editorBlockDataList = editorLevelData.BlockDataList
	for i, v in pairs(editorBlockDataList) do
		local shadowData = ShadowNodeData.new(v)
		this._shadowDataDic[shadowData.Pos] = shadowData
	end
end

function ShadowModel._initMapPathData()
	for i, v in pairs(this._shadowDataDic) do
		local t = {}
		this._mapPathData[i] = t
		local aroundShadow = this._getAroundShadow(i)
		for pos, shadowNodeData in pairs(aroundShadow) do
			t[pos] = true
		end
	end
end

---注册决定视野明暗的阵营
---@param eCampList ECamp[]
function ShadowModel.SetShadowExploreViewerCamp(eCampList)
	this._shadowExpolreCampList = eCampList
end

---获取阴影节点数据
---@return ShadowNodeData
function ShadowModel.GetShadowData()
	return this._shadowDataDic
end

---指定位置是否可见
---@return boolean
function ShadowModel.IsPosVisiable(pos)
	local shadowData = this.GetShadowNodeData(pos)
	if not shadowData then
		return false
	else
		return not shadowData:IsDark()
	end
end

---高亮指定区域，其余区域均为黑的
---@param posList GridPos[]
function ShadowModel.LightTargetPosList(posList)
	this._resetShadow()
	for i, v in pairs(posList) do
		local shadowNodeData = this.GetShadowNodeData(v)
		shadowNodeData:SetLight()
	end
end

---------------------------------------viewer------------------------------
----增加Viewer  list
---@param viewerList IViewer[]
function ShadowModel.AddViwerList(viewerList)
	for i, v in pairs(viewerList) do
		this.AddViewer(v)
	end
end

----增加Viewer  单独
---@param viewer IViewer
function ShadowModel.AddViewer(viewer)
	if not table.contains(this._viewerList, viewer) then
		table.insert(this._viewerList, viewer)
	else
		--clog("view has add to  shadowmodel"..tostring(viewer.Camp).."  "..tostring(viewer.Id))
	end
end

----移除viewer
---@param viewer IViewer
function ShadowModel.RemoveViewer(viewer)
	return table.removeValue(this._viewerList, viewer)
end

---获取指定位置的viewer
---@return IViewer[]
function ShadowModel._getViewerByPos(pos)
	local viewerList = {}
	for i, v in ipairs (this._viewerList) do
		if v.Pos == pos then
			table.insert(viewerList , v)
		end
	end
	return viewerList
end

---获取viewer 通过id
function ShadowModel.GetViewer(id)
	for i, v in ipairs(this._viewerList) do
		if v.Id == id then
			return v
		end
	end
	clog("not exit viewer "..tostring(id))
	return nil
end

----获取所有的viewer
function ShadowModel.GetAllViewer()
	return this._viewerList
end
---------------------------------------------------------------------
-----更新战争阴影
function ShadowModel.UpdateFOW()
	---@type table<GridPos , IViewer>
	for i, viewer in pairs(this._viewerList) do
		viewer:ResetObservers()
		viewer.LastTimeVisiable = viewer.Visiable
		viewer.Visiable = false
	end
	for i, v in pairs(this._shadowDataDic) do
		v:Reset()
	end
	local msgList = {}
	for i, viewer in ipairs(this._viewerList) do
		local viewArea = this.GetViewerViewArea(viewer)
		for _, pos in pairs(viewArea) do
			local shadowNodeData = this.GetShadowNodeData(pos)
			shadowNodeData:AddObserverCamp(viewer.Camp)
			local beSeenViewerList = this._getViewerByPos(pos)
			for i, v in ipairs(beSeenViewerList) do
				v:AddObserver(viewer.Camp)
			end
			if shadowNodeData:IsDark() and  table.contains(this._shadowExpolreCampList , viewer.Camp) then
				shadowNodeData:SetLight()
				for i, v in ipairs(beSeenViewerList) do
					v.Visiable = true
					if not v.LastTimeVisiable then
						table.insert(msgList , MSGFirstObeserveViewer:Build({ Viewer = viewer , BeSeenViewer = v, ViewerCamp = v.Camp , HasBeenSeen = v.HasBeenSeen}))
						if not v.HasBeenSeen then
							v.HasBeenSeen = true
						end
					end
				end
			end
		end
	end
	for i, v in ipairs(msgList) do
		EventBus:Brocast(v):Yield()
	end
end

----获取viewer视野范围
---@param iViewer IViewer
---@return GridPos[]
function ShadowModel.GetViewerViewArea(iViewer)
	if iViewer.ViewerType == EViewerType.Normal then
		return this._getViewArea(iViewer)
	elseif iViewer.ViewerType == EViewerType.Force then
		return this._getForceViewArea(iViewer)
	end
end

----获取视野范围  固定视野不考虑消耗
---@param iViewer IViewer
function ShadowModel._getForceViewArea(iViewer)
	local posList = iViewer.Pos:GetMultiQuadPos(iViewer:GetView())
	local areaList = {}
	for i, v in ipairs(posList) do
		if this._assertShadowNodeData(v) then
			table.insert(areaList , v)
		end
	end
	return areaList
end

---获取视野范围 考虑消耗
---@param iviewer IViewer
function ShadowModel._getViewArea(iviewer )
	local pos = iviewer.Pos
	local power = iviewer:GetView()
	---@type table<GridPos , number>
	local book = {}								--- 计算记录
	local viewDisRecord = {}					--- 记录距离
	local startNode = this._shadowDataDic[pos]
	if not startNode then
		clog("pos out of range"..tostring(pos))
		return {}
	end

	---@type table<ShadowNodeData, number>
	viewDisRecord[startNode] = power

	local index = pos
	while index do
		local min = 0
		local curNode
		for node, cost in pairs(viewDisRecord) do
			if not book[node.Pos] and cost >= min then
				min = cost
				index = node.Pos
				curNode = node
			end
		end
		if not book[index] then
			book[index] = 1
			for i, v in pairs(viewDisRecord) do
				local viewCost = this._getViewCost(i)
				if this._mapPathData[index][i.Pos] and v > viewCost + min then
					viewDisRecord[i] = viewCost + min
				end
			end
			local around = this._getAroundShadow(index)
			for i, v in ipairs(around) do
				if not viewDisRecord[v] then
					local val =  min -  this._getViewCost(v)
					if val >= 0 then
						viewDisRecord[v] = val
					end
				end
			end
		else
			index = nil
		end
	end
	local list = {}
	for i, v in pairs(book) do
		table.insert(list , i)
	end
	return list
end

---获取指定位置的战争阴影数据
---@return ShadowNodeData
function ShadowModel.GetShadowNodeData(pos)
	local data = this._shadowDataDic[pos]
	if not data then
		clog("pos wrong "..tostring(pos))
	end
	return data
end

---重置战争阴影
function ShadowModel._resetShadow()
	for i, v in pairs(this._shadowDataDic) do
		v:SetDark()
	end
end

---战争阴影数据断言
function ShadowModel._assertShadowNodeData(pos)
	return (this._shadowDataDic[pos] ~= nil)
end

---获取上下左右周围四个方向的阴影数据
---@param pos GridPos
---@return ShadowNodeData[]
function ShadowModel._getAroundShadow(pos)
	local t = {}
	local quadPos = pos:GetQuadPos()
	for _, pos in pairs(quadPos) do
		if this._assertShadowNodeData(pos) then
			table.insert(t , this._shadowDataDic[pos])
		end
	end
	return t
end

---获取视野值消耗
---@param shadowNode ShadowNodeData
function ShadowModel._getViewCost(shadowNode)
	return shadowNode:GetViewCost() * this._viewCostCofficient
end

---增加buff
---@param buff BaseBuff
function ShadowModel.AddBuff(buff)
	if not this._buffers[buff.BuffId] then
		this._buffers[buff.BuffId] = buff
		buff:OnStart()
	else
		this._buffers[buff.BuffId]:Refresh(buff)
	end
	this._updateBuffCofficient()
end

---移除buff
---@param buff BaseBuff
function ShadowModel.RemoveBuff(buff)
	this._buffers[buff.BuffId] = nil
	this._updateBuffCofficient()
end

function ShadowModel._updateBuffCofficient()
	for i, v in pairs(this._buffers) do
		this._viewCostCofficient = v.Run("GlobalView" , this._viewCostCofficient)
	end
end

function ShadowModel.Dispose()
	this._viewerList = {}
	this._shadowExpolreCampList = {}
	this._shadowDataDic = {}
	this._buffers = {}
	this._mapPathData = {}
	this._viewCostCofficient = 1
end

return ShadowModel