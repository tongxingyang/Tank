---@class RecruitTankInfoPanel
local RecruitTankInfoPanel = class('RecruitTankInfoPanel',uView)
RecruitTankInfoPanel.ViewPrefabPath = 'Prefab/Main/UI/RecruitTankInfoPanel.prefab'

---@type GameObject
RecruitTankInfoPanel.gameObject = nil

---@type Transform
RecruitTankInfoPanel.transform = nil

---@type RectTransform
RecruitTankInfoPanel.ContentTransform = nil

---@type GameObject
RecruitTankInfoPanel.RecruitTankPropertyViewObject = nil


--==userCode==--
local UIViewContainer = require("Game.Tools.UIViewContainer")
local RecruitTankPropertyView = require("Game.Main.UI.RecruitTankPropertyView")
---@type UIViewContainer
RecruitTankInfoPanel.TankInfoViewContainer = nil
RecruitTankInfoPanel.CurTankData = nil
---@type  RecruitTankPropertyView
RecruitTankInfoPanel.TankPropertyView = nil

--==userCode==--

function RecruitTankInfoPanel:Init()
    self.TankPropertyView = RecruitTankPropertyView.New()
    self.TankPropertyView:Binding(self.RecruitTankPropertyViewObject)
    self.TankPropertyView:Init()
    self.TankInfoViewContainer = UIViewContainer.New("Game.Main.UI.RecruitTankInfoView" , self.ContentTransform , nil , function(data)
        self:UpdateCurTank(data)
    end)
end

function RecruitTankInfoPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.ContentTransform = luaUiFields[1]
    self.RecruitTankPropertyViewObject = luaUiFields[2]

end

function RecruitTankInfoPanel:Dispose()

end



function RecruitTankInfoPanel:OnButtonClickcloseBtn()
    PanelManager.Close(PanelManager.PanelEnum.RecruitTankInfoPanel)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.RecruitTankInfoPanel , RecruitTankInfoPanel)

function RecruitTankInfoPanel:OnOpen(param)
    local tankDataList = param.TankList
    local tank = table.getFirstValue(tankDataList)
    self.TankInfoViewContainer:Update(tankDataList)
    local view = self.TankInfoViewContainer:GetView(tank)
    view:OnChose()
end

function RecruitTankInfoPanel:OnClose()

end

function RecruitTankInfoPanel:UpdateCurTank(data)
   self.CurTankData = data
    local tankUse = data.Count
    local maxCount = data.TankMaxCount
    ---@type TankData
    local tankData = data.TankInfo
    self.TankPropertyView:Update(tankData)
   -- self.nameText.text = LocalizationMgr.GetDes(tankData.Name)
   -- self.turretTypeText.text = tankData.TurretType
   -- self.tankTypeText.text = tankData.TankType
   -- self.dmgText.text = "总伤害："..tankData.Atk
   -- self.armorText.text = "总伤害："..tankData.Armored
   -- self.armorDetailText.text = GameTools.GetTankArmorDes(tankData)
   -- self.dmgDetailText.text = GameTools.GetTankDmgDes(tankData)
   -- --TODO solider ICON
end
--==userCode==--

return RecruitTankInfoPanel
