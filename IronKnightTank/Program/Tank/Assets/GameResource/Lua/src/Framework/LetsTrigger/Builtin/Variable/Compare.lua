---------------------------------------------
--- Compare
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------

---@style [Left][Relation][Right]
---@path 关系/比较
---@description 比较两个指定值的关系
---@vartype boolean
---@variable Compare
---@class Compare : ScriptVariable
local Compare = class("Compare")

---@description 左值
---@type any
---@parameter Left
Compare.Left = nil

---@description 右值
---@type any
---@parameter Right
Compare.Right = nil

---@description 关系符号
---@default ==
---@type string
---@enum ==,~=,>,>=,<,<=
---@parameter Relation
Compare.Relation = nil

function Compare:Execute()
    return Compare.Relation(self.Relation,self.Left,self.Right)
end

function Compare.Relation(relation,leftValue,rightValue)
    if relation == "=" or relation == "==" then
        return leftValue == rightValue
    end
    if relation == "~=" then
        return leftValue ~= rightValue
    end
    if leftValue ~= nil and rightValue ~= nil then
        if relation == ">" then
            return leftValue > rightValue
        end
        if relation == ">=" then
            return leftValue >= rightValue
        end
        if relation == "<" then
            return leftValue < rightValue
        end
        if relation == "<=" then
            return leftValue <= rightValue
        end
    end
    return false
end

return Compare