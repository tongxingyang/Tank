---------------------------------------------
--- SceneActor 情景模块
--- Created by Tianc.
--- DateTime: 2018/04/15
---------------------------------------------

---@class SceneActor
local SceneActor = {}

SceneActor.Property = 10

--- 在模块第一次被加载时执行
---@return Progress|nil
function SceneActor.Startup()end

--- 在进入情景前执行
function SceneActor.OnPrepareScene()end

--- 在进入情景时执行
function SceneActor.OnIntoScene()end

--- 在离开情景时执行
function SceneActor.OnLeaveScene()end


return SceneActor