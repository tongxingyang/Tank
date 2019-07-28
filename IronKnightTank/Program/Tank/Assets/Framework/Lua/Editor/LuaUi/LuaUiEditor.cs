
// ----------------------------------------------------------------------------
// <copyright file="LuaUiEditor.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>21/12/2015</date>
// ----------------------------------------------------------------------------

namespace Assets.Framework.Lua.Editor.LuaUi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Assets.Framework.Lua.Editor.Util;
    using Assets.Framework.Lua.LuaUi;
    using Assets.Framework.Lua.LuaUi.Event;
    using Assets.Tools.Script.Editor.Inspector.Field;
    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Editor.Window;
    using Assets.Tools.Script.Helper;

    using UnityEditor;

    using UnityEngine;

    using Editor = UnityEditor.Editor;
    using Object = UnityEngine.Object;

    [CustomEditor(typeof(LuaUi), true)]
    public class LuaUiEditor : Editor
    {
        public static LuaUi CurrUi;
        public static LuaUi CopyUi;

        public static string[] Events = null;

        private UnityEngine.Object defaultObj;

        private OnGUIUtility guiUtility = new OnGUIUtility();

        private Dictionary<string, LuaUi.LuaUiField> fields = new Dictionary<string, LuaUi.LuaUiField>();

        private List<string> eventList = new List<string>();

        private string[] splitChar = new[] { "\r\n" };

        private Color areaColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);

        private string cachePath;

        void OnEnable()
        {
            this.defaultObj = CreateInstance(typeof(ScriptableObject));
        }

        public override void OnInspectorGUI()
        {
            if (Events == null)
            {
                this.ReadEventLabels();
            }
            try
            {
                var s = this.defaultObj.name;
            }
            catch (Exception)
            {
                this.defaultObj = CreateInstance(typeof(ScriptableObject));
            }
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;
            GUI.skin.box.richText = true;
            GUI.skin.textArea.richText = true;
            GUI.skin.textField.richText = true;
            GUI.skin.toggle.richText = true;
            GUI.skin.window.richText = true;

            var luaUi = this.target as LuaUi;
            CurrUi = luaUi;
            if (luaUi.FileName.IsNullOrEmpty())
            {
                var luauiName = luaUi.gameObject.name;
                luaUi.FileName = luauiName;
//                luaUi.FileName = luauiName.EndsWith("Panel") ? luauiName : string.Format("{0}Panel", luauiName);
                luaUi.FileName = luaUi.FileName.Replace("_dynamic", "");
                luaUi.FileName = luaUi.FileName.Replace("_Dynamic", "");
                luaUi.FileName = luaUi.FileName.Replace("dynamic_", "");
                luaUi.FileName = luaUi.FileName.Replace("Dynamic_", "");

            }
            luaUi.FileName = EditorGUILayout.TextField("FileName", luaUi.FileName);

            
            var currObject = PrefabUtility.FindPrefabRoot(luaUi.gameObject);
            if (currObject.scene.isLoaded)
            {
                //Scene
                var prefab = PrefabUtility.GetPrefabParent(currObject);
                var prefabPath = AssetDatabase.GetAssetPath(prefab);
				if (prefabPath.StartsWith(XQFramework.FrameworkConst.GameResourceRootDir))
                {
					prefabPath = prefabPath.Replace(XQFramework.FrameworkConst.GameResourceRootDir, "");
                }
                if (prefab == null || prefabPath != luaUi.PrefabPath)
                {
                    //Miss prefab
                    luaUi.PrefabPath = prefabPath;
                    luaUi.PrefabPath = EditorGUILayout.TextField("Prefab", luaUi.PrefabPath);
                }
                else
                {
                    //Assets/GameResource/Prefab/New Folder/TestView.prefab
                    luaUi.PrefabPath = EditorGUILayout.TextField("Prefab", luaUi.PrefabPath);
                }
            }
            else
            {
                //AssetDatabase
            }

            this.ShowFields(luaUi);
            this.ShowEvents(luaUi);
            this.ShowLife(luaUi);

            var luaUiFields = luaUi.Fields;
            this.fields.Clear();
            string error="";
            foreach (var luaUiField in luaUiFields)
            {
                if (luaUiField.FieldName.IsNullOrEmpty())
                {
                    error = "未填写变量名";
                }
                else if (this.fields.ContainsKey(luaUiField.FieldName))
                {
                    error = "变量名重复";
                }
                else
                {
                    this.fields.Add(luaUiField.FieldName, luaUiField);
                }
            }
            if (error.IsNOTNullOrEmpty())
            {
                EditorGUILayout.HelpBox(error, MessageType.Error);
            }
            else
            {
                GUILayout.BeginHorizontal();
                //
                string findPath = this.cachePath;
                if (findPath == null)
                {
                    string[] findAssets = null;
                    if (luaUi.FileName.IsNOTNullOrEmpty())
                    {
                        findAssets = AssetDatabase.FindAssets(luaUi.FileName);
                    }
                    if (findAssets != null)
                    {
                        foreach (var findAsset in findAssets)
                        {
                            var assetPath = AssetDatabase.GUIDToAssetPath(findAsset);
                            if (assetPath.StartsWith("Assets/StreamingAssets/"))
                            {
                                continue;
                            }
                            if (assetPath.EndsWith(string.Format("/{0}.lua", luaUi.FileName)))
                            {
                                findPath = assetPath;
                            }
                        }
                        this.cachePath = findPath;
                    }
                    else
                    {
                        this.cachePath = string.Empty;
                    }
                }
                else
                {
                    var oldfile = Path.GetFileNameWithoutExtension(findPath);
                    if (oldfile != luaUi.FileName) findPath = this.cachePath = null;
                }
                if (this.cachePath.IsEmpty()) findPath = null;       //新建的获取一次后就不在进行检测（卡顿严重），直到导出后再检测一次

                string fileName = null;
                string panelPath = null;
                if (findPath != null)
                {
                    var substring = Application.dataPath.Substring(0, Application.dataPath.Length - 6);

                    var s = substring + findPath;
                    var strings = s.Split('/');
                    fileName = strings[strings.Length - 1];
                    panelPath = strings.ToList().GetRange(0, strings.Length - 1).Joint("/") + "/";
                }

                if (findPath == null)
                {
                    if (GUITool.Button("Export", ColorTool.GetColorFromRGBHexadecimal("6ce26c")))
                    {
                        ExportMonoUiLuaFile.ExportLuaUiFile(luaUi);
                        AssetDatabase.Refresh();
                        this.cachePath = null;
                    }
                }
                else
                {
                    if (GUITool.Button("Replace", ColorTool.GetColorFromRGBHexadecimal("6ce26c")))
                    {
                        ExportMonoUiLuaFile.ExportLuaUiFile(luaUi, panelPath, fileName);
                        AssetDatabase.Refresh();
                    }
                    if (GUITool.Button("Open", ColorTool.GetColorFromRGBHexadecimal("007acc")))
                    {
                        OpenFileUtil.OpenLuaFile(fileName, 0);
                        //                        var loadAssetAtPath = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(findPath);
                        //                                                AssetDatabase.OpenAsset(loadAssetAtPath);
                    }
                }
                if (GUITool.Button("Preview", ColorTool.GetColorFromRGBHexadecimal("a31515")))
                {

                    var exportLuaUiFile = ExportMonoUiLuaFile.PreviewLuaFile(luaUi, panelPath, fileName);
                    var popCustomWindow = PopCustomWindow.ShowPopWindow(new Vector2(800,600));
                    popCustomWindow.DrawGUI = () =>
                        {
                            EditorGUILayout.TextArea(exportLuaUiFile);
                        };
                }

                if (GUITool.Button("≡", ColorTool.GetColorFromRGBHexadecimal("a31515"), GUILayout.Width(20)))
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Copy"), false, this.Copy);
                    menu.AddItem(new GUIContent("Paste"), false, this.Paste);

                    menu.ShowAsContext();
                }
                GUILayout.EndHorizontal();
            }
            EditorUtility.SetDirty(luaUi);  //这块也有卡顿，不过目前没法细分哪些是被改变的
        }

        private void ReadEventLabels()
        {
//            eventList.Clear();
            Events = LuaEventTriggerHelper.GetEventList().ToArray();
            //            var fieldInfos = typeof(LuaUi.LuaUiEvent).GetFields(BindingFlags.Static|BindingFlags.Public);
            //            foreach (var fieldInfo in fieldInfos)
            //            {
            //                eventList.Add(fieldInfo.Name);
            //            }
            //
            //            Events = strings
        }

        private void ShowLife(LuaUi luaUi)
        {
            if (luaUi.Life == null)
            {
                luaUi.Life = new LuaUi.LuaUiLife();
                luaUi.Life.Dispose = true;
            }

            if (this.guiUtility.OpenClose("Messages", "flow overlay box", "LuaUiEditorLifeCallback"))
            {
                FieldInspectorTool.ShowObject(luaUi.Life);
            }
        }

        private void ShowFields(LuaUi luaUi)
        {
            if (luaUi.Fields == null)
            {
                luaUi.Fields = new List<LuaUi.LuaUiField>();
            }
            if (this.defaultObj != null)
            {
                this.defaultObj.name = "Drag in and add field";
            }

            if (this.guiUtility.OpenClose("Fields", "flow overlay box", "LuaUiEditorFields"))
            {
                for (int i = 0; i < luaUi.Fields.Count; i++)
                {
                    var luaUiField = luaUi.Fields[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUITool.GetAreaGUIStyle(this.areaColor));
                    FieldInspectorTool.ShowObject(luaUiField);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.Width(10));
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        luaUi.Fields.RemoveAt(i);
                        i--;
                    }
                    GUI.color = Color.yellow;
                    if (i + 1 < luaUi.Fields.Count && GUILayout.Button(new GUIContent("I", "追加插入"), GUILayout.Width(20)))
                    {
                        luaUi.Fields.Insert(i + 1, new LuaUi.LuaUiField());
                    }
                    GUI.color = Color.white;
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }


                GUILayout.BeginHorizontal();
                var objectField = EditorGUILayout.ObjectField(this.defaultObj, typeof(Object), true);
                if (objectField != this.defaultObj)
                {
                    string[] typeNames;
                    Object[] objects;
                    LuaUiFieldInspector.GetSelectableObjects(objectField, out objects, out typeNames);
                    if (objects.Length == 1)
                    {
                        luaUi.Fields.Add(new LuaUi.LuaUiField() { Source = objectField, FieldType = objects[0] });
                    }
                    else if (objects.Length > 1)
                    {
                        GenericMenu menu = new GenericMenu();
                        GenericMenu.MenuFunction2 selectType = (t) =>
                            {
                                var o = t as Object;
                                luaUi.Fields.Add(new LuaUi.LuaUiField() { Source = objectField, FieldType = o });
                            };
                        for (int i = 0; i < objects.Length; i++)
                        {
                            var o = objects[i];
                            menu.AddItem(new GUIContent(typeNames[i]), false, selectType, o);
                        }
                        menu.ShowAsContext();
                    }
                }
                GUILayout.EndHorizontal();
            }
            
        }

        private void ShowEvents(LuaUi luaUi)
        {
            if (luaUi.Events == null)
            {
                luaUi.Events = new List<LuaUi.LuaUiEvent>();
            }
            if (this.defaultObj != null)
            {
                this.defaultObj.name = "Drag in and add event";
            }

            if (this.guiUtility.OpenClose("Events", "flow overlay box", "LuaUiEditorEvents"))
            {
                for (int i = 0; i < luaUi.Events.Count; i++)
                {
                    var luaUiEvent = luaUi.Events[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUITool.GetAreaGUIStyle(this.areaColor));
                    FieldInspectorTool.ShowObject(luaUiEvent);
                    GUILayout.EndVertical();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        luaUi.Events.RemoveAt(i);
                        i--;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }


                GUILayout.BeginHorizontal();
                var objectField = EditorGUILayout.ObjectField(this.defaultObj, typeof(Object), true);
                if (objectField != this.defaultObj && objectField is GameObject)
                {
                    GenericMenu menu = new GenericMenu();
                    GenericMenu.MenuFunction2 selectType = (t) =>
                    {
                        var selectEventName = t as string;
                        luaUi.Events.Add(new LuaUi.LuaUiEvent() { Node = objectField as GameObject,EventType = selectEventName });
                    };
                    for (int i = 0; i < Events.Length; i++)
                    {
                        var o = Events[i];
                        menu.AddItem(new GUIContent(o), false, selectType, o);
                    }
                    menu.ShowAsContext();

                }
                GUILayout.EndHorizontal();
            }
        }

        private void Copy()
        {
            CopyUi = CurrUi;
        }

        private void Paste()
        {
            CurrUi.FileName = CopyUi.FileName;
            CurrUi.Fields = new List<LuaUi.LuaUiField>();
            CurrUi.Events = new List<LuaUi.LuaUiEvent>();

            //Paste fields
            foreach (var copyFiled in CopyUi.Fields)
            {
                var currField = new LuaUi.LuaUiField()
                {
                    FieldName = copyFiled.FieldName,
                    FieldType = copyFiled.FieldType,
                    Source = copyFiled.Source
                };

                if (copyFiled.Source is GameObject)
                {
                    var copyFieldSource = copyFiled.Source as GameObject;
                    if (copyFieldSource.gameObject == CopyUi.gameObject)
                    {
                        currField.Source = CurrUi.gameObject;
                        if (copyFiled.FieldType is GameObject)
                        {
                            currField.FieldType = currField.Source;
                        }
                    }
                    else if (copyFieldSource.transform.IsChildOf(CopyUi.transform))
                    {
                        var path = ExportMonoUiLuaFile.GetPath(CopyUi.gameObject, copyFieldSource);
                        var currFieldSource = CurrUi.transform.Find(path).gameObject;
                        currField.Source = currFieldSource;

                        if (copyFiled.FieldType is GameObject)
                        {
                            currField.FieldType = currField.Source;
                        }
                        else
                        {
                            var components = copyFieldSource.GetComponents(copyFiled.FieldType.GetType());
                            var findIndex = components.ToList().FindIndex(e => e == copyFiled.FieldType);
                            var component = currFieldSource.GetComponents(copyFiled.FieldType.GetType())[findIndex];
                            currField.FieldType = component;
                        }
                    }
                }
                //默认处理

                CurrUi.Fields.Add(currField);
            }

            //Paste events
            foreach (var copyEvent in CopyUi.Events)
            {
                var currEvent = new LuaUi.LuaUiEvent()
                {
                    Node = copyEvent.Node,
                    EventType = copyEvent.EventType
                };

                if (copyEvent.Node == CopyUi.gameObject)
                {
                    currEvent.Node = CurrUi.gameObject;
                }
                else if (copyEvent.Node.transform.IsChildOf(CopyUi.transform))
                {
                    var path = ExportMonoUiLuaFile.GetPath(CopyUi.gameObject, copyEvent.Node);
                    var currEventNode = CurrUi.transform.Find(path).gameObject;
                    currEvent.Node = currEventNode;
                }
                //默认处理

                CurrUi.Events.Add(currEvent);
            }
        }
    }

    [InitializeOnLoad]
    public class HierarchyIcons
    {

        static HierarchyIcons()
        {
            EditorApplication.hierarchyWindowItemOnGUI += ShowIcon;
        }

        static void ShowIcon(int ID, Rect r)
        {
            var go = EditorUtility.InstanceIDToObject(ID) as GameObject;
            if (go == null || go.GetComponent<LuaUi>() == null) return;
            r.x = r.xMax - 18;
            r.width = 18;
            GUI.Label(r, "ui");
        }
    }
}
