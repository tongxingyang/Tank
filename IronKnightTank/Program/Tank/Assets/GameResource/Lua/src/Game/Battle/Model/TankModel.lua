---------------------------------------------
--- TankModel
--- Created by thrt520.
--- DateTime: 2018/6/7
---------------------------------------------
---@class TankModel
local TankModel = {}
local this = TankModel

local MapModel = require("Game.Battle.Model.MapModel")
local ShadowModel = require("Game.Battle.Model.ShadowModel")
local PlayerFightTank = require("Game.Battle.Model.Data.Tank.PlayerFightTank")
local NPCFightTank = require("Game.Battle.Model.Data.Tank.NPCFightTank")
local MapConfig = require("Game.Battle.Config.MapConfig")
local AtkAreaHelper = require("Game.Battle.Model.AtkAreaHelper")

---@type table<number , BaseFightTank>
this._allTankDataDic = {}
-----@type table<ECamp , table<number , BaseFightTank>>
this._allTankCampDic = {}
---@type table<number , boolean> 坦克id缓存
this.curId = 0

local receiveCommand = {
    require("Game.Event.Message.Battle.MSGRoundStart"),
    require("Game.Event.Message.Battle.MSGCreateNpc"),
}

local MSGTankDestory = require("Game.Event.Message.Battle.MSGTankDestory")

------------------------------------------------------------------------------------
--- sceneActor
------------------------------------------------------------------------------------

function TankModel.OnIntoScene()
    this._initNpc()
    this._initPlayer()
    this._registerCommandHandler()
end

function TankModel.OnLeaveScene()
    this.Dispose()
    this._unregisterCommandHandler()
end

function TankModel._initNpc()
    local levelData = require(BattleConfig.LevelFolderPath .. BattleConfig.MapScriptName)
    ---@type EditorBlockData[]
    local editorBlockDataList = levelData.BlockDataList
    for i, v in pairs(editorBlockDataList) do
        if v.NPCId ~= 0 then
            local tankData = this.CreateNpcTank(v.NPCId)
            local pos = GridPos.New(v.pos.x, v.pos.y)
            tankData.Toward = v.NpcToward
            this.AddTank(tankData)
            this.ArrangeTank(tankData.Id, pos)
        end
    end
end

function TankModel._initPlayer()
    local config = require("Game.Script.FightTankConfig")
    for i, v in pairs(config) do
        local tankData = this.CreatePlayerTank(v.UnitId, v.SoliderLv, v.SkillLv, v.TankId, v.SoliderHit, v.SoliderRaipFire)
        this.AddTank(tankData)
        tankData.Toward = MapConfig.DefaultToward
    end
end

------------------------------------------------------------------------------------
--- function
------------------------------------------------------------------------------------
function TankModel._registerCommandHandler()
    EventBus:RegisterReceiver(receiveCommand, TankModel)
end

function TankModel._unregisterCommandHandler()
    EventBus:UnregisterReceiver(receiveCommand, TankModel)
end
------------------------------------------------------------------------------------
--- event handler
------------------------------------------------------------------------------------
---@param msg MSGCreateNpc
function TankModel.OnMSGCreateNpc(msg)
    local npcTank = this.CreateNpcTank(msg.NpcId)
    this.AddTank(npcTank)
    npcTank.Toward = msg.Toward
    local pos = MapModel.GetAvailablePos(msg.Pos)
    this.ArrangeTank(npcTank.Id, pos)
    local MSGCreateTank = require("Game.Event.Message.Battle.MSGCreateTank")
    EventBus:Brocast(MSGCreateTank:Build({ BaseFightTankDataList = { npcTank } }))
end

---@param msg MSGRoundStart
function TankModel.OnMSGRoundStart(msg)
    for i, v in pairs(this._allTankDataDic) do
        if v.BodytHurtRound > 0 then
            v.BodytHurtRound = v.BodytHurtRound - 1
        end
        if v.TurretHurtRound > 0 then
            v.TurretHurtRound = v.TurretHurtRound - 1
        end
        v:ResetPower()
    end
end
------------------------------------------------------------------------------------
----返回所有的坦克数据
---@return table<number , BaseFightTank>
function TankModel.GetAllTankData()
    return this._allTankDataDic
end

----获取坦克数据
---@return BaseFightTank
function TankModel.GetTankData(tankId)
    if not this._allTankDataDic[tankId] then
        clog("WRONG :      no target tank id " .. tostring(tankId))
    end
    return this._allTankDataDic[tankId]
end

------获取敌方阵营所有坦克
function TankModel.GetEnemyCampTank(camp)
    local realCamp = (BattleConfig.NpcCamp == camp) and BattleConfig.PlayerCamp or BattleConfig.NpcCamp
    return this.GetCampTanks(realCamp)
end

------获取我方阵营所有坦克
----@return table<number , BaseFightTank>
function TankModel.GetCampTanks(camp)
    return this._allTankCampDic[camp]
end

----创建npcTank
---@return PlayerFightTank
function TankModel.CreateNpcTank(npcId)
    local npcTank = NPCFightTank.new(npcId)
    npcTank.Visiable = false
    return npcTank
end

----创建玩家坦克
---@return PlayerFightTank
function TankModel.CreatePlayerTank(soliderId, soliderLv, soliderSkillLv, tankId, hit)
    local playerTank = PlayerFightTank.new(soliderId, soliderLv, tankId, soliderSkillLv)
    playerTank.Visiable = true
    if hit then
        playerTank.Solider.Hit = hit
    end
    return playerTank
end

----增加坦克
---@param tank BaseFightTank
function TankModel.AddTank(tank)
    local id = this._getTankId()
    tank.Id = id
    this._allTankDataDic[id] = tank
    local campDic = this._allTankCampDic[tank.Camp] or {}
    campDic[id] = tank
    this._allTankCampDic[tank.Camp] = campDic
    this._allTankCampDic[tank.Camp][id] = tank
end

----移除坦克
---@param tankId number
function TankModel.RemoveTank(tankId)
    this.DisArrangeTank(tankId)
    local tankData = this.GetTankData(tankId)
    local tankDataList = this.GetCampTanks(tankData.Camp)
    tankDataList[tankId] = nil
    this._allTankDataDic[tankId] = nil

    if not tankData.IsFakeTank then
        EventBus:Brocast(MSGTankDestory:Build({ TankData = tankData }))
    end
end

---布置坦克
function TankModel.ArrangeTank(tankId, pos)
    if this._assertTank(tankId) then
        local tank = this._allTankDataDic[tankId]
        tank.IsArrange = true
        MapModel.ArrangeMover(tank, pos)
        tank.Pos = pos
        ShadowModel.AddViewer(tank)
    else
        clog("no tank " .. tostring(tankId))
    end
end

---取消布置坦克
---@param tankId number
function TankModel.DisArrangeTank(tankId)
    local tank = this.GetTankData(tankId)
    if tank then
        MapModel.RemoveMover(tank)
        ShadowModel.RemoveViewer(tank)
        tank.IsArrange = false
        tank.Pos = nil
    else
        clog("no player tank" .. tostring(tankId))
    end
end

---坦克移动路径
---@return GridPos[]
function TankModel.GetTankMovePath(tankId, targetPos)
    if not this._assertTank(tankId) then
        return nil
    end
    local tank = this._allTankDataDic[tankId]
    return MapModel.GetMovePath(tank.Pos, targetPos, tank)
end

---提示可攻击坦克
function TankModel.TipsCanAttackTank(tankid)
    local tankDataList = this.GetCanAttackTank(tankid)
    for i, v in pairs(tankDataList) do
        v.CanBeAtk = true
    end
end

---取消提示可攻击坦克
function TankModel.CancelTipsCanAttackTank()
    for i, v in pairs(this._allTankDataDic) do
        v.CanBeAtk = false
    end
end

---选择坦克
function TankModel.ChosePlayerTank(tankId)
    local tankData = this.GetTankData(tankId)
    if tankData then
        tankData:Chose()
        return tankData
    end
end

---取消选择坦克
function TankModel.CancelChosePlayerTank()
    PlayerFightTank.CancelChose()
end

----获取可以攻击的tank
---流程：拿到所有在攻击范围内的敌方坦克
--- 判断敌方坦克和我方坦克在攻击路线上面是否有障碍物  没有采用传统思路先拿到攻击范围再获取坦克  是因为没有找到好的算法
----@return BaseFightTank[]
function TankModel.GetCanAttackTank(tankId)
    local fightTankData = this.GetTankData(tankId)
    if not fightTankData then
        return
    end
    if not fightTankData:CanAtk() then
        return {}
    end
    local tankDataList = {}

    local atkArea
    if fightTankData.TankData.TurretType == ETurretType.Rotate then
        atkArea = fightTankData.Pos:GetMultiQuadPos(fightTankData:GetAtkDistance())
    else
        atkArea = AtkAreaHelper.GetFixedAtkArea(fightTankData.Pos, fightTankData:GetAtkDistance(), fightTankData.Toward)
    end
    local enemyTankList = this.GetEnemyCampTank(fightTankData.Camp)
    for i, enemyTank in pairs(enemyTankList) do
        if table.contains(atkArea, enemyTank.Pos) and enemyTank:IsVisiableForCamp(fightTankData.Camp)
                and not AtkAreaHelper.IsAtkBlock(fightTankData, enemyTank.Pos) then
            table.insert(tankDataList, enemyTank)
        end
    end
    return tankDataList
end

---旋转坦克
---@param tankId number
---@param toward EToward
function TankModel.RotateTank(tankId, toward)
    local tankData = this.GetTankData(tankId)
    if tankData then
        tankData.Toward = toward
    end
end

----根据位置获取坦克
---@param pos
---@return BaseFightTank
function TankModel.GetTankByPos(pos)
    for i, v in pairs(this._allTankDataDic) do
        if v.Pos == pos then
            return v
        end
    end
    return nil
end

---分配可用坦克ID
function TankModel._getTankId(soliderId, tankId)

    this.curId = this.curId + 1
    return this.curId
end

----坦克断言
function TankModel._assertTank(tankId)
    return this._allTankDataDic[tankId] ~= nil
end

function TankModel.Dispose()
    this._allTankDataDic = {}
    this._allTankCampDic = {}
    this.curId = 0
end

return TankModel