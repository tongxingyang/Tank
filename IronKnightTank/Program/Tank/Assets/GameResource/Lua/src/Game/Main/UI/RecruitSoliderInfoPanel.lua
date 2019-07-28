---@class RecruitSoliderInfoPanel
local RecruitSoliderInfoPanel = class('RecruitSoliderInfoPanel',uView)
RecruitSoliderInfoPanel.ViewPrefabPath = 'Prefab/Main/UI/RecruitSoliderInfoPanel.prefab'

---@type GameObject
RecruitSoliderInfoPanel.gameObject = nil

---@type Transform
RecruitSoliderInfoPanel.transform = nil

---@type Text
RecruitSoliderInfoPanel.judgeContentTextText = nil

---@type Image
RecruitSoliderInfoPanel.iconImage = nil

---@type Text
RecruitSoliderInfoPanel.nameText = nil

---@type Text
RecruitSoliderInfoPanel.smallTankText = nil

---@type Text
RecruitSoliderInfoPanel.middleTankText = nil

---@type Text
RecruitSoliderInfoPanel.heavyTankText = nil

---@type Text
RecruitSoliderInfoPanel.tankFighterText = nil

---@type Text
RecruitSoliderInfoPanel.hitText = nil

---@type RectTransform
RecruitSoliderInfoPanel.ContentTransform = nil

---@type Text
RecruitSoliderInfoPanel.titleText = nil


--==userCode==--
---@type SoliderData
RecruitSoliderInfoPanel.CurSoliderData = nil
---@type UIViewContainer
RecruitSoliderInfoPanel.SoliderInfoContainer = nil
local UIViewContainer = require("Game.Tools.UIViewContainer")
--==userCode==--

function RecruitSoliderInfoPanel:Init()
    self.SoliderInfoContainer = UIViewContainer.new("Game.Main.UI.RecruitSoliderInfoView",self.ContentTransform , nil , function (data)
        self:UpdateCurSolider(data)
    end)
end

function RecruitSoliderInfoPanel:Binding(object)
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
    self.titleText = luaUiFields[10]

end

function RecruitSoliderInfoPanel:Dispose()

end



function RecruitSoliderInfoPanel:OnButtonClickcloseBtn()
    PanelManager.Close(PanelManager.PanelEnum.RecruitSoliderInfoPanel)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.RecruitSoliderInfoPanel  , RecruitSoliderInfoPanel)

---@param param table
function RecruitSoliderInfoPanel:OnOpen(param)
    local soliderList = param.SoliderList
    local solider = soliderList[1]
    self.SoliderInfoContainer:Update(soliderList)
    ---@type RecruitSoliderInfoView
    local view = self.SoliderInfoContainer:GetView(solider)
    if view then
        view:OnChose()
    end

     self.titleText.text = param.IsAll and "当前车长" or"招募车长"
end

function RecruitSoliderInfoPanel:OnClose()

end

function RecruitSoliderInfoPanel:UpdateCurSolider(soliderData)
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

return RecruitSoliderInfoPanel
