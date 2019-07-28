--[[
@author Ben
Copyright(c) 2015-2016 ShangHai Overture Network Technoloty Co., Ltd, All rights reserved.

先进先出

版本: 2016-02-23
]]
---@class Queue
Queue = {}
Queue.__index = Queue

---@return Queue
function Queue.New()
    local o = { Count = 0, InIdx = 0, OutIdx = 1 }
    setmetatable(o,Queue)
    return o
end

function Queue:Size()
    return self.Count
end

function Queue:Offer(element)
    self.Count = self.Count + 1
    self.InIdx = self.InIdx + 1
    self[self.InIdx] = element
end

function Queue:Poll()
    local count = self.Count
    if count < 1 then
        self.Count = 0
        self.InIdx = 0
        self.OutIdx = 1
        return nil
    end
    local eml = self[self.OutIdx]
    self[self.OutIdx] = nil
    self.Count = count - 1
    self.OutIdx = self.OutIdx + 1
    return eml
end

function Queue:Peek()
    if self.Count < 1 then
        return nil
    else
        return self[self.OutIdx]
    end
end

function Queue:Clear()
    for i = 1, self.Count do
        self[i] = nil
    end
    self.Count = 0
    self.InIdx = 0
    self.OutIdx = 1
end

function Queue:Print()
    local str="size:" .. tostring(self:Size()) .. "  "
    for i = 1, self.Count do
        str = str .. tostring(i) .. ":" .. tostring(self[i]) .. ","
    end
    print(str)
end