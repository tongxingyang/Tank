--[[
@author Ben
Copyright(c) 2015-2016 ShangHai Overture Network Technoloty Co., Ltd, All rights reserved.

字符串的扩展

版本:
]]
---@class utf8
utf8 = require("Framework.Util.utf8")

local ssub = string.sub
local sgsub = string.gsub
local smatch = string.match
local sfind = string.find
local sreverse = string.reverse
local tinsert = table.insert
local tconcat = table.concat
local sformat = string.format
local schar = string.char
local sbyte = string.byte
local mrandom = math.random
local tonumber = tonumber


---为了提示，用正规的function写法
---去掉所有空格，是所有
function string.trimall(str)
    if str == nil then
        return str
    end
    --gsub 会返回替换后的字符串和替换的次数，这里只取第一个参数
    local str = sgsub(str, ' ', '')
    return str
end

---去掉前后空格
function string.trim(str)
    if str == nil then
        return str
    end
    return smatch(str, "%s*(.-)%s*$")
end

---替换所有出现的字符串
--@param 字符串
--@param 需要替换的字符串
--@param 替换成新的字符串
--@return 返回字符串
function string.replace(str, oldstr, newstr)
    if str == nil or oldstr == nil or newstr == nil then
        return str
    end
    local str = sgsub(str, oldstr, newstr)
    return str
end

---判断str是否以substr开头。是返回true，否返回false
function string.startswith(str, substr)
    if str == nil or substr == nil then
        return false
    end
    return sfind(str, substr) ~= nil
end

---判断str是否以substr结尾。是返回true，否返回false
function string.endswith(str, substr)
    if str == nil or substr == nil then
        return false
    end
    local str_tmp = sreverse(str)
    local substr_tmp = sreverse(substr)
    return sfind(str_tmp, substr_tmp) ~= nil
end

---截取字符串，从获取到的findstr往后截，不包括findstr本身
--@param @class string 字符串
--@param @class string 查找的字符串
--@param @class bool 是否是从尾巴获取
--@return 返回字符串
function string.substr(str, findstr, last)
    local _char = #findstr == 1 and '%' or ''
    if last then
        local _,_,idx = sfind(str, ".*" .. _char .. findstr .. "()")
        if(idx ~= nil) then
            return ssub(str, idx)
        end
        return str
    end
    local _,_,_,ret = sfind(str, "(" .. _char .. findstr .. ")(.+)")
    return ret
end

---字符串长度截取
-- @param s
-- @param n
-- @return
function string.sublen(s, n)
    return ssub(s, 1, n)
end

---截取字符串，从头截到findstr位置，不包括findstr本身
--@param @class string 字符串
--@param @class string 查找的字符串
--@param @class bool 是否是从尾巴获取
function string.substrhead(str, findstr, last)
    local llen = #findstr
    local _char = llen == 1 and '%' or ''

    if last then
        local _,idx = sfind(str, ".*" .. _char .. findstr .. "()")
        if(idx ~= nil) then
            return ssub(str, 1, idx - llen )
        end
        return str
    end

    local _,idx = sfind(str, _char .. findstr)
    return ssub(str, 1, idx - llen)
end

---获取findstr的位置
--@param @class string 字符串
--@param @class string 查找的字符串
--@param @class bool 是否是从尾巴获取
--@return 位置，不存在返回nil
function string.indexof(str, findstr, last)
    local _char = #findstr == 1 and '%' or ''
    if(last == true) then
        local _, idx = sfind(str, ".*" .. _char .. findstr .. "()")
        if(idx ~= nil) then
            return idx
        end
        return nil
    end

    local _,idx = sfind(str, _char .. findstr)
    if(idx ~= nil) then
        return idx
    end
    return nil
end

---分割字符串
--@param 字符串
--@param 分隔符，默认逗号
--@return 分割后的table
function string.split(str, sep)
    local sep, fields = sep or ",", {}
    local pattern = sformat("([^%s]+)", sep)
    sgsub(str, pattern, function(c) tinsert(fields, c) end)
    return fields
end

---拼接字符串，根据分隔符进行拼接
--@param 字符串之间的分隔符，如果没有=nil
--@param 需要拼接的table数据
--@return 返回字符串
function string.join(sep, ...)
    local tab = {...}
    for i,v in pairs(tab) do
        tab[i] = v == nil and "nil" or tostring(v)
    end
    return tconcat(tab, sep)
end

local seed = {'1','2','3','4','5','a','b','c','6','7','8','9','0','d','e','f'}
local seed_num = {'1','2','3','4','5','6','7','8','9','0'}

---生成len长度的guid，中间没有分隔符
function string.guid(llen)
    local strs = {}
    for i = 1, llen do
        strs[i] = seed[mrandom(1,16)]
    end
    return tconcat(strs)
end

function string.guidnum(llen)
    local strs = {}
    for i = 1, llen do
        strs[i] = seed_num[mrandom(1,10)]
    end
    return tconcat(strs)
end

---获得一个带正负号的数字字符串
function string.formatSignedNum(num)
    if num >= 0 then
        return sformat("+%s",num)
    end
    return tostring(num)
end

---转换为字符串
-- @param str
-- @return @ class string
function string.hexto(str)
    return (str:gsub('..', function (cc)
        return schar(tonumber(cc, 16))
    end))
end

---转换成hex
-- @param str
-- @return
function string.tohex(str)
    return (str:gsub('.', function (c)
        return sformat('%02X', sbyte(c))
    end))
end

function string.encode_uri_3986(str)
    str = sgsub(str, '\n', '\r\n')
    --return sgsub(str, '([^%w %-%_%.%~])', function(c)return sformat('%%%02X', sbyte(c))end)   --空格不保留也进行编码，SB U9
    return sgsub(str, '([^%w%-%_%.%~])', function(c)return sformat('%%%02X', sbyte(c))end)
    --return sgsub(str, ' ', '+'')  --把空格变成+
end
