---------------------------------------------
--- TimelineBasedScript
--- 基于Coroutine时间轴驱动的脚本
---
--- Created by Tianc.
--- DateTime: 2018/04/28
---------------------------------------------

---@class TimelineBasedScript : LScript
local TimelineBasedScript = class("TimelineBasedScript",require("Framework.LetsTrigger.Script.LScript"))
local ScriptSemaphore = require("Framework.LetsTrigger.Script.ScriptSemaphore")
ScriptSemaphore.RegisterSemaphore("Atomic",require("Framework.LetsTrigger.Timeline.AtomicScriptSemaphore"))
ScriptSemaphore.RegisterSemaphore("Linear",require("Framework.LetsTrigger.Timeline.LinearScriptSemaphore"))
ScriptSemaphore.RegisterSemaphore("Parallel",require("Framework.LetsTrigger.Timeline.ParallelScriptSemaphore"))

---@type string @脚本控制驱动类型
TimelineBasedScript.SemaphoreType = "Atomic"

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

---@private
function TimelineBasedScript:_run(params,id,runState)
    ---@type ScriptSemaphore
    local semaphore = ScriptSemaphore.CreateSemaphore(self.SemaphoreType,id,self,coroutine.running())
    semaphore:AcquireStartScript()
    local runtimeInstance = self:StartupAnInstance(id,params,runState,semaphore)
    self:OnRun(runtimeInstance)
    self:ShutdownAnInstance(runtimeInstance)
end

return TimelineBasedScript