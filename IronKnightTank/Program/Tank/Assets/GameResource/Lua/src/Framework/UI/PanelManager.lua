---------------------------------------------
--- PanelManager
--- Created by Tianc.
--- DateTime: 2018/04/17
---------------------------------------------

---@class PanelManager
PanelManager = {}

---@type table<string,class> 面板名字-视图的映射配置
local registerPanels = {}

---@type table<string,Panel> 已经缓存的面板
local cachedPanels

-----------------------------------------------------------------------------
--- panel
-----------------------------------------------------------------------------

--- 打开面板
---@param panelName string 面板名字 
---@param param any
---@param onOpend nil|fun(panel)
function PanelManager.Open(panelName,param,onOpend)
    PanelManager._load(panelName,function(panel)
        PanelManager._open(panel,param)
        if onOpend then
            onOpend(panel)
        end
    end)
end

--- 打开面板
---@param panelName string 面板名字
---@param param any
---@return Panel
function PanelManager.OpenYield(panelName,param)
    return coroutine.callwait(nil,PanelManager.Open,3,panelName,param)
end

--- 关闭面板
---@param panelName string|Panel 
---@param param any
function PanelManager.Close(panelName,param)
    if type(panelName) == "table" then
        panelName = panelName.PanelName
    end
    local panel = cachedPanels[panelName]
    if panel then
        panel.gameObject:SetActive(false)
        if panel.OnClose then
            panel:OnClose(param)
        end
    end
end

--- 销毁面板
---@param panelName string|Panel
function PanelManager.Destroy(panelName)
    if type(panelName) == "table" then
        panelName = panelName.PanelName
    end
    local panel = cachedPanels[panelName]
    if panel then
        panel:Destroy()
    end
end

--- 注册一个PanelClass到Panel
---@param panelName string 面板名字
---@param panelClass class
function PanelManager.Register(panelName,panelClass)
    registerPanels[panelName] = panelClass
end

--- 注册一个PanelClass到Panel
---@param panelName string 面板名字
---@param panelClass class
function PanelManager.RegisterProxy(panelName,proxyClass,panelClass)

end

--- 读取/加载
---@param panelName string 
---@param onLoaded fun(panel:Panel)
---@private
function PanelManager._load(panelName, onLoaded)
    local panel = cachedPanels[panelName]
    if panel == nil then
        local panelClass = registerPanels[panelName]
        assert(panelClass,string.format("The panel '%s' not register.",panelName))

        ViewManager.CreateView(panelClass,function(view)
            panel = view
            panel.PanelName = panelName
            cachedPanels[panelName] = panel
            onLoaded(panel)
        end)
    else
        onLoaded(panel)
    end
end

--- 打开，设为可见
---@param panel Panel
---@param param any
---@private
function PanelManager._open(panel,param)
    panel.gameObject:SetActive(true)
    if panel.OnOpen then
        panel:OnOpen(param)
    end
end

-----------------------------------------------------------------------------
--- interface of SceneActor
-----------------------------------------------------------------------------

--- 在进入情景前执行
---@private
function PanelManager.OnPrepareScene()
    cachedPanels = {}
end

--- 在离开情景时执行
---@private
function PanelManager.OnLeaveScene()
    for i, panel in pairs(cachedPanels) do
        panel:Destroy()
    end
    cachedPanels = nil
end

return PanelManager