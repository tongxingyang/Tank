---@class Stack
Stack = {}
Stack.__index=Stack

function Stack.New()
    local o = { Count = 0 }
    ReferenceCounter.MarkWithTraceback("Stack", o)
    setmetatable(o,Stack)
    return o
end

function Stack:Size()
    return self.Count
end

function Stack:Push(element)
    self.Count = self.Count+1
    self[self.Count]=element
end

function Stack:Pop()
    local count = self.Count
    if count < 1 then
        return nil
    end
    local eml = self[count]
    self[count] = nil
    self.Count = count - 1
    return eml
end

function Stack:Peek()
    return self[self.Count]
end

function Stack:Clear()
    for i = 1, self.Count do
        self[i] = nil
    end
    self.Count = 0
end

function Stack:Print()
    local str="size:" .. tostring(self:Size()) .. "  "
    for i = 1, self.Count do
        str = str .. tostring(i) .. ":" .. tostring(self[i]) .. ","
    end
    print(str)
end

--[[
local test = Stack.New()

test:Push("a")
test:Push("b")
test:Push("c")
test:Push("d")

test:Print()

--print(test:Pop())
--print(test:Pop())
--print(test:Pop())
--print(test:Pop())
--print(test:Pop())

test:Print()

test:Push("a")
test:Push("b")
test:Push("c")
test:Push("d")
test:Clear()
test:Print()
]]--