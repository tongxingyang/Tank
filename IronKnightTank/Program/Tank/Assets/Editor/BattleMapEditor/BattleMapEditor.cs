// ----------------------------------------------------------------------------
// <copyright file="MapEditorWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>09/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Editor.BattleMapEditor { 
    using UnityEngine;
    using UnityEditor;
    using LuaInterface;
    using Assets.Tools.Script.Core.File;
    using System.IO;
    using Assets.Framework.Lua.Editor.Util;
    using Assets.Framework.LetsScript.Editor;

    public class BattleMapEditor :  Editor{

        [MenuItem("Assets/MapEidtor/Open %q")]
        public static void OpenBattleMapEditorWin()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var fullPath = FileUtility.GetFullPath(path);
            var luacode = File.ReadAllText(fullPath);
            var table = EditorLuaState.lua.DoString<LuaTable>(luacode);
            var json = EditorLuaState.jsonEncode.Invoke<LuaTable, string>(table);
            Debug.Log(json);
            BattleMap map = BattleMap.DeserializeJson(json);
            
            map.FillMap();
            EditorWindow.GetWindow<BattleMapEditorWindow>().Init(map, fullPath);
        }

        [MenuItem("Assets/MapEidtor/Create")]
        public static void CreateTestScript()
        {
            LetsScriptEditor.CreateScript("MapConfigTemplate");
        }
    }
}