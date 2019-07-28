---@class RecruitTankEnsureWin
local RecruitTankEnsureWin = class('RecruitTankEnsureWin',uView)
RecruitTankEnsureWin.ViewPrefabPath = 'Prefab/Main/UI/RecruitTankEnsureWin.prefab'

---@type GameObject
RecruitTankEnsureWin.gameObject = nil

---@type Transform
RecruitTankEnsureWin.transform = nil

---@type InputField
RecruitTankEnsureWin.InputFieldField = nil

---@type Text
RecruitTankEnsureWin.costContentText = nil

---@type GameObject
RecruitTankEnsureWin.RecruitTankPropertyViewObject = nil


--==userCode==--
local MSGRecruitTankRequest = require("Game.Event.Message.Main.MSGRecruitTankRequest")
local RecruitTankPropertyView = require("Game.Main.UI.RecruitTankPropertyView")
---@type number
RecruitTankEnsureWin._maxCount = 0
---@type number
RecruitTankEnsureWin._cost= 0
---@type TankData
RecruitTankEnsureWin._tankData = nil
---@type number
RecruitTankEnsureWin.count = nil
RecruitTankEnsureWin.Data = nil
---@type RecruitTankPropertyView
RecruitTankEnsureWin.RecruitTankPropertyView = nil
--==userCode==--

function RecruitTankEnsureWin:Init()
    self.RecruitTankPropertyView = RecruitTankPropertyView.New()
    self.RecruitTankPropertyView:Binding(self.RecruitTankPropertyViewObject)
    self.RecruitTankPropertyView:Init()
    self.RecruitTankPropertyView:SetType(true)
end

function RecruitTankEnsureWin:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.InputFieldField = luaUiFields[1]
    self.costContentText = luaUiFields[2]
    self.RecruitTankPropertyViewObject = luaUiFields[3]

end

function RecruitTankEnsureWin:Dispose()

end



function RecruitTankEnsureWin:OnButtonClickcancelBtn()
    PanelManager.Close(PanelManager.PanelEnum.RecruitTankEnsureWin)
end

function RecruitTankEnsureWin:OnButtonClickensureBtn()
    EventBus:Brocast(MSGRecruitTankRequest:Build({TankData = self.Data}))
end

function RecruitTankEnsureWin:OnButtonClickaddBtn()
    self.count = math.clamp(self.count + 1 , 1 , self._maxCount)
    self:UpdateNumAndCost()
end

function RecruitTankEnsureWin:OnButtonClicksubBtn()
    self.count = math.clamp(self.count - 1 , 1 , self._maxCount)
    self:UpdateNumAndCost()
end

function RecruitTankEnsureWin:OnInputFieldEndEditInputField(content)
    local count = tonumber(content)
    if count then
        self.count =  math.clamp(count , 1 , self._maxCount)
        self:UpdateNumAndCost()
    else
        self.InputFieldField.text = ""
    end
end


--==userCode==--
PanelManager.Register(PanelManager.PanelEnum.RecruitTankEnsureWin, RecruitTankEnsureWin)
---@param param table
function RecruitTankEnsureWin:OnOpen(param)
    self.Data = param.TankData
    self._tankData = param.TankData.TankData
    self._maxCount = param.TankData.TankMaxCount
    self._cost = param.TankData.TankUse
    self.costContentText.text = tostring(self._cost)
    --self.tankNameText.text = LocalizationMgr.GetDes(self._tankData.Name)
    self.count = 1
    self:UpdateNumAndCost()
    --ResMgr.SetImageSprite(self._tankData:GetTankIconPath() , self.tankIconImage)
    self.RecruitTankPropertyView:Update(self._tankData)
    --self.MaxCountText.text = self._maxCount
    --self.tankTypeText.text =  self._tankData.TankType
    --self.turretTypeText.text =  self._tankData.TurretType
    --self.dmgText.text ="总伤害".. self._tankData.Atk
    --self.armorText.text = "总护甲"..self._tankData.Armored
    --self.dmgDetialText.text = self:GetDmgDes(self._tankData)
    --self.armorDetialText.text = self:GetArmorDes(self._tankData)
end

function RecruitTankEnsureWin:OnClose()

end

function RecruitTankEnsureWin:UpdateNumAndCost()
    self.InputFieldField.text = tostring(self.count)
    self.costContentText.text = "消耗:"..tostring(self.count * self._cost)
end

---@param tankData TankData
function RecruitTankEnsureWin:GetDmgDes(tankData )
    local str = ""
    local step = 200
    for i = 1, 8 do
        local des = (i - 1) * step.."-"..i *step
        des = des..":"..tankData:GetDmg(i)
        des = des .."\n"
        str = str..des
    end
    return str
end

---@param tankData TankData
function RecruitTankEnsureWin:GetArmorDes(tankData)
    local turretFrontDes = "炮塔正面:"..tankData:GetEquipmentArmored(EHitRes.HitTurret , EHitTankPos.Front).."\n"
    local turretSideDes = "炮塔侧面:"..tankData:GetEquipmentArmored(EHitRes.HitTurret , EHitTankPos.RightSide).."\n"
    local turretBackDes = "炮塔背面:"..tankData:GetEquipmentArmored(EHitRes.HitTurret , EHitTankPos.Back).."\n"
    local bodyFrontDes = "炮塔正面:"..tankData:GetEquipmentArmored(EHitRes.HitBody , EHitTankPos.Front).."\n"
    local bodySideDes = "炮塔侧面:"..tankData:GetEquipmentArmored(EHitRes.HitBody , EHitTankPos.RightSide).."\n"
    local bodyBackDes = "炮塔背面:"..tankData:GetEquipmentArmored(EHitRes.HitBody , EHitTankPos.Back).."\n"
    return turretFrontDes..turretSideDes..turretBackDes..bodyFrontDes..bodySideDes..bodyBackDes
end

--==userCode==--

return RecruitTankEnsureWin
