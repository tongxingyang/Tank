---------------------------------------------
--- JsonDataMgr
--- Created by thrt520.
--- DateTime: 2018/5/7
---------------------------------------------
---@class JsonDataMgr
JsonDataMgr = {}
local this = JsonDataMgr
local cjson = require("cjson")
local util = require("cjson.util")

this.LoadedCount = 0
this.TotalCount = 0
this.loadedJson = {}
this.LoadSave = {}

function JsonDataMgr.GetJsonData(dataName, id)
    if not id then
        clog("id nil wrong")
        return
    end
    if this.loadedJson[dataName] then
        local jsonData = this.loadedJson[dataName]
        local key = dataName .. "_ID"
        for i, v in pairs(jsonData) do
            if v[key] == id then
                return v
            end
        end
        clog("fileld" .. key .. "json data not exit key : " .. id)
    else
        clog("json data no exit " .. dataName)
    end
end

function JsonDataMgr.GetAllJsonData(dataName)
    if this.loadedJson[dataName] then
        return this.loadedJson[dataName]
    end
    clog("json name wrong " .. tostring(dataName))
    return nil
end

function JsonDataMgr.GetSaveJosnData(dataName, id)
    if not id then
        clog("id nil wrong")
        return
    end
    if this.LoadSave[dataName] then
        local jsonData = this.LoadSave[dataName]
        local key = dataName .. "_ID"
        for i, v in pairs(jsonData) do
            if v[key] == id then
                return v
            end
        end
        clog("fileld" .. key .. "json data not exit key : " .. id)
    else
        clog("json data no exit " .. dataName)
    end
end

function JsonDataMgr.GetAllSaveJosnData(dataName)
    if this.LoadSave[dataName] then
        return this.LoadSave[dataName]
    else
        clog("json name wrong " .. tostring(dataName))
        return nil
    end
end

---预加载  在游戏开始时启动
function JsonDataMgr.PreLoadData(progress)
    for i, v in pairs(DataConst.DataName) do
        this._loadData("Data/" .. v .. ".json", v, progress)
    end
    for i, v in pairs(DataConst.SaveName) do
        this._loadSave("/Save/" .. v .. ".save", v, progress)
    end
end

function JsonDataMgr._loadData(path, dataName, progress)
    local totalCount = 0
    for i, v in pairs(DataConst.DataName) do
        totalCount = totalCount + 1
    end
    ResMgr.LoadText(path, function(textAsset)
        this.loadedJson[dataName] = cjson.decode(textAsset.text)
        this.LoadedCount = this.LoadedCount + 1
        local pro = this.LoadedCount / totalCount
        progress(pro)
    end)
end

--读取存档
function JsonDataMgr._loadSave(path, dataName, progress)
    ResMgr.LoadSave(path, function(textAsset)
        this.LoadSave[dataName] = cjson.decode(textAsset)
    end)
end

--保存存档
function JsonDataMgr.DoSave(path, saveData)
    local _saveData = cjson.encode(this.Filter_Class(saveData));
    ResMgr.DoSave(path, _saveData);
end

--过滤class
function JsonDataMgr.Filter_Class(saveData)
    local temp = {};
    for key, value in pairs(saveData) do
        if type(value) == "table" then
            if tostring(key) ~= "class" then
                temp[tostring(key)] = this.Filter_Class(value)
            end
        else
            temp[tostring(key)] = value
        end
    end
    return temp;
end

function JsonDataMgr.Dispose()
    this.loadedJson = {}
    this.LoadSave = {}
end

---@param id number
---@return UnitData
function JsonDataMgr.GetUnitData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.Unit, id)
end

---@return TankUnitData
function JsonDataMgr.GetTankUnitData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.TankUnit, id)
end

---@return TerrainEleementData
function JsonDataMgr.GetTerrianEleementData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.TerrainEleement, id)
end

---@return EnemyUnitData
function JsonDataMgr.GetEnemyUnitData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.EnemyUnit, id)
end

---@return UnitSkillData
function JsonDataMgr.GetUnitSkillData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.UnitSkill, id)
end

---@return LanguageData
function JsonDataMgr.GetLanguageData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.Language, id)
end

---@return InitiativeSkillData
function JsonDataMgr.GetInitiativeSkillData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.InitiativeSkill, id)
end

---@return LevelData
function JsonDataMgr.GetLevelData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.Level, id)
end


--TODO 完善RecruitData
function JsonDataMgr.GetRecruitData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.RecruitData, id)
end

---@return ProfilePictureData
function JsonDataMgr.GetProfilePictureData(id)
    return JsonDataMgr.GetJsonData(DataConst.DataName.ProfilePicture , id)
end


---@return InitiativeSkillData[]
function JsonDataMgr.GetAllInitiativeSkillData()
    return this.loadedJson[DataConst.DataName.InitiativeSkill]
end


---@return ProfilePictureData[]
function JsonDataMgr.GetAllProfilePictureData()
    return this.loadedJson[DataConst.DataName.ProfilePicture]
end

---@return UnitSkillData[]
function JsonDataMgr.GetAllUnitSKillData()
    return this.loadedJson[DataConst.DataName.UnitSkill]
end

return JsonDataMgr