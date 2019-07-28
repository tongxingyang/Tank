// ----------------------------------------------------------------------------
// <copyright file="ConfigReader.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>18/10/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editortool.Reader
{
    using System;
    using System.IO;

    using Assets.Tools.Script.File;

    /// <summary>
    /// The config reader.
    /// </summary>
    public abstract class ConfigReader
    {
        /// <summary>
        /// The use project save game configuration
        /// </summary>
        public const string UseProjectPath = "UseProjectSaveGameConfig";

        /// <summary>
        /// The project path
        /// </summary>
        public static string ProjectPath = string.Empty;
        
    }

    /// <summary>
    /// 配置文件读写工具
    /// </summary>
    /// <typeparam name="T">需要存放的数据类型</typeparam>
    public abstract class ConfigReader<T> : ConfigReader where T : class, new()
    {
        /// <summary>
        /// 从外部目录读取数据
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns>T.</returns>
        public virtual T GetFromExternalCache(string root)
        {
            try
            {
                T data = null;
                string directoryPath = GetDirectoryPath(root);
                if (Directory.Exists(directoryPath))
                {
                    data = ReadFromFile(root);
                }
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 从本地（个人文件，不共享）数据读取
        /// </summary>
        /// <returns>T.</returns>
        public virtual T GetFromLocalCache()
        {
#if UNITY_EDITOR
            bool b = UnityEditor.EditorPrefs.GetBool(ConfigReader.UseProjectPath);
            if (b)
            {
                return this.GetFromProjectCache();
            }
#endif
            try
            {
                T data = null;
                string directoryPath = GetDirectoryPath(LoadPath.saveLoadPath);
                if (Directory.Exists(directoryPath))
                {
                    data = ReadFromFile(LoadPath.saveLoadPath);
                }
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 从项目（项目内共享）数据读取
        /// </summary>
        /// <returns>T.</returns>
        public virtual T GetFromProjectCache()
        {
            T data;
            string directoryPath = GetDirectoryPath(ConfigReader.ProjectPath);
            if (!ResSafeFileUtil.DirectoryExists(directoryPath))
            {
                data = new T();
            }
            else
            {
                data = ReadFromFile(ConfigReader.ProjectPath);
            }
            return data;
        }
#if UNITY_EDITOR
        /// <summary>
        /// 保存到外部的目录
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="root">The root.</param>
        public virtual void SaveToExternalCache(T data, string root)
        {
            string directoryPath = GetDirectoryPath(root);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            WriteToFile(data, root);
        }

        /// <summary>
        /// 保存到本地
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void SaveToLocalCache(T data)
        {
#if UNITY_EDITOR
            bool b = UnityEditor.EditorPrefs.GetBool(ConfigReader.UseProjectPath);
            if (b)
            {
                SaveToProjectCache(data);
                return;
            }
#endif
            string directoryPath = GetDirectoryPath(LoadPath.saveLoadPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            WriteToFile(data, LoadPath.saveLoadPath);
        }

        /// <summary>
        /// 保存到项目
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void SaveToProjectCache(T data)
        {
            string directoryPath = GetDirectoryPath(string.Empty);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            WriteToFile(data, string.Empty);
        }

        /// <summary>
        /// 数据写入文件
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="root">文件存放位置根目录</param>
        public abstract void WriteToFile(T data, string root);
#endif
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="root">文件存放位置根目录</param>
        /// <returns>T.</returns>
        public abstract T ReadFromFile(string root);

        /// <summary>
        /// 数据存放文件夹目录
        /// </summary>
        /// <param name="root">文件存放位置根目录</param>
        /// <returns>目录路径</returns>
        public virtual string GetDirectoryPath(string root)
        {
#if UNITY_EDITOR
            string path = string.Format("{0}Assets/GameResource/Data/{1}", root, typeof(T).Name);
            return path.Replace("Table", "");


#else
            return string.Format("Data/{0}", typeof(T).Name);
#endif
        }
    }
}