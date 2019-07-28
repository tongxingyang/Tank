---------------------------------------------
--- View
--- Created by Tianc.
--- DateTime: 2018/04/15
---------------------------------------------

---@class View
local View = class("View")
uView = View

---@type string
View.ViewPrefabPath = nil

---@type GameObject
View.gameObject = nil

---@type Transform
View.transform = nil

---@type ViewProxy
View.ViewProxyClass = nil

--- 初始化
function View:Init()end

--- 绑定prefab组件到属性
---@param object GameObject
function View:Binding(object)end

--- 销毁
function View:Destroy()
    Object.Destroy(self.gameObject)
end

return View