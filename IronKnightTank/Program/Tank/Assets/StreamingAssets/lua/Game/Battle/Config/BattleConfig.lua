---------------------------------------------
--- BattleConfig
---战斗配置脚本
---集成大部分与战斗有关的配置
--- Created by thrt520.
--- DateTime: 2018/6/7
---------------------------------------------
---@class BattleConfig
BattleConfig = {}

BattleConfig.TankAtkDistance = 8 					-----最大攻击距离
BattleConfig.LevelS = 1								-----坦克向性S系数
BattleConfig.LevelA = 0.95							-----坦克向性A系数
BattleConfig.LevelB = 0.9							-----坦克向性B系数
BattleConfig.LevelC = 0.85							-----坦克向性C系数
BattleConfig.TargetTank = 1.1						-----天命型号系数
BattleConfig.LevelId = 1001				 			-----关卡ID
BattleConfig.PassTime = 3 							-----坦克损坏时间
BattleConfig.MapScriptName = "DemoLevelMap01" 		-----地图文件
BattleConfig.LevelFolderPath = "Game.Script.Map." 	-----关卡父路径
BattleConfig.MaxDMG = 300							-----最大伤害 用于攻击表演时攻击伤害进度条
BattleConfig.MaxArmored = 300						-----最大护甲 用于攻击表演时攻击护甲进度条
BattleConfig.MaxHit = 200							-----最大命中 用于攻击表演时攻击护甲进度条
BattleConfig.MaxProject = 200						-----最大投影量 用于攻击表演时攻击护甲进度条
BattleConfig.AtkSliderTweenTime = 0.5 				-----攻击时进度条动画时间
BattleConfig.AtkSliderTweenIntervalTime = 0.2 		-----攻击时进度条动画间隔时间
BattleConfig.PlayerCamp = ECamp.Red   				-----玩家阵营
BattleConfig.NpcCamp = ECamp.Blue   				-----NPC阵营
BattleConfig.NormalAtkCofficient = 0.8   			-----移動攻擊命中系数
BattleConfig.MoveCost = 1   						-----移动消耗
BattleConfig.AtkCost = 1   							-----攻击消耗
BattleConfig.DoubelMoveCost = 2   					-----双倍移动消耗
BattleConfig.NormalMoveCost = 1   					-----普通移动消耗
BattleConfig.FocusAtkCost = 2   					-----精准攻击消耗
BattleConfig.NormalAtkCost = 2   					-----普通攻击消耗
BattleConfig.HitJudgeAngle = 30   					-----攻击位置判定夹角
BattleConfig.TankMoveTime = 0.2						-----坦克移动一格动画表演时间
BattleConfig.TankRotateTime = 1						-----坦克旋转动画表演时间
BattleConfig.TankTalkTime = 3						-----对话框展示时间
BattleConfig.TankAimTime = 3						-----坦克旋转炮塔时间
BattleConfig.IsTankHideInShadow = false				-----坦克黑暗中是否隐藏

return BattleConfig
