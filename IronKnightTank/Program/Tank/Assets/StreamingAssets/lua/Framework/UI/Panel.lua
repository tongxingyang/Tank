---------------------------------------------
--- Panel
--- Created by Tianc.
--- DateTime: 2018/04/17
---------------------------------------------

---@class Panel : View
local Panel = class("Panel",require("Framework.UI.View"))

--- 面板被打开时
---@param param any
function Panel:OnOpen(param)end

--- 面板被关闭时
---@param param any
function Panel:OnClose(param)end

return Panel