---@class LogicBlockView
local LogicBlockView = class('LogicBlockView',uView)
LogicBlockView.ViewPrefabPath = 'Prefab/Battle/View/LogicBlockView.prefab'

---@type GameObject
LogicBlockView.gameObject = nil

---@type Transform
LogicBlockView.transform = nil

---@type GameObject
LogicBlockView.HighLightAnim = nil

---@type GameObject
LogicBlockView.CanMoveMaskObject = nil

---@type TextMesh
LogicBlockView.textMesh = nil

---@type GameObject
LogicBlockView.choseTipsObject = nil

---@type GameObject
LogicBlockView.CanMoveMask2Object = nil

---@type GameObject
LogicBlockView.skillReleaseAreaObject = nil

---@type GameObject
LogicBlockView.skillEffectAreaObject = nil

---@type GameObject
LogicBlockView.levelSkillAreaObject = nil


--==userCode==--
---@type GridPos
LogicBlockView._pos = nil
---@type BlockData
LogicBlockView.blockData = nil

local MSGMouseEnterBlock = require("Game.Event.Message.Battle.MSGMouseEnterBlock")
local MapConfig = require("Game.Battle.Config.MapConfig")
local MSGClickBlock = require("Game.Event.Message.Battle.MSGClickBlock")
--==userCode==--

function LogicBlockView:Init()
    
end

function LogicBlockView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.HighLightAnim = luaUiFields[1]
    self.CanMoveMaskObject = luaUiFields[2]
    self.textMesh = luaUiFields[3]
    self.choseTipsObject = luaUiFields[4]
    self.CanMoveMask2Object = luaUiFields[5]
    self.skillReleaseAreaObject = luaUiFields[6]
    self.skillEffectAreaObject = luaUiFields[7]
    self.levelSkillAreaObject = luaUiFields[8]

end

function LogicBlockView:Dispose()

end



function LogicBlockView:OnOnMouseEnterquad()
    self.choseTipsObject:SetActive(true)

    EventBus:Brocast(MSGMouseEnterBlock:Build({GridPos = self.blockData.Pos}))
end

function LogicBlockView:OnOnMouseExitquad()
    self.choseTipsObject:SetActive(false)
end

function LogicBlockView:OnOnMouseUpAsButtonquad()
    EventBus:Brocast(MSGClickBlock:Build({gridPos = self._pos}))
end


--==userCode==--
---@param blockData BlockData
function LogicBlockView:Update(blockData)
    self.blockData = blockData
    self._pos = blockData.Pos
    self.transform.position = MapConfig.GetWorldPos(self._pos)
    self.textMesh.text = blockData.CostSpeed
    if blockData.IsMoveableArea then
        self:_moveAreaHighLight()
    else
        self:_cancelMoveAreaHighLight()
    end
    self.skillEffectAreaObject:SetActive(blockData.SkillEffectArea )
    self.skillReleaseAreaObject:SetActive(blockData.SkillReleaseArea and not blockData.SkillEffectArea)
    self.levelSkillAreaObject:SetActive(blockData.LevelSkillEffectArea)
end

-----出生范围高亮
function LogicBlockView:_bornHighLight()
    self.HighLightAnim:SetActive(true)
end

-----取消出生范围高亮
function LogicBlockView:_cancelBornHighLight()
    self.HighLightAnim:SetActive(false)
end

-----移动范围高亮
function LogicBlockView:_moveAreaHighLight()
    self.CanMoveMaskObject:SetActive(self.blockData.CostPower == 1)
    self.CanMoveMask2Object:SetActive(self.blockData.CostPower == 2)
end

---取消移动范围高亮
function LogicBlockView:_cancelMoveAreaHighLight()
    self.CanMoveMaskObject:SetActive(false)
    self.CanMoveMask2Object:SetActive(false)
end

--==userCode==--

return LogicBlockView
