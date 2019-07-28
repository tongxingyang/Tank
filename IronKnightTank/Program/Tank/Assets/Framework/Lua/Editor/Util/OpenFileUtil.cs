// ----------------------------------------------------------------------------
// <copyright file="OpenFileUtil.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>12/01/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.Lua.Editor.Util
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// 文件打开工具
    /// </summary>
    public class OpenFileUtil
    {
        private static EditorWindow consoleWindow;

        /// <summary>
        /// 打开lua文件
        /// </summary>
        /// <param name="fileName">文件名（不带后缀）</param>
        /// <param name="line">行号</param>
        public static void OpenLuaFile(string fileName, int line)
        {
            if (fileName.EndsWith(".lua"))
            {
                fileName = fileName.Replace(".lua", "");
            }
            var findAssets = AssetDatabase.FindAssets(fileName);

            foreach (var findAsset in findAssets)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(findAsset);
                if (assetPath.EndsWith(string.Format("/{0}.lua", fileName)) && !assetPath.Contains("StreamingAssets"))
                {
                    var loadAssetAtPath = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                    AssetDatabase.OpenAsset(loadAssetAtPath, line);
                }
            }
        }

        public static T FindFile<T>(string fileName, string extension) where T : UnityEngine.Object
        {
            var findAssets = AssetDatabase.FindAssets(fileName);

            foreach (var findAsset in findAssets)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(findAsset);
                if (assetPath.EndsWith(string.Format("/{0}.{1}", fileName, extension)))
                {
                    var loadAssetAtPath = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    return loadAssetAtPath;
                }
            }
            return null;
        }

        public static List<T> FindFiles<T>(string fileName, string extension) where T : UnityEngine.Object
        {
            List<T> list = new List<T>();
            var findAssets = AssetDatabase.FindAssets(fileName);

            foreach (var findAsset in findAssets)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(findAsset);
                if (assetPath.EndsWith(string.Format("/{0}.{1}", fileName, extension)))
                {
                    var loadAssetAtPath = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    list.Add(loadAssetAtPath);
                }
            }
            return list;
        }

        /// <summary>
        /// 是否存在lua文件
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if [has lua file] [the specified file name]; otherwise, <c>false</c>.</returns>
        public static bool HasLuaFile(string fileName)
        {
            if (fileName.EndsWith(".lua"))
            {
                fileName = fileName.Replace(".lua", "");
            }
            var findAssets = AssetDatabase.FindAssets(fileName);

            foreach (var findAsset in findAssets)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(findAsset);
                if (assetPath.EndsWith(string.Format("/{0}.lua", fileName)) && !assetPath.Contains("StreamingAssets"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据系统标准的console窗口当前选中打开文件
        /// </summary>
        [MenuItem("Window/Tools/OpenFileWithLog %g")]
        public static void OpenWithLog()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType("UnityEditor.ConsoleWindow");

                if (type != null)
                {

                    consoleWindow = EditorWindow.GetWindow(type);
                    var field = type.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    var value = field.GetValue(consoleWindow) as string;
                    OpenFile(value);
                }
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private static void OpenFile(string msg)
        {
            //是lua输出
            if (msg.Contains("stack traceback"))
            {
                OpenLuaFileWithLog(msg);
            }
            else if (msg.StartsWith("[luafile]"))
            {
                OpenLuaFileWithPathTag(msg);
            }
        }

        private static void OpenLuaFileWithPathTag(string msg)
        {
            msg = msg.Replace("[", "");
            var strings = msg.Split(']');
            string fileName = strings[1];
            int lineIndex = Convert.ToInt32(strings[2]);
            OpenLuaFile(fileName, lineIndex);
            consoleWindow.ShowNotification(new GUIContent(String.Format("{0} Line:{1}", fileName, lineIndex)));
        }

        /// <summary>
        /// 打开lua类文件
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private static void OpenLuaFileWithLog(string msg)
        {
            var strings = msg.Split(new string[] { "\n" }, StringSplitOptions.None);
            bool started = false;
            for (int index = 0; index < strings.Length; index++)
            {
                var line = strings[index];
                if (line.TrimStart().StartsWith("stack traceback:"))
                {
                    started = true;
                    continue;
                }
                if (!started)
                {
                    continue;
                }
                if (line.Contains("LogUtil"))
                {
                    continue;
                }

                if (line.Contains("event.lua"))
                {
                    continue;
                }

                if (line.Contains("coroutine.lua"))
                {
                    continue;
                }

                if (line.Contains("[C]:"))
                {
                    continue;
                }
                
                var split = line.Split(':');
                var fileName = split[0].Trim();
                var lineIndex = split[1].Trim();
                OpenLuaFile(fileName, 0);
                consoleWindow.ShowNotification(new GUIContent(string.Format("{0} Line:{1}", fileName, lineIndex)));
                break;
                //                string line = s.Replace("LuaException: [string \"system/event.lua\"]:165: [string \"system/coroutine.lua\"]:36:", "");
                //                line = line.Replace("LuaException: [string \"luaevent\"]:156:", "");
                //                line = line.Replace("LuaException: [string \"system/event.lua\"]:165: [string \"system/coroutine.lua\"]:59:", "");
                //                line = line.Replace("LuaException: [string \"system/event.lua\"]:165: [string \"system/coroutine.lua\"]:24:", "");
                //                line = line.Replace("LuaException: [string \"system/event.lua\"]:165:", "");
                //                line = line.Replace("LuaException: [string \"system/coroutine.lua\"]:24:", "");
                //                line = line.Replace("LuaException: [string \"system/coroutine.lua\"]:36:", "");
                //                line = line.Replace("LuaException: [string \"luaevent\"]:156: [string \"System/coroutine\"]:62:", "");
                //
                //                int lineIndex;
                //                var word = DoLuaTraceback(line, out lineIndex);
                //                if (word.IsNOTNullOrEmpty())
                //                {
                //                    if (word.Contains("LogUtil"))
                //                    {
                //                        continue;
                //                    }
                //                    
                //                    var formatWord = word.Replace("\\", "/");
                //                    var splitWords = formatWord.Split('/');
                //                    try
                //                    {
                //                        var fileName = splitWords[splitWords.Length - 1];
                //                        fileName = fileName.Replace("\"", "");
                //                        if (fileName.EndsWith(".lua"))
                //                        {
                //                            fileName = fileName.Replace(".lua", "");
                //                        }
                //                        if (fileName.Contains("."))
                //                        {
                //                            var split = fileName.Split('.');
                //                            fileName = split[split.Length - 1];
                //                        }
                //                        OpenLuaFile(fileName, lineIndex);
                //                        consoleWindow.ShowNotification(new MenuGUIContent(String.Format("{0} Line:{1}", fileName, lineIndex)));
                //                        break;
                //                    }
                //                    catch (Exception)
                //                    {
                //
                //                        throw;
                //                    }
                //                }
            }
        }

        private static string DoLuaTraceback(string txt, out int line)
        {
            line = 0;
            //        string txt = " [string \"asdcasdcasvfd\"]:22";

            string re1 = "(.*?)";  // White Space 1
            string re2 = "(\\[)";   // Any Single Character 1
            string re3 = "(string)";    // Word 1
            string re4 = "( )"; // White Space 2
            string re5 = "(\".*?\")";   // Double Quote String 1
            string re6 = "(\\])";   // Any Single Character 2
            string re7 = "(:)"; // Any Single Character 3
            string re8 = "(\\d+)";  // Integer Number 1

            Regex r = new Regex(re1 + re2 + re3 + re4 + re5 + re6 + re7 + re8, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(txt);
//            foreach (var group in m.Groups)
//            {
//                Debug.Log(group.ToString());
//            }
            if (m.Success)
            {
                string g = m.Groups[5].ToString();
                line = Convert.ToInt32(m.Groups[8].ToString());
                return g;
            }
            return null;
        }
    }
}