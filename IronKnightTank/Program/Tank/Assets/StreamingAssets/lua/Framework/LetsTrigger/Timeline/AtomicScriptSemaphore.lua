---------------------------------------------
--- AtomicScriptSemaphore
--- 不允许与其他脚本或者自己的另一个脚本实例同时运行
---
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------
---@class AtomicScriptSemaphore : ScriptSemaphore
local AtomicScriptSemaphore = class("AtomicScriptSemaphore",require("Framework.LetsTrigger.Timeline.TimelineSemaphore"))

function AtomicScriptSemaphore:AcquireStartScript()
    local coTimeLine = self:GetCoroutineTimeLine()
    coTimeLine:Request(self.runtimeInstanceId,self.co,true):Yield()
end

function AtomicScriptSemaphore:AcquireExecuteAction()
end

return AtomicScriptSemaphore