---@class BattleEndConditionPanel
local BattleEndConditionPanel = class('BattleEndConditionPanel',uView)
BattleEndConditionPanel.ViewPrefabPath = 'Prefab/Battle/UI/BattleEndConditionPanel.prefab'

---@type GameObject
BattleEndConditionPanel.gameObject = nil

---@type Transform
BattleEndConditionPanel.transform = nil

---@type Text
BattleEndConditionPanel.winText = nil

---@type Text
BattleEndConditionPanel.loseText = nil


--==userCode==--

--==userCode==--

function BattleEndConditionPanel:Init()
    
end

function BattleEndConditionPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.winText = luaUiFields[1]
    self.loseText = luaUiFields[2]

end

function BattleEndConditionPanel:Dispose()

end



function BattleEndConditionPanel:OnButtonClickcloseButton()
    self.gameObject:SetActive(false)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.BattleEndConditionPanel , BattleEndConditionPanel)


function BattleEndConditionPanel:OnOpen(param)
    self:Update(param.WinDes , param.FailCon)
end

function BattleEndConditionPanel:Update(winCon , loseCon)
    self.winText.text = LocalizationMgr.GetDes(winCon)
    self.loseText.text = LocalizationMgr.GetDes(loseCon)
end

--==userCode==--

return BattleEndConditionPanel
