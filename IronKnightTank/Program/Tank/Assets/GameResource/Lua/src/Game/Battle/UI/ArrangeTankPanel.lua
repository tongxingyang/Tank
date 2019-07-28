---@class ArrangeTankPanel
local ArrangeTankPanel = class('ArrangeTankPanel',uView)
ArrangeTankPanel.ViewPrefabPath = 'Prefab/Battle/UI/ArrangeTankPanel.prefab'

---@type GameObject
ArrangeTankPanel.gameObject = nil

---@type Transform
ArrangeTankPanel.transform = nil

---@type RectTransform
ArrangeTankPanel.ContentTransform = nil

---@type Button
ArrangeTankPanel.EndButton = nil


--==userCode==--
local TankPropertyViewClass =require("Game.Battle.UI.TankPropertyView")
local MSGEndArrange = require("Game.Event.Message.Battle.MSGEndArrange")
local MSGQuickArrangeTank = require("Game.Event.Message.Battle.MSGQuickArrangeTank")
ArrangeTankPanel._tankPropertyViewList = nil


local receivedCommands = {
    require("Game.Event.Message.Battle.MSGUpdateTank")
}
--==userCode==--

function ArrangeTankPanel:Init()
    self._tankPropertyViewList = {}
    EventBus:RegisterReceiver(receivedCommands , self , self)
end

function ArrangeTankPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.ContentTransform = luaUiFields[1]
    self.EndButton = luaUiFields[2]

end

function ArrangeTankPanel:Dispose()
    EventBus:UnregisterReceiver(receivedCommands , self)
end



function ArrangeTankPanel:OnClickQucikArrangButton(eventData)
    EventBus:Brocast(MSGQuickArrangeTank.new())
end

function ArrangeTankPanel:OnButtonClickEndButton()
    EventBus:Brocast(MSGEndArrange.new())
    PanelManager.Close(PanelManager.PanelEnum.ArrangeTankPanel )
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.ArrangeTankPanel,ArrangeTankPanel)
------------------------------------------------------------------------------------------------
function ArrangeTankPanel:OnOpen(param)
    local fightTankData = param.FightTankData
    coroutine.createAndRun(self._update, self , fightTankData)
end

---@param fightTankData BaseFightTank[]
function ArrangeTankPanel:_update(fightTankData)

    local isEnd = true
    for i, v in pairs(fightTankData) do
        local  view =  self:_CreateTankPropertyView(v)
        self._tankPropertyViewList[v.Id] = view
        view:Update(v)
        isEnd = isEnd and v.IsArrange or false
    end
    self.EndButton.interactable = isEnd
end

---@param fightTankData BaseFightTank
function ArrangeTankPanel:_CreateTankPropertyView(fightTankData)
    local  view = self._tankPropertyViewList[fightTankData.Id]
    if not view then
        view = ViewManager.GetPoolViewYield(nil  , TankPropertyViewClass)
        GameObjectUtilities.SetTransformParentAndNormalization(view.transform , self.ContentTransform)
    end
    return view
end

--- MSGUpdateTank 事件处理
---@param msg MSGUpdateTank
function ArrangeTankPanel:OnMSGUpdateTank(msg)
    self:_update(msg.TankDataArray)
end

--==userCode==--

return ArrangeTankPanel
