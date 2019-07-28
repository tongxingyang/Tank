---------------------------------------------
--- ParallelScriptTimeLine
--- 允许与任何脚本并行
---
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------
---@class ParallelScriptSemaphore : ScriptSemaphore
local ParallelScriptSemaphore = class("ParallelScriptSemaphore")

function ParallelScriptSemaphore:AcquireStartScript()
end

function ParallelScriptSemaphore:AcquireExecuteAction()
end

return ParallelScriptSemaphore