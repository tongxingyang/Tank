---------------------------------------------
--- ShadowViewData
---	战争阴影视图数据
--- Created by thrt520.
--- DateTime: 2018/7/5
---------------------------------------------
---@class ShadowViewData
local ShadowViewData = class("ShadowViewData")
---@type number 贴图索引
ShadowViewData.textureIndex = 0
---@type GridPos 位置
ShadowViewData.Pos = nil
---@type table<GridPos , ShadowNodeData> 引用的阴影数据
ShadowViewData.ShadowNodeDatas = nil

local TextureIndex = {8 , 2 , 4 , 1}

function ShadowViewData:ctor(pos)
	self.Pos = pos
	self.ShadowNodeDatas = {}
end

---@param shadowNodeData  ShadowNodeData
function ShadowViewData:SetShadowNodeData(shadowNodeData)
	self.ShadowNodeDatas[shadowNodeData.Pos] = shadowNodeData
end

----计算获取贴图索引
function ShadowViewData:GetTextureIndex()
	local index = 0
	local basePos = self.Pos  + GridPos.New(-0.5 , -0.5  )
	local texIndex = 0
	for i = 0, 1 do
		for j = 0, 1 do
			texIndex = texIndex + 1
			local pos = GridPos.New(basePos.x + i  , basePos.y + j)
			if self.ShadowNodeDatas[pos] then
				index = index + (self.ShadowNodeDatas[pos]:IsDark() and TextureIndex[texIndex] or 0)
			end
		end
	end
	return index
end
return ShadowViewData