---------------------------------------------
--- ViewProxy
--- Created by Tianc.
--- DateTime: 2018/04/25
---------------------------------------------

---@class ViewProxy
local ViewProxy = class("ViewProxy")
ViewProxy.new = nil
ViewProxy.New = nil

function ViewProxyClass(className)
    local c = class(className,ViewProxy)
    c._new = c.new
    c.new = nil
    c.New = nil
    return c
end

--- 创建代理.
--- 将view的方法调用转发到代理上
---@param proxyClass class
---@param view View
---@return ViewProxy
function ViewProxy.CreateViewProxy(proxyClass,view)
    --代理View方法的实体对象
    local viewProxy = proxyClass._new()
    --保存了View被代理前的一组方法,用于转接到真实的View方法
    local viewOperator = viewProxy:_transpondView(view.class)
    setmetatable(viewOperator,{ __index = view})
    viewProxy.View = viewOperator
    return viewProxy
end

function ViewProxy:_transpondView(view)
    local viewProxy = {}
    for funcName, viewFunc in pairs(view) do
        local proxyFunc = self[funcName]
        if self:_needTranspond(funcName, viewFunc, proxyFunc) then
            self:_transpondMethod(funcName, viewFunc, proxyFunc, viewProxy, view)
        end
    end
    return viewProxy
end

function ViewProxy:_transpondMethod(funcName, viewFunc, proxyFunc, viewProxy, view)
    viewProxy[funcName] = viewFunc
    view[funcName] = function(o,...)
        proxyFunc(self,...)
    end
end

function ViewProxy:_needTranspond(funcName, viewFunc, proxyFunc)
    return proxyFunc and type(viewFunc) == "function" and type(proxyFunc) == "function"
end


return ViewProxy