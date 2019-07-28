---------------------------------------------
--- ShadowNodeData
--- 战争阴影节点数据
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class ShadowNodeData
local ShadowNodeData = class("ShadowNodeData")

---@type GridPos
ShadowNodeData.Pos = nil

---@type boolean
ShadowNodeData._isDark = true

---@type boolean 弃用
ShadowNodeData.CullingFov = true

---@type number
ShadowNodeData.ViewCost = 1

ShadowNodeData.RawViewCost = 1

---@type number
ShadowNodeData.LeftPower = 1

---@type BaseBuff[]
ShadowNodeData.buffs = nil

---@type ECamp[]
ShadowNodeData.ObserverCamps = nil

function ShadowNodeData:ctor(editorBlockData)
	self.Pos = GridPos.New(editorBlockData.pos.x , editorBlockData.pos.y)
	local tTerrian = JsonDataMgr.GetTerrianEleementData( editorBlockData.TerrianId)
	self.CullingFov = (tTerrian.Can_by == 1)
	self._isDark = true
	self.RawViewCost = tTerrian.Vision_Consumption
	self.ViewCost = self.RawViewCost
	self.buffs = {}
	self.ObserverCamps = {}
end

function ShadowNodeData:SetLight()
	self._isDark = false
end

function ShadowNodeData:SetDark()
	self._isDark = true
end

function ShadowNodeData:IsDark()
	return self._isDark
end

----增加buff
---@param buff BaseBuff
function ShadowNodeData:RemoveBuff(buff)
	self.buffs[buff.BuffId] = nil
	self:_updateViewCost()
end

----移除buff
---@param buff BaseBuff
function ShadowNodeData:AddBuff(buff)
	if not self.buffs[buff.BuffId] then
		self.buffs[buff.BuffId] = buff
		buff:OnStart()
	else
		self.buffs[buff.BuffId]:Refresh(buff)
		buff:Refresh(buff)
	end
	self:_updateViewCost()
end


----增加观察者阵营
function ShadowNodeData:AddObserverCamp(eCamp)
	if not table.contains( self.ObserverCamps, eCamp) then
		table.insert(self.ObserverCamps, eCamp)
	end
end

function ShadowNodeData:GetViewCost()
	return self.ViewCost
end

function ShadowNodeData:_updateViewCost()
	local viewCost = self.RawViewCost
	for i, v in pairs(self.buffs) do
		viewCost = v:Run("ViewCost" , viewCost)
	end
	self.ViewCost = viewCost
end

function ShadowNodeData:Reset()
	self:ResetObserverCamp()
	self:SetDark();
end

function ShadowNodeData:ResetObserverCamp()
	self.ObserverCamps = {}
end

function ShadowNodeData:GetObserverCamps()
	local t = {}
	for i, v in pairs(self.ObserverCamps) do
		table.insert(t , v)
	end
	for i, v in pairs(self.buffs) do
		table.insert(t , v.ObserverCamp)
	end
	return t
end

return ShadowNodeData