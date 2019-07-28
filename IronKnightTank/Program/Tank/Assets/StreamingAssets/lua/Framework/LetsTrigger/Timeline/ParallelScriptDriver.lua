---------------------------------------------
--- ParallelScriptTimeLine
--- 允许与任何脚本并行
---
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------
---@class ParallelScriptDriver : ScriptDriver
local ParallelScriptDriver = class("ParallelScriptDriver")


function ParallelScriptDriver:ctor(scriptInstance, scriptThread)
end

function ParallelScriptDriver:StartRequest()
end

function ParallelScriptDriver:ActionRequest()
end

return ParallelScriptDriver