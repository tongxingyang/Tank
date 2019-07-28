---------------------------------------------
--- ActorTest2
--- Created by thrt520.
--- DateTime: 2018/5/19
---------------------------------------------
---@class ActorTest2
local ActorTest2 = {}


--- 在模块第一次被加载时执行
---@return Progress|nil
function ActorTest2.Startup()
	ActorTest2.Progress = 0
	clog("start up ActorTest2.Progress = 0")
	coroutine.createAndRun(function ()
		coroutine.wait(1)
		ActorTest2.Progress = 1
		clog("start up ActorTest2.Progress = 1")
	end)
end

--- 在进入情景前执行
function ActorTest2.OnPrepareScene()end

--- 在进入情景时执行
function ActorTest2.OnIntoScene()end

--- 在离开情景时执行
function ActorTest2.OnLeaveScene()end


return ActorTest2