---------------------------------------------
--- LevelData
---	编辑器产出的地图数据
--- Created by thrt520.
--- DateTime: 2018/5/10
---------------------------------------------
---@class EditorLevelData
local EditorLevelData = {}
EditorLevelData.Height = 0
EditorLevelData.Width = 0
EditorLevelData.GridHeight = 0
EditorLevelData.GridWidth = 0
EditorLevelData.Name = ""
EditorLevelData.DefaultToward = 0
---@type EditorBlockData[]
EditorLevelData.BlockDataList = nil
return EditorLevelData