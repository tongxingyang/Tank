---@class BattleEndPanel
local BattleEndPanel = class('BattleEndPanel',uView)
BattleEndPanel.ViewPrefabPath = 'Prefab/Battle/UI/BattleEndPanel.prefab'

---@type GameObject
BattleEndPanel.gameObject = nil

---@type Transform
BattleEndPanel.transform = nil

---@type Text
BattleEndPanel.desText = nil


--==userCode==--

--==userCode==--

function BattleEndPanel:Init()
    
end

function BattleEndPanel:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.desText = luaUiFields[1]

end

function BattleEndPanel:Dispose()

end



function BattleEndPanel:OnClickback(eventData)

end

function BattleEndPanel:OnPressrestart(isDown,gameObject)
    --local game = require("Game.Game")
    --local SceneManager = require("Framework.Scene.SceneManager")
    --local battle = game.Battle
    --SceneManager.SwicthScene(battle)
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.BattleEndPanel,BattleEndPanel)

function BattleEndPanel:OnOpen(param)
    local winCamp = param.WinCamp
    if winCamp == ECamp.Red then
        self.desText.text = "玩家胜利"
    else
        self.desText.text = "npc胜利"
    end
end


function BattleEndPanel:OnClose(param)

end
--==userCode==--

return BattleEndPanel
