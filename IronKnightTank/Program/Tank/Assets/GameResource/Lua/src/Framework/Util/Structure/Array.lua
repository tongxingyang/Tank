---@classdef Array

Array = {}
Array.__index=Array

function Array.New()
    local o = { Count = 0 }
    
    ReferenceCounter.MarkWithTraceback("Array", o)
    setmetatable(o,Array)
    return o
end

function Array:Size()
    return self.Count
end

function Array:Add(element)
    self[self.Count + 1]=element
    self.Count=self.Count+1
end

function Array:Insert(element,index)
    local count=self.Count
    if index > count+1 or index < 1 then
        return false
    end
    for i = count+1, index+1,-1 do
        self[i]=self[i-1]
    end
    self[index]=element
    self.Count=count+1
    return true
end

function Array:Remove(element)
    local index=self:FindIndex(element)
    return self:RemoveAt(index)
end
function Array:Reverse()
    local count=self.Count
    for i=1,count do
        local temp=self[i]
        self[i]=self[count-i+1]
        self[count-i+1]=temp
    end
end

function Array:RemoveAt(index)
    local count=self.Count
    if index > count or index < 1 then
        return false
    end
    for i = index, count-1 do
        self[i]=self[i+1]
    end
    
    self[count] = nil
    self.Count=count-1
    
    return true
end

function Array:Clear()
    for i = 1, self.Count do
        self[i]=nil
    end
    self.Count=0
end

function Array:ClearCnt()
    self.Count = 0
end

function Array:Find(condition)
    for i = 1, self.Count do
        local element=self[i]
        if condition(self[i]) then
            return element
        end
    end
    return nil
end

function Array:FindAll(condition)
    --- @class Array
    local list = Array.New()
    for i = 1, self.Count do
        local element=self[i]
        if condition(self[i]) then
            list:Add(element)
        end
    end
    return list
end

function Array:FindIndex(element)
    for i = 1, self.Count do
        if self[i] == element then
            return i
        end
    end
    return -1
end

function Array:Sort(comparison)
    if comparison==nil then
        table.sort(self)
    end
    table.sort(self,comparison)
end


function Array:Print()
    local str="size:" .. tostring(self:Size()) .. "  "
    for i = 1, self.Count do
        str = str .. tostring(i) .. ":" .. tostring(self[i]) .. ","
    end
    --print(str)
end

--for i = 1, 999999 do
--    local name="glbname_" .. tostring(i)
--    _G[name]="asd154dsfv9sd41f6vg4werthbe8r541ynbe4rt8yn4189rtyhn41r8h8ny4r8ty48nrt4y8j" .. name;
--end


--[[
local test = Array.New()

test:Add("a")
test:Add("b")
test:Add("c")
test:Add("d")

local function sortfc(l,r)
	--print(l)
	--print(r)
	return l=="c"
end
test:Sort(sortfc)
test:Print()
--print(test["sss"])

test:Insert("x",3)
test:Print()
--print(test[3])
]]--