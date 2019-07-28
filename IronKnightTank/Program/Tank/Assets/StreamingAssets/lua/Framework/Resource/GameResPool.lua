---------------------------------------------
--- GameResPool
--- Created by thrt520.
--- DateTime: 2018/7/12
---------------------------------------------
---@class GameResPool
local GameResPool = {}

---@type table<string , ObjectPool>
local cachedResPool


function GameResPool.Init()
	cachedResPool = {}
end

--- 获取一个受对象池管理的游戏资源（使用完后调用ReturnPoolRes归还）
---@param resPath string @资源路径
---@param onComplete fun(obj:object) @创建成功回调
function GameResPool.GetRes(resPath , onComplete)
	local pool = GameResPool._getResPool(resPath , onComplete)
	if pool:HasAvailableObject() then
		local obj = pool:Create()
		onComplete(obj)
		return obj
	end
	return pool:Create(onComplete)
end

--- 获取一个受对象池管理的游戏资源（使用完后调用ReturnPoolRes归还）
------@param co coroutine
---@param resPath string @资源路径
---@return object
function GameResPool.GetResYield(co , resPath)
	return coroutine.callwait(co , GameResPool.GetRes , 2 , resPath)
end

---@param resPath string
---@param resPath string
function GameResPool._getResPool(resPath , onComplete)
	local pool = cachedResPool[resPath]
	if not pool then
		pool = ObjectPool.New(100 , function (onComplete)
			local res = ResMgr.LoadObject(resPath , onComplete)
			return res
		end , nil , function (o)
			Object.Destory(o)
		end)
		cachedResPool[resPath] = pool
	end
	return pool
end


--- 归还res到对象池
---@param resPath string
---@param res object
function GameResPool.ReturnPoolRes(resPath , res)
	if not cachedResPool then return end
	local pool = cachedResPool[resPath]
	if not pool then
		Object.Destory(res)
	else
		pool:Return(res)
	end
end



return GameResPool