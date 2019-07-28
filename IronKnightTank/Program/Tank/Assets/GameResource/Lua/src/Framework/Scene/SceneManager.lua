---------------------------------------------
--- SceneManager 情景模块
--- Created by Tianc.
--- DateTime: 2018/04/15
---------------------------------------------
---@class SceneManager
local SceneManager = {}

---@type Scene
SceneManager.CurrentScene = nil

local loadingCount
local loadingActors
local onProgressCallback
local emptyTable = {}
local zeroTable = { Progress = 0 }

---@type Scene
local toScene

--- 切换情景
---@param scene Scene
---@param onProgress fun(progress:number) @进度回调
function SceneManager.SwicthScene(scene, onProgress)
    toScene = scene
    coroutine.createAndRun(SceneManager._swicthScene, onProgress)
end

function SceneManager._swicthScene(onProgress)
    local tasks = {}

    if SceneManager.CurrentScene ~= nil then
        table.insert(tasks, SceneManager._leaveScene)
        if SceneManager.CurrentScene.Name ~= toScene.Name then
            table.insert(tasks, SceneManager._loadUnityScene)
        end
    else
        SceneManager.CurrentScene = toScene
        toScene = nil
    end
    table.insert(tasks, SceneManager._startup)
    table.insert(tasks, SceneManager._prepareScene)
    table.insert(tasks, SceneManager._intoScene)

    for i, task in ipairs(tasks) do
        xpcall(SceneManager._waitTask, logException, task, i, #tasks, onProgress)
    end
end

function SceneManager._waitTask(task, taskIndex, taskCount, onProgress)
    local co = coroutine.running()
    task(function(progress)
        onProgress((progress + taskIndex - 1) / taskCount)
        if progress == 1 then
            coroutine.resume(co)
        end
    end)
    return coroutine.yield()
end

function SceneManager._startup(onProgress)
    SceneManager._load(onProgress, "Startup", function(actor)
        return not actor.__started
    end, function(actor)
        actor.__started = true
    end)
end

function SceneManager._leaveScene(onProgress)
    SceneManager._load(onProgress, "OnLeaveScene")
end

function SceneManager._intoScene(onProgress)
    SceneManager._load(onProgress, "OnIntoScene")
end

function SceneManager._prepareScene(onProgress)
    SceneManager._load(onProgress, "OnPrepareScene")
end

function SceneManager._loadUnityScene(onProgress)
    UnitySceneManager.SwitchScene(toScene.Name, function()
        SceneManager.CurrentScene = toScene
        toScene = nil
        onProgress(1)
    end)
end

---并行执行
function SceneManager._load(onProgress, entryName, before, after)
    local actors = SceneManager.CurrentScene.Actors
    onProgressCallback = onProgress
    loadingCount = 0
    loadingActors = {}

    for i, actor in ipairs(actors) do
        if actor[entryName] ~= nil and (before == nil or before(actor, entryName)) then
            loadingCount = loadingCount + 1
            loadingActors[actor] = zeroTable
        end
    end

    for i, actor in ipairs(actors) do
        if loadingActors[actor] ~= nil then
            local actorsProgress = actor[entryName]()
            if actorsProgress then
                actorsProgress.__actorName = actor.__cname
                actorsProgress.__actor = actor
                loadingActors[actor] = actorsProgress
            else
                loadingActors[actor] = emptyTable
            end
            if after then
                after(actor, entryName)
            end
        end
    end

    SceneManager.Progress = 0
    repeatCall(SceneManager._loadingLoop, 0)
end

function SceneManager._loadingLoop()
    local totalProgress = 0
    for actor, progress in pairs(loadingActors) do
        totalProgress = ((progress.Progress == nil or progress.Progress > 1) and 1 or progress.Progress) + totalProgress
    end
    SceneManager.Progress = loadingCount ~= 0 and totalProgress / loadingCount or 1

    local onProgress = onProgressCallback
    if SceneManager.Progress >= 1 then
        onProgressCallback = nil
        loadingActors = nil
        stopRepeatCall(SceneManager._loadingLoop)
    end

    if onProgress ~= nil then
        onProgress(SceneManager.Progress)
    end
end

return SceneManager