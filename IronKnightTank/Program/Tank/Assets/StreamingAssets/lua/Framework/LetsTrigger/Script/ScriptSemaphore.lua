---------------------------------------------
--- ScriptSemaphore
--- 脚本运行信号量
--- 管理脚本执行许可,从整体上保证脚本系统能按一定规则运行
---
--- Created by Tianc.
--- DateTime: 2018/04/28
---------------------------------------------

---@class ScriptSemaphore
local ScriptSemaphore = class("ScriptSemaphore")

ScriptSemaphore.Config = {}

--- 注册一个Semaphore类型
function ScriptSemaphore.RegisterSemaphore(name,class)
    ScriptSemaphore.Config[name] = class
end

--- 创建一个Semaphore实例
---@return ScriptSemaphore
function ScriptSemaphore.CreateSemaphore(name,id,script,scriptThread)
    assert(ScriptSemaphore.Config[name],string.format("Can not find script semaphore '%s'.",name))
    return ScriptSemaphore.Config[name].new(id,script,scriptThread)
end


---@type number @脚本运行实例id
---@protected
ScriptSemaphore.runtimeInstanceId = nil

---@type thread @脚本运行co
---@protected
ScriptSemaphore.co = nil

---@type LScript @脚本
---@protected
ScriptSemaphore.script = nil

---@private
function ScriptSemaphore:ctor(id,script,scriptThread)
    self.runtimeInstanceId = id
    self.co = scriptThread
    self.script = script
end

---------------------------------------------------------
--- interface
---------------------------------------------------------

--- 请求运行脚本
function ScriptSemaphore:AcquireStartScript()end

--- 请求执行动作
function ScriptSemaphore:AcquireExecuteAction()end



return ScriptSemaphore