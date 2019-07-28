---------------------------------------------
--- BlockData
---	地图的最基本组成单位
--- Created by thrt520.
--- DateTime: 2018/5/4
---------------------------------------------
---@class BlockData
local BlockData = class("BlockData")
local this = BlockData
---@type GridPos 位置
this.Pos = nil
---@type 能否通过
this.CanPass = true
---@type boolean 遮挡视野
this.CullingFOV = false
---@type boolean 阻挡攻击
this.Blocking = false
---@type boolean 出生位置
this.IsBornBlock = false
---@type boolean 被占据
this.IsHold = false
---@type number 地形id
this.TerrianId = 0
---@type number 地形遮挡系数
this.CovertAddition = 0
---@type number 移动消耗
this.MoveCost = 0
---@type number 地形类型
this.TerrianType = 0
---@type boolean 可移动区域
this.IsMoveableArea = false
---@type boolean 技能释放区域
this.SkillReleaseArea = false
---@type boolean 技能作用区域
this.SkillEffectArea = false
---@type boolean 关卡释放技能提示区域
this.LevelSkillEffectArea = false
---@type boolean
this.CanMoverPass = false
---@type boolean
this.CanMoverEnter = false
----------算法需求 不用在意
this.hCost = 0				-----A*算法使用
this.gCost = 0				-----A*算法使用
this.parent = 0 			-----移动区域算法使用
this.Cost = 0 				-----移动区域算法使用
this.CostSpeed = 0 				-----移动区域算法使用
this.CostPower = 0 				-----移动区域算法使用

---@param editorBlockData EditorBlockData
function BlockData:ctor(editorBlockData)
	self.TerrianId = editorBlockData.TerrianId
	local tData = JsonDataMgr.GetTerrianEleementData( self.TerrianId)
	self.CanPass = (tData.Can_by == 1)
	self.CullingFOV = (tData.Blocking == 1)
	self.Blocking = (tData.Blocking == 1)
	self.Pos = GridPos.New(editorBlockData.pos.x , editorBlockData.pos.y)
	self.IsBornBlock = editorBlockData.IsBornBlock
	self.CovertAddition = tData.Covert_Addition
	self.MoveCost = tData.Speed_Punished
	self.TerrianType = tData.TerrainEleement_Type
end

function BlockData:GetFCost()
	return self.hCost + self.gCost
end

return BlockData
