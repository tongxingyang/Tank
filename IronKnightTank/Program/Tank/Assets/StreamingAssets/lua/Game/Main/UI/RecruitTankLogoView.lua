---@class RecruitTankLogoView
local RecruitTankLogoView = class('RecruitTankLogoView',uView)
RecruitTankLogoView.ViewPrefabPath = 'Prefab/Main/UI/RecruitTankLogoView.prefab'

---@type GameObject
RecruitTankLogoView.gameObject = nil

---@type Transform
RecruitTankLogoView.transform = nil

---@type Image
RecruitTankLogoView.iconImage = nil

---@type Text
RecruitTankLogoView.nameText = nil

---@type Text
RecruitTankLogoView.countText = nil


--==userCode==--
local MSGOpenRecruitEnsureWinRequest = require("Game.Event.Message.Main.MSGOpenRecruitEnsureWinRequest")

---@type table
RecruitTankLogoView.tankData = nil
---@type number
RecruitTankLogoView.count = 0
---@type function
RecruitTankLogoView.event = 0
--==userCode==--

function RecruitTankLogoView:Init()
    
end

function RecruitTankLogoView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.iconImage = luaUiFields[1]
    self.nameText = luaUiFields[2]
    self.countText = luaUiFields[3]

end

function RecruitTankLogoView:Dispose()

end



function RecruitTankLogoView:OnButtonClickButton()
    EventBus:Brocast(MSGOpenRecruitEnsureWinRequest:Build({TankData = self.tankData}))
end


--==userCode==--
function RecruitTankLogoView:Update(data)
    clog("tank max Countt "..tostring(data.TankMaxCount))
    self.tankData = data
    local count = data.TankMaxCount
    local tankData = data.TankData
    self.nameText.text = LocalizationMgr.GetDes(tankData.Name)
    self.countText.text = count
    ResMgr.SetImageSprite(tankData:GetTankIconPath() , self.iconImage)
end

---@param event function
function RecruitTankLogoView:SetEvent(event)

end
--==userCode==--

return RecruitTankLogoView
