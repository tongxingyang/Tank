---@classdef HashTable

require("Framework.Util.Structure.Array")

HashTable = {}
HashTable.__index=HashTable

function HashTable.New()
    local o = {}
    ReferenceCounter.MarkWithTraceback("HashTable", o)
    setmetatable(o,HashTable)
    return o
end

function HashTable:Size()
    local count=0
    for k,v in pairs(self) do
        count=count+1
    end
    return count
end

function HashTable:Add(key,value)
    self[key]=value
end

function HashTable:Remove(key)
    if self[key] ~=nil then
        self[key]=nil
    end
end

function HashTable:Clear()
    for k,v in pairs(self) do
        self[k]=nil
    end
end

function HashTable:ContainsKey(key)
    return self[key] ~= nil
end

function HashTable:HasKey(key,value)
    return self[key] ~= nil
end

function HashTable:GetKeys()
    local keys=Array.New()
    for k,v in pairs(self) do
        keys:Add(k)
    end
    return keys
end

function HashTable:GetValues()
    local values=Array.New()
    for k,v in pairs(self) do
        values:Add(v)
    end
    return values
end

function HashTable:GetKeyValues()
    local kvs=Array.New()
    for k,v in pairs(self) do
        kvs:Add({Key=k,Value=v})
    end
    return kvs
end

function HashTable:Print()
    --print("count:" .. tostring(self:Size()))
    for key,value in pairs(self) do
        local str=tostring(key) .. ":" .. tostring(value)
        --print(str)
    end
end

--[[
--print("-----------------------------------")
local hash=HashTable.New()
hash:Add("aaa","aaavalue")
hash:Add("bbb","bbbvalue")
hash:Add("ccc","cccvalue")
--print(hash["aaa"])
--print("===")
hash:Print()
--print("===")
hash:Remove("aaa")
hash:Print()
--print("===")
hash:Clear()
hash:Print()
--print("===")
local key={aaa="aaa"}
hash:Add(key,"keyvalue")
hash:Print()
--print("-----------------------------------")
]]--