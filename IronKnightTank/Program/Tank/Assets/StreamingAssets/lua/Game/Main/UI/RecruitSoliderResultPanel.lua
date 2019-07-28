---@class RecruitSoliderResultPanel
local RecruitSoliderResultPanel = class('RecruitSoliderResultPanel',uView)
RecruitSoliderResultPanel.ViewPrefabPath = 'Prefab/Main/UI/RecruitSoliderResultPanel.prefab'

---@type GameObject
RecruitSoliderResultPanel.gameObject = nil

---@type Transform
RecruitSoliderResultPanel.transform = nil

---@type Text
RecruitSoliderResultPanel.judgeContentTextText = nil

---@type Image
RecruitSoliderResultPanel.iconImage = nil

---@type Text
RecruitSoliderResultPanel.nameText = nil

---@type Text
RecruitSoliderResultPanel.smallTankText = nil

---@type Text
RecruitSoliderResultPanel.middleTankText = nil

---@type Text
RecruitSoliderResultPanel.heavyTankText = nil

---@type Text
RecruitSoliderResultPanel.tankFighterText = nil

---@type Text
RecruitSoliderResultPanel.hitText = nil

---@type RectTransform
RecruitSoliderResultPanel.ContentTransform = nil


--==userCode==--
---@type SoliderData
RecruitSoliderResultPanel.CurSoliderData = nil
---@type UIViewContainer
RecruitSoliderResultPanel.SoliderInfoContainer = nil
local UIViewContainer = require("Game.Tools.UIViewContainer")
--==userCode==--

function RecruitSoliderResultPanel:Init()
    self.SoliderInfoContainer = UIViewContainer.new("Game.Main.UI.RecruitSoliderInfoView",self.ContentTransform , nil , function (data)
        self:UpdateCurSolider(data)
    end)
end

function RecruitSoliderResultPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.judgeContentTextText = luaUiFields[1]
    self.iconImage = luaUiFields[2]
    self.nameText = luaUiFields[3]
    self.smallTankText = luaUiFields[4]
    self.middleTankText = luaUiFields[5]
    self.heavyTankText = luaUiFields[6]
    self.tankFighterText = luaUiFields[7]
    self.hitText = luaUiFields[8]
    self.ContentTransform = luaUiFields[9]

end

function RecruitSoliderResultPanel:Dispose()

end



function RecruitSoliderResultPanel:OnButtonClickcloseBtn()
    PanelManager.Close(PanelManager.PanelEnum.RecruitSoliderResultPanel)
end


--==userCode==--
--PanelManager.Register(PanelManager.PanelEnum.RecruitSoliderResultPanel  , RecruitSoliderResultPanel)

---@param param table
function RecruitSoliderResultPanel:OnOpen(param)
    local soliderList = param.SoliderList
    local solider = soliderList[1]
    self.SoliderInfoContainer:Update(soliderList)
    ---@type RecruitSoliderInfoView
    local view = self.SoliderInfoContainer:GetView(solider)
    view:OnChose()

end

function RecruitSoliderResultPanel:OnClose()


end

function RecruitSoliderResultPanel:UpdateCurSolider(soliderData)
    self.CurSoliderData = soliderData
    self.heavyTankText.text = ETankCompatibility.GetDes(self.CurSoliderData.Compatibility_Heavy)
    self.middleTankText.text = ETankCompatibility.GetDes(self.CurSoliderData.Compatibility_Middle)
    self.smallTankText.text = ETankCompatibility.GetDes(self.CurSoliderData.Compatibility_Small)
    self.tankFighterText.text = ETankCompatibility.GetDes(self.CurSoliderData.Compatibility_Fighter)
    self.hitText.text = tostring(self.CurSoliderData.Hit)
    self.nameText.text = self.CurSoliderData.Name
    self.judgeContentTextText.text = self.CurSoliderData.Describe
    --ResMgr.SetImageSprite()
    --TODO solider ICON
end

--==userCode==--

return RecruitSoliderResultPanel
