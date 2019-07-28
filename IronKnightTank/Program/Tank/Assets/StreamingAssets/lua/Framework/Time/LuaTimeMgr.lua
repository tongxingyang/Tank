---------------------------------------------
--- LuaTimeMgr
--- Created by thrt520.
--- DateTime: 2018/5/31
---------------------------------------------
---@class LuaTimeMgr
local LuaTimeMgr = {}

local cTimeMgr = TankTimeManager

---@param timeScale number
function LuaTimeMgr.ChangeTimeScale(timeScale)
	cTimeMgr.ChangeTimeScale(timeScale)
end

function LuaTimeMgr.Pause()
	cTimeMgr.ChangeTimeScale(0)
end

function LuaTimeMgr.Resume()
	cTimeMgr.ChangeTimeScale(1)
end

return LuaTimeMgr