---@class SolideResultPanel
local SolideResultPanel = class('SolideResultPanel',uView)
SolideResultPanel.ViewPrefabPath = 'Prefab/Battle/UI/SolideResultPanel.prefab'

---@type GameObject
SolideResultPanel.gameObject = nil

---@type Transform
SolideResultPanel.transform = nil

---@type RectTransform
SolideResultPanel.bgTransform = nil


--==userCode==--

--==userCode==--

function SolideResultPanel:Init()
    
end

function SolideResultPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.bgTransform = luaUiFields[1]

end

function SolideResultPanel:Dispose()

end



function SolideResultPanel:OnButtonClickcontinueButton()
    local MSGBattleEndContinue = require("Game.Event.Message.Battle.MSGBattleEndContinue")
    EventBus:Brocast(MSGBattleEndContinue.New())
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.SolideResultPanel , SolideResultPanel)
--==userCode==--

return SolideResultPanel
