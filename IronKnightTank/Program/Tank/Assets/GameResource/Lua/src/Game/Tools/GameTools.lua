---------------------------------------------
--- GameTools
--- Created by thrt520.
--- DateTime: 2018/8/28
---------------------------------------------
GameTools = {}

---打开提示面板
---@param content string
---@param callback Func<boolean>
GameTools.Alert = function(content , callback)
	PanelManager.Open(PanelManager.PanelEnum.AlertPanel , {Content = content , CallBack = callback})
end


---关闭提示面板
GameTools.CloseAlert = function()
	PanelManager.Close(PanelManager.PanelEnum.AlertPanel)
end

GameTools.GetTankDmgDes = function(tankData)
	local str = ""
	local step = 200
	for i = 1, 8 do
		local des = (i - 1) * step.."-"..i *step
		des = des..":"..tankData:GetDmg(i)
		des = des .."\n"
		str = str..des
	end
	return str
end

GameTools.GetTankArmorDes = function(tankData)
	local turretFrontDes = "炮塔正面:"..tankData:GetEquipmentArmored(EHitRes.HitTurret , EHitTankPos.Front).."\n"
	local turretSideDes = "炮塔侧面:"..tankData:GetEquipmentArmored(EHitRes.HitTurret , EHitTankPos.RightSide).."\n"
	local turretBackDes = "炮塔背面:"..tankData:GetEquipmentArmored(EHitRes.HitTurret , EHitTankPos.Back).."\n"
	local bodyFrontDes = "炮塔正面:"..tankData:GetEquipmentArmored(EHitRes.HitBody , EHitTankPos.Front).."\n"
	local bodySideDes = "炮塔侧面:"..tankData:GetEquipmentArmored(EHitRes.HitBody , EHitTankPos.RightSide).."\n"
	local bodyBackDes = "炮塔背面:"..tankData:GetEquipmentArmored(EHitRes.HitBody , EHitTankPos.Back).."\n"
	return turretFrontDes..turretSideDes..turretBackDes..bodyFrontDes..bodySideDes..bodyBackDes
end

GameTools.PrintTable = function(tab)
	for i, v in pairs(tab) do
		clog("i:"..tostring(i).." v:"..tostring(v))
	end
end