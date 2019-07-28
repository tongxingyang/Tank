---------------------------------------------
--- LScriptUtil
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------

---@class LScriptUtil
local LScriptUtil = {}


--- 或
---@param boolVariables ScriptVariable[]
---@return boolean
function LScriptUtil.CalculateOr(boolVariables)
    if boolVariables == nil then
        return true
    end
    local index = 1
    while true do
        local conditionValue = boolVariables[index]
        if conditionValue == nil then
            return index == 1
        end
        if conditionValue then
            return true
        end
        index = index + 1
    end
end

--- 与
---@param boolVariables ScriptVariable[]
---@return boolean
function LScriptUtil.CalculateAnd(boolVariables)
    if boolVariables == nil then
        return true
    end
    local index = 1
    while true do
        local conditionValue = boolVariables[index]
        if conditionValue == nil then
            return true
        end
        if conditionValue == false then
            return false
        end
        index = index + 1
    end
end

--- 依次执行动作
---@param actions ScriptAction[]
function LScriptUtil.RunActionsInSequence(actions)
    if actions == nil or #actions == 0 then
        return
    end
    local semaphore = actions[1].RuntimeInstance.Semaphore
    for i, v in ipairs(actions) do
        v:Execute():Yield()
        semaphore:AcquireExecuteAction()
    end
end

--- 依次执行动作
---@param actions ScriptAction[]
function LScriptUtil.RunActionsAtSameTime(actions)
    if actions == nil or #actions == 0 then
        return
    end
    local semaphore = actions[1].RuntimeInstance.Semaphore
    local states = {}
    for i, v in ipairs(actions) do
        actions[i] = v:Execute()
    end

    semaphore:AcquireExecuteAction()
    for i, v in ipairs(states) do
        v:Yield()
    end
end

return LScriptUtil