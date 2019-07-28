---@class RecruitTankView
local RecruitTankView = class('RecruitTankView',uView)
RecruitTankView.ViewPrefabPath = 'Prefab/Main/UI/RecruitTankView.prefab'

---@type GameObject
RecruitTankView.gameObject = nil

---@type Transform
RecruitTankView.transform = nil

---@type GameObject
RecruitTankView.bgOnObject = nil

---@type GameObject
RecruitTankView.bgOffObject = nil

---@type Text
RecruitTankView.tanknameText = nil


--==userCode==--

--==userCode==--

function RecruitTankView:Init()
    
end

function RecruitTankView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.bgOnObject = luaUiFields[1]
    self.bgOffObject = luaUiFields[2]
    self.tanknameText = luaUiFields[3]

end

function RecruitTankView:Dispose()

end



function RecruitTankView:OnButtonClickbgOff()

end


--==userCode==--

--==userCode==--

return RecruitTankView
