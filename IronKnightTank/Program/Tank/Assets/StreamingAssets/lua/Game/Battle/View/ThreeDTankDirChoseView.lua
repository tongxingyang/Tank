---@class ThreeDTankDirChoseView
local ThreeDTankDirChoseView = class('ThreeDTankDirChoseView',uView)
ThreeDTankDirChoseView.ViewPrefabPath = 'Prefab/Battle/View/spriteTankDirChoseView.prefab'

---@type GameObject
ThreeDTankDirChoseView.gameObject = nil

---@type Transform
ThreeDTankDirChoseView.transform = nil


--==userCode==--
local MSGClickTankChoseDir = require("Game.Event.Message.Battle.MSGClickTankChoseDir")
--==userCode==--

function ThreeDTankDirChoseView:Init()
    
end

function ThreeDTankDirChoseView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    

end

function ThreeDTankDirChoseView:Dispose()

end



function ThreeDTankDirChoseView:OnOnMouseDownup()
    self:Click(EToward.Up)
end

function ThreeDTankDirChoseView:OnOnMouseDowndown()
    self:Click(EToward.Down)
end

function ThreeDTankDirChoseView:OnOnMouseDownright()
    self:Click(EToward.Right)

end

function ThreeDTankDirChoseView:OnOnMouseDownleft()
    self:Click(EToward.Left)

end

function ThreeDTankDirChoseView:OnOnMouseDownleftUp()
    self:Click(EToward.LeftUp)
end

function ThreeDTankDirChoseView:OnOnMouseDownrightUp()
    self:Click(EToward.RightUp)
end

function ThreeDTankDirChoseView:OnOnMouseDownleftDown()
    self:Click(EToward.LeftDown)
end

function ThreeDTankDirChoseView:OnOnMouseDownrightDown()
    self:Click(EToward.RightDown)
end


--==userCode==--
function ThreeDTankDirChoseView:Click(toward)
    EventBus:Brocast(MSGClickTankChoseDir:Build({Toward = toward}))
end

function ThreeDTankDirChoseView:SetPos(pos)
    self.transform.position = pos
end
--==userCode==--

return ThreeDTankDirChoseView
