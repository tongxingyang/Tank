// ----------------------------------------------------------------------------
// <copyright file="TableConfigReader.cs" company="上海序曲网络科技有限公司">
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
    using System.Collections.Generic;
    using System.IO;

    using Assets.Tools.Script.Core.File;
    using Assets.Tools.Script.File;

    using ParadoxNotion.Serialization;

    using UnityEngine;

    /// <summary>
    /// Class TableConfigReader.
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    public class TableConfigReader<T, TData> : ConfigReader<T>
        where T : class, ITextConfigTable<TData>, new()
        where TData : INameData
    {
        
#if UNITY_EDITOR
        private Dictionary<string, TData> currDatas = new Dictionary<string, TData>();

        /// <summary>
        /// 更新某个文件
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="preDataName"></param>
        public void RefreshDataFile(TData data, string preDataName)
        {
            if (data == null)
            {
                return;
            }
            bool b = UnityEditor.EditorPrefs.GetBool(ConfigReader.UseProjectPath);
            string root = b ? string.Empty : LoadPath.saveLoadPath;

            //删除旧的
//            string preTxtFilePath = this.GetTxtFilePath(preDataName, root);
            this.DeleteDataFile(preDataName, root);

            //添加新的
            this.CreateDataFile(data, root);
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="root">The root.</param>
        public override void WriteToFile(T data, string root)
        {
            string directoryPath = GetDirectoryPath(root);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            List<TData> datas = data.GetDatas();

            foreach (var currData in datas)
            {
				if(currDatas.ContainsKey(currData.GetDataName()))
				{
					UnityEngine.Debug.Log("文件名重复:"+currData.GetDataName());
					data.RemoveData(currData);
				}
				else
				{
					this.currDatas.Add(currData.GetDataName(), currData);
				}
                
            }
            
            //删除已经被移除的数据文件
            var strings = Directory.GetFiles(this.GetDirectoryPath(root));
            foreach (string filePath in strings)
            {
                string file = String.Empty;
                try
                {
                    file = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    this.DeleteDataFile(file,root);
                }
                catch (Exception e)
                {
                    Debug.Log("Load file error at " + file);
                    Debug.Log(e);
                }
            }
            this.currDatas.Clear();

            //写入新的数据
            for (int i = 0; i < datas.Count; i++)
            {
                this.CreateDataFile(datas[i], root);
            }
        }
#endif
        /// <summary>
        /// Reads from file.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns>T.</returns>
        public override T ReadFromFile(string root)
        {
            T table = new T();

            var strings = Directory.GetFiles(this.GetDirectoryPath(root));
            foreach (string filePath in strings)
            {
                string file = string.Empty;
                try
                {
                    if (System.IO.Path.GetExtension(filePath) == ".meta")
                    {
                        continue;
                    }
                    file = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    if (file.IsNullOrEmpty() || file == "\r\n")
                    {
                        continue;
                    }
                    string txtFilePath = this.GetTxtFilePath(file, root);
                    string serialize = ResSafeFileUtil.ReadFile(txtFilePath);
                    TData deserialize = JSON.Deserialize<TData>(serialize);
                    table.AddData(deserialize);
                }
                catch (Exception e)
                {
                    Debug.Log("Load file error at " + file);
                    Debug.Log(e);
                }
            }
            
            return table;
        }

        /// <summary>
        /// Gets the text file path.
        /// </summary>
        /// <param name="dataName">Name of the data.</param>
        /// <param name="root">The root.</param>
        /// <returns>System.String.</returns>
        protected virtual string GetTxtFilePath(string dataName, string root)
        {
            return string.Format("{0}/{1}{2}", GetDirectoryPath(root), dataName, ResSafeFileUtil.GetSuffix(".json"));
        }
#if UNITY_EDITOR
        private void DeleteDataFile(string file,string root)
        {
            if (file.IsNullOrEmpty() || file == "\r\n" || this.currDatas.ContainsKey(file))
            {
                return;
            }
            string filePath = this.GetTxtFilePath(file, root);
            //删除文件本身
            UnityEditor.FileUtil.DeleteFileOrDirectory(filePath);
            //如果有.meta文件也一并删除
            string txtMetaPath = filePath + ".meta";
            if (File.Exists(txtMetaPath))
            {
                UnityEditor.FileUtil.DeleteFileOrDirectory(filePath + ".meta");
            }
        }

        private void CreateDataFile(TData elm, string root)
        {
            string dataName = elm.GetDataName();
            if (dataName.IsNullOrEmpty() || dataName == "\r\n")
            {
                return;
            }
            string txtFilePath = this.GetTxtFilePath(dataName, root);

            string serialize = JSON.Serialize(typeof(TData), elm);
            File.WriteAllText(txtFilePath, serialize);
   
        }
#endif
    }
}