using System;

using Assets.Tools.Script.Editor.Tool;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

public class OnGUIUtility
{
    public bool OpenClose(string title,string style,string cacheName = null)
    {
        if (cacheName == null)
        {
            cacheName = title;
        }
        bool state = EditorPrefs.GetBool(cacheName, false);
        string text = title;
        if (state)
        {
            text = "\u25BC" + (char)0x200a + text;
        }
        else
        {
            text = "\u25BA" + (char)0x200a + text;
        }
        GUILayout.BeginHorizontal((GUIStyle)style);
        if (GUITool.Button(text, Color.clear))
        {
            EditorPrefs.SetBool(cacheName, !state);
        }
        GUILayout.EndHorizontal();
        state = EditorPrefs.GetBool(cacheName, false);
        return state;
    }

    public void ScriptField(string title, Type type)
    {
        ScriptField(title, type, GUI.skin.button);
    }
    public void ScriptField(string title,Type type,GUIStyle style)
    {
        string[] s = AssetDatabase.FindAssets(type.Name);
        Object editAsset = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(s[0]));
        GUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(title, editAsset, typeof(Object));
        if (GUILayout.Button("编辑脚本", style))
        {
            AssetDatabase.OpenAsset(editAsset);
        }
        GUILayout.EndHorizontal();
    }
}