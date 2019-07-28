---@class LoadLevelPanel
local LoadLevelPanel = class('LoadLevelPanel',uView)
LoadLevelPanel.ViewPrefabPath = 'Prefab/Welcome/UI/LoadLevelPanel.prefab'

---@type GameObject
LoadLevelPanel.gameObject = nil

---@type Transform
LoadLevelPanel.transform = nil

---@type InputField
LoadLevelPanel.InputFieldField = nil


--==userCode==--

--==userCode==--

function LoadLevelPanel:Init()
    
end

function LoadLevelPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.InputFieldField = luaUiFields[1]

end

function LoadLevelPanel:Dispose()

end



function LoadLevelPanel:OnButtonClickButton()
    local MSGLoadLevelRequest =require("Game.Event.Message.Welcome.MSGLoadLevelRequest")
    EventBus:Brocast(MSGLoadLevelRequest:Build({LevelId = tonumber(self.InputFieldField.text)}))
end

function LoadLevelPanel:OnButtonClickButton2()
    local MSGOpenRecruit = require("Game.Event.Message.Welcome.MSGOpenRecruit")
    EventBus:Brocast(MSGOpenRecruit.new())
    --PanelManager.Open(PanelManager.PanelEnum.RecruitPanel)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.LoadLevelPanel,LoadLevelPanel)
--==userCode==--

return LoadLevelPanel
