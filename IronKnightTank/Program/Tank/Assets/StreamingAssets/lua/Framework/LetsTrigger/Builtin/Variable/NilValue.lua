---------------------------------------------
--- NilValue
--- Created by Tianc.
--- DateTime: 2018/04/26
---------------------------------------------

---@style 不存在
---@path 常规/不存在
---@description 一个代表不存在的值
---@vartype nil
---@variable NilValue
---@class NilValue : ScriptVariable
local NilValue = class("NilValue")

function NilValue:Execute()
    return nil
end

return NilValue