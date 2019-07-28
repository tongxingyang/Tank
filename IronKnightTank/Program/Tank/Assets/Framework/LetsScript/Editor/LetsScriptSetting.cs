// ----------------------------------------------------------------------------
// <copyright file="LetsScriptSetting.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>02/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor
{
    using System.Collections.Generic;
    using System.IO;

    using Assets.Framework.LetsScript.Editor.Script;
    using Assets.Tools.Script.Core.File;

    using ParadoxNotion.Serialization;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// 设置
    /// </summary>
    public class LetsScriptSetting
    {
        /// <summary>
        /// 当前版本号
        /// </summary>
        public static string Version = "0.1.1";

        /// <summary>
        /// Lua命令路径
        /// </summary>
        public static List<string> LuaCommandPath;

        /// <summary>
        /// 当前可用触发点
        /// </summary>
        public static List<ScriptTriggerPoint> TriggerPoint;
        

        public static void Init()
        {
            var configPath = GetConfigPath();
            if (configPath != null)
            {
                var readAllText = File.ReadAllText(configPath);
                var config = JSON.Deserialize<Config>(readAllText);
                config.SetSetting();
            }
        }

        public static void Save(string configPath = null)
        {
            if (configPath == null)
            {
                configPath = GetConfigPath();
            }
            var config = new Config().ReadSetting();
            var json = JSON.Serialize<Config>(config);
            File.WriteAllText(configPath,json);
        }

        private static string GetConfigPath()
        {
            var findAssets = AssetDatabase.FindAssets("LetsScriptSetting");
            foreach (var findAsset in findAssets)
            {
                var guidToAssetPath = AssetDatabase.GUIDToAssetPath(findAsset);
                if (guidToAssetPath.EndsWith(".config"))
                {
                    return FileUtility.GetFullPath(guidToAssetPath);
                }
            }
            Debug.LogError("Configuration file missing!");
            return null;
        }

        public static ScriptTriggerPoint GetTriggerPoint(string name)
        {
            foreach (var scriptTriggerPoint in TriggerPoint)
            {
                if (scriptTriggerPoint.Name == name)
                {
                    return scriptTriggerPoint;
                }
            }
            return null;
        }


        /// <summary>
        /// 负责序列化
        /// </summary>
        private class Config
        {
            [SerializeField]
            public string Version = "0.1.1";
            [SerializeField]
            public List<string> LuaCommandPath;
            [SerializeField]
            public List<ScriptTriggerPoint> TriggerPoint;
            public Config ReadSetting()
            {
                this.Version = LetsScriptSetting.Version;
                this.LuaCommandPath = LetsScriptSetting.LuaCommandPath;
                this.TriggerPoint = LetsScriptSetting.TriggerPoint;
                return this;
            }

            public Config SetSetting()
            {
                LetsScriptSetting.Version = this.Version;
                LetsScriptSetting.LuaCommandPath = this.LuaCommandPath;
                LetsScriptSetting.TriggerPoint = this.TriggerPoint;
                return this;
            }
        }
    }
}