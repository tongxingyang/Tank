---------------------------------------------
--- DigitArithmetic
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------

---@style [Left][Operator][Right]
---@path 数学计算/运算
---@description 运算两个数字
---@vartype number
---@variable DigitArithmetic
---@class DigitArithmetic : ScriptVariable
local DigitArithmetic = class("DigitArithmetic",require("Framework.LetsTrigger.Command.ScriptVariable"))

---@description 左值
---@type number
---@parameter Left
DigitArithmetic.Left = nil

---@description 关系符号
---@default +
---@type string
---@enum +,-,×,÷
---@parameter Operator
DigitArithmetic.Operator = nil


---@description 右值
---@type number
---@parameter Right
DigitArithmetic.Right = nil

function DigitArithmetic:Execute()
    return DigitArithmetic.Operation(self.Left,self.Operator,self.Right)
end

function DigitArithmetic.Operation(l,r,op)
    if op == "+" then
        return l + r
    elseif op == "-" then
        return l - r
    elseif op == "×" then
        return l * r
    elseif op == "÷" then
        return l / r
    end
end

return DigitArithmetic