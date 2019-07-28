// ----------------------------------------------------------------------------
// <copyright file="PlayerPrefsCache.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>06/01/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Serialized.LocalCache
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Assets.Tools.Script.File;

    using LuaInterface;

    /// <summary>
    /// 用户缓存数据集
    /// </summary>
    public class PlayerPrefsCache
    {
        private const string FragmentLengthTag = "⌈жㅢ";

        private const string DeleteTag = "⌈дㅢ";

        private const string SplitTag = "⌈εㅢ";

        /// <summary>
        /// 碎片最大长度,超过后在下一次加载时自动整理碎片，小于等于0则无视长度
        /// </summary>
        public int MaxFragmentLength = 0;

        private bool holdStreamWriter;

        private Dictionary<string, string> datas;

        private string path;

        private List<DirtyData> dirtyDatas = new List<DirtyData>();

        public PlayerPrefsCache(string name)
        {
            this.holdStreamWriter = false;
            this.path = name + ".gs";
            this.MaxFragmentLength = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPrefsCache"/> class.
        /// </summary>
        /// <param name="name">保存路径（含文件名）</param>
        /// <param name="maxFragmentLength">碎片最大长度,超过后在下一次加载时自动整理碎片，0则无视长度</param>
        /// <param name="holdStreamWriter">是否要一直保持文件写入流</param>
        public PlayerPrefsCache(string name, int maxFragmentLength,bool holdStreamWriter)
        {
            this.holdStreamWriter = holdStreamWriter;
            this.path = name + ".gs";
            this.MaxFragmentLength = maxFragmentLength;
        }

        ~PlayerPrefsCache()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.ReleaseStreamWriter();
        }


        /// <summary>
        /// 重置/添加指定值
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetString(string key, string value)
        {
            if (this.datas == null)
            {
                this.Reload();
            }
            this.datas[key] = value;
            DirtyData.AddSetString(key, value, this.dirtyDatas);
        }

        /// <summary>
        /// 获得指定值
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public string GetString(string key)
        {
            if (this.datas == null)
            {
                this.Reload();
            }
            string value;
            this.datas.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// 删除指定值
        /// </summary>
        /// <param name="key">The key.</param>
        public void DeleteKey(string key)
        {
            if (this.datas == null)
            {
                this.Reload();
            }
            this.datas.Remove(key);
            DirtyData.AddDeleteKey(key, this.dirtyDatas);
        }

        /// <summary>
        /// 获得所有值
        /// </summary>
        /// <returns>System.Collections.Generic.List&lt;System.String&gt;.</returns>
        public List<string> GetAllString()
        {
            if (this.datas == null)
            {
                this.Reload();
            }
            List<string> values = new List<string>();
            foreach (var data in this.datas)
            {
                if (data.Key == FragmentLengthTag)
                {
                    continue;
                }
                values.Add(data.Value);
            }
            return values;
        }

        /// <summary>
        /// 获得所有键值对
        /// </summary>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.String, System.String&gt;.</returns>
        public Dictionary<string, string> GetAllKV()
        {
            if (this.datas == null)
            {
                this.Reload();
            }
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (var data in this.datas)
            {
                if (data.Key == FragmentLengthTag)
                {
                    continue;
                }
                values[data.Key] = data.Value;
            }
            return values;
        }

        /// <summary>
        /// 获得所有键值对
        /// </summary>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.String, System.String&gt;.</returns>
        public void GetAllKVToLuaTable(LuaTable table)
        {
            if (this.datas == null)
            {
                this.Reload();
            }
            foreach (var data in this.datas)
            {
                if (data.Key == FragmentLengthTag)
                {
                    continue;
                }
                table[data.Key] = data.Value;
            }
        }

        /// <summary>
        /// 清除所有数据并且保存
        /// </summary>
        public void Clear()
        {
            if (this.datas == null)
            {
                this.datas = new Dictionary<string, string>();
            }
            else
            {
                this.datas.Clear();
            }
            this.DefragSave();
        }

        /// <summary>
        /// 保存改变的脏数据
        /// </summary>
        public void Save()
        {
            if (this.dirtyDatas.Count <= 0)
            {
                return;
            }
            if (this.datas == null)
            {
                this.Reload();
            }

            StringBuilder builder = new StringBuilder(this.datas.Count << 4);
            for (int i = 0; i < this.dirtyDatas.Count; i++)
            {
                var d = this.dirtyDatas[i];
                builder.Append(d.Key);
                builder.Append(SplitTag);
                builder.Append(d.Value);
                builder.Append(SplitTag);
            }
            this.dirtyDatas.Clear();

            if (this.MaxFragmentLength > 0)
            {
                var newTextLen = builder.Length;
                string currFragmentLen;
                var tryGetValue = this.datas.TryGetValue(FragmentLengthTag, out currFragmentLen);
                if (tryGetValue)
                {
                    var len = Convert.ToInt32(currFragmentLen);
                    newTextLen += len;
                }
                this.datas[FragmentLengthTag] = newTextLen.ToString();

                builder.Append(FragmentLengthTag);
                builder.Append(SplitTag);
                builder.Append(newTextLen.ToString());
                builder.Append(SplitTag);
            }

            ESFile.Save(builder.ToString(), this.path, FileMode.Append);
        }
         
        /// <summary>
        /// 整理文件内容碎片并重新保存
        /// </summary>
        public void DefragSave()
        {
            if (this.datas == null)
            {
                this.Reload();
            }
            if (this.MaxFragmentLength > 0)
            {
                this.datas.Remove(FragmentLengthTag);
            }

            this.dirtyDatas.Clear();

            StringBuilder builder = new StringBuilder(this.datas.Count << 4);
            foreach (var d in this.datas)
            {
                builder.Append(d.Key);
                builder.Append(SplitTag);
                builder.Append(d.Value);
                builder.Append(SplitTag);
            }

            this.ReleaseStreamWriter();
            ESFile.Save(builder.ToString(), this.path, FileMode.Create);
            this.HoldStreamWriter();
        }

        /// <summary>
        /// 从缓存加载数据
        /// </summary>
        public void Reload()
        {
            try
            {
                this.ReleaseStreamWriter();
                string cacheText = null;
                if (ESFile.Exists(this.path))
                {
                    cacheText = ESFile.LoadString(this.path);
                }
                this.dirtyDatas.Clear();
                this.datas = new Dictionary<string, string>();
                if (cacheText.IsNOTNullOrEmpty())
                {
                    var strings = cacheText.Split(new string[] { SplitTag }, StringSplitOptions.None);
                    for (int i = 0; i < strings.Length - 1; i += 2)
                    {
                        var key = strings[i];
                        var value = strings[i + 1];

                        if (value.StartsWith(DeleteTag) && value == DeleteTag + key)
                        {
                            this.datas.Remove(key);
                        }
                        else
                        {
                            this.datas[key] = value;
                        }
                    }

                    if (this.MaxFragmentLength > 0)
                    {
                        string currFragmentLen;
                        var tryGetValue = this.datas.TryGetValue(FragmentLengthTag, out currFragmentLen);
                        if (tryGetValue)
                        {
                            var len = Convert.ToInt32(currFragmentLen);
                            if (len > this.MaxFragmentLength)
                            {
                                this.DefragSave();
                            }
                        }
                    }
                }

                this.HoldStreamWriter();
            }
            catch (Exception e)
            {
                DebugConsole.Log(e);
            }
        }

        private void HoldStreamWriter()
        {
            if (this.holdStreamWriter)
            {
                ESFile.HoldStreamWriter(this.path, FileMode.Append);
            }
        }

        private void ReleaseStreamWriter()
        {
            if (this.holdStreamWriter)
            {
                ESFile.ReleaseStreamWriter(this.path);
            }
        }

        /// <summary>
        /// 脏数据
        /// </summary>
        private struct DirtyData
        {
            public static void AddSetString(string key, string value, List<DirtyData> dirtyData)
            {
                dirtyData.Add(new DirtyData() { Key = key, Value = value });
            }

            public static void AddDeleteKey(string key, List<DirtyData> dirtyData)
            {
                dirtyData.Add(new DirtyData() { Key = key, Value = DeleteTag + key });
            }

            public string Key;

            public string Value;
        }


    }
}