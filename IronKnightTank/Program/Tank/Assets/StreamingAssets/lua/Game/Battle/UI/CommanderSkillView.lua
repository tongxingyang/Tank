---@class CommanderSkillView
local CommanderSkillView = class('CommanderSkillView',uView)
CommanderSkillView.ViewPrefabPath = 'Prefab/Battle/UI/CommanderSkillView.prefab'

---@type GameObject
CommanderSkillView.gameObject = nil

---@type Transform
CommanderSkillView.transform = nil

---@type Image
CommanderSkillView.skillIcon = nil

---@type Text
CommanderSkillView.skillName = nil

---@type Text
CommanderSkillView.skillTimes = nil

---@type GameObject
CommanderSkillView.banCoverObject = nil


--==userCode==--
local MSGReleaseSkillRequest = require("Game.Event.Message.Battle.MSGReleaseSkillRequest")

CommanderSkillView.skillData = nil
--==userCode==--

function CommanderSkillView:Init()
    
end

function CommanderSkillView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.skillIcon = luaUiFields[1]
    self.skillName = luaUiFields[2]
    self.skillTimes = luaUiFields[3]
    self.banCoverObject = luaUiFields[4]

end

function CommanderSkillView:Dispose()

end



function CommanderSkillView:OnButtonClickicon()

    EventBus:Brocast(MSGReleaseSkillRequest:Build({SkillId = self.skillData.Id}))
end


--==userCode==--

---@param skillData  SkillData
function CommanderSkillView:Update(skillData)
    self.skillData = skillData
    local tSkillData = JsonDataMgr.GetInitiativeSkillData(skillData.Id)
    local skillIconPath = "Sprite/Icon/Skill_Icon/"..tSkillData.Skill_Icon..".png"
    self.skillTimes.text  = skillData.CurTime
    self.skillName.text = LocalizationMgr.GetDes( tSkillData.Name)
    ResMgr.SetImageSprite(skillIconPath , self.skillIcon)
    self.banCoverObject:SetActive(skillData:IsBan())
end

--==userCode==--

return CommanderSkillView
