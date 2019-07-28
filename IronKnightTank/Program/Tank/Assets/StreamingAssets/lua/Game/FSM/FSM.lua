---------------------------------------------
--- FSM
--- Created by thrt520.
--- DateTime: 2018/6/20
---------------------------------------------
---@class FSM
local FSM = class("FSM")
local this = FSM

---@type table<number , IState>
this._stateDic = {}
---@type IState
this._curState = nil

function FSM:ctor()
	self._stateDic = {}
end

---@param estate EManualState
---@param param nil | table
function FSM:ChangeState(estate , param)
	if not self:_assertState(estate) then
		clog("estate not exit "..tostring(estate))
		return
	end
	if self._curState then
		self._curState.OnExit()
	end
	local nextState = self._stateDic[estate]
	self._curState = nextState
	nextState.OnEnter(param)
end

---@param state IState
function FSM:AddState( state)
	self._stateDic[state.StateEnum] = state
	state.FSM = self
end

function FSM:_assertState(eState)
	return (self._stateDic[eState] ~= nil)
end

function FSM:GetCurStateEnum()
	return self._curState.StateEnum
end

function FSM:Dispose()
	this._stateDic = {}
	if self._curState then
		self._curState.OnExit()
	end
	this._curState = nil
end

return FSM
