---@class CombatDisplayPanel
local CombatDisplayPanel = class('CombatDisplayPanel',uView)
CombatDisplayPanel.ViewPrefabPath = 'Prefab/Battle/UI/CombatDisplayPanel.prefab'

---@type GameObject
CombatDisplayPanel.gameObject = nil

---@type Transform
CombatDisplayPanel.transform = nil

---@type Slider
CombatDisplayPanel.playerSliderSlider = nil

---@type Slider
CombatDisplayPanel.npcSliderSlider = nil

---@type Text
CombatDisplayPanel.desLabel = nil

---@type Image
CombatDisplayPanel.playerSolideIconImage = nil

---@type Text
CombatDisplayPanel.playerHitNameText = nil

---@type Text
CombatDisplayPanel.playerHitContentText = nil

---@type Text
CombatDisplayPanel.playerAtkNameText = nil

---@type Text
CombatDisplayPanel.playerAtkContentText = nil

---@type Text
CombatDisplayPanel.playerDefNameText = nil

---@type Text
CombatDisplayPanel.playerDefContentText = nil

---@type Text
CombatDisplayPanel.playerProjectNameText = nil

---@type Text
CombatDisplayPanel.playerProjectContentText = nil

---@type Image
CombatDisplayPanel.npcSolideIconImage = nil

---@type Text
CombatDisplayPanel.npcHitNameText = nil

---@type Text
CombatDisplayPanel.npcHitContentText = nil

---@type Text
CombatDisplayPanel.npcAtkNameText = nil

---@type Text
CombatDisplayPanel.npcAtkContentText = nil

---@type Text
CombatDisplayPanel.npcDefNameText = nil

---@type Text
CombatDisplayPanel.npcDefContentText = nil

---@type Text
CombatDisplayPanel.npcProjectNameText = nil

---@type Text
CombatDisplayPanel.npcProjectContentText = nil

---@type GameObject
CombatDisplayPanel.playerAtkSignObject = nil

---@type GameObject
CombatDisplayPanel.playerDefSignObject = nil

---@type GameObject
CombatDisplayPanel.npcAtkSignObject = nil

---@type GameObject
CombatDisplayPanel.npcDfSignObject = nil

---@type RectTransform
CombatDisplayPanel.aimAnchorTransform = nil

---@type Text
CombatDisplayPanel.playersoliderNameText = nil

---@type Text
CombatDisplayPanel.enemysoliderNameText = nil


--==userCode==--
CombatDisplayPanel.IsPlayerAtk =  false
CombatDisplayPanel.Speed =  0
CombatDisplayPanel.Time =  0

local receiveCommands = {
    require("Game.Event.Message.Battle.MSGDisplayFight"),
}


--CombatDisplayPanel.LoopSliderWidth = 0

--==userCode==--

function CombatDisplayPanel:Init()
    EventBus:RegisterReceiver(receiveCommands  , self , self)
end

function CombatDisplayPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.playerSliderSlider = luaUiFields[1]
    self.npcSliderSlider = luaUiFields[2]
    self.desLabel = luaUiFields[3]
    self.playerSolideIconImage = luaUiFields[4]
    self.playerHitNameText = luaUiFields[5]
    self.playerHitContentText = luaUiFields[6]
    self.playerAtkNameText = luaUiFields[7]
    self.playerAtkContentText = luaUiFields[8]
    self.playerDefNameText = luaUiFields[9]
    self.playerDefContentText = luaUiFields[10]
    self.playerProjectNameText = luaUiFields[11]
    self.playerProjectContentText = luaUiFields[12]
    self.npcSolideIconImage = luaUiFields[13]
    self.npcHitNameText = luaUiFields[14]
    self.npcHitContentText = luaUiFields[15]
    self.npcAtkNameText = luaUiFields[16]
    self.npcAtkContentText = luaUiFields[17]
    self.npcDefNameText = luaUiFields[18]
    self.npcDefContentText = luaUiFields[19]
    self.npcProjectNameText = luaUiFields[20]
    self.npcProjectContentText = luaUiFields[21]
    self.playerAtkSignObject = luaUiFields[22]
    self.playerDefSignObject = luaUiFields[23]
    self.npcAtkSignObject = luaUiFields[24]
    self.npcDfSignObject = luaUiFields[25]
    self.aimAnchorTransform = luaUiFields[26]
    self.playersoliderNameText = luaUiFields[27]
    self.enemysoliderNameText = luaUiFields[28]

end

function CombatDisplayPanel:Dispose()
    EventBus:UnregisterReceiver(receiveCommands  , self , self)
end




--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.CombatDisplayPanel,CombatDisplayPanel)
------------------------------------------------------------------------------------------------
---event handler
------------------------------------------------------------------------------------------------
---@param msg MSGDisplayFight
function CombatDisplayPanel:OnMSGDisplayFight(msg)
    self.desLabel.text = msg.Des
    if  msg.AtkSliderValue then
        msg:Pend()
        local atkSlider = self.IsPlayerAtk and self.playerSliderSlider or self.npcSliderSlider
        LuaTools.SliderTween(atkSlider , msg.AtkSliderValue , msg.Time , function ()
            msg:Restore()
        end)
    end
    if msg.DefSliderValue  then
        msg:Pend()
        local defSlider = not self.IsPlayerAtk and self.playerSliderSlider or self.npcSliderSlider
        LuaTools.SliderTween(defSlider , msg.DefSliderValue , msg.Time , function ()
            msg:Restore()
        end)
    end
    if not msg.AtkSliderValue and not msg.DefSliderValue then
        msg:Pend()
        coroutine.wait(msg.Time)
        msg:Restore()
    end
end
------------------------------------------------------------------------------------------------
function CombatDisplayPanel:OnOpen(param)
    ---@type BaseFightTank
    local atkTank = param.atkTank
    ---@type BaseFightTank
    local defTank = param.defTank
    self.IsPlayerAtk = atkTank.IsPlayer
    local playerTank = self.IsPlayerAtk and atkTank or defTank
    local npcTank = not self.IsPlayerAtk and atkTank or defTank
    self:SetNpcAttribute(npcTank)
    self:SetPlayerAttribute(playerTank)
    self:ResetSlider()
end


function CombatDisplayPanel:OnClose(param)

end
------------------------------------------------------------------------------------------------
---@param playerTank BaseFightTank
function CombatDisplayPanel:SetPlayerAttribute(playerTank)
    local solider = playerTank.Solider
    local tank = playerTank.TankData
    self.playerAtkContentText.text = tank.Atk
    self.playerDefContentText.text = tank.Armored
    self.playerHitContentText.text = solider.Hit
    self.playerProjectContentText.text = tank.Projection

    ResMgr.SetImageSprite(solider:GetSoliderIconPath() , self.playerSolideIconImage )
    self.playerAtkSignObject:SetActive(self.IsPlayerAtk)
    self.playerDefSignObject:SetActive(not self.IsPlayerAtk)
    self.playersoliderNameText.text =  LocalizationMgr.GetDes(solider.Name)
end

---@param npcTank BaseFightTank
function CombatDisplayPanel:SetNpcAttribute(npcTank)
    local solider = npcTank.Solider
    local tank = npcTank.TankData
    self.npcAtkContentText.text = tank.Atk
    self.npcDefContentText.text = tank.Armored
    self.npcHitContentText.text = solider.Hit
    self.npcProjectContentText.text = tank.Projection
    ResMgr.SetImageSprite(solider:GetSoliderIconPath() , self.npcSolideIconImage )
    self.npcAtkSignObject:SetActive(not self.IsPlayerAtk)
    self.npcDfSignObject:SetActive(self.IsPlayerAtk)
    self.enemysoliderNameText = solider.Name
end



function CombatDisplayPanel:ResetSlider()
    self.npcSliderSlider.value = 0
    self.playerSliderSlider.value = 0
end


--==userCode==--

return CombatDisplayPanel
