---@class TankRandarMapView
local TankRandarMapView = class('TankRandarMapView',uView)
TankRandarMapView.ViewPrefabPath = ''

---@type GameObject
TankRandarMapView.gameObject = nil

---@type Transform
TankRandarMapView.transform = nil

---@type TankRadarMap
TankRandarMapView.RadarMapMap = nil

---@type Text
TankRandarMapView.viewContentTextText = nil

---@type Text
TankRandarMapView.speedContentTextText = nil

---@type Text
TankRandarMapView.projectContentTextText = nil


--==userCode==--

--==userCode==--

function TankRandarMapView:Init()
    
end

function TankRandarMapView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.RadarMapMap = luaUiFields[1]
    self.viewContentTextText = luaUiFields[2]
    self.speedContentTextText = luaUiFields[3]
    self.projectContentTextText = luaUiFields[4]

end

function TankRandarMapView:Dispose()

end




--==userCode==--
function TankRandarMapView:Update()

end
--==userCode==--

return TankRandarMapView
