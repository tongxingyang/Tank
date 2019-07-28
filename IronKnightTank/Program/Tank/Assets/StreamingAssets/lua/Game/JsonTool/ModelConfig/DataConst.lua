---------------------------------------------
--- DataConst
--- Json数据表配置
--- Created by thrt520.
--- DateTime: 2018/5/7
---------------------------------------------
DataConst = {}


DataConst.DataName = {
    Unit = "Unit",
    TankUnit = "TankUnit",
    TerrainEleement = "TerrainEleement",
    EnemyUnit = "EnemyUnit",
    UnitSkill = "UnitSkill",
    UnitAttributesAdd = "UnitAttributesAdd",
    Language = "Language",
    InitiativeSkill = "InitiativeSkill",
    Level = "Level",
    ProfilePicture = "ProfilePicture",
    --RecruitData = "RecruitData",
    Battle = "Battle",
}

---@type table 存档名
DataConst.SaveName = {
    --PlayerData = "PlayerData",
    --SettingData = "SettingData",
    SaveData = "SaveData",
}

---@type table 渲染质量
DataConst.RenderQuality = {
    VeryLow = 1,
    Low = 2,
    Medium = 3,
    High = 4,
    VeryHigh = 5,
    Ultra = 6,
}

---@type table 当前语言
DataConst.CurLanguage = {
    Cn = 1,
    En = 2,
}

DataConst.Language_Field = {
    Language_ID = "Language_ID",
    Text_Ch = "Text_Ch",
    Text_En = "Text_En",
}