---@class TankAttributeView
local TankAttributeView = class('TankAttributeView',uView)
TankAttributeView.ViewPrefabPath = 'Prefab/Battle/UI/ConfirmAtkPanel.prefab'

---@type GameObject
TankAttributeView.gameObject = nil

---@type Transform
TankAttributeView.transform = nil

---@type Image
TankAttributeView.tankIconImage = nil

---@type Text
TankAttributeView.tankNameText = nil

---@type Text
TankAttributeView.soliderNameText = nil

---@type Image
TankAttributeView.soliderIconImage = nil

---@type Text
TankAttributeView.hitContent = nil

---@type Text
TankAttributeView.atkContent = nil

---@type Text
TankAttributeView.projectContent = nil

---@type Text
TankAttributeView.defContent = nil


--==userCode==--

--==userCode==--

function TankAttributeView:Init()
    
end

function TankAttributeView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.tankIconImage = luaUiFields[1]
    self.tankNameText = luaUiFields[2]
    self.soliderNameText = luaUiFields[3]
    self.soliderIconImage = luaUiFields[4]
    self.hitContent = luaUiFields[5]
    self.atkContent = luaUiFields[6]
    self.projectContent = luaUiFields[7]
    self.defContent = luaUiFields[8]

end

function TankAttributeView:Dispose()

end




--==userCode==--
---@param fightTankData BaseFightTank
function TankAttributeView:Update(fightTankData)
    local soliderData = fightTankData.Solider
    local tankData = fightTankData.TankData
    ResMgr.SetImageSprite(fightTankData.Solider:GetSoliderIconPath() , self.soliderIconImage)
    self.hitContent.text = tostring(soliderData.Hit)
    self.atkContent.text = tostring(tankData.Atk)
    self.defContent.text = tostring(tankData.Armored)
    self.projectContent.text = tostring(tankData.Projection)
    ResMgr.SetImageSprite(fightTankData.TankData:GetTankIconPath() , self.tankIconImage)
    self.soliderNameText.text = LocalizationMgr.GetDes(soliderData.Name)
    self.tankNameText.text = LocalizationMgr.GetDes(tankData.Name)
end

--==userCode==--

return TankAttributeView
