---@class RecruitTankPropertyView
local RecruitTankPropertyView = class('RecruitTankPropertyView',uView)
RecruitTankPropertyView.ViewPrefabPath = 'Prefab/Main/UI/RecruitTankInfoPanel.prefab'

---@type GameObject
RecruitTankPropertyView.gameObject = nil

---@type Transform
RecruitTankPropertyView.transform = nil

---@type Image
RecruitTankPropertyView.iconImage = nil

---@type Text
RecruitTankPropertyView.nameText = nil

---@type Text
RecruitTankPropertyView.turretTypeText = nil

---@type Text
RecruitTankPropertyView.tankTypeText = nil

---@type Text
RecruitTankPropertyView.dmgText = nil

---@type Text
RecruitTankPropertyView.armorText = nil

---@type Text
RecruitTankPropertyView.dmgDetailText = nil

---@type Text
RecruitTankPropertyView.armorDetailText = nil

---@type GameObject
RecruitTankPropertyView.TankRandarMapViewObject = nil

---@type Text
RecruitTankPropertyView.tankCountText = nil


--==userCode==--
---@type TankRandarMapView
RecruitTankPropertyView.TankRandarMapView = nil
RecruitTankPropertyView.showCurCountOrMaxCount = false
local TankRandarMapView = require("Game.Main.UI.TankRandarMapView")
--==userCode==--

function RecruitTankPropertyView:Init()
    self.TankRandarMapView =TankRandarMapView.New()
    self.TankRandarMapView:Binding(self.TankRandarMapViewObject)
end

function RecruitTankPropertyView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.iconImage = luaUiFields[1]
    self.nameText = luaUiFields[2]
    self.turretTypeText = luaUiFields[3]
    self.tankTypeText = luaUiFields[4]
    self.dmgText = luaUiFields[5]
    self.armorText = luaUiFields[6]
    self.dmgDetailText = luaUiFields[7]
    self.armorDetailText = luaUiFields[8]
    self.TankRandarMapViewObject = luaUiFields[9]
    self.tankCountText = luaUiFields[10]

end

function RecruitTankPropertyView:Dispose()

end




--==userCode==--
---@param tankData TankData
function RecruitTankPropertyView:Update(tankData)
    ResMgr.SetImageSprite(tankData:GetTankIconPath() , self.iconImage)
    self.nameText.text = LocalizationMgr.GetDes(tankData.Name)
    self.turretTypeText.text = tankData.TurretType
    self.tankTypeText.text = tankData.TankType
    self.dmgText.text = "总伤害："..tankData.Atk
    self.armorText.text = "总伤害："..tankData.Armored
    self.armorDetailText.text = GameTools.GetTankArmorDes(tankData)
    self.dmgDetailText.text = GameTools.GetTankDmgDes(tankData)
    self.TankRandarMapView:Update()
end

----@param showCurCountOrMaxCount boolean
function RecruitTankPropertyView:SetType(showCurCountOrMaxCount)
    self.showCurCountOrMaxCount = showCurCountOrMaxCount
end
--==userCode==--

return RecruitTankPropertyView
