// ----------------------------------------------------------------------------
// <copyright file="ObjectDebugConsoleWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>24/05/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Cosmos.Console
{
    using System;

    using Assets.Extends.EXTools.Debug.Console;
    using Assets.Scripts.Tools.Debug;

    using UnityEditor;

    using UnityEngine;

    public class ObjectDebugConsoleWindow : EditorWindow
    {
        private bool findConsole = false;

        [MenuItem("Window/Tools/DebugConsole/ObjectConsole")]
        public static void Open()
        {
            var objectDebugConsoleWindow = GetWindow<ObjectDebugConsoleWindow>("ObjectConsole");
            objectDebugConsoleWindow.autoRepaintOnSceneChange = true;
        }

        private Vector2 scroll;

        private string SearchContent;

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;
            GUI.skin.box.richText = true;
            GUI.skin.textArea.richText = true;
            GUI.skin.textField.richText = true;
            GUI.skin.toggle.richText = true;
            GUI.skin.window.richText = true;

            if (DebugConsole.consoleImpl == null)
            {
                findConsole = false;
                return;
            }
            var debugConsole = DebugConsole.consoleImpl as FullDebugConsole;
            if (debugConsole == null)
            {
                findConsole = false;
                return;
            }
            if (!Application.isPlaying)
            {
                findConsole = false;
                return;
            }

            if (!findConsole)
            {
                findConsole = true;
                debugConsole.AnalyseDisplayer.ShowNewObject(debugConsole.ObjectList, "Debug");
            }

            var defaultHeight = debugConsole.DefaultHeight;
            try
            {
                this.ShowObjectFinder(debugConsole);
                debugConsole.DefaultHeight = (int)(Screen.height * 1.4f);

                scroll = GUILayout.BeginScrollView(scroll);
                debugConsole.AnalyseDisplayer.Show();
                GUILayout.EndScrollView();

                if (Event.current.keyCode == KeyCode.R && Event.current.type == EventType.KeyDown)
                {
                    Event.current.Use();
                    debugConsole.AnalyseDisplayer.ShowNewObject(debugConsole.ObjectList, "Debug");
                }
                else if ((Event.current.keyCode == KeyCode.Q || Event.current.keyCode == KeyCode.Escape) && Event.current.type == EventType.KeyDown)
                {
                    Event.current.Use();
                    debugConsole.AnalyseDisplayer.Back();
                    this.Focus();
                }
            }
            catch (Exception e)
            {
//                Debug.Log(e);
            }
            //还原
            debugConsole.DefaultHeight = defaultHeight;
        }

        private void ShowObjectFinder(FullDebugConsole debugConsole)
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            this.doFileString = EditorGUILayout.TextField(this.doFileString, (GUIStyle)"ToolbarSeachTextField",GUILayout.Width(200));
            
            if (GUILayout.Button("", (GUIStyle)"ToolbarSeachCancelButton"))
            {
                this.SearchContent = "";
                GUIUtility.keyboardControl = 0;
            }
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                debugConsole.AnalyseDisplayer.ShowNewObject(debugConsole.ObjectList, "Debug");
            }

            if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                debugConsole.Clear(2);
            }

            if (GUILayout.Button("DoFile", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                if (LuaDebugConsole.LuaManager != null)
                {
                    try
                    {
                        var luaFunction = LuaDebugConsole.LuaManager.GetFunction("XQRequireRedo");
                        var objects = luaFunction.Invoke<string, object[]>(doFileString);
                        DebugConsole.Log(objects);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }

                }
            }

            

            GUILayout.EndHorizontal();
        }

        private string doFileString
        {
            get
            {
                return EditorPrefs.GetString(Application.dataPath + "ObjectDebugConsoleWindowdoFileString", "");
            }
            set
            {
                EditorPrefs.SetString(Application.dataPath + "ObjectDebugConsoleWindowdoFileString", value);
            }
        }
    }
}