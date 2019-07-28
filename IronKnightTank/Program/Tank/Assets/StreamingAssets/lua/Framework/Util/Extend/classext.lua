function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    local copyTab = _copy(object)
    lookup_table = nil
    _copy = nil
    --ReferenceCounter.MarkWithTraceback("clone", copyTab)

    return copyTab
end

--- 定义类型
---@param classname string @类名
---@param super table|nil @基类
function class(classname, super)
    local superType = type(super)
    local cls
    if superType ~= 'function' and superType ~= 'table' then
        superType = nil
        super = nil
    end

    if superType == 'function' or (super and super.__ctype == 1) then
        -- inherited from native C++ Object
        cls = {}
        if superType == 'table' then
            -- copy fields from super
            for k,v in pairs(super) do cls[k] = v end
            cls.__create = super.__create
            cls.super = super
        else
            cls.__create = super
            cls.ctor = function() end
        end
        cls.__cname = classname
        cls.__ctype = 1

        function cls.new(...)
            local instance = cls.__create(...)
            -- copy fields from class to  native object
            for k, v in pairs(cls) do instance[k] = v end
            instance.class = cls
            instance:ctor(...)
            --ReferenceCounter.MarkWithTraceback(classname, instance)
            return instance
        end
        cls.New = cls.new
    else
        -- inherited from Lua Object
        if super then
            cls = {}
            setmetatable(cls, {__index = super})
            cls.super = super
        else
            cls = {ctor = function() end}
        end
        cls.__cname = classname
        cls.__ctype = 2 --lua
        cls.__index = cls

        function cls.new(...)
            local instance = setmetatable({}, cls)
            instance.class = cls
            instance:ctor(...)
            --ReferenceCounter.MarkWithTraceback(classname, instance)
            return instance
        end
        cls.New = cls.new
    end
    return cls
end

--- 创建包含self的上下文环境
---@param self table
---@param selffunc fun
---@return fun
function createSelfContext(self,selffunc)
    return function(...)
        selffunc(self,...)
    end
end

function isclass(o, cname)
    if (not o) or (not cname) then return false end

    local ic = o and (o.__cname == cname)
    if (not ic) and (o and o.super) then
        return isclass(o.super, cname)
    else
        return ic
    end
end

function import(moduleName, currentModuleName)
    local currentModuleNameParts
    local moduleFullName = moduleName
    local offset = 1

    while true do
        if string.byte(moduleName, offset) ~= 46 then -- .
            moduleFullName = string.sub(moduleName, offset)
            if currentModuleNameParts and #currentModuleNameParts > 0 then
                moduleFullName = table.concat(currentModuleNameParts, ".") .. "." .. moduleFullName
            end
            break
        end
        offset = offset + 1

        if not currentModuleNameParts then
            if not currentModuleName then
                local n,v = debug.getlocal(3, 1)
                currentModuleName = v
            end

            currentModuleNameParts = string.split(currentModuleName, ".")
        end
        table.remove(currentModuleNameParts, #currentModuleNameParts)
    end

    return require(moduleFullName)
end
