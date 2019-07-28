// ----------------------------------------------------------------------------
// <copyright file="ProjectTortoiseSVNMenu.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>12/03/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Editor.Tools
{
    using System;
    using System.Collections.Generic;

    using Assets.ThridPartyTool.Tools.TortoiseSVN.Editor;
    using Assets.Tools.Script.Editor.Tool;

    using ParadoxNotion.Serialization;

    using UnityEditor;

    using UnityEngine;
    
    public class TortoiseSVNMenuEx : ITortoiseSVNSettingEditor
    {

        [MenuItem("Assets/TortoiseSVN/Project Commit", false, 115)]
        [MenuItem("SVN/Project Exploit/Commit", false, 116)]
        private static void SVNCommit()
        {
            TortoiseSVNMenu.TortoiseSVNCommit(SVNPath.ToArray());
        }

        [MenuItem("Assets/TortoiseSVN/Project Update", false, 114)]
        [MenuItem("SVN/Project Exploit/Update", false, 117)]
        private static void SVNUpdate()
        {
            TortoiseSVNMenu.TortoiseSVNUpdate(SVNPath.ToArray());
        }
        [MenuItem("SVN/Project Exploit/Setting Window", false, 118)]
        private static void SVNSetting()
        {
            TortoiseSVNMenu.OpenSettingWindow();
        }

        private static string keyOfSVNPath
        {
            get
            {
                return String.Format("{0}ProjectTortoiseSVNMenuKey", Application.dataPath);
            }
        }

        [MenuItem("SVN/Update battle", false, 112)]
        private static void SVNBattleConfigUpdate()
        {
            TortoiseSVNMenu.TortoiseSVNUpdate(GetBattlePath());
        }

        [MenuItem("SVN/Commit battle", false, 113)]
        private static void SVNBattleConfigCommit()
        {
            TortoiseSVNMenu.TortoiseSVNCommit(GetBattlePath());
        }

        [MenuItem("SVN/Lua/Update", false, 114)]
        private static void SVNLuaUpdate()
        {
            TortoiseSVNMenu.TortoiseSVNUpdate(new[] { "Assets/GameResource/Lua" });
        }

        [MenuItem("SVN/Lua/Commit", false, 115)]
        private static void SVNLuaCommit()
        {
            TortoiseSVNMenu.TortoiseSVNUpdate(new[] { "Assets/GameResource/Lua" });
        }

        private static string[] GetBattlePath()
        {
            return new[]
                       {
                           "Assets/GameResource/Data/UnitConfig",
                           "Assets/GameResource/Data/Skill",
                           "Assets/GameResource/Data/AIConfig",
                           "Assets/GameResource/Data/BattleConfig/Battle",
                           "Assets/GameResource/Lua/User/BattleSystem/Script",
                           "Assets/Doc",
                           "Assets/Editor",
                           "Assets/ThridPartyTool/Tools/TortoiseSVN/Editor/TortoiseSVNMenu.cs",
                       };
        }

        private static List<string> SVNPath
        {
            get
            {
                var hasKey = EditorPrefs.HasKey(keyOfSVNPath);
                if (hasKey)
                {
                    return JSON.Deserialize<List<string>>(EditorPrefs.GetString(keyOfSVNPath));
                }
                List<string> path = new List<string>()
                                    {
                                        "Assets/GameResource",
                                        "Assets/Editor",
                                        "Assets/Scripts",
                                        "Assets/ThridPartyTool",
                                        "Assets/LuaFramework/Editor/CustomSettings.cs",
                                    };
                EditorPrefs.SetString(keyOfSVNPath, JSON.Serialize<List<string>>(path));
                return path;
            }
            set
            {
                EditorPrefs.SetString(keyOfSVNPath, JSON.Serialize<string[]>(value));
            }
        }

        public void ShowTortoiseSVNSetting()
        {
            var svnPath = SVNPath;
            GUILayout.Label("Project Commit/Update 目录".SetSize(14, true));
            for (int i = 0; i < svnPath.Count; i++)
            {
                GUILayout.BeginHorizontal();
                svnPath[i] = EditorGUILayout.TextField(svnPath[i]);
                if (GUILayout.Button("X", GUILayout.Width(30)))
                {
                    svnPath.RemoveAt(i);
                    i--;
                }
                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add"))
            {
                svnPath.Add("");
            }

            SVNPath = svnPath;
        }

        public int GetTortoiseSVNSettingOrder()
        {
            return 5;
        }
    }
}