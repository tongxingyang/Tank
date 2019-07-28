---------------------------------------------
--- InfrastructureTask
--- Created by Tianc.
--- DateTime: 2018/04/16
---------------------------------------------

---@class InfrastructureTask

local InfrastructureTask = {}

InfrastructureTask.Weight = 5
---@type LuaModuleTaskProxy
InfrastructureTask.Proxy = nil

function InfrastructureTask.StartTask()
    LocalizationMgr.Init()
    InfrastructureTask.Proxy:SetProgress(1,"");
end

return InfrastructureTask