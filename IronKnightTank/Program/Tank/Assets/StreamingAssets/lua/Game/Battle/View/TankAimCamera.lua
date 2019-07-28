---@class TankAimCamera
local TankAimCamera = class('TankAimCamera',uView)
TankAimCamera.ViewPrefabPath = 'Prefab/Battle/View/TankAimCamera.prefab'

---@type GameObject
TankAimCamera.gameObject = nil

---@type Transform
TankAimCamera.transform = nil

---@type CombatAimCamera
TankAimCamera.camCamera = nil

---@type Transform
TankAimCamera.anchorTransform = nil


--==userCode==--

TankAimCamera.TankMeshCache = nil
TankAimCamera.CurTankMesh = nil

local receiveCommand = {
    require("Game.Event.Message.Battle.MSGShowHitPos")
}

--==userCode==--

function TankAimCamera:Init()
    self.TankMeshCache = {}
    self:_registerCommandHandler()
end

function TankAimCamera:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = object:GetComponent(LuaUiClassType)
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
    self.camCamera = luaUiFields[1]
    self.anchorTransform = luaUiFields[2]

end

function TankAimCamera:Dispose()
    self:_unregisterCommandHandler()
    self.CurTankMesh = nil
    self.TankMeshCache = nil
end




--==userCode==--

function TankAimCamera:_registerCommandHandler()
    EventBus:RegisterSelfReceiver(receiveCommand,self )
end

function TankAimCamera:_unregisterCommandHandler()
    EventBus:UnregisterSelfReceiver(receiveCommand,self)
end
---------------------------------------------------------
---event handler
---------------------------------------------------------
------MSGShowHitPos
---@param msg MSGShowHitPos
function TankAimCamera:OnMSGShowHitPos(msg)
    msg:Pend()
    local angle = msg.Angle -  self.CurTankMesh.transform.eulerAngles
    self.anchorTransform:DTLocalRotate(angle , msg.Time , function ()
        msg:Restore()
    end)
end
---------------------------------------------------------

---@param tankView TankView
---@param msg MSGOpenAimTankCam
function TankAimCamera:SetTank(tankView , msg)
    coroutine.createAndRun(function ()
        GameObjectUtilities.SetTransformParentAndNormalization(tankView.transform,self.anchorTransform)
        GameObjectUtilities.SetLayerWidthChildren(tankView.gameObject ,9 )
        tankView:CloseMoveEffect()
        tankView.transform.localEulerAngles = msg.TankAngles +tankView.tankAnchorTransform.localEulerAngles
        tankView.tankAnchorTransform.localEulerAngles = Vector3.New(0 ,0 ,0 )
        self.anchorTransform.localEulerAngles = Vector3.New(0 , 0, 0)
        local tTank = JsonDataMgr.GetTankUnitData( tankView._tankData.TankData.Id)
        local tankModelPath = tTank.Tank_Model
        local matPath1 = "Material/TankWire/".. tankModelPath.."_Frame_1.mat"
        local matPath2 = "Material/TankWire/".. tankModelPath.."_Frame_2.mat"
        local tankMesh = self:GetTankMeshPrefab(tankView._tankData , matPath2)

        tankMesh.transform.localEulerAngles = tankView.transform.localEulerAngles
        local mat1 = ResMgr.LoadMaterialYield(nil , matPath1)
        tankView:SetMaterial(mat1)
        self.CurTankMesh = tankMesh
        self.anchorTransform:DTCircle(Vector3.New( 0  , 720 , 0) , msg.RotateTime)
    end)
end

function TankAimCamera:CloseTankView()
    self.CurTankMesh.gameObject:SetActive(false)
end

function TankAimCamera:Close()
    self:CloseTankView()
    self.gameObject:SetActive(false)
end

---@param tankData BaseFightTank
function TankAimCamera:GetTankMeshPrefab(tankData ,matPath )
    local tTank = JsonDataMgr.GetTankUnitData( tankData.TankData.Id)
    local tankModelPath = tTank.Tank_Model

    tankModelPath = "Prefab/Tank/"..tankModelPath..".prefab"
    local tankMesh =  self.TankMeshCache[tankModelPath]
    if not tankMesh then
        local tank =  ResMgr.InstantiatePrefabYield(nil , tankModelPath , self.anchorTransform )
        GameObjectUtilities.SetLayerWidthChildren(tank ,9 )
        tankMesh = tank:GetComponent("TankMesh")
        --local mat2 = ResMgr.LoadMaterialYield(nil , matPath)
        ResMgr.LoadMaterial(matPath , function (mat2)
            tankMesh:SetMaterial(mat2)
        end)

        self.TankMeshCache[tankModelPath] = tankMesh
    end
    tankMesh.gameObject:SetActive(true)
    return tankMesh
end

function TankAimCamera:Dispose()
    self.TankMeshCache = nil
    self.CurTankMesh = nil
end

--==userCode==--

return TankAimCamera
