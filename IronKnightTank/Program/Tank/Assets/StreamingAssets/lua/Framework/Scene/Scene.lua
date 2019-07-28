---------------------------------------------
--- Scene 情景
--- 一个情景下的一组模块集合
---
--- Created by Tianc.
--- DateTime: 2018/04/15
---------------------------------------------

---@class Scene
local Scene = class("Scene")

---@type string
Scene.Name = nil

---@type SceneActor[]
Scene.Actors = nil


return Scene