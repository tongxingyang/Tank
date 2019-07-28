---------------------------------------------
--- Delay
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------

---@style 延时[Time]秒
---@path 流程控制/延时
---@description 延时指定时间
---@action Delay
---@class Delay : ScriptAction
local Delay = class("Delay",require("Framework.LetsTrigger.Command.ScriptAction"))

---@description 延时时间
---@type number
---@parameter Time
Delay.Time = nil

function Delay:OnExecute()
    delayCall(self.End,self.Time,nil,self)
end

return Delay