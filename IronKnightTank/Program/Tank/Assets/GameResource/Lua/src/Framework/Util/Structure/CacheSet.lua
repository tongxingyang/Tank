---@classdef CacheSet
local CacheSet = {}

local tinsert = table.insert

CacheSet.__index = CacheSet
function CacheSet.New(size)
    local o = {}
    o._lfuStep = 0
    o._set = {}
    o._lfu = {}
    o._count = 0
    o._size = size
    o._overflow = math.floor(size*0.2)
    o._maxCount = o._size + o._overflow
    setmetatable(o,CacheSet)
    return o
end

---@class dic<key,value>
CacheSet._set = nil
---@class dic<key,lfuStep>
CacheSet._lfu = nil
---@class number
CacheSet._count = nil

---@class number
CacheSet._lfuStep = nil
---@class number
CacheSet._size = nil
---@class number
CacheSet._overflow = nil
---@class number
CacheSet._maxCount = nil


function CacheSet:Add(key,value)
    self._lfuStep = self._lfuStep + 1
    self._set[key] = value
    self._lfu[key] = self._lfuStep
    self._count = self._count + 1
    
    if self._count > self._maxCount then
        return self:_release()
    end
end

function CacheSet:Delete(key)
    self._set[key] = nil
    self._lfu[key] = nil
    self._count = self._count - 1
end

function CacheSet:Get(key)
    if self._set[key] == nil then
        return nil
    end
    self._lfuStep = self._lfuStep + 1
    self._lfu[key] = self._lfuStep
    return self._set[key]
end

function CacheSet:Has(key)
    return self._set[key] ~= nil
end

function CacheSet:GetSet()
    return self._set
end

function CacheSet:Clear()
    self._set = {}
    self._lfu = {}
    self._count = 0
end

function CacheSet:_release()
    local release = {}
    local releaseSteps = {}
    local releasevk = {}
    local overflow = self._overflow + 1
    
    for k,v in pairs(self._lfu) do
        releasevk[v] = k
        tinsert(releaseSteps,v)
    end
    table.sort(releaseSteps)
    
    for i = 1, overflow do
        local step = releaseSteps[i]
        local k = releasevk[step]
        release[k] = self._lfu[k]
    end
    
    for k,v in pairs(release) do
        self._set[k] = nil
        self._lfu[k] = nil
        self._count = self._size
    end
    
    return release
end

return CacheSet