---@class RecruitTankInfoView
local RecruitTankInfoView = class('RecruitTankInfoView',uView)
RecruitTankInfoView.ViewPrefabPath = 'Prefab/Main/UI/RecruitTankInfoView.prefab'

---@type GameObject
RecruitTankInfoView.gameObject = nil

---@type Transform
RecruitTankInfoView.transform = nil

---@type GameObject
RecruitTankInfoView.bgOnObject = nil

---@type GameObject
RecruitTankInfoView.bgOffObject = nil

---@type Text
RecruitTankInfoView.playerNameText = nil


--==userCode==--
local MSGOpenRecruitEnsureWinRequest = require("Game.Event.Message.Main.MSGOpenRecruitEnsureWinRequest")

---@type table
RecruitTankInfoView.tankData = nil
---@type number
RecruitTankInfoView.count = 0
---@type function
RecruitTankInfoView.event = 0
---@type function
RecruitTankInfoView._callBack = nil
---@type RecruitSoliderInfoView
RecruitTankInfoView._choseTankInfoView = nil
--==userCode==--

function RecruitTankInfoView:Init()
    
end

function RecruitTankInfoView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.bgOnObject = luaUiFields[1]
    self.bgOffObject = luaUiFields[2]
    self.playerNameText = luaUiFields[3]

end

function RecruitTankInfoView:Dispose()

end



function RecruitTankInfoView:OnButtonClickbgOff()
    self:OnChose()
end


--==userCode==--
function RecruitTankInfoView:Update(data)
    self.tankData = data
    local tankUse = data.Count
    local maxCount = data.TankMaxCount
    ---@type TankData
    local tankData = data.TankInfo
    GameTools.PrintTable(tankData)
    self.playerNameText.text = LocalizationMgr.GetDes(tankData.Name)..string.format("(%d)" , tankUse)
    --self.countText.text = tankUse
end

---@param event function
function RecruitTankInfoView:SetEvent(event)
    self._callBack = event
end

function RecruitTankInfoView:OnChose()
    if  RecruitTankInfoView._choseTankInfoView then
        RecruitTankInfoView._choseTankInfoView:SetNormal()
    end
    RecruitTankInfoView._choseTankInfoView = self
    self:SetHighLight()
    if self._callBack then
        self._callBack(self.tankData)
    else
        clog("no callback")
    end
end

function RecruitTankInfoView:SetHighLight()
    self.bgOnObject:SetActive(true)
    self.bgOffObject:SetActive(false)
end

function RecruitTankInfoView:SetNormal()
    self.bgOnObject:SetActive(false)
    self.bgOffObject:SetActive(true)
end
--==userCode==--

return RecruitTankInfoView
