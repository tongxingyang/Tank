---@class ConfirmAtkPanel
local ConfirmAtkPanel = class('ConfirmAtkPanel',uView)
ConfirmAtkPanel.ViewPrefabPath = 'Prefab/Battle/UI/ConfirmAtkPanel.prefab'

---@type GameObject
ConfirmAtkPanel.gameObject = nil

---@type Transform
ConfirmAtkPanel.transform = nil

---@type Text
ConfirmAtkPanel.hitRateText = nil

---@type Text
ConfirmAtkPanel.destoryRateText = nil

---@type GameObject
ConfirmAtkPanel.playerAttributeObject = nil

---@type GameObject
ConfirmAtkPanel.npcAttributeObject = nil


--==userCode==--
local TankAttributeView = require("Game.Battle.UI.TankAttributeView")
local MSGConfirmAtk =require("Game.Event.Message.Battle.MSGConfirmAtk")
local MSGClickCancelButton = require("Game.Event.Message.Battle.MSGClickCancelButton")

----@type TankAttributeView
ConfirmAtkPanel.playerTankAttributeView = nil

----@type TankAttributeView
ConfirmAtkPanel.enemyTankAttributeView = nil
--==userCode==--

function ConfirmAtkPanel:Init()
    self.playerTankAttributeView = TankAttributeView.new()
    self.playerTankAttributeView:Binding(self.playerAttributeObject)
    self.enemyTankAttributeView = TankAttributeView.new()
    self.enemyTankAttributeView:Binding(self.npcAttributeObject)
end

function ConfirmAtkPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.hitRateText = luaUiFields[1]
    self.destoryRateText = luaUiFields[2]
    self.playerAttributeObject = luaUiFields[3]
    self.npcAttributeObject = luaUiFields[4]

end

function ConfirmAtkPanel:Dispose()

end



function ConfirmAtkPanel:OnClickcancelBtn(eventData)
    EventBus:Brocast(MSGClickCancelButton.new())
end

function ConfirmAtkPanel:OnClickconfirmBtn(eventData)
    EventBus:Brocast(MSGConfirmAtk.new())
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.ConfirmAtkPanel,ConfirmAtkPanel)
function ConfirmAtkPanel:OnOpen(param)
    local atkTank = param.AtkTank
    local defTank = param.DefTank
    local hitRate = param.HitRate
    local destoryRate = param.DestoryRate
    self.destoryRateText.text = string.format("%.1f" , destoryRate * 100).."%"
    self.hitRateText.text = string.format("%.1f" , hitRate * 100).."%"
    self.playerTankAttributeView:Update(atkTank)
    self.enemyTankAttributeView:Update(defTank)
end

function ConfirmAtkPanel:OnClose()

end

--==userCode==--

return ConfirmAtkPanel
