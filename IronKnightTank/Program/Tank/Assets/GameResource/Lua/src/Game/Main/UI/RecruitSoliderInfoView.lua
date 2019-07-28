---@class RecruitSoliderInfoView
local RecruitSoliderInfoView = class('RecruitSoliderInfoView',uView)
RecruitSoliderInfoView.ViewPrefabPath = 'Prefab/Main/UI/RecruitSoliderInfoView.prefab'

---@type GameObject
RecruitSoliderInfoView.gameObject = nil

---@type Transform
RecruitSoliderInfoView.transform = nil

---@type GameObject
RecruitSoliderInfoView.bgOnObject = nil

---@type GameObject
RecruitSoliderInfoView.bgOffObject = nil

---@type Text
RecruitSoliderInfoView.playerNameText = nil

--==userCode==--
---@type function
RecruitSoliderInfoView._callBack = nil
---@type RecruitSoliderInfoView
RecruitSoliderInfoView._choseSoliderInfoView = nil
---@type SoliderData
RecruitSoliderInfoView._soliderData = nil
--==userCode==--

function RecruitSoliderInfoView:Init()
    
end

function RecruitSoliderInfoView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.bgOnObject = luaUiFields[1]
    self.bgOffObject = luaUiFields[2]
    self.playerNameText = luaUiFields[3]

end

function RecruitSoliderInfoView:Dispose()

end

function RecruitSoliderInfoView:OnButtonClickbgOff()
    self:OnChose()
end


--==userCode==--
---@param data SoliderData
function RecruitSoliderInfoView:Update(data)
    self._soliderData = data
    self.playerNameText.text = data.Name
end

---@param callback function
function RecruitSoliderInfoView:SetEvent(callback)
    self._callBack = callback
end

function RecruitSoliderInfoView:OnChose()
    if  RecruitSoliderInfoView._choseSoliderInfoView then
        RecruitSoliderInfoView._choseSoliderInfoView:SetNormal()
    end
    RecruitSoliderInfoView._choseSoliderInfoView = self
    self:SetHighLight()
    if self._callBack then
        self._callBack(self._soliderData)
    else
        clog("no callback")
    end
end

function RecruitSoliderInfoView:SetHighLight()
    self.bgOnObject:SetActive(true)
    self.bgOffObject:SetActive(false)
end

function RecruitSoliderInfoView:SetNormal()
    self.bgOnObject:SetActive(false)
    self.bgOffObject:SetActive(true)
end
--==userCode==--

return RecruitSoliderInfoView
