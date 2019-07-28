---------------------------------------------
--- TimelineSemaphore
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------

---@class TimelineSemaphore
local TimelineSemaphore = class("TimelineSemaphore",require("Framework.LetsTrigger.Script.ScriptSemaphore"))
local CoroutineTimeLine = require("Framework.Util.Structure.CoroutineTimeLine")

--TODO:目前所有脚本共用一个timeline
local timeLine = CoroutineTimeLine.New()

---------------------------------------------------------
--- function
---------------------------------------------------------

--- 获得脚本运行时间轴
---@param script LScript
---@return CoroutineTimeLine
---@protected
function TimelineSemaphore:GetCoroutineTimeLine()
    return timeLine
end


return TimelineSemaphore