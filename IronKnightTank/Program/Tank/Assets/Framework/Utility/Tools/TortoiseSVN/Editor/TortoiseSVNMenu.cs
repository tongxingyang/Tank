// ----------------------------------------------------------------------------
// <copyright file="TortoiseSVNMenu.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>12/3/2016</date>
// ----------------------------------------------------------------------------

namespace Assets.ThridPartyTool.Tools.TortoiseSVN.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Reflec;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class TortoiseSVNMenu : EditorWindow
    {
        [MenuItem("Assets/TortoiseSVN/Setting", false, 119)]
        public static void OpenSettingWindow()
        {
            var tortoiseSvnMenu = GetWindow<TortoiseSVNMenu>("TortoiseSVN");
        }

        [MenuItem("Assets/TortoiseSVN/Commit", false, 113)]
        public static void SVNCommit()
        {
            TortoiseSVNCommit(GetSelectionPath());
        }

        [MenuItem("Assets/TortoiseSVN/Update", false, 112)]
        public static void SVNUpdate()
        {
            TortoiseSVNUpdate(GetSelectionPath());
        }


        public static void TortoiseSVNUpdate(string[] path)
        {
            RunCmd(TortoiseSVNSetting.TortoiseProcPath,
                string.Format("/command:update  /path:\"{0}\"", BuildPath(path)));
            AssetDatabase.Refresh();
        }

        public static void TortoiseSVNCommit(string[] path)
        {
            RunCmd(TortoiseSVNSetting.TortoiseProcPath,
                string.Format("/command:commit  /path:\"{0}\"", BuildPath(path)));
            AssetDatabase.Refresh();
        }


        public static string[] GetSelectionPath()
        {
            string[] path = new string[Selection.objects.Length];
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                var o = Selection.objects[i];
                var assetPath = AssetDatabase.GetAssetPath(o);
                path[i] = assetPath;
            }

            return path;
        }

        private static string BuildPath(string[] selectionPath)
        {
            StringBuilder pathBuilder = new StringBuilder();
            var dataPath = Application.dataPath;
            dataPath = Path.GetDirectoryName(dataPath);
            for (int i = 0; i < selectionPath.Length; i++)
            {
                var path = selectionPath[i];
                var directoryName = Path.GetDirectoryName(path);
                if (directoryName.StartsWith(dataPath))
                {
                    pathBuilder.Append(path);
                }
                else
                {
                    pathBuilder.Append(string.Format("{0}/{1}", dataPath, path));
                }

                if (i < selectionPath.Length - 1)
                {
                    pathBuilder.Append("*");
                }
            }

            return pathBuilder.ToString();
        }

        private static string GetFullPath(Object o)
        {
            var assetPath = AssetDatabase.GetAssetPath(o);
            var fullPath = string.Format("{0}{1}", Application.dataPath, assetPath.Substring(6, assetPath.Length - 6));
            return fullPath;
        }

        /// <summary>
        /// 运行cmd命令
        /// 会显示命令窗口
        /// </summary>
        /// <param name="cmdExe">指定应用程序的完整路径</param>
        /// <param name="cmdStr">执行命令行参数</param>
        static bool RunCmd(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    //指定启动进程是调用的应用程序和命令行参数
                    ProcessStartInfo psi = new ProcessStartInfo(cmdExe, cmdStr);
                    myPro.StartInfo = psi;
                    myPro.Start();
                    myPro.WaitForExit();
                    result = true;
                }
            }
            catch
            {
            }

            return result;
        }

        private static List<ITortoiseSVNSettingEditor> settingEditors;

        private static Vector2 scrollView;

        private void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;
            GUI.skin.box.richText = true;
            GUI.skin.textArea.richText = true;
            GUI.skin.textField.richText = true;
            GUI.skin.toggle.richText = true;
            GUI.skin.window.richText = true;

            scrollView = GUILayout.BeginScrollView(scrollView);
            if (settingEditors == null)
            {
                settingEditors = new List<ITortoiseSVNSettingEditor>();
                var types = AssemblyTool.FindTypesInCurrentDomainWhereExtend<ITortoiseSVNSettingEditor>();
                foreach (var type in types)
                {
                    var tortoiseSvnSettingEditor = ReflecTool.Instantiate(type) as ITortoiseSVNSettingEditor;
                    settingEditors.Add(tortoiseSvnSettingEditor);
                }

                settingEditors.Sort(
                    (l, r) => l.GetTortoiseSVNSettingOrder() - r.GetTortoiseSVNSettingOrder());
            }

            for (int i = 0; i < settingEditors.Count; i++)
            {
                var tortoiseSvnSettingEditor = settingEditors[i];
                tortoiseSvnSettingEditor.ShowTortoiseSVNSetting();
            }

            GUILayout.EndScrollView();
        }
    }

    public class TortoiseSVNSetting : ITortoiseSVNSettingEditor
    {
        public static string TortoiseProcPath
        {
            get
            {
                if (!EditorPrefs.HasKey(TortoiseProcPathKey))
                {
                    EditorPrefs.SetString(TortoiseProcPathKey, "TortoiseProc.exe");
                }

                return EditorPrefs.GetString(TortoiseProcPathKey);
            }
            set { EditorPrefs.SetString(TortoiseProcPathKey, value); }
        }

        private static string TortoiseProcPathKey
        {
            get
            {
                var key = string.Format("{0}TortoiseProc.exe 目录", Application.dataPath);
                return key;
            }
        }

        public void ShowTortoiseSVNSetting()
        {
            GUILayout.Label("TortoiseProc.exe 目录".SetSize(14, true));

            TortoiseProcPath = EditorGUILayout.TextField(TortoiseProcPath);
        }

        public int GetTortoiseSVNSettingOrder()
        {
            return 1;
        }
    }
}