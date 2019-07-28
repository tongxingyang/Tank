---@classdef TimeUtil
local TimeUtil = {}

---@class number 一天的时间（秒）
TimeUtil.DayTime = 86400

---@class number 一小时的时间（秒）
TimeUtil.HourTime = 3600

---@class number 和标准时间的时差
TimeUtil.UTCDifference = nil

local _datetime_pattern = "(%d+)/(%d+)/(%d+) (%d+):(%d+):(%d+)"

function TimeUtil.FormatTimeSpanHHMMSS(time)
    local ut = os.date('!*t',time)
    local h = (ut.day-1)*24 + ut.hour
    local m = ut.min
    local s = ut.sec
    return string.format("%s:%s:%s",string.format("%02d", h),string.format("%02d", m),string.format("%02d", s))
end

function TimeUtil.FormatTimeSpanHMM(time)
    local ut = os.date('!*t',time)
    local h = (ut.day-1)*24 + ut.hour
    local m = ut.min
    local s = ut.sec
    return string.format("%s:%s",string.format("%s", h),string.format("%02d", m))
end

function TimeUtil.GetDataText(time)
    return os.date(getLangText("code_time_ymd"),time)
end

function TimeUtil.GetFullDataText(time)
    if type(time) == "table" then
        time = os.time(time)
    end
    return os.date(getLangText("code_time_ymdhm"),time)
end

--- 获取时间和指定时间之间的差值（约束到日）
-- @param dailyTime @class number 每日时间(本地)
-- @param time @class number 检测的时间(UTC)
-- @return
function TimeUtil.GetUTCDailyDifference(dailyTime,time)
    if dailyTime < TimeUtil.UTCDifference then
        dailyTime = dailyTime + TimeUtil.DayTime
    end
    local utcDelayRefreshTime = dailyTime - TimeUtil.UTCDifference  
    local utcDayTime = time % TimeUtil.DayTime
    local dTime = math.abs(utcDayTime-utcDelayRefreshTime)
    if utcDayTime >= utcDelayRefreshTime then
        dTime = TimeUtil.DayTime - dTime
    end
    return dTime
end

--- 获取时间和指定时间之间的差值（约束到日）
-- @param dailyTime 每日时间(本地)
-- @param time 检测的时间(本地)
-- @return
function TimeUtil.GetLocalDailyDifference(dailyTime,time)
    local utcDelayRefreshTime = dailyTime  
    local utcDayTime = time % TimeUtil.DayTime
    local dTime = math.abs(utcDayTime-utcDelayRefreshTime)
    if utcDayTime >= utcDelayRefreshTime then
        dTime = TimeUtil.DayTime - dTime
    end
    return dTime
end

--- 判断两个UTC时间在本地时区不在同一天
-- @param time1
-- @param time2
-- @return
function TimeUtil.IsDifferentDay(time1,time2)
    local day1 = os.date('*t',time1)
    local day2 = os.date('*t',time2)
    return day1.day ~= day2.day or day1.month ~= day2.month or day1.year ~= day2.year
end

--- time2 - time1 差了多少天
-- @param time1 (本地时间)
-- @param time2 (本地时间)
-- @param dailyTime 每日时间（每天的过天秒数）
-- @return 天数
function TimeUtil.GetDayDistance(time1,time2,dailyTime)
    local dailyDif = TimeUtil.GetLocalDailyDifference(dailyTime, time1)
    local day = ((time2 - time1) - dailyDif)/TimeUtil.DayTime
    return math.floor(day) + 1
end

--- 转换标准UTC时间到本地时间(+时区)
-- @param time
-- @return
function TimeUtil.UTCToLocalTime(time)
    return time + TimeUtil.UTCDifference
end

function TimeUtil.LocalToUTCTime(time)
    return time - TimeUtil.UTCDifference
end

function TimeUtil.ParseDate(datetime_str)
    local year, month, day, hour, min, sec = datetime_str:match(_datetime_pattern)
    return os.time({year=tonumber(year), month=tonumber(month), day=tonumber(day), hour=tonumber(hour), min=tonumber(min), sec=tonumber(sec)})
end

local t = os.time()
local ut = os.date('!*t',t)
local lt = os.date('*t',t)
TimeUtil.UTCDifference = os.difftime(os.time(lt),os.time(ut))



return TimeUtil