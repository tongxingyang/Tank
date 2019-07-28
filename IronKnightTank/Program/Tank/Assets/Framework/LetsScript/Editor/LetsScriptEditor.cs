// ----------------------------------------------------------------------------
// <copyright file="LetsScriptEditor.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>03/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor
{
    using System.Reflection;

    using Assets.Framework.LetsScript.Editor.Renderer.Lua;
    using Assets.Framework.LetsScript.Editor.Script;
    using Assets.Framework.LetsScript.Editor.Script.Editor;
    using Assets.Tools.Script.Core.File;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// 脚本编辑器人口
    /// </summary>
    public class LetsScriptEditor
    {
        public static bool Inited = false;


        [MenuItem("Assets/LetsScript/Open %e")]
        public static void OpenScript()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            OpenScript(path);
        }

        public static void OpenScript(string path)
        {
            var scriptData = ScriptSerializer.DeserializeFile(FileUtility.GetFullPath(path));

            if (!Inited)
            {
                InitEnvironment();
            }

            var scriptEditorWindow = ScriptEditorWindowFactory.GetWindow(scriptData);
            if (scriptEditorWindow != null)
            {
                scriptEditorWindow.Data = scriptData;
                scriptEditorWindow.Show();
            }
            else
            {
                Debug.LogError("Can not find an editor window for this script.");
            }
        }

        /// <summary>
        /// 创建脚本
        /// 在当前选中位置创建指定模版脚本
        /// </summary>
        /// <param name="templeteFileName">Name of the templete file.</param>
        public static void CreateScript(string templeteFileName)
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            
            CreateScript(templeteFileName, path + "/" + templeteFileName + ".lua");
        }

        /// <summary>
        /// 创建脚本
        /// 在指定路径创建指定模版脚本
        /// </summary>
        /// <param name="templeteFileName">The templete file path.</param>
        /// <param name="savePath">The save path.</param>
        public static void CreateScript(string templeteFileName, string savePath)
        {
            var templatePath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(templeteFileName)[0]);
            MethodInfo method = typeof(ProjectWindowUtil).GetMethod("CreateScriptAsset", BindingFlags.Static | BindingFlags.NonPublic);
            if (method != null)
            {
                method.Invoke(null, new object[] { templatePath, savePath });
            }
        }

        /// <summary>
        /// 初始化编辑器环境
        /// </summary>
        public static void InitEnvironment()
        {
            LetsScriptSetting.Init();
            string[] commandPathList = new string[LetsScriptSetting.LuaCommandPath.Count];
            for (int i = 0; i < LetsScriptSetting.LuaCommandPath.Count; i++)
            {
                commandPathList[i] = LetsScriptSetting.LuaCommandPath[i];
            }
            LuaCommandAssembly.Read(commandPathList);
            Inited = true;
        }
    }
}