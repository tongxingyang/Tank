---------------------------------------------
--- IfElse
--- Created by Tianc.
--- DateTime: 2018/04/26
---------------------------------------------

---@style 如果[Condition]成立则执行
---@style --[True]
---@style 否则
---@style --[False]
---@path 流程控制/条件分支
---@description 根据指定条件是否成立，执行不同的脚本分支
---@action IfElse
---@class IfElse : ScriptAction
local IfElse = class("IfElse",require("Framework.LetsTrigger.Command.ScriptAction"))
local LScriptUtil = require("Framework.LetsTrigger.Util.LScriptUtil")

---@description 成立
---@type ScriptAction[]
---@parameter True
IfElse.True = nil

---@description 不成立
---@type ScriptAction[]
---@parameter False
IfElse.False = nil

---@description 条件
---@type boolean
---@parameter Condition
IfElse.Condition = nil

function IfElse:OnExecute()
    local actions = self.Condition and self.True or self.False
    LScriptUtil.RunActionsInSequence(actions)
    self:End()
end

return IfElse