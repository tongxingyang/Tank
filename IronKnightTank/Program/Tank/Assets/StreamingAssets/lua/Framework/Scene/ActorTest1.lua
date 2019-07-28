---------------------------------------------
--- ActorTest1
--- Created by thrt520.
--- DateTime: 2018/5/19
---------------------------------------------
---@class ActorTest1
local ActorTest1 = {}


--- 在模块第一次被加载时执行
---@return Progress|nil
function ActorTest1.Startup()
	ActorTest1.Progress = 0
	clog("start up ActorTest1.Progress = 0")
	coroutine.createAndRun(function ()
		coroutine.wait(1)
		ActorTest1.Progress = 1
		clog("start up ActorTest1.Progress = 1")
	end)
end

--- 在进入情景前执行
function ActorTest1.OnPrepareScene()end

--- 在进入情景时执行
function ActorTest1.OnIntoScene()end

--- 在离开情景时执行
function ActorTest1.OnLeaveScene()end


return ActorTest1