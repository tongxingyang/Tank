---@class TankSignView
local TankSignView = class('TankSignView',uView)
TankSignView.ViewPrefabPath = 'Prefab/Battle/View/TankSignView.prefab'

---@type GameObject
TankSignView.gameObject = nil

---@type Transform
TankSignView.transform = nil

---@type GameObject
TankSignView.arrowObject = nil

---@type GameObject
TankSignView.tanhaoObject = nil


--==userCode==--

--==userCode==--

function TankSignView:Init()
    
end

function TankSignView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.arrowObject = luaUiFields[1]
    self.tanhaoObject = luaUiFields[2]

end

function TankSignView:Dispose()

end




--==userCode==--

function TankSignView:SetMode(isMineTank)
    self.arrowObject:SetActive(isMineTank)
    self.tanhaoObject:SetActive(not isMineTank)
end

function TankSignView:Close()
    self.arrowObject:SetActive(false)
    self.tanhaoObject:SetActive(false)
end

function TankSignView:SetPos(pos)
    self.transform.position = pos
end

--==userCode==--

return TankSignView
