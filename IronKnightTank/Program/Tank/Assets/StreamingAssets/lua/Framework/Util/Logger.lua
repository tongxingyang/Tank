
function logException(e)
    clog('<color=red>LuaExcepiton:</color> :' .. tostring(e) )
    --clog('LuaExcepiton:' .. tostring(e) .. traceback('', true))
end

--运行时间分析
local _timeAnalyzer
local _timeAnalyzerBlock
function beginTimeAnalyzer()
    _timeAnalyzer = {}
    _timeAnalyzerBlock = {}
end

function beginTimeAnalyzerBlock(key)
    _timeAnalyzerBlock[key] = os.clock()
end

function endTimeAnalyzerBlock(key)
    _timeAnalyzer[key] = (_timeAnalyzer[key] or 0) + (os.clock() - _timeAnalyzerBlock[key])
end

function endTimeAnalyzer(dontPrintResult)
    if dontPrintResult == nil or not dontPrintResult then
        for k,v in pairs(_timeAnalyzer) do
            clog(string.format("%s:%s",k,v))
        end
    end
    local analyzeResult = _timeAnalyzer
    _timeAnalyzer = nil
    _timeAnalyzerBlock = nil
    return analyzeResult
end

--DebugConsole 重载方法,加入运行栈信息
local _debugconsole = {}
_debugconsole.Log = DebugConsole.Log
_debugconsole.LogToChannel = DebugConsole.LogToChannel
_debugconsole.DebugBreak = DebugConsole.DebugBreak

local debugConsoleMetatable = getmetatable(DebugConsole)
setmetatable(DebugConsole, nil)

function DebugConsole.DebugBreak(...)
    local arg = {...}
    if #arg>0 then
        DebugConsole.Log(...)
    end
    _debugconsole.DebugBreak()
end

DebugConsole.Break = DebugConsole.DebugBreak

--- 输出到游戏内控制台
function DebugConsole.Log(...)
    local arg = {...}
    table.insert(arg, traceback(""))
    _debugconsole.Log(unpack(arg))
end

--- 输出到游戏内控制台指定频道
-- @param channel @class int
function DebugConsole.LogToChannel(channel,...)
    local arg = {...}
    table.insert(arg, traceback(""))
    _debugconsole.LogToChannel(channel,unpack(arg))
end

clog = DebugConsole.Log

clog2 = DebugConsole.LogToChannel


setmetatable(DebugConsole, debugConsoleMetatable)