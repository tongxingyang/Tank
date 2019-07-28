---------------------------------------------
--- ParallelActions
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------

---@style 同时执行以下动作
---@style --[Actions]
---@path 流程控制/并行执行
---@description 同时执行一组动作
---@action ParallelActions
---@class ParallelActions : ScriptAction
local ParallelActions = class("ParallelActions",require("Framework.LetsTrigger.Command.ScriptAction"))
local LScriptUtil = require("Framework.LetsTrigger.Util.LScriptUtil")

---@description 并行动作组
---@type ScriptAction[]
---@parameter Actions
ParallelActions.Actions = nil

function ParallelActions:OnExecute()
    LScriptUtil.RunActionsAtSameTime(self.Actions)
    self:End()
end

return ParallelActions