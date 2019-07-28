---@class RecruitEnsureWin
local RecruitEnsureWin = class('RecruitEnsureWin',uView)
RecruitEnsureWin.ViewPrefabPath = 'Prefab/Main/UI/RecruitEnsureWin.prefab'

---@type GameObject
RecruitEnsureWin.gameObject = nil

---@type Transform
RecruitEnsureWin.transform = nil

---@type GameObject
RecruitEnsureWin.newObject = nil

---@type GameObject
RecruitEnsureWin.oldObject = nil

---@type Text
RecruitEnsureWin.costLabel = nil

---@type Text
RecruitEnsureWin.costContentText = nil

---@type Text
RecruitEnsureWin.numberText = nil


--==userCode==--
RecruitEnsureWin.cost = 0
RecruitEnsureWin.number = 0
RecruitEnsureWin.isOld = false
RecruitEnsureWin.MaxCount = 0
local MSGRecruitSolider = require("Game.Event.Message.Welcome.MSGRecruitSolider")
--==userCode==--

function RecruitEnsureWin:Init()
    
end

function RecruitEnsureWin:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.newObject = luaUiFields[1]
    self.oldObject = luaUiFields[2]
    self.costLabel = luaUiFields[3]
    self.costContentText = luaUiFields[4]
    self.numberText = luaUiFields[5]

end

function RecruitEnsureWin:Dispose()

end



function RecruitEnsureWin:OnButtonClickadd()

    self.number = math.clamp(self.number + 1  , nil , self.MaxCount)
    self:UpdateNumberAndCost()
end

function RecruitEnsureWin:OnButtonClicksub()
    self.number = math.clamp(self.number - 1 , 1 )
    self:UpdateNumberAndCost()
end

function RecruitEnsureWin:OnButtonClickensuerBtn()
    PanelManager.Close(PanelManager.PanelEnum.RecruitEnsureWin)
    EventBus:Brocast(MSGRecruitSolider:Build({Count = self.number , IsOld = self.isOld}))
end

function RecruitEnsureWin:OnButtonClickcancelBtn()
    PanelManager.Close(PanelManager.PanelEnum.RecruitEnsureWin)
end


--==userCode==--

PanelManager.Register(PanelManager.PanelEnum.RecruitEnsureWin , RecruitEnsureWin)
---@param cost number
---@param isOld boolean
function RecruitEnsureWin:OnOpen(param)
    self.cost = param.cost
    self.isOld = param.isOld
    self.MaxCount = param.maxCount
    self.gameObject:SetActive(true)
    self.oldObject:SetActive(self.isOld)
    self.newObject:SetActive(not self.isOld)
    self.number = 1
    self:UpdateNumberAndCost()
end

function RecruitEnsureWin:Close()
    self.gameObject:SetActive(false)
end

function RecruitEnsureWin:UpdateNumberAndCost()
    self.costContentText.text = tostring(self.number * self.cost)
    self.numberText.text = tostring(self.number)
end

--==userCode==--

return RecruitEnsureWin
