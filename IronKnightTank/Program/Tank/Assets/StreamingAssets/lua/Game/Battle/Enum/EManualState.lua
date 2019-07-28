---------------------------------------------
--- EPlayerCtrlState
--- Created by thrt520.
--- DateTime: 2018/6/20
---------------------------------------------
---@class EManualState
EManualState = {
	NoneState 			= 		0 ,		---空状态
	EnsureAtkState 		= 		1 ,		---确认攻击状态
	EnsureDirState 		= 		2 ,		---确认方向状态
	TankMoveState 		= 		3 ,		---坦克移动表演状态
	TankAtkState 		= 		4 ,		---坦克攻击表演状态
	DefaultState 		= 		5 ,		---默认无选中状态
	MineTankState 		= 		6 ,		---选择我方坦克状态
	EnemyTankState 		= 		7 ,		---选中敌方坦克状态
	FinishCmdState 		= 		8 ,		---完成指令状态，是指完成一个操作后的状态，如果还有剩余的能量点数会转入，MineTankState 否则转入EnsureDirState
	EnsureSkillState 	= 		9 ,		---确认技能状态，管理所有的指挥官技能Controller，在Controller完成后发送释放技能指令给CombatController
}
return EManualState