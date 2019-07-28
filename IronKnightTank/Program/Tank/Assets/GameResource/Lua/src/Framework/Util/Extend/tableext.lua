--[[
@author Ben
Copyright(c) 2015-2016 ShangHai Overture Network Technoloty Co., Ltd, All rights reserved.

table的扩展

版本:
]]

local ipairs = ipairs
local pairs = pairs
local type = type
local setmetatable = setmetatable
local mrandom = math.random
local mfloor = math.floor
local tremove = table.remove
local tinsert = table.insert

table.empty = {}

function table.tostring(t)
    return tostring(t)
end

function table.readOnly(tb)
    local t = getmetatable(tb) or {}
    t.__newindex = function()
        error("this table is read only.")
    end
    setmetatable(tb,t)
end


---根据值来删除table，用于是索引下标的table
--@param @class table
--@param 删除的值
--@param true 删除全部=item的值 @class bool
function table.removeValue(list, item, removeAll)
    local rmCount = 0
    for i = 1, #list do
        if list[i - rmCount] == item then
            tremove(list, i - rmCount)
            if removeAll then
                rmCount = rmCount + 1
            else
                return true
            end
        end
    end
    return rmCount ~= 0
end

---查找匹配的内容
--@param @class table
--@param 判断方法（bool actionBool(v)）
function table.find(list, actionBool)
    for _, v in pairs(list) do
        if actionBool(v) then
            return v
        end
    end
    return nil
end

--- 查找元素在列表当中的位序
-- @param list
-- @param element
-- @return
function table.findIndex(list, element)
    for i, v in ipairs(list) do
        if v == element then
            return i
        end
    end
    return -1
end

---获取tabel的长度，不是连续下标方式时使用
--@param table
function table.count(tb)
    local cnt = 0
    for _, _ in pairs(tb) do
        cnt = cnt + 1
    end
    return cnt
end

---是否包含数据
--@return true 包含 @class bool
function table.contains(tb, element)
    if tb == nil then
        return false
    end
    for _, value in pairs(tb) do
        if value == element then
            return true
        end
    end
    return false
end

---乱序table排列，仅限于连续下标的使用
function table.shuffle(tb)
    local iter = #tb
    if iter == 0 then return end
    local j
    for i = iter, 2, -1 do
        j = mrandom(i)
        tb[i], tb[j] = tb[j], tb[i]
    end
end

---清空table，用于是索引下标的table
function table.clear(tb)
    for i = #tb, 1, -1 do
        tremove(tb, i)
    end
end

---清空table，用于不是连续下标的table
function table.clearToNil(tb)
    for k, _ in pairs(tb) do
        tb[k] = nil
    end
end

---new个新表，把所有的值重新创建一边（就不用在初始化里重新写一遍赋值，偷懒）
function table.setmetatable(target, src)
    setmetatable(target, src)
    for key, value in pairs(src) do
        local t = type(value)
        if t ~= 'function' and key ~= '__index' then
            if t == 'table' then
                target[key] = {}
            else
                target[key] = value
            end
        end
    end
end

---复制一份到新的table中
-- @param tb 要复制出来的table
-- @param new 新table，如果不为nil，则用此参数的table
-- @return @class table
function table.copy(tb, new)
    if not tb then return new end
    new = new or {}
    for key, value in pairs(tb) do
        new[key] = value
    end
    return new
end

---复制一份到新的table中，下标连续的方式
-- @param tb
-- @param start 开始位置 不填默认从第一个开始
-- @param ends 结束位置 不填默认原始table长度
-- @return
function table.icopy(tb, start, ends)
    local new = {}
    if not start then start = 1 end
    if not ends then ends = #tb end
    for i = start, ends do
        tinsert(new, tb[i])
    end
    return new
end

---value是list的table，把所有的list都复制到一个列表中
-- @param tb
-- @return
function table.copyvaluelist(tb)
    local new = {}
    for _, values in pairs(tb) do
        for _, v in pairs(values) do
            tinsert(new, v)
        end
    end
    return new
end

---反转table排序
-- @param tb
function table.reverse(tb)
    local llen = #tb
    for i = 1, mfloor(llen / 2) do
        local r = llen - i + 1
        tb[i], tb[r] = tb[r], tb[i]
    end
    return tb
end

---把一个table列表加入到另一个里面
-- @param src
-- @param target
-- @return
function table.addrange(src, target)
    for _, t in pairs(target) do
        tinsert(src, t)
    end
end

function table.iaddrange(src, target)
    for _, t in ipairs(target) do
        tinsert(src, t)
    end
end

function table.create()
    local t = {}
    ReferenceCounter.MarkWithTraceback("{}",t,1)
    return t
end

---只复制值，不复制方法和metatable
function cloneData(object)
    local lookup_table = {}
    local function _copy(o)
        if type(o) ~= "table" then
            return o
        elseif lookup_table[o] then
            return lookup_table[o]
        end
        local new_table = {}
        lookup_table[o] = new_table
        for key, value in pairs(o) do
            if type(value) ~= "function" and type(key) ~= "function" then
                new_table[_copy(key)] = _copy(value)
            end
        end
        return new_table
    end
    local copyTab = _copy(object)
    lookup_table = nil
    _copy = nil
    ReferenceCounter.MarkWithTraceback("cloneData", copyTab)
    return copyTab
end

---复制方法
function table.copyFunction(source,target)
    for k,v in pairs(source) do
        if type(v) == "function" then
            target[k] = v
        end
    end
    return target
end

---把table序列化成string
-- @param obj
-- @return
function table.serialize(obj)
    if obj == nil then return end
    local lua = {}
    local t = type(obj)
    if t == "number" then
        tinsert(lua, obj)
    elseif t == "boolean" then
        tinsert(lua, tostring(obj))
    elseif t == "string" then
        tinsert(lua, string.format("%q", obj))
    elseif t == "table" then
        tinsert(lua, "{")
        for k, v in pairs(obj) do
            tinsert(lua, "[" .. table.serialize(k) .. "]=" .. table.serialize(v) .. ",")
        end
        local metatable = getmetatable(obj)
        if metatable ~= nil and type(metatable.__index) == "table" then
            for k, v in pairs(metatable.__index) do
                tinsert(lua, "[" .. table.serialize(k) .. "]=" .. table.serialize(v) .. ",")
            end
        end
        tinsert(lua, "}")
    elseif t == "nil" then
        return nil
    else
        error("can not serialize a " .. t .. " type.")
    end
    return table.concat(lua)
end

---反序列化
-- @param lua
-- @return
function table.deserialize(lua)
    if lua == nil then return end
    local t = type(lua)
    if t == 'table' then
        return lua
    elseif t == "nil" or lua == "" then
        return nil
    elseif t == "number" or t == "string" or t == "boolean" then
        lua = tostring(lua)
    else
        error("can not deserialize a " .. t .. " type.")
    end
    lua = "return " .. lua
    local func = loadstring(lua)
    if func == nil then return nil end
    return table.deserialize(func())
end

---复制所有字段和方法
function table.copyKV(from,to)
    for k,v in pairs(from) do
        to[k] = v
    end
end

---list转字典
function table.listToDic(list,key)
    local dic = {}
    for i,v in ipairs(list) do
        dic[v[key]] = v
    end
    return dic
end

function table.getFirstKey(enumerator)
    if enumerator[1] then
        return 1
    end
    for k,v in pairs(enumerator) do
        return k
    end
end

function table.getFirstValue(enumerator)
    if enumerator[1] then
        return enumerator[1]
    end
    for k,v in pairs(enumerator) do
        return v
    end
end

function table.unpack(tb)
    return unpack(tb)
end

function table.unpacklist(list, length)
    if length == 0 then
        return nil
    elseif length == 1 then
        return list[1]
    elseif length == 2 then
        return list[1], list[2]
    elseif length == 3 then
        return list[1], list[2], list[3]
    elseif length == 4 then
        return list[1], list[2], list[3], list[4]
    elseif length == 5 then
        return list[1], list[2], list[3], list[4], list[5]
    elseif length == 6 then
        return list[1], list[2], list[3], list[4], list[5], list[6]
    elseif length == 7 then
        return list[1], list[2], list[3], list[4], list[5], list[6], list[7]
    elseif length == 8 then
        return list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8]
    elseif length == 9 then
        return list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8], list[9]
    elseif length == 10 then
        return list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8], list[9], list[10]
    elseif length == 11 then
        return list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8], list[9], list[10], list[11]
    elseif length == 12 then
        return list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8], list[9], list[10], list[11], list[12]
    elseif length == 13 then
        return list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8], list[9], list[10], list[11], list[12], list[13]
    elseif length == 14 then
        return list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8], list[9], list[10], list[11], list[12], list[13], list[14]
    elseif length == 15 then
        return list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8], list[9], list[10], list[11], list[12], list[13], list[14], list[15]
    end
end



table.readOnly(table.empty)