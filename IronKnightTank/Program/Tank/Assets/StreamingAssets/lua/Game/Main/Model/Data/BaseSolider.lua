---------------------------------------------
--- BaseSolider
---    士兵基类
--- Created by thrt520.
--- DateTime: 2018/6/8
---------------------------------------------
---@class BaseSolider
local BaseSolider = class("BaseSolider")

---@type number
BaseSolider.Id = 0
---@type number 等级
BaseSolider.Level = 0
---@type number 名字
BaseSolider.Name = 0
---@type number 命中
BaseSolider.Hit = 0
---@type number 技能id
BaseSolider.SkillID = 0
---@type number 能量
BaseSolider.Power = 0
---@type number 最大能量
BaseSolider.MaxPower = 0
---@type BaseSoliderSkill 技能脚本
BaseSolider.Skill = nil
---@type number 成长
BaseSolider.Growth = 0
---@type number 技能等级 已弃用
BaseSolider.SkillLevel = 0
----------------------------------
----skill 技能相关
----------------------------------
---@type number 视野增幅
BaseSolider.ViewAdvanced = 0
---@type number 速度增幅
BaseSolider.SpeedAdvanced = 0
---@type number 投影量系数
BaseSolider.ProjectCofficient = 0
---@type number 射程增幅
BaseSolider.FireRangeAdvanced = 0
---@type number 幸运
BaseSolider.Luck = 0
---@type boolean 旋转惩罚已弃用
BaseSolider.IsRotationPenalty = false
---@type number 炮弹装填速度系数 已弃用
BaseSolider.ReloadSpeedCofficient = 1
---@type number 命中倾向
BaseSolider.HitPrefer = 0
----------------------------------

function BaseSolider:ctor(index)
    self.Id = index;
    local data = JsonDataMgr.GetTankUnitData(tankId);
    self.Level = data.Level;
    self.Name = data.Name;
    self.Hit = data.Hit;
    self.SkillID = data.SkillID;
    self.Power = data.Power;
    self.MaxPower = data.MaxPower;
    self.Skill = data.Skill;
    self.SkillLevel = data.SkillLevel;
    self.ViewAdvanced = data.ViewAdvanced;
    self.SpeedAdvanced = data.SpeedAdvanced;
    self.ProjectCofficient = data.ProjectCofficient;
    self.FireRangeAdvanced = data.FireRangeAdvanced;
    self.Luck = data.Luck;
    self.IsRotationPenalty = data.IsRotationPenalty;
    self.ReloadSpeedCofficient = data.ReloadSpeedCofficient;
    self.HitPrefer = data.HitPrefer;
end

function BaseSolider:_initSkill()
    if self.SkillID ~= 0 then
        local skillData = JsonDataMgr.GetUnitSkillData(self.SkillID)
        local skillPath = skillData.Skill_Script --skillData[DataConst.UnitSkill_Field.Skill_Script]
        if skillPath then
            self.Skill = require("Game.Script.SoliderSkill." .. skillPath)
        end
    else
        self.Skill = require("Game.Script.SoliderSkill.BaseSoliderSkill")
    end
    self.ViewAdvanced = self.Skill:GetViewStrengthen(self.SkillLevel)
    self.SpeedAdvanced = self.Skill:GetSpeedStrengthen(self.SkillLevel)
    self.ProjectCofficient = self.Skill:GetHideStrengthen(self.SkillLevel)
    self.FireRangeAdvanced = self.Skill:GetRangeStrengthen(self.SkillLevel)
    self.Luck = self.Skill:GetLuck(self.SkillLevel)
    self.IsRotationPenalty = self.Skill:RotationPenalty(self.SkillLevel) == 0
    self.ReloadSpeedCofficient = self.Skill:GetReload_Speed(self.SkillLevel)
    self.HitPrefer = self.Skill:GetRangeStrengthen(self.SkillLevel)
end

function BaseSolider:GetArmoredCoefficient(eHitRes, armoredPosType)
    return self.Skill:GetArmoredStrengthenCoefficient(eHitRes, armoredPosType, self.SkillLevel)
end

function BaseSolider:GetDamageCoefficient(dis)
    return self.Skill:GetDamageStrengthenCoefficient(dis, self.SkillLevel)
end

function BaseSolider:GetDmgCofficientForTargetTank(tankType)
    return self.Skill:GetDmgStrengthForTargetTank(tankType, self.SkillLevel)
end

---获取士兵头像
function BaseSolider:GetSoliderIconPath()

end

---获取士兵名称
function BaseSolider:GetSoliderName()
    return LocalizationMgr.GetDes(self.Name)
end

return BaseSolider