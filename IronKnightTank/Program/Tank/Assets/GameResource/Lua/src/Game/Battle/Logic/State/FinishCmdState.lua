---------------------------------------------
--- FinishCmdState
---	完成操作状态
---	这个state起节点的作用并非真实的状态  ， 进入这个状态后会根据tank的状态转入不同的状态
--- Created by thrt520.
--- DateTime: 2018/7/30
---------------------------------------------
local FinishCmdState = {}
local this = FinishCmdState
---@type EManualState
FinishCmdState.StateEnum = EManualState.FinishCmdState
---@type FSM
FinishCmdState.FSM = nil
local TankModel = require("Game.Battle.Model.TankModel")

function FinishCmdState.OnEnter(param)
	local tankData = TankModel.GetTankData(param.TankId)
	if tankData:GetPower() > 0 then
		this.FSM:ChangeState(EManualState.MineTankState , {TankId = param.TankId})
	else
		this.FSM:ChangeState(EManualState.EnsureDirState , {TankId = param.TankId})
	end
end

function FinishCmdState.OnExit()

end

return FinishCmdState