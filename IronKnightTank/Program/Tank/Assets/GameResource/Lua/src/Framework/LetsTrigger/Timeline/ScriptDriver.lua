---------------------------------------------
--- ScriptDriver
--- Created by Tianc.
--- DateTime: 2018/04/28
---------------------------------------------

---@class ScriptDriver
local ScriptDriver = class("ScriptDriver")
local CoroutineTimeLine = require("Framework.Util.Structure.CoroutineTimeLine")

--TODO:目前所有脚本共用一个timeline
local timeLine = CoroutineTimeLine.New()

---@type number @脚本运行实例id
---@protected
ScriptDriver.runtimeInstanceId = nil

---@type thread @脚本运行co
---@protected
ScriptDriver.co = nil

---@type LScript @脚本
---@protected
ScriptDriver.script = nil


function ScriptDriver:ctor(id,script,scriptThread)
    self.runtimeInstanceId = id
    self.co = scriptThread
    self.script = script
end

---------------------------------------------------------
--- interface
---------------------------------------------------------

function ScriptDriver:StartRequest()end

function ScriptDriver:ActionRequest()end

---------------------------------------------------------
--- function
---------------------------------------------------------

--- 获得脚本运行时间轴
---@param script LScript
---@return CoroutineTimeLine
function ScriptDriver:GetCoroutineTimeLine()
    return timeLine
end


return ScriptDriver