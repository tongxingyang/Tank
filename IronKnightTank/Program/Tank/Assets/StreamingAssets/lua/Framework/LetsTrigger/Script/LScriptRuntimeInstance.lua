---------------------------------------------
--- LScriptRuntimeInstance
--- 可编程命令脚本运行实例(从命令配置生成对应的一组运行对象)
--- 除去已经显示声明的字段外，其他配置信息亦会被对象化到该实例
---
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------

---@class LScriptRuntimeInstance
local LScriptRuntimeInstance = class("LScriptRuntimeInstance")

---@type table @实例运行环境变量表
LScriptRuntimeInstance.Blackboard = nil

---@type table @事件变量表
LScriptRuntimeInstance.Parameters = nil

---@type AsyncState @实例运行状态
LScriptRuntimeInstance.RunState = nil

---@type ScriptSemaphore @驱动控制
LScriptRuntimeInstance.Semaphore = nil

---@type number @运行id
LScriptRuntimeInstance.GUID = nil

function LScriptRuntimeInstance:ctor()
    self.RunState = AsyncState.New()
end

--- 当前是否正在运行
---@return boolean
function LScriptRuntimeInstance:IsRunning()
    return not self.RunState:IsDone()
end

--- 当前是否运行结束
---@return boolean
function LScriptRuntimeInstance:IsDone()
    return self.RunState:IsDone()
end

--- 关闭实例
function LScriptRuntimeInstance:Shutdown()
    self.RunState:End()
end

--- 启动实例
---@param id number @运行实例ID
---@param params table @运行参数
---@param state AsyncState @运行状态
---@param semaphore ScriptSemaphore @执行控制
function LScriptRuntimeInstance:Startup(id,params,runState,semaphore)
    self.RunState = runState
    self.GUID = id
    self.Semaphore = semaphore
    self.Parameters = params or {}
end


return LScriptRuntimeInstance