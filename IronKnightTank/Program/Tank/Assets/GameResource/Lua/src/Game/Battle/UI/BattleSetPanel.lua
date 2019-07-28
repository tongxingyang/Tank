---@class BattleSetPanel
local BattleSetPanel = class('BattleSetPanel',uView)
BattleSetPanel.ViewPrefabPath = 'Prefab/Battle/UI/BattleSetPanel.prefab'

---@type GameObject
BattleSetPanel.gameObject = nil

---@type Transform
BattleSetPanel.transform = nil

---@type Slider
BattleSetPanel.musicSliderSlider = nil

---@type Slider
BattleSetPanel.audioSliderSlider = nil


--==userCode==--

--==userCode==--

function BattleSetPanel:Init()
    
end

function BattleSetPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.musicSliderSlider = luaUiFields[1]
    self.audioSliderSlider = luaUiFields[2]

end

function BattleSetPanel:Dispose()

end



function BattleSetPanel:OnButtonClickcloseButton()
    PanelManager.Close(PanelManager.PanelEnum.BattleSetPanel)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.BattleSetPanel , BattleSetPanel)
--==userCode==--

return BattleSetPanel
