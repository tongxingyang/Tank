require("Framework.Util.Structure.Stack")

---@class ObjectPool
ObjectPool = {}
ObjectPool.__index=ObjectPool

--- 生成对象池
---@param maxCount number 对象池最大容量
---@param createFunc fun(...):any 生成对象的方法
---@param restoreFunc fun(obj:any) @optional 还原对象，当把对象归还时触发（如果不需要可以不填写）
---@param disposeFunc fun(obj:any) @optional 彻底销毁对象的方法（如果不用特殊销毁可以不填写）
---@return ObjectPool
function ObjectPool.New(maxCount,createFunc,restoreFunc,disposeFunc)
    local o =
    {
        CreateFunc = createFunc,
        DisposeFunc = disposeFunc,
        RestoreFunc = restoreFunc,
        MaxCount = maxCount,
        AvailableObjects = Stack.New()
    }
    ReferenceCounter.MarkWithTraceback("ObjectPool", o,2)
    setmetatable(o,ObjectPool)
    return o
end

---请求一个object 当空闲对象不足时，使用CreateFunc生成一个新的对象
---@param ... create的参数,最终会给CreateFunc使用
---@return 生成的对象
function ObjectPool:Create(...)
    if self.AvailableObjects.Count > 0 then
        return self.AvailableObjects:Pop();
    end
    return self.CreateFunc(...);
end

---判断当前是否有空闲对象
---@return bool
function ObjectPool:HasAvailableObject()
    return self.AvailableObjects.Count > 0
end

--- 归还不使用的object，使其变为空闲对象
---@param obj @class table 空闲下来的object
---@return @class bool 当归还数量大于pool的最大值是，返回false，并且调用DisposeFunc
function ObjectPool:Return(obj)
    if self.AvailableObjects.Count < self.MaxCount then
        if self.RestoreFunc ~= nil then
            self.RestoreFunc(obj)
        end
        self.AvailableObjects:Push(obj);
        return true;
    end
    if self.DisposeFunc ~= nil then
        self.DisposeFunc(obj)
    end
    return false;
end

---销毁对象池中现有的所有空闲对象
function ObjectPool:Clear()
    if self.DisposeFunc ~= nil then
        local disposeFunc=self.DisposeFunc
        for k,v in ipairs(self.AvailableObjects) do
            disposeFunc(v)
        end
    end
    self.AvailableObjects:Clear()
end

--[[
--print("--------------")
local function cfunc()
	return Stack.New()
end
local function dfunc(obj)
	--print("dispose" .. tostring(obj))
end

local test=ObjectPool.New(2,cfunc,dfunc)

local c1 = test:Create()
--print(c1)
local c2 = test:Create()
--print(c2)

test:Return(c1)
local c3 = test:Create()
--print(c3)

local c4 = test:Create()
--print(c4)

--print(test:Return(c2))
--print(test:Return(c3))
--print(test:Return(c4))
--print("--------------")
]]--