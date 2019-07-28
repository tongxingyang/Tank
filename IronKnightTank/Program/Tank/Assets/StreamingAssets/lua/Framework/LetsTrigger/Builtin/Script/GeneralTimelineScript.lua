---------------------------------------------
--- GeneralTimelineScript
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------
---@class GeneralTimelineScript : TimelineBasedScript
local GeneralTimelineScript = class("GeneralTimelineScript",require("Framework.LetsTrigger.Timeline.TimelineBasedScript"))
local LScriptUtil = require("Framework.LetsTrigger.Util.LScriptUtil")

--------------------------------------------------------
--- 需要重载定义的字段
--------------------------------------------------------

---@type table @用与脚本环境下的变量定义
GeneralTimelineScript.Blackboard = nil

---@type string[] @脚本执行触发点
GeneralTimelineScript.TriggerPoint = nil

---@type Variable[] @条件
GeneralTimelineScript.ConditionList = nil

---@type ScriptAction[] @行为
GeneralTimelineScript.ActionList = nil

--------------------------------------------------------
--- 内部
--------------------------------------------------------

GeneralTimelineScript.ContentFields = {"ConditionList","ActionList","Blackboard"}

---@param instance LScriptRuntimeInstance
function GeneralTimelineScript:OnRun(instance)
    if LScriptUtil.CalculateAnd(instance.ConditionList) then
        LScriptUtil.RunActionsInSequence(instance.ActionList)
    end
end

return GeneralTimelineScript