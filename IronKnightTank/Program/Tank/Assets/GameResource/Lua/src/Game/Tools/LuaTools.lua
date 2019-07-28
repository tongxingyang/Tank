---------------------------------------------
--- LuaTools
--- Created by thrt520.
--- DateTime: 2018/6/20
---------------------------------------------
local LuaTools = {}

CTools = Assets.Scripts.Game.LuaTools.LuaTools

---@param slider Slider
---@param val number
---@param time number
---@param onComplete nil|fun
function LuaTools.SliderTween(slider , val , time , onComplete)
	CTools.SliderTween(slider , val , time , onComplete)
end


---@param worldPos Vector3
---@param rectTran RectTransform
---@return Vector3
function LuaTools.TransferToWorldPosUIWorldPos(worldPos , rectTran)
	return CTools.LuaTransferWorldPos2UIWorldPos(worldPos , rectTran )
end

---@param worldPos Vector2
---@param rectTran RectTransform
---@return Vector3
function LuaTools.TransferToWorldPosUILocalPos(worldPos , rectTran)
	return CTools.LuaTransferWorldPos2UILocalPos(worldPos , rectTran )
end


function LuaTools.SetTranParent(tran , parent)
	CTools.SetTranParent(tran , parent)
end

function LuaTools.SetTranParentAndNormalized(tran , parent)
	CTools.SetTranParentAndNormalized(tran , parent)
end

---@param rectTran RectTransform
---@return Vector2
function LuaTools.TransferUIPosToScreenPos(rectTran)
	return CTools.LuaGetUIScreenPos(rectTran )
end

function LuaTools.DTSlidetTween(slider , val , time , onComplete , onUpdate)
	return CTools.LuaSliderDTTween(slider , val , time , onComplete , onUpdate)
end

function LuaTools.DTSpriteFade(sprite , val , time  , onComplete , onUpdate)
	return CTools.LuaSpriteDTFade(sprite , val , time , onComplete , onUpdate)
end


return LuaTools
