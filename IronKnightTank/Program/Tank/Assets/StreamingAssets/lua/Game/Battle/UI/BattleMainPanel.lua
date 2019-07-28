---@class BattleMainPanel
local BattleMainPanel = class('BattleMainPanel',uView)
BattleMainPanel.ViewPrefabPath = 'Prefab/Battle/UI/BattleMainPanel.prefab'

---@type GameObject
BattleMainPanel.gameObject = nil

---@type Transform
BattleMainPanel.transform = nil

---@type GameObject
BattleMainPanel.tipsObject = nil

---@type Text
BattleMainPanel.roundvalText = nil

---@type Text
BattleMainPanel.roundDesText = nil

---@type GameObject
BattleMainPanel.ButtonGridObject = nil

---@type Button
BattleMainPanel.atkButton = nil

---@type Button
BattleMainPanel.skipButton = nil

---@type Button
BattleMainPanel.hightAtkButton = nil

---@type RectTransform
BattleMainPanel.skillButtonGridTransform = nil

---@type Text
BattleMainPanel.skillDes = nil

---@type GameObject
BattleMainPanel.cancelObject = nil

---@type GameObject
BattleMainPanel.skillDesObject = nil

---@type Text
BattleMainPanel.campActionTipsText = nil

---@type Text
BattleMainPanel.winconditionText = nil

---@type GameObject
BattleMainPanel.playerRoundLogo = nil

---@type GameObject
BattleMainPanel.enemyRoundLogo = nil

---@type Text
BattleMainPanel.roundStartText = nil

---@type Text
BattleMainPanel.roundNumberText = nil

---@type GameObject
BattleMainPanel.winConditionObject = nil

---@type ActionViewPlayer
BattleMainPanel.tipsPlayer = nil

---@type GameObject
BattleMainPanel.winObject = nil

---@type GameObject
BattleMainPanel.failObject = nil


--==userCode==--
local MSGClickSetButton = require("Game.Event.Message.Battle.MSGClickSetButton")
local MSGClickWinConButton = require("Game.Event.Message.Battle.MSGClickWinConButton")
local MSGClickAtkButton = require("Game.Event.Message.Battle.MSGClickAtkButton")
local MSGClickFocusAtkBtn = require("Game.Event.Message.Battle.MSGClickFocusAtkBtn")
local MSGClickSkipButton = require("Game.Event.Message.Battle.MSGClickSkipButton")
local MSGClickEndRoundBtn =require("Game.Event.Message.Battle.MSGClickEndRoundBtn")
local MSGClickCancelButton =require("Game.Event.Message.Battle.MSGClickCancelButton")

local CombGrid =require("Game.Tools.CombaGrid.CombGrid")
local UIViewContainer = require("Game.Tools.UIViewContainer")
BattleMainPanel.closeSkillView = nil
---@type UIViewContainer
BattleMainPanel.skillViewContainer = nil

local receiveCommands = {
    require("Game.Event.Message.Battle.MSGUpdateManualState"),
    require("Game.Event.Message.Battle.MSGUpdateManualButtonEnable"),
    require("Game.Event.Message.Battle.MSGRoundStart"),
    require("Game.Event.Message.Battle.MSGUpdateSkill"),
    require("Game.Event.Message.Battle.MSGShowSkillDes"),
    require("Game.Event.Message.Battle.MSGCloseSkillDes"),
    require("Game.Event.Message.Battle.MSGCampRoundEnd"),
    require("Game.Event.Message.Battle.MSGPlayActiveCampView"),
    require("Game.Event.Message.Battle.MSGShowWinCondition"),
    require("Game.Event.Message.Battle.MSGDisplayBattleEndAim"),
}
--==userCode==--

function BattleMainPanel:Init()
    EventBus:RegisterReceiver(receiveCommands , self , self)
    local CombGrid = CombGrid.new(3 ,77.4 , 66.6 )
    CombGrid:SetStartCorner(2)
    self.skillViewContainer =UIViewContainer.new("Game.Battle.UI.CommanderSkillView" , self.skillButtonGridTransform , CombGrid)
end

function BattleMainPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.tipsObject = luaUiFields[1]
    self.roundvalText = luaUiFields[2]
    self.roundDesText = luaUiFields[3]
    self.ButtonGridObject = luaUiFields[4]
    self.atkButton = luaUiFields[5]
    self.skipButton = luaUiFields[6]
    self.hightAtkButton = luaUiFields[7]
    self.skillButtonGridTransform = luaUiFields[8]
    self.skillDes = luaUiFields[9]
    self.cancelObject = luaUiFields[10]
    self.skillDesObject = luaUiFields[11]
    self.campActionTipsText = luaUiFields[12]
    self.winconditionText = luaUiFields[13]
    self.playerRoundLogo = luaUiFields[14]
    self.enemyRoundLogo = luaUiFields[15]
    self.roundStartText = luaUiFields[16]
    self.roundNumberText = luaUiFields[17]
    self.winConditionObject = luaUiFields[18]
    self.tipsPlayer = luaUiFields[19]
    self.winObject = luaUiFields[20]
    self.failObject = luaUiFields[21]

end

function BattleMainPanel:Dispose()

end



function BattleMainPanel:OnButtonClickhightAtk()
    EventBus:Brocast(MSGClickFocusAtkBtn.new())
end

function BattleMainPanel:OnButtonClickatk()
    EventBus:Brocast(MSGClickAtkButton.new())
end

function BattleMainPanel:OnButtonClickskip()
    EventBus:Brocast(MSGClickSkipButton.new())
end

function BattleMainPanel:OnButtonClickEndRound()
    EventBus:Brocast(MSGClickEndRoundBtn.New())
end

function BattleMainPanel:OnButtonClickcancel()

    EventBus:Brocast(MSGClickCancelButton.New())
end

function BattleMainPanel:OnButtonClickwinConButton()

    EventBus:Brocast(MSGClickWinConButton.new())
end

function BattleMainPanel:OnButtonClicksetButton()

    EventBus:Brocast(MSGClickSetButton.new())
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.BattleMainPanel,BattleMainPanel)
------------------------------------------------------------------------------------------------
---event handler
------------------------------------------------------------------------------------------------
----MSGUpdateManualState
---@param msg MSGUpdateManualState
function BattleMainPanel:OnMSGUpdateManualState(msg)
    if msg.State == EManualState.DefaultState then
        self.ButtonGridObject:SetActive(false)
    elseif msg.State == EManualState.MineTankState then
        self.ButtonGridObject:SetActive(true)
    else
        self.ButtonGridObject:SetActive(false)
    end
    if msg.State == EManualState.EnsureAtkState
            or msg.State == EManualState.MineTankState
            or msg.State == EManualState.EnemyTankState
            or msg.State == EManualState.EnsureSkillState then
        self.cancelObject.gameObject:SetActive(true)
    else
        self.cancelObject.gameObject:SetActive(false)
    end

end

----MSGUpdateManualButtonEnable
---@param msg MSGUpdateManualButtonEnable
function BattleMainPanel:OnMSGUpdateManualButtonEnable(msg)
    self.atkButton.gameObject:SetActive(msg.AtkButtonVisiable)
    self.hightAtkButton.gameObject:SetActive(msg.FocusAtkButtonVisiable)
    self.atkButton.interactable = msg.AtkButtonEnable
    self.hightAtkButton.interactable = msg.FocusAtkButtonEnable
    self.skipButton.interactable = msg.SkipButtonEnable
end


----MSGRoundStart
---@param msg MSGRoundStart
function BattleMainPanel:OnMSGRoundStart(msg)
    self.roundvalText.text = tostring(msg.CurRound)
end

----MSGPlayActiveCampView
---@param msg MSGPlayActiveCampView
function BattleMainPanel:OnMSGPlayActiveCampView(msg)
    coroutine.createAndRun(function ()
        self.tipsObject:SetActive(true)
        if msg.Camp == BattleConfig.PlayerCamp then
            self.playerRoundLogo:SetActive(true)
            self.enemyRoundLogo:SetActive(false)
            self.roundStartText.text = "我方回合"
        else
            self.playerRoundLogo:SetActive(false)
            self.enemyRoundLogo:SetActive(true)
            self.roundStartText.text = "敌方回合"
        end
        msg:Pend()
        self.roundNumberText.text = tostring(msg.Round)
        self.tipsPlayer:PlayOnceView("roundStart" , function ()
            self.tipsObject:SetActive(false)
            msg:Restore()
        end)
    end)
end

----MSGCloseSkillDes
---@param msg MSGCloseSkillDes
function BattleMainPanel:OnMSGCloseSkillDes(msg)
    self.skillDesObject.gameObject:SetActive(false)
end

----MSGShowSkillDes
---@param msg MSGShowSkillDes
function BattleMainPanel:OnMSGShowSkillDes(msg)
    self.skillDes.text = LocalizationMgr.GetDes( msg.Des)
    self.skillDesObject:SetActive(true)
end

----MSGUpdateSkill
---@param msg MSGUpdateSkill
function BattleMainPanel:OnMSGUpdateSkill(msg)
    self.skillViewContainer:Update(msg.SKillDataList)
end


----MSGCampRoundEnd
---@param msg MSGCampRoundEnd
function BattleMainPanel:OnMSGCampRoundEnd(msg)
    if msg.Camp == BattleConfig.PlayerCamp then
        self.campActionTipsText.gameObject:SetActive(true)
        self.campActionTipsText.text = "敌方正在行动。。。。。"
    else
        self.campActionTipsText.gameObject:SetActive(false)
    end
end


----MSGShowWinCondition
---@param msg MSGShowWinCondition
function BattleMainPanel:OnMSGShowWinCondition(msg)
    coroutine.createAndRun(function ()
        msg:Pend()
        self.winConditionObject:SetActive(true)
        self.winconditionText.text = LocalizationMgr.GetDes(msg.Des)
        coroutine.wait(msg.Time)
        self.winConditionObject:SetActive(false)
        msg:Restore()
    end)
end


----MSGDisplayBattleEndAim
---@param msg MSGDisplayBattleEndAim
function BattleMainPanel:OnMSGDisplayBattleEndAim(msg)
    msg:Pend()
    self.winObject:SetActive(msg.Win)
    self.failObject:SetActive(not msg.Win)
    coroutine.wait(1)
    self.winObject:SetActive(false)
    self.failObject:SetActive(false)
    msg:Restore()
end
------------------------------------------------------------------------------------------------
--==userCode==--

return BattleMainPanel
