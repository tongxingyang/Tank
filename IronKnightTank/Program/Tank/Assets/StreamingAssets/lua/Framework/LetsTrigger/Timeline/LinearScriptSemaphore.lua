---------------------------------------------
--- LinearScriptSemaphore
--- 当运行中触发了其他脚本的运行，则先运行其他脚本，直至其他脚本运行结束，本脚本继续
---
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------
---@class LinearScriptSemaphore : ScriptSemaphore
local LinearScriptSemaphore = class("LinearScriptSemaphore",require("Framework.LetsTrigger.Timeline.TimelineSemaphore"))

function LinearScriptSemaphore:AcquireStartScript()
    local coTimeLine = self:GetCoroutineTimeLine()
    coTimeLine:Request(self.runtimeInstanceId,self.co,false):Yield()
end

function LinearScriptSemaphore:AcquireExecuteAction()
    local coTimeLine = self:GetCoroutineTimeLine()
    coTimeLine:Request(self.runtimeInstanceId,self.co,false):Yield()
end

return LinearScriptSemaphore