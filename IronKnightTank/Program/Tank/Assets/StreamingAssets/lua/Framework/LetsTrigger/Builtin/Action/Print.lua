---------------------------------------------
--- Print
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------

---@style 打印[Msg]到[Channel]
---@path 调试/打印信息
---@description 打印信息
---@action Print
---@class Print : ScriptAction
local Print = class("Print",require("Framework.LetsTrigger.Command.ScriptAction"))

---@description 输出频道
---@default 55
---@type number
---@parameter Channel
Print.Channel = nil

---@description 打印信息
---@type any
---@parameter Msg
Print.Msg = nil


function Print:OnExecute()
    clog2(self.Channel or 1,self.Msg)
    self:End()
end

return Print