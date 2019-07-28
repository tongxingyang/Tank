---@class BattleResultPanel
local BattleResultPanel = class('BattleResultPanel',uView)
BattleResultPanel.ViewPrefabPath = 'Prefab/Battle/UI/BattleResultPanel.prefab'

---@type GameObject
BattleResultPanel.gameObject = nil

---@type Transform
BattleResultPanel.transform = nil

---@type Text
BattleResultPanel.goldText = nil

---@type Text
BattleResultPanel.expText = nil

---@type Text
BattleResultPanel.goldValText = nil

---@type Text
BattleResultPanel.expValText = nil

---@type Text
BattleResultPanel.battleGradeText = nil


--==userCode==--

--==userCode==--

function BattleResultPanel:Init()
    
end

function BattleResultPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.goldText = luaUiFields[1]
    self.expText = luaUiFields[2]
    self.goldValText = luaUiFields[3]
    self.expValText = luaUiFields[4]
    self.battleGradeText = luaUiFields[5]

end

function BattleResultPanel:Dispose()

end



function BattleResultPanel:OnButtonClickcontinueButton()
    local MSGClickBattleResultContinueButton = require("Game.Event.Message.Battle.MSGClickBattleResultContinueButton")
    EventBus:Brocast(MSGClickBattleResultContinueButton.New())
    PanelManager.Close(PanelManager.PanelEnum.BattleResultPanel)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.BattleResultPanel , BattleResultPanel)
--==userCode==--

return BattleResultPanel
