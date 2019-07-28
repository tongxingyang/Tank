---------------------------------------------
--- LScript
--- 可编程命令脚本
---
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------

---@class LScript
local LScript = class("LScript")
local ScriptCommandFactory = require("Framework.LetsTrigger.Command.ScriptCommandFactory")
local LScriptRuntimeInstance = require("Framework.LetsTrigger.Script.LScriptRuntimeInstance")
local AsyncState = require("Framework.Util.Structure.AsyncState")

---@type number @每次触发脚本运行都会分配一个自增的guid
local guid = 0

---@type table @用与脚本环境下的变量定义
LScript.Blackboard = nil

---@type string[] @脚本执行触发点
LScript.TriggerPoint = nil

---@type string[] @脚本环境依赖的字段
LScript.ContentFields = nil

---@type ObjectPool @实例池
---@private
LScript._instancePool = nil

function LScript:Run(params,id) end

--- 运行脚本
---@param params @运行参数
function LScript:RunScript(params)
    local id = self:_newGUID()
    local state = self:Run(params,id)
    return state or AsyncState.DoneState
end


--- 绑定到事件域，并开始等待触发
---@param eventDomain EventDomain
function LScript:Binding(eventDomain)
    self.eventDomain = eventDomain
    self.triggerListener = function(msg)
        self:RunScript(msg)
    end
    for i, v in ipairs(self.TriggerPoint) do
        eventDomain:AddListener(v,self.triggerListener)
    end
end

--- 停止等待触发
function LScript:Unbinding()
    if self.triggerListener ~= nil then
        for i, v in ipairs(self.TriggerPoint) do
            self.eventDomain:RemoveListener(v,self.triggerListener)
        end
    end
    self.triggerListener = nil
end

--- 销毁
function LScript:Dispose()
    self:Unbinding()
    if self._instancePool ~= nil then
        self._instancePool:Clear()
    end
    self._instancePool = nil
end

--- 创建脚本运行实例
---@param id number @运行实例ID
---@param params table @运行参数
---@param state AsyncState @运行状态
---@param semaphore ScriptSemaphore @执行控制
---@return LScriptRuntimeInstance
function LScript:StartupAnInstance(id,params,state,semaphore)
    if self._instancePool == nil then
        self._instancePool = ObjectPool.New(10,function()
            local runtimeInstance = LScriptRuntimeInstance.new()
            for i, fieldName in ipairs(self.ContentFields) do
                local config = self[fieldName]
                runtimeInstance[fieldName] = self:BuildContent(runtimeInstance,config)
            end
            return runtimeInstance
        end)
    end
    ---@type LScriptRuntimeInstance
    local instance = self._instancePool:Create()
    instance:Startup(id,params,state,semaphore)
    return instance
end

--- 归还运行实例到池
---@param runtimeInstance LScriptRuntimeInstance
function LScript:ShutdownAnInstance(runtimeInstance)
    runtimeInstance:Shutdown()
    if self._instancePool ~= nil then
        self._instancePool:Return(runtimeInstance)
    end
end

--- 创建运行环境
---@param runtimeInstance LScriptRuntimeInstance
---@param config table
function LScript:BuildContent(runtimeInstance,config)
    if config ~= nil and type(config) == "table" then
        local content
        if ScriptCommandFactory.IsCommand(config) then
            content = ScriptCommandFactory.GetCommand(runtimeInstance, self, config)
        else
            content = {}
        end
        self:_buildChildrenContent(runtimeInstance,content,config)
        return content
    end
    return config
end

--- 递归实例化所有配置到运行状态
--- 通过metatable重载ScriptCommand上的Variable字段读取，使得能够直接执行运算并获得值
---@param scriptRuntime LScriptRuntimeInstance
function LScript:_buildChildrenContent(scriptRuntime, runtimeContent, config)
    local variableProxy
    for k, v in pairs(config) do
        local childContent = self:BuildContent(scriptRuntime,v)
        if ScriptCommandFactory.IsVariable(childContent) then
            if variableProxy == nil then
                variableProxy = {}
            end
            variableProxy[k] = childContent
        else
            runtimeContent[k] = childContent
        end
    end
    runtimeContent.__variableProxy = variableProxy

    local cmt = setmetatable({},getmetatable(runtimeContent))
    setmetatable(runtimeContent,{ __index = function(t, k)
        --clog2(22,"k",k)
        --clog2(22,"config",config)
        local value = cmt[k]
        if value then
            --clog2(22,"v1",value)
            return value
        end

        if variableProxy then
            local variableCommand = variableProxy[k]
            if variableCommand then
                value = variableCommand:Execute()
                --clog2(22,"v2",value)
                return value
            end
        end

        --clog2(22,"v5",value)
        return value
    end})
end

--- 获取一个GUID,为每一次运行
---@return number
function LScript:_newGUID()
    guid = guid + 1
    return guid
end

return LScript