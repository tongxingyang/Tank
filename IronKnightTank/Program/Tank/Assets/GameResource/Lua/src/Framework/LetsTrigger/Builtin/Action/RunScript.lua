---------------------------------------------
--- RunScript
--- Created by Tianc.
--- DateTime: 2018/04/29
---------------------------------------------
---@class RunScript : ScriptAction
local RunScript = class("RunScript",require("Framework.LetsTrigger.Command.ScriptAction"))

---@type string
RunScript.ScriptName = nil

function RunScript:OnExecute()
    ---@type LScript
    local script = require(self.ScriptName)
    script:RunScript(self.RuntimeInstance.Parameters):Yield()
    self:End()
end

return RunScript