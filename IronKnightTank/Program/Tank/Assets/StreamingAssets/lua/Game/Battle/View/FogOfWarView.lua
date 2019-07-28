---@class FogOfWarView
local FogOfWarView = class('FogOfWarView',uView)
FogOfWarView.ViewPrefabPath = 'Prefab/Battle/View/FogOfWarView.prefab'

---@type GameObject
FogOfWarView.gameObject = nil

---@type Transform
FogOfWarView.transform = nil

---@type TextMesh
FogOfWarView.textMesh = nil

---@type SpriteRenderer
FogOfWarView.Mask1Renderer = nil


--==userCode==--
local MapConfig  = require("Game.Battle.Config.MapConfig")

FogOfWarView._loadSpriteCallBack = nil
--==userCode==--

function FogOfWarView:Init()
    
end

function FogOfWarView:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.textMesh = luaUiFields[1]
    self.Mask1Renderer = luaUiFields[2]

end

function FogOfWarView:Dispose()

end




--==userCode==--
---@param shadowViewData ShadowViewData
function FogOfWarView:Update(shadowViewData)
    local index = shadowViewData:GetTextureIndex()
    if index == 0 then
        self.Mask1Renderer.gameObject:SetActive(false)
    else
        self.Mask1Renderer.gameObject:SetActive(true)
        self.Mask1Renderer.sprite = self._loadSpriteCallBack(index)
    end
end

function FogOfWarView:SetPos(shadowViewData)
    self.transform.position = MapConfig.GetWorldPos(shadowViewData.Pos )
end

function FogOfWarView:SetGetSpriteCallBack(callBack)
    self._loadSpriteCallBack = callBack
end
--==userCode==--

return FogOfWarView
