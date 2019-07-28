---------------------------------------------
--- AtomicScriptDriver
--- 不允许与其他脚本或者自己的另一个脚本实例同时运行
---
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------
---@class AtomicScriptDriver : ScriptDriver
local AtomicScriptDriver = class("AtomicScriptDriver",require("Framework.LetsTrigger.Timeline.ScriptDriver"))

function AtomicScriptDriver:StartRequest()
    local coTimeLine = self:GetCoroutineTimeLine()
    coTimeLine:Request(self.runtimeInstanceId,self.co,true):Yield()
end

function AtomicScriptDriver:ActionRequest()
end

return AtomicScriptDriver