---------------------------------------------
--- BaseBuff
--- Created by thrt520.
--- DateTime: 2018/8/10
---------------------------------------------
---@class BaseBuff
local BaseBuff = class("BaseBuff")
BaseBuff.BuffId = 0
function BaseBuff:ctor()

end


function BaseBuff:OnStart()

end

---@return any
function BaseBuff:Run(type, val)
	return val
end

function BaseBuff:OnEnd()

end

---@param buff BaseBuff
function BaseBuff:Refresh(buff)

end

return BaseBuff