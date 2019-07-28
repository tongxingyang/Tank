---------------------------------------------
--- TimelineBasedScript
--- 基于Coroutine时间轴驱动的脚本
---
--- Created by Tianc.
--- DateTime: 2018/04/28
---------------------------------------------

---@class TimelineBasedScript : LScript
local TimelineBasedScript = class("TimelineBasedScript",require("Framework.LetsTrigger.Script.LScript"))

---@type string @脚本控制驱动类型
TimelineBasedScript.DriverType = "Atomic"

---@type table @时间轴驱动方式配置
TimelineBasedScript.DriverTypeConfig = {
    Atomic = require("Framework.LetsTrigger.Timeline.AtomicScriptDriver"),
    Linear = require("Framework.LetsTrigger.Timeline.LinearScriptDriver"),
    Parallel = require("Framework.LetsTrigger.Timeline.ParallelScriptDriver"),
}

---------------------------------------------------------
--- interface
---------------------------------------------------------

function TimelineBasedScript:OnRun(runtimeInstance)end

---------------------------------------------------------
--- function
---------------------------------------------------------

function TimelineBasedScript:Run(params,id)
    local runState = AsyncState.New()
    coroutine.createAndRun(TimelineBasedScript._run,self,params,id,runState)
    return runState
end

function TimelineBasedScript:_run(params,id,runState)
    ---@type ScriptDriver
    local driver = TimelineBasedScript.DriverTypeConfig[self.DriverType].new(id,self,coroutine.running())
    driver:StartRequest()
    local runtimeInstance = self:BuildRuntimeInstance(id)
    runtimeInstance:Start(params,runState,driver)
    self:OnRun(runtimeInstance)
end

return TimelineBasedScript