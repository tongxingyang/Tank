---------------------------------------------
--- ManualCommander
---玩家手动控制Commander
---    负责玩家管理操作逻辑
--- Created by thrt520.
--- DateTime: 2018/6/20
---------------------------------------------
---@class ManualCommander
local ManualCommander = class("ManualCommander")
local this = ManualCommander
local FSMClass = require("Game.FSM.FSM")

---@type FSM 状态机
this.FSM = nil
---@type ECamp 阵营
this.Camp = BattleConfig.PlayerCamp

local receiveCommand = {
    require("Game.Event.Message.Battle.MSGFirstObeserveViewer"),
    require("Game.Event.Message.Battle.MSGActiveNextCamp"),
}

function ManualCommander:ctor(camp)
    self.Camp = camp
    self:Init()
end

function ManualCommander:_registerCommandHandler()
    EventBus:RegisterSelfReceiver(receiveCommand, self)
end

function ManualCommander:_unregisterCommandHandler()
    EventBus:UnregisterSelfReceiver(receiveCommand, self)
end

--------------------------------
---even handler
--------------------------------
---@param msg MSGFirstObeserveViewer
function ManualCommander:OnMSGFirstObeserveViewer(msg)
    if msg.ViewerCamp == self.Camp then
        return
    else
        coroutine.createAndRun(function()
            msg:Pend()
            local DiscoverNpcAction = require("Game.Battle.Action.DiscoverNpcAction")
            local action = DiscoverNpcAction.new(msg.Viewer, msg.BeSeenViewer, msg.HasBeenSeen)
            action:Action()
            msg:Restore()
        end)
    end
end

---@param msg MSGActiveNextCamp
function ManualCommander:OnMSGActiveNextCamp(msg)
    if msg.Camp == self.Camp then
        self.FSM:ChangeState(EManualState.DefaultState)
    end
end
--------------------------------
function ManualCommander:Init()
    self:_registerCommandHandler()
    self.FSM = FSMClass.new()
    self.FSM:AddState(require("Game.Battle.Logic.State.DefaultState"))
    self.FSM:AddState(require("Game.Battle.Logic.State.MineTankState"))
    self.FSM:AddState(require("Game.Battle.Logic.State.EnemyTankState"))
    self.FSM:AddState(require("Game.Battle.Logic.State.TankMoveState"))
    self.FSM:AddState(require("Game.Battle.Logic.State.EnsureDirState"))
    self.FSM:AddState(require("Game.Battle.Logic.State.EnsureAtkState"))
    self.FSM:AddState(require("Game.Battle.Logic.State.TankAtkState"))
    self.FSM:AddState(require("Game.Battle.Logic.State.NoneState"))
    self.FSM:AddState(require("Game.Battle.Logic.State.FinishCmdState"))
    self.FSM:AddState(require("Game.Battle.Logic.State.EnsureSkillState"))
end

function ManualCommander:Dispose()
    self.FSM:Dispose()
    self:_unregisterCommandHandler()
end

return ManualCommander