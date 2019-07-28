---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by thrt520.
--- DateTime: 2018/4/13 11:32
---
---@class GameResMgr

local GameResMgr = {}
local this = GameResMgr
---@type table
this.callBackDic = {}
local CSharpResMgr = ResourcesManager.Instance ---C#

---@param savePath 存档位置
---@param loadCallBack 回调
function GameResMgr.LoadSave(savePath, loadCallBack)
    if not savePath then
        clog("textPath nil wrong")
        return
    end
    SaveManager.Instance:LoadSave(savePath, this._handleCallBack(loadCallBack))
end

---@param savePath 存档位置
---@param saveData 保存数据
function GameResMgr.DoSave(savePath, saveData)
    if not savePath then
        clog("textPath nil wrong")
        return
    end
    SaveManager.Instance:DoSave(savePath, saveData)
end

---@param string prefabPath
---@param loadCallBack  func(go : Object)
function GameResMgr.LoadObject(path, loadCallBack)
    if not path or path == "" then
        clog("path nil wrong")
        return
    end
    CSharpResMgr:LoadObject(path, this._handleCallBack(loadCallBack))
end

---@param string prefabPath
---@param loadCallBack  func(go : GameObject)
function GameResMgr.LoadPrefab(prefabPath, loadCallBack)
    if not prefabPath or prefabPath == "" then
        clog("prefab path nil wrong")
        return
    end
    CSharpResMgr:LoadPrefab(prefabPath, this._handleCallBack(loadCallBack))
end

---@param string prefabPath
---@param loadCallBack  func(go : GameObject)
function GameResMgr.InstantiatePrefab(prefabPath, parent, loadCallBack)
    this.LoadPrefab(prefabPath, function(prefab)
        local clonePrefab
        if parent ~= nil then
            clonePrefab = GameObject.Instantiate(prefab, parent)
        else
            clonePrefab = GameObject.Instantiate(prefab)
        end
        loadCallBack(clonePrefab)
    end)
end

function GameResMgr.InstantiatePrefabYield(co, prefabPath, parent)
    return coroutine.callwait(co, GameResMgr.InstantiatePrefab, 3, prefabPath, parent)
end

---@param string texturePath
---@param loadCallBack  func(texture : Texture)
function GameResMgr.LoadTexture(texturePath, loadCallBack)
    if not texturePath or texturePath == "" then
        clog("texturePath nil wrong")
        return
    end
    CSharpResMgr:LoadTexture(texturePath, this._handleCallBack(loadCallBack))
end

---@param string texturePath
---@param rawImage  RawImage
function GameResMgr.SetRawImageTexture(texturePath, rawImage)
    if rawImage == nil then
        clog("rawImage nil wrong")
        return
    end
    this.LoadTexture(texturePath, function(obj)
        rawImage.texture = obj
    end)
end

---@param string spritePath
---@param loadCallBack  func(sprite:Sprite)
function GameResMgr.LoadSprite(spritePath, loadCallBack)
    if not spritePath or spritePath == "" then
        clog("spritePath nil wrong")
        return
    end
    CSharpResMgr:LoadSprite(spritePath, this._handleCallBack(loadCallBack))
end

function GameResMgr.LoadSpriteYield(co, spritePath)
    return coroutine.callwait(co, GameResMgr.LoadSprite, 2, spritePath)
end

---@param string spritePath
---@param image  Image
function GameResMgr.SetImageSprite(spritePath, image)
    if image == nil then
        clog("image nil wrong")
        return
    end
    this.LoadSprite(spritePath, function(sprite)
        image.sprite = sprite
    end)
end

---@param string matPath
---@param loadCallBack func(mat:Material)
function GameResMgr.LoadMaterial(matPath, loadCallBack)
    if not matPath or matPath == "" then
        clog("matPath nil wrong")
        return
    end
    CSharpResMgr:LoadMaterial(matPath, this._handleCallBack(loadCallBack))
end

function GameResMgr.LoadMaterialYield(co, matPath)
    return coroutine.callwait(co, GameResMgr.LoadMaterial, 2, matPath)
end

---@param string audioPath
---@param loadCallBack func(audioClip:AudioClip)
function GameResMgr.LoadAudioClip(audioPath, loadCallBack)
    if not audioPath then
        clog("audioPath nil wrong")
        return
    end
    CSharpResMgr:LoadAudioClip(audioPath, this._handleCallBack(loadCallBack))
end

---@param string textPath
---@param loadCallBack func(textAssset:TextAsset)
function GameResMgr.LoadText(textPath, loadCallBack)
    if not textPath then
        clog("textPath nil wrong")
        return
    end
    CSharpResMgr:LoadText(textPath, this._handleCallBack(loadCallBack))
end

---@param string spritePath
---@param SpriteRender spriteRender
function GameResMgr.SetSpriteRenderSprite(spritePath, spriteRender)
    if spriteRender == nil then
        clog("spriteRender nil wrong   path" .. spritePath)
        return
    end
    this.LoadSprite(spritePath, function(sprite)
        spriteRender.sprite = sprite
    end)
end

---@param string texturePath
---@param SpriteRender meshRender
function GameResMgr.SetMeshRendererTexture(texturePath, meshRender)
    if meshRender == nil then
        clog("meshRender nil wrong   path" .. texturePath)
        return
    end
    this.LoadTexture(texturePath, function(texture)
        meshRender.material.mainTexture = texture
    end)
end

---@param string matPath
---@param SpriteRender meshRender
function GameResMgr.SetMeshRendererMat(matPath, meshRender)
    if meshRender == nil then
        clog("meshRender nil wrong   path" .. matPath)
        return
    end
    this.LoadMaterial(matPath, function(mat)
        meshRender.material = mat
    end)
end

function GameResMgr._handleCallBack(callBack)
    local newCallBack = function(obj)
        if obj then
            callBack(obj)
        end
        this.callBackDic[callBack] = nil
    end
    this.callBackDic[callBack] = newCallBack
    return newCallBack
end

return GameResMgr