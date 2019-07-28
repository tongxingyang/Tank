---@class AlertPanel
local AlertPanel = class('AlertPanel',uView)
AlertPanel.ViewPrefabPath = 'Prefab/Common/UI/AlertPanel.prefab'

---@type GameObject
AlertPanel.gameObject = nil

---@type Transform
AlertPanel.transform = nil

---@type Text
AlertPanel.contentText = nil


--==userCode==--
AlertPanel.callBack = nil
--==userCode==--

function AlertPanel:Init()
    
end

function AlertPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.contentText = luaUiFields[1]

end

function AlertPanel:Dispose()

end



function AlertPanel:OnButtonClickyesBtn()
    if self.callBack then
        self.callBack(true)
    end
    PanelManager.Close(PanelManager.PanelEnum.AlertPanel)
end

function AlertPanel:OnButtonClicknoBtn()
    if self.callBack then
        self.callBack(false)
    end
    PanelManager.Close(PanelManager.PanelEnum.AlertPanel)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.AlertPanel , AlertPanel)
function AlertPanel:OnOpen(param)
    local content = param.Content

    self.contentText.text = content
    self.callBack = param.CallBack


end
--==userCode==--

return AlertPanel
