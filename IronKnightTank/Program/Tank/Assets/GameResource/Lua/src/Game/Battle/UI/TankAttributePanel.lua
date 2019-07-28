---@class TankAttributePanel
local TankAttributePanel = class('TankAttributePanel',uView)
TankAttributePanel.ViewPrefabPath = 'Prefab/Battle/UI/TankAttributePanel.prefab'

---@type GameObject
TankAttributePanel.gameObject = nil

---@type Transform
TankAttributePanel.transform = nil

---@type Image
TankAttributePanel.soliderIconImage = nil

---@type Image
TankAttributePanel.tankIconImage = nil

---@type Text
TankAttributePanel.skillInfoText = nil

---@type Text
TankAttributePanel.soliderHitnameText = nil

---@type Text
TankAttributePanel.soliderHitcontentText = nil

---@type Text
TankAttributePanel.tankAtknameText = nil

---@type Text
TankAttributePanel.tankAtkcontentText = nil

---@type Text
TankAttributePanel.tankDefnameText = nil

---@type Text
TankAttributePanel.tankDefcontentText = nil

---@type Text
TankAttributePanel.tankProjectnameText = nil

---@type Text
TankAttributePanel.tankProjectcontentText = nil

---@type Text
TankAttributePanel.tankNameText = nil

---@type Text
TankAttributePanel.soliderNameText = nil

---@type RectTransform
TankAttributePanel.bgTransform = nil

---@type GameObject
TankAttributePanel.power1Object = nil

---@type GameObject
TankAttributePanel.power2Object = nil


--==userCode==--

--==userCode==--

function TankAttributePanel:Init()
    
end

function TankAttributePanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.soliderIconImage = luaUiFields[1]
    self.tankIconImage = luaUiFields[2]
    self.skillInfoText = luaUiFields[3]
    self.soliderHitnameText = luaUiFields[4]
    self.soliderHitcontentText = luaUiFields[5]
    self.tankAtknameText = luaUiFields[6]
    self.tankAtkcontentText = luaUiFields[7]
    self.tankDefnameText = luaUiFields[8]
    self.tankDefcontentText = luaUiFields[9]
    self.tankProjectnameText = luaUiFields[10]
    self.tankProjectcontentText = luaUiFields[11]
    self.tankNameText = luaUiFields[12]
    self.soliderNameText = luaUiFields[13]
    self.bgTransform = luaUiFields[14]
    self.power1Object = luaUiFields[15]
    self.power2Object = luaUiFields[16]

end

function TankAttributePanel:Dispose()

end




--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.TankAttributePanel,TankAttributePanel)
------------------------------------------------------------------------------------------
function TankAttributePanel:OnOpen(param)
    local fightTankData = param.fightTankData
    self:_update(fightTankData)
    if param.IsLeft then
        self.bgTransform.localPosition = Vector3.New(-48 , 192)
    else
        self.bgTransform.localPosition = Vector3.New(796 , 192)
    end
end

---@param fightTankData BaseFightTank
function TankAttributePanel:_update(fightTankData)
    local soliderData = fightTankData.Solider
    local tankData = fightTankData.TankData
    ResMgr.SetImageSprite( soliderData:GetSoliderIconPath() , self.soliderIconImage)
    self.soliderHitcontentText.text = tostring(soliderData.Hit)
    self.tankAtkcontentText.text = tostring(tankData.Atk)
    self.tankDefcontentText.text = tostring(tankData.Armored)
    self.tankProjectcontentText.text = tostring(tankData.Projection)
    ResMgr.SetImageSprite(tankData:GetTankIconPath() , self.tankIconImage)
    self.soliderNameText.text = LocalizationMgr.GetDes(soliderData.Name) --soliderData.Name
    self.tankNameText.text = LocalizationMgr.GetDes(tankData.Name)
    if fightTankData.IsPlayer then
        self.power1Object:SetActive(fightTankData:GetPower() >= 1 )
        self.power2Object:SetActive(fightTankData:GetPower() >= 2 )
    else
        self.power1Object:SetActive(false )
        self.power2Object:SetActive(false )
    end
end

--==userCode==--

return TankAttributePanel
