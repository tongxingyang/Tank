---@class TankTrajectoryView
local TankTrajectoryView = class('TankTrajectoryView',uView)
TankTrajectoryView.ViewPrefabPath = 'Prefab/Battle/View/TankTrajectoryView.prefab'

---@type GameObject
TankTrajectoryView.gameObject = nil

---@type Transform
TankTrajectoryView.transform = nil

---@type LineRenderer
TankTrajectoryView.TrajectoryRenderer = nil


--==userCode==--
---已弃用
--==userCode==--

function TankTrajectoryView:Init()
    
end

function TankTrajectoryView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.TrajectoryRenderer = luaUiFields[1]

end

function TankTrajectoryView:Dispose()

end




--==userCode==--

function TankTrajectoryView:Show(start  , endPos)
    self.gameObject:SetActive(true)
    self.TrajectoryRenderer:SetPosition(0 , start)
    self.TrajectoryRenderer:SetPosition(1 , endPos)
end

function TankTrajectoryView:Close()
    self.gameObject:SetActive(false)
end

--==userCode==--

return TankTrajectoryView
