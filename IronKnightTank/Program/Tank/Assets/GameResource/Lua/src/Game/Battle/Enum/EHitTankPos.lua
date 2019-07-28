---------------------------------------------
--- EHitTankPos
--- Created by thrt520.
--- DateTime: 2018/8/2
---------------------------------------------
---@class EHitTankPos
EHitTankPos ={}
EHitTankPos.Front = 3
EHitTankPos.Back = 1

EHitTankPos.RightSide = 2
EHitTankPos.LeftSide = 4

EHitTankPos.GetDes = function(eHitPos)
	if eHitPos == EHitTankPos.Front then
		return "Front"
	elseif eHitPos == EHitTankPos.Back then
		return "Back"
	elseif eHitPos == EHitTankPos.RightSide or eHitPos == EHitTankPos.LeftSide then
		return "Side"
	else
		return "未知的位置 "..tostring(eHitPos)
	end
end


return EHitTankPos