using UnityEngine;
using UnityEditor;
using LuaInterface;
using Assets.Tools.Script.Core.File;
using System.IO;
using Assets.Framework.Lua.Editor.Util;
using Assets.Framework.LetsScript.Editor;
using Assets.Editor.CampaignMapEditor;

public class CampaignMapEditor
{
    [MenuItem("Assets/CampaignMapEidtor/Open %f")]
    public static void OpenBattleMapEditorWin()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var fullPath = FileUtility.GetFullPath(path);
        var luacode = File.ReadAllText(fullPath);
        var table = EditorLuaState.lua.DoString<LuaTable>(luacode);
        var json = EditorLuaState.jsonEncode.Invoke<LuaTable, string>(table);
        //Debug.Log(json);
        CampaignMap map = CampaignMap.DeserializeJson(json);
        EditorWindow.GetWindow<CampaignMapEditorWin>().Init(map , fullPath);

    }

    [MenuItem("Assets/CampaignMapEidtor/Create")]
    public static void CreateTestScript()
    {
        LetsScriptEditor.CreateScript("CampaignMapTemplate");
    }
}
