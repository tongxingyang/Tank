// ----------------------------------------------------------------------------
// <copyright file="LetsScriptSettingWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>04/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Util
{
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor.Script;
    using Assets.Tools.Script.Editor.Tool;

    using UnityEditor;

    using UnityEngine;

    public class LetsScriptSettingWindow : EditorWindow
    {
        [MenuItem("Assets/LetsScript/Setting")]
        public static void OpenSetting()
        {
            LetsScriptSetting.Init();
            GetWindow<LetsScriptSettingWindow>();
        }


        private void OnGUI()
        {
            this.CommandPath();
            this.TriggerPoint();


            if (GUILayout.Button("保存"))
            {
                LetsScriptSetting.Save();
            }
            if (GUILayout.Button("刷新"))
            {
                LetsScriptEditor.InitEnvironment();
            }
        }

        private void TriggerPoint()
        {
            
            if (LetsScriptSetting.TriggerPoint == null)
            {
                LetsScriptSetting.TriggerPoint = new List<ScriptTriggerPoint>();
            }

            GUILayout.Label("事件触发点".SetSize(24,true));
            for (int i = 0; i < LetsScriptSetting.TriggerPoint.Count; i++)
            {
                var point = LetsScriptSetting.TriggerPoint[i];
                GUILayout.BeginHorizontal();

                point.Path = EditorGUILayout.TextField(point.Path, GUILayout.Width(300));
                GUILayout.Space(10);
                GUILayout.Label("ID");
                point.Name = EditorGUILayout.TextField(point.Name, GUILayout.Width(200));

                if (GUILayout.Button("X", GUILayout.Width(40)))
                {
                    LetsScriptSetting.TriggerPoint.RemoveAt(i);
                    i--;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("添加触发点"))
            {
                LetsScriptSetting.TriggerPoint.Add(new ScriptTriggerPoint());
            }
            GUILayout.Label("");
        }
        private void CommandPath()
        {
            if (LetsScriptSetting.LuaCommandPath == null)
            {
                LetsScriptSetting.LuaCommandPath = new List<string>();
            }
            GUILayout.Label("Lua命令脚本路径".SetSize(24, true));

            for (int i = 0; i < LetsScriptSetting.LuaCommandPath.Count; i++)
            {
                var commandPath = LetsScriptSetting.LuaCommandPath[i];
                GUILayout.BeginHorizontal();
                LetsScriptSetting.LuaCommandPath[i] = EditorGUILayout.TextField(commandPath);
                if (GUILayout.Button("X", GUILayout.Width(40)))
                {
                    LetsScriptSetting.LuaCommandPath.RemoveAt(i);
                    i--;
                }
                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("添加Lua命令路径"))
            {
                LetsScriptSetting.LuaCommandPath.Add("");
            }
            GUILayout.Label("");
        }
    }
}