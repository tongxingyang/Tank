---@classdef ExpressionTool
local ExpressionTool = {}

function ExpressionTool.Relation(relation,leftValue,rightValue)
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



return ExpressionTool

