---@class LoadingPanel
local LoadingPanel = class('LoadingPanel',uView)
LoadingPanel.ViewPrefabPath = 'Prefab/Common/UI/LoadingPanel.prefab'

---@type GameObject
LoadingPanel.gameObject = nil

---@type Transform
LoadingPanel.transform = nil

---@type Image
LoadingPanel.bgImage = nil

---@type Slider
LoadingPanel.SliderSlider = nil

--==userCode==--

--==userCode==--

function LoadingPanel:Init()
    
end

function LoadingPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.bgImage = luaUiFields[1]
    self.SliderSlider = luaUiFields[2]

end

function LoadingPanel:Dispose()

end

--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.LoadingPanel , LoadingPanel)
function LoadingPanel:SetSlider(val)
    self.SliderSlider.value = val
end

--==userCode==--

return LoadingPanel
