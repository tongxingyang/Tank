---------------------------------------------
--- Variable
--- Created by Tianc.
--- DateTime: 2018/04/26
---------------------------------------------

---@style 变量[Name]
---@path 常规/变量
---@description 运算两个数字
---@vartype any
---@variable Variable
---@class Variable : ScriptVariable
local Variable = class("Variable")

---@description 变量名字
---@type string
---@parameter Name
Variable.Name = nil

function Variable:Execute()
    local var = self.RuntimeInstance.Blackboard[self.Name]
    if var ~= nil then
        return var
    end
    return self.RuntimeInstance.Parameters[self.Name]
end

return Variable