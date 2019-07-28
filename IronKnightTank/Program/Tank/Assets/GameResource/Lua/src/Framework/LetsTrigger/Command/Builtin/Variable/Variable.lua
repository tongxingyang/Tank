---------------------------------------------
--- Variable
--- Created by Tianc.
--- DateTime: 2018/04/26
---------------------------------------------

---@class Variable
local Variable = class("Variable")

---@type string @变量名字
Variable.Name = nil

function Variable:Execute()
    return self.Script.Blackboard[self.Name] or self.Script.Parameters[self.Name]
end

return Variable