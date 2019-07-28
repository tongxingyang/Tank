---@class SoliderTalkPanel
local SoliderTalkPanel = class('SoliderTalkPanel',uView)
SoliderTalkPanel.ViewPrefabPath = 'Prefab/Battle/UI/SoliderTalkPanel.prefab'

---@type GameObject
SoliderTalkPanel.gameObject = nil

---@type Transform
SoliderTalkPanel.transform = nil

---@type Image
SoliderTalkPanel.solideIconImage = nil

---@type Text
SoliderTalkPanel.TextText = nil

---@type Text
SoliderTalkPanel.soliderNameText = nil


--==userCode==--
SoliderTalkPanel.Co = nil
--==userCode==--

function SoliderTalkPanel:Init()
    
end

function SoliderTalkPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.solideIconImage = luaUiFields[1]
    self.TextText = luaUiFields[2]
    self.soliderNameText = luaUiFields[3]

end

function SoliderTalkPanel:Dispose()

end



function SoliderTalkPanel:OnClickmask(eventData)
    PanelManager.Close(PanelManager.PanelEnum.SoliderTalkPanel)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.SoliderTalkPanel,SoliderTalkPanel)
------------------------------------------------------------------------------------------
function SoliderTalkPanel:OnOpen(param)
    if self.closeCallBack then
        self.closeCallBack()
    end
    self.closeCallBack = param.CloseCallBack
    local iconPath = param.IconPath
    local content = param.Content
    if iconPath then
        ResMgr.SetImageSprite(iconPath , self.solideIconImage )
    end
    self.TextText.text  = content
    self.soliderNameText.text =  param.SoliderName
    if self.Co then
        coroutine.stop(self.Co)
    end
    self.Co = coroutine.createAndRun(function ()
        coroutine.wait(3)
        self.Co = nil
        PanelManager.Close(PanelManager.PanelEnum.SoliderTalkPanel)
    end)
end


function SoliderTalkPanel:OnClose(param)
    if self.closeCallBack then
        self.closeCallBack()
    end
    if self.Co then
        coroutine.stop(self.Co)
        self.Co = nil
    end
end
--==userCode==--

return SoliderTalkPanel
