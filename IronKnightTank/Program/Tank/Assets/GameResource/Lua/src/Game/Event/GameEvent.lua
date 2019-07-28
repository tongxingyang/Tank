---------------------------------------------
--- GameEvent
--- Created by Tianc.
--- DateTime: 2018/04/16
---------------------------------------------

---@class GameEvent
local GameEvent = {}
local EventDomain = require("Framework.Event.EventDomain")

--------------------------------------------
-- 事件域定义
---------------------------------------------

---@type EventDomain @全局事件域
EventBus = EventDomain.new()



return GameEvent