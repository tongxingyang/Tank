---------------------------------------------
--- LScriptInstance
--- 可编程命令脚本运行实例(从命令配置生成对应的一组运行对象)
--- 除去已经显示声明的字段外，其他配置信息亦会被对象化到该实例
---
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------

---@class LScriptInstance
local LScriptInstance = class("LScriptInstance")
local AsyncState = require("Framework.Util.Structure.AsyncState")

---@type table @实例运行环境变量表
LScriptInstance.Blackboard = nil

---@type table @事件变量表
LScriptInstance.Parameters = nil

---@type AsyncState @实例运行状态
LScriptInstance.RunState = nil

---@type ScriptDriver @驱动控制
LScriptInstance.TimelineDriver = nil

---@type number @运行id
LScriptInstance.GUID = nil

function LScriptInstance:ctor()
    self.RunState = AsyncState.New()
end

function LScriptInstance:IsRunning()
    return not self.RunState:IsDone()
end

function LScriptInstance:IsDone()
    return self.RunState:IsDone()
end

function LScriptInstance:Close()
    self.RunState:End()
end

function LScriptInstance:Start(params,runState,driver)
    self.RunState = runState
    self.TimelineDriver = driver
    self.Parameters = params or {}
end


return LScriptInstance