---------------------------------------------
--- IViewer
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class IViewer : IUnit
local IViewer = class("IViewer")
---@type EToward@朝向
IViewer.Toward = nil
---@type GridPos@位置
IViewer.Pos = nil
---@type boolean@是否被看到过
IViewer.HasBeenSeen = nil

---@type boolean@上次是否被看到
IViewer.LastTimeVisiable = nil
---@type boolean@可见性
IViewer.Visiable = false
---@type ECamp[]@观察到他的阵营
IViewer.ObserversCamp = nil
---@type boolean
IViewer.IsIgnoreViewCost = false
---@type EViewerType
IViewer.ViewerType = EViewerType.Normal

---@return number
function IViewer:GetView()

end

function IViewer:ResetObservers()

end

---@param camp ECamp
function IViewer:AddObserver(camp)

end

---@param ecampList ECamp[]
function IViewer:AddObservers(ecampList)

end

---@param ECamp
function IViewer:IsVisiableForCamp(camp)

end

function IViewer:GetViewerName()
	return ""
end

function IViewer:GetViewerIcon()

end