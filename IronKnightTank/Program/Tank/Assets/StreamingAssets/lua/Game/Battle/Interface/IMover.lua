---------------------------------------------
--- IMover
--- Created by thrt520.
--- DateTime: 2018/6/7
---mover 接口  只是一个模板  没有实际作用主要用于MapModel中计算
---------------------------------------------
---@class IMover : IUnit
local IMover = {}

---@type GridPos
IMover.Pos = nil

---@return boolean
function IMover:CanPassTerrian(terriantType)

end

---@return number
function IMover:GetMoveSpeed()

end

---@return number
---@param blockData BlockData
function IMover:GetMoveCost(blockData)

end

---@return boolean
function IMover:IsIsInflectByTerrian(terrianType)

end

---@param mover IMover
function IMover:IsBlockMove(mover)

end

---@param mover IMover
function IMover:IsBlockAtk(mover)

end

---@return boolean
function IMover:GetPower()

end

return IMover