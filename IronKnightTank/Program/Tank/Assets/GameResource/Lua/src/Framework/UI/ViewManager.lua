---------------------------------------------
--- ViewManager
--- Created by Tianc.
--- DateTime: 2018/04/14
---------------------------------------------

---@class ViewManager
ViewManager = {}

require("Framework.UI.View")
require("Framework.Util.Structure.ObjectPool")
require("Framework.Resource.GameResMgr")

---@type Transform
local viewRoot

---@type Transform
local viewPoolRoot

---@type table<class,ObjectPool>
local cachedPoolViews

---@type table<class,View>
local singletonViews

---@type Transform
ViewManager.Root = nil

---@type table
ViewManager.Config = {
    RootName = "",
}

-----------------------------------------------------------------------------
--- view
-----------------------------------------------------------------------------


--- 创建一个view
---@param viewClass class @view模块
---@param onComplete fun(view:View) @创建成功回调
---@return View
function ViewManager.CreateView(viewClass,onComplete)
    return ViewManager._createView(viewClass,viewRoot,onComplete)
end

--- 创建一个view
---@param co coroutine @view模块
---@param viewClass class @view模块
---@return View
function ViewManager.CreateViewYield(co,viewClass)
    return coroutine.callwait(co,ViewManager.CreateView,2,viewClass)
end

--- 获取一个受对象池管理的View实例（使用完后调用ReturnPoolView归还）
---@param viewClass class @view模块
---@param onComplete fun(view:View) @创建成功回调
---@return View
function ViewManager.GetPoolView(viewClass,onComplete)
    ---@type ObjectPool
    local pool = ViewManager._getViewPool(viewClass)
    if pool:HasAvailableObject() then
        local view = pool:Create()
        onComplete(view)
        return view
    end
    return pool:Create(onComplete)
end


--- 获取一个受对象池管理的View实例（使用完后调用ReturnPoolView归还）
---@param co coroutine @view模块
---@param viewClass class @view模块
---@return View
function ViewManager.GetPoolViewYield(co,viewClass)
    return coroutine.callwait(co,ViewManager.GetPoolView,2,viewClass)
end

--- 归还View到对象池
---@param view View
function ViewManager.ReturnPoolView(view)
    if not cachedPoolViews then return end
    local viewClass = view.class
    local pool = cachedPoolViews[viewClass]
    if pool ~= nil then
        view.gameObject:SetActive(false)
        pool:Return(view)
    else
        Object.Destroy(view.gameObject)
    end
end

--- 绑定已经存在的GameObject对象到view
---@param gameObject GameObject 
---@param viewClass class
---@return View
function ViewManager.BindingView(gameObject,viewClass)
    local view = viewClass.new()
    ViewManager._bindingView(view,gameObject,nil,nil)
    return view
end

--- 创建一个view
---@param viewClass class @view模块
---@param root Transform @父节点
---@param onComplete fun(view:View) @创建成功回调
---@return View
---@private
function ViewManager._createView(viewClass,root,onComplete)
    ---@type View
    local view = viewClass.new()
    ResMgr.InstantiatePrefab(view.ViewPrefabPath,root ,function(prefab)
        ViewManager._bindingView(view,prefab,root,onComplete)
    end )
    return view
end

--- 绑定并初始化视图对象
---@private
function ViewManager._bindingView(view,gameObject,parent,onComplete)
    if parent ~= nil then
        GameObjectUtilities.SetTransformParentAndNormalization(gameObject.transform,parent)
    end

    --xpcall(view.Binding, logException,view, prefab)
    --xpcall(view.Init, logException,view)

    if view.ViewProxyClass ~= nil then
        view.ViewProxyClass:CreateViewProxy(view)
    end

    view:Binding(gameObject)
    view:Init()

    if onComplete then
        onComplete(view)
    end
end

---@private
function ViewManager._getViewPool(viewClass)
    local pool = cachedPoolViews[viewClass]

    if pool == nil then
        pool = ObjectPool.New(100,function(onComplete)
            local view = ViewManager._createView(viewClass,viewPoolRoot,onComplete)
            return view
        end,nil,function(o)
            Object.Destroy(o.gameObject)
        end)
        cachedPoolViews[viewClass] = pool
    end
    return pool
end

-----------------------------------------------------------------------------
--- interface of SceneActor
-----------------------------------------------------------------------------

--- 在进入情景前执行
---@private
function ViewManager.OnPrepareScene()
    local root = GameObject.Find(ViewManager.Config.RootName)
    viewRoot = root.transform
    viewPoolRoot = GameObject.New("_viewcache").transform
    --viewPoolRoot.parent = viewRoot
    ViewManager.Root = viewRoot

    cachedPoolViews = {}
    singletonViews = {}
end

--- 在离开情景时执行
---@private
function ViewManager.OnLeaveScene()
    for k, v in pairs(singletonViews) do
        v:Destroy()
    end
    for k,v in pairs(cachedPoolViews) do
        v:Clear()
    end
    Object.Destroy(viewPoolRoot.gameObject)
end

return ViewManager