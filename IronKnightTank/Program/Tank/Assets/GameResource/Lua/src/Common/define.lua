GameObject = UnityEngine.GameObject
Object = UnityEngine.Object
GameObjectUtilities = Assets.Tools.Script.Helper.GameObjectUtilities
LuaUiClassType = typeof(Assets.Framework.Lua.LuaUi.LuaUi)
ResourcesManager = XQFramework.Resource.ResourcesManager
InputManager = Assets.Scripts.Game.Input.InputManager
UnitySceneManager = XQFramework.Scene.UnitySceneManager
TankTimeManager = Assets.Scripts.Game.Time.TankTimeManager
Screen = UnityEngine.Screen

require("Framework.Util.ReferenceCounter")
require("Framework.Util.Extend.classext")
require("Framework.Util.Logger")
require("Framework.Util.Extend.stringext")
require("Framework.Util.Extend.tableext")
require("Framework.Util.Extend.coroutineext")
require("Framework.Util.Timer")
require("Framework.Util.utf8")
require("Framework.Util.Structure.AsyncState")
require("Framework.Util.Structure.Queue")
require("Framework.Event.EventArgsBasics")
require("Framework.Event.Command")
require("Game.Event.GameEvent")
require("Game.JsonTool.ModelConfig.DataConst")
require("Game.JsonTool.JsonDataMgr")
require("Framework.UI.ViewManager")
require("Framework.UI.PanelManager")
require("Framework.UI.ViewProxy")
require("Framework.LetsTrigger.Script.LScript")
require("Game.Tools.MathTools")
ResMgr = require("Framework.Resource.GameResMgr")
LuaTools = require("Game.Tools.LuaTools")
LocalizationMgr = require("Game.Localization.LocalizationMgr")
require("Game.Main.Model.DataMgr")
