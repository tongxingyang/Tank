---@class RecruitPanel
local RecruitPanel = class('RecruitPanel',uView)
RecruitPanel.ViewPrefabPath = 'Prefab/Main/UI/RecruitPanel.prefab'

---@type GameObject
RecruitPanel.gameObject = nil

---@type Transform
RecruitPanel.transform = nil

---@type Text
RecruitPanel.soliderNumText = nil

---@type Text
RecruitPanel.tankNumText = nil

---@type Text
RecruitPanel.TextText = nil


--==userCode==--
local MSGClickRecruitSolideButton = require("Game.Event.Message.Welcome.MSGClickRecruitSolideButton")
local MSGClickCurRecruitSoliderButton = require("Game.Event.Message.Welcome.MSGClickCurRecruitSoliderButton")
local MSGClickCurRecruitTankButton = require("Game.Event.Message.Welcome.MSGClickCurRecruitTankButton")
local MSGClickRecruitTankButton = require("Game.Event.Message.Welcome.MSGClickRecruitTankButton")
local MSGShowAllRecruitSoliderRequest = require("Game.Event.Message.Main.MSGShowAllRecruitSoliderRequest")
local MSGShowAllRecruitTankRequest = require("Game.Event.Message.Main.MSGShowAllRecruitTankRequest")
local receiveCommands = {
    require("Game.Event.Message.Welcome.MSGUpdateRecruitPanel")
}
--==userCode==--

function RecruitPanel:Init()
    EventBus:RegisterSelfReceiver(receiveCommands , self)
end

function RecruitPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.soliderNumText = luaUiFields[1]
    self.tankNumText = luaUiFields[2]
    self.TextText = luaUiFields[3]

end

function RecruitPanel:Dispose()
    EventBus:UnregisterSelfReceiver(receiveCommands , self)
end



function RecruitPanel:OnButtonClickbackButton()
    PanelManager.Close(PanelManager.PanelEnum.RecruitPanel)
end

function RecruitPanel:OnButtonClickreadyButton()

end

function RecruitPanel:OnButtonClicknewSoliderButton()
    EventBus:Brocast(MSGClickRecruitSolideButton:Build({IsOld = false }))
end

function RecruitPanel:OnButtonClickoldSoliderButton()
    EventBus:Brocast(MSGClickRecruitSolideButton:Build({IsOld = true }))
end

function RecruitPanel:OnButtonClicktankButton()
    EventBus:Brocast(MSGClickRecruitTankButton.New())
end

function RecruitPanel:OnButtonClicksoliderNum()
    EventBus:Brocast(MSGShowAllRecruitSoliderRequest.New())
end

function RecruitPanel:OnButtonClicktankNum()
    EventBus:Brocast(MSGShowAllRecruitTankRequest.New())

end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.RecruitPanel , RecruitPanel)

---@param param table
function RecruitPanel:OnOpen(param)
    self:Update(param.TankMax , param.SoliderMax , param.TankCount , param.SoliderCount)
end

function RecruitPanel:OnClose()

end


function RecruitPanel:Update(maxTankCount , maxSoliderCount , curTankCount , curSoliderCount)
    --clog("update "..tostring(curSoliderCount).."  max"..tostring(maxSoliderCount))
    self.soliderNumText.text = curSoliderCount.."/"..maxSoliderCount
    self.tankNumText.text = curTankCount.."/"..maxTankCount
end

----MSGUpdateRecruitPanel事件
---@param msg MSGUpdateRecruitPanel
function RecruitPanel:OnMSGUpdateRecruitPanel(msg)
    self:Update(msg.MaxTankCount , msg.MaxSoliderCount , msg.CurTankCount , msg.CurSoliderCount)
end
--==userCode==--

return RecruitPanel
