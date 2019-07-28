---@class BlockView
local BlockView = class('BlockView',uView)
BlockView.ViewPrefabPath = 'Prefab/Battle/View/BlockView.prefab'

---@type GameObject
BlockView.gameObject = nil

---@type Transform
BlockView.transform = nil

---@type MeshRenderer
BlockView.CubeRenderer = nil


--==userCode==--
---@type BlockData
BlockView.blockData = nil
---@type GridPos
BlockView._pos = nil

local MapConfig = require("Game.Battle.Config.MapConfig")
--==userCode==--

function BlockView:Init()

end

function BlockView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.CubeRenderer = luaUiFields[1]

end

function BlockView:Dispose()

end




--==userCode==--

---@param blockData BlockData
function BlockView:Load(blockData)
    self.transform.position = MapConfig.GetWorldPos(blockData.Pos)
    local tTerrian = JsonDataMgr.GetTerrianEleementData( blockData.TerrianId)
    ResMgr.SetMeshRendererMat("Material/Block/"..tTerrian.Resource_Name , self.CubeRenderer)
end
--==userCode==--

return BlockView
