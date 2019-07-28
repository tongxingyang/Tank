---------------------------------------------
--- PlaneViewer
--- Created by thrt520.
--- DateTime: 2018/8/14
---------------------------------------------
----@class PlaneViewer : IViewer
local PlaneViewer = class("PlaneViewer" , require("Game.Battle.Interface.IViewer"))
PlaneViewer.IsIgnoreViewCost = true
PlaneViewer.View = 0
PlaneViewer.Camp = 0
PlaneViewer.IsIgnoreViewCost = true
---@type EViewerType 视野类型
PlaneViewer.ViewerType = EViewerType.Force

function PlaneViewer:ctor(camp , view , pos)
	self.View = view
	self.Camp = camp
	self.Pos = pos
end

function PlaneViewer:GetView()
	return self.View
end

function PlaneViewer:GetViewerName()
	return "飛機"
end


function PlaneViewer:ResetObservers()

end

---@param camp ECamp
function PlaneViewer:AddObserver(camp)

end

---@param ecampList ECamp[]
function PlaneViewer:AddObservers(ecampList)

end

---@param ECamp
function PlaneViewer:IsVisiableForCamp(camp)

end

function PlaneViewer:GetViewerIcon()
	return ""
end

return PlaneViewer