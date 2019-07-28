---@class TankPropertyView
local TankPropertyView = class('TankPropertyView',uView)
TankPropertyView.ViewPrefabPath = 'Prefab/Battle/UI/TankPropertyView.prefab'

---@type GameObject
TankPropertyView.gameObject = nil

---@type Transform
TankPropertyView.transform = nil

---@type Text
TankPropertyView.Text = nil

---@type GameObject
TankPropertyView.IsSetFlag = nil

---@type GameObject
TankPropertyView.highLightObject = nil


--==userCode==--

TankPropertyView.FightTankData = nil


local MSGClickTankPropretyView = require("Game.Event.Message.Battle.MSGClickTankPropretyView")

--==userCode==--

function TankPropertyView:Init()
end

function TankPropertyView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.Text = luaUiFields[1]
    self.IsSetFlag = luaUiFields[2]
    self.highLightObject = luaUiFields[3]

end

function TankPropertyView:Dispose()

end



function TankPropertyView:OnClickbg(eventData)
    EventBus:Brocast(MSGClickTankPropretyView:Build({TankId = self.FightTankData.Id}))
end


--==userCode==--
---@param fightTankData PlayerFightTank
function TankPropertyView:Update(fightTankData)
    self.FightTankData = fightTankData
    local name = LocalizationMgr.GetDes(fightTankData.TankData.Name)
    self.Text.text = name
    self.IsSetFlag:SetActive(fightTankData.IsArrange)
    if fightTankData.IsChose then
        self:HighLight()
    else
        self:CancelHightLight()
    end
end

function TankPropertyView:HighLight()
    self.highLightObject:SetActive(true)
end

function TankPropertyView:CancelHightLight()
    self.highLightObject:SetActive(false)
end
--==userCode==--

return TankPropertyView
