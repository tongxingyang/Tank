---------------------------------------------
--- 坦克移动动作
---	坦克按照移动路径一格一格移动同时刷新数据
---	注意第一格是坦克初始位置所以从第二个开始移动
--- Created by thrt520.
--- DateTime: 2018/5/29
---------------------------------------------
---@class TankMoveAction
local TankMoveAction = class("TankMoveAction")

---@type TankMoveData
TankMoveAction.moveData = nil
---@type BaseFightTank
TankMoveAction.tankData = nil
---@type number
TankMoveAction.tankId = nil
---@type boolean
TankMoveAction.isMine = nil
TankMoveAction._co = nil

local TankModel = require("Game.Battle.Model.TankModel")
local MSGRotateTank = require("Game.Event.Message.Battle.MSGRotateTank")
local MSGMoveTankSingle = require("Game.Event.Message.Battle.MSGMoveTankSingle")
local ViewFacade =require("Game.Battle.View.ViewFacade")
local ShadowModel = require("Game.Battle.Model.ShadowModel")

---@param tankMoveData TankMoveData
function TankMoveAction:ctor(tankMoveData)
	self.moveData = tankMoveData
end

function TankMoveAction:Action(holder)
	if holder then
		holder:Pend()
	end
	self.tankData = TankModel.GetTankData(self.moveData.TankId)
	self.tankId = self.moveData.TankId
	self.isMine = self.tankData.IsPlayer
	local oldPos = self.tankData.Pos
	local len = #self.moveData.GridPosList
	for i = 2, len do   ---第一個為原坐標 因此从第二个开始
		if self.tankData.Visiable then
			ViewFacade.CamFollowTank(self.tankData.Id)
		end
		local newToward = EToward.Vector2Enum( self.moveData.GridPosList[i] - self.moveData.GridPosList[i - 1])
		self:_rotateTank(newToward)
		self:_moveTank(self.moveData.GridPosList[i])
		ViewFacade.StopCamFollow()
	end
	ShadowModel.UpdateFOW()
	ViewFacade.UpdateAllTank()
	ViewFacade.UpdateShadowView()
	self.tankData:CostPower(self.moveData.CostPower)
	self.tankData.Pos = oldPos
	TankModel.ArrangeTank(self.tankData.Id , self.moveData.GridPosList[len])
	if holder then
		holder:Restore()
	end
end

---旋转坦克
function TankMoveAction:_rotateTank(toward)
	local nowToward = self.tankData.Toward
	if toward == nowToward then
		return
	end
	local angles = EToward.GetAngle(toward)
	EventBus:Brocast(MSGRotateTank:Build({TankId = self.tankId , Toward = toward , IsMine = self.isMine , Angel =angles ,Time = BattleConfig.TankRotateTime})):Yield()
	self.tankData.Toward = toward
end

---坦克移动
function TankMoveAction:_moveTank(gridPos)

	EventBus:Brocast(MSGMoveTankSingle:Build({TankId = self.tankId , TargetPos = gridPos , IsMine = self.isMine , Time =  BattleConfig.TankMoveTime})):Yield()
	self.tankData:SetPos(gridPos)
	ShadowModel.UpdateFOW()
	ViewFacade.UpdateAllTank()
	ViewFacade.UpdateShadowView()
end


return TankMoveAction