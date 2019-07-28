---------------------------------------------
--- Compare
--- Created by Tianc.
--- DateTime: 2018/04/27
---------------------------------------------

---@class Compare
local Compare = class("Compare")

---@type any @左值
Compare.Left = nil

---@type any @右值
Compare.Right = nil

---@type string @关系符号
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