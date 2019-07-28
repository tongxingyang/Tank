---@class RecruitTankPanel
local RecruitTankPanel = class('RecruitTankPanel',uView)
RecruitTankPanel.ViewPrefabPath = 'Prefab/Main/UI/RecruitTankPanel.prefab'

---@type GameObject
RecruitTankPanel.gameObject = nil

---@type Transform
RecruitTankPanel.transform = nil

---@type RectTransform
RecruitTankPanel.ViewParent = nil


--==userCode==--
local UIViewContainer = require("Game.Tools.UIViewContainer")
RecruitTankPanel.TankInfoViewContainer = nil
--==userCode==--

function RecruitTankPanel:Init()
    self.TankInfoViewContainer = UIViewContainer.New("Game.Main.UI.RecruitTankLogoView" , self.ViewParent )

end

function RecruitTankPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.ViewParent = luaUiFields[1]

end

function RecruitTankPanel:Dispose()

end



function RecruitTankPanel:OnButtonClickcloseBtn()
    PanelManager.Close(PanelManager.PanelEnum.RecruitTankPanel)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.RecruitTankPanel, RecruitTankPanel)
function RecruitTankPanel:OnOpen(param)
    self:Update(param.TankDataList)
end

function RecruitTankPanel:OnClose()

end

function RecruitTankPanel:Update(tankDataList)
    self.TankInfoViewContainer:Update(tankDataList)
end
--==userCode==--

return RecruitTankPanel
