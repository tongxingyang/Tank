// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Data
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// 承载具体脚本内容，以及脚本渲染需要的数据
    /// 可以表示：列表类型，键值对类型，简单值类型
    /// </summary>
    [Serializable]
    public class CommonContent : ScriptableObject
    {
        /// <summary>
        /// 编辑器渲染用的数据
        /// </summary>
        [NonSerialized]
        public EditorData Editor = new EditorData();

        /// <summary>
        /// 键值对数据-键
        /// </summary>
        [SerializeField]
        private List<string> contentNames = new List<string>();

        /// <summary>
        /// 键值对数据-值
        /// </summary>
        [SerializeField]
        private List<CommonContent> contentValues = new List<CommonContent>();

        /// <summary>
        /// 列表数据
        /// </summary>
        [SerializeField]
        private List<CommonContent> contentList = new List<CommonContent>();

        /// <summary>
        /// 简单值
        /// </summary>
        [SerializeField]
        private SimpleValue simpleValue = new SimpleValue();



        /// <summary>
        /// 设置为简单值类型，并且赋予一个值
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetValue(object value)
        {
            this.contentList.Clear();
            this.contentNames.Clear();
            this.contentValues.Clear();
            this.simpleValue.SetValue(value);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object AsValue()
        {
            return this.simpleValue.GetValue();
        }

        public T AsValue<T>()
        {
            return (T)this.simpleValue.GetValue();
        }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <returns>System.Object.</returns>
        public string GetValueType()
        {
            return this.simpleValue.GetValueType();
        }


        /// <summary>
        /// 克隆对象(不包含编辑器数据)
        /// </summary>
        /// <returns>CommonContent.</returns>
        public CommonContent Clone()
        {
            CommonContent clone = new CommonContent();
            clone.simpleValue.SetValue(this.simpleValue.GetValue());
            for (int i = 0; i < this.contentNames.Count; i++)
            {
                var propertyName = this.contentNames[i];
                clone.contentNames.Add(propertyName);
                clone.contentValues.Add(this.contentValues[i].Clone());
            }
            for (int i = 0; i < this.contentList.Count; i++)
            {
                var editorAnyValue = this.contentList[i];
                clone.contentList.Add(editorAnyValue.Clone());
            }
            return clone;
        }

        /// <summary>
        /// 获取保存在键值对或者列表上的子内容
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>CommonContent.</returns>
        public CommonContent GetChildContent(object key)
        {
            if (key is string)
            {
                return this.GetChildContent(key as string);
            }
            if (key is int)
            {
                return this.GetChildContent((int)key);
            }
            return null;
        }

        public CommonContent GetChildContent(string key)
        {
            for (int i = 0; i < this.contentNames.Count; i++)
            {
                if (this.contentNames[i] == key)
                {
                    return this.contentValues[i];
                }
            }
            return null;
        }

        public CommonContent GetChildContent(int index)
        {
            if (this.contentList.Count > index)
            {
                return this.contentList[index];
            }
            return null;
        }

        /// <summary>
        /// 设置一个属性到键值对或者列表上
        /// </summary>
        /// <param name="key">string键或者int序号</param>
        /// <param name="content">The content.</param>
        public void SetChildContent(object key, CommonContent content)
        {
            if (key is string)
            {
                this.SetChildContent(key as string, content);
            }
            if (key is int)
            {
                this.SetChildContent((int)key, content);
            }
        }

        public void SetChildContent(string key, CommonContent content)
        {
            for (int i = 0; i < this.contentNames.Count; i++)
            {
                if (this.contentNames[i] == key)
                {
                    this.contentValues[i] = content;
                    return;
                }
            }
            this.contentNames.Add(key);
            this.contentValues.Add(content);
        }

        public void SetChildContent(int index, CommonContent content)
        {
            if (this.contentList.Count <= index)
            {
                this.contentList.Add(content);
            }
            else
            {
                this.contentList[index] = content;
            }
        }

        /// <summary>
        /// 插入内容到列表上的指定序号
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="index">The index.</param>
        public void InsertChildContent(CommonContent content, int index)
        {
            if (index >= this.contentList.Count)
            {
                this.contentList.Add(content);
            }
            else
            {
                this.contentList.Insert(index, content);
            }
        }

        /// <summary>
        /// 移除内容
        /// </summary>
        /// <param name="value">The value.</param>
        public void RemoveContent(object value)
        {
            if (SimpleValue.IsValue(value))
            {
                if (this.simpleValue.GetValue() == value)
                {
                    this.simpleValue.Clear();
                }
            }
            else
            {
                var luaDynamicConfig = value as CommonContent;
                if (luaDynamicConfig == null)
                {
                    return;
                }
                for (int i = 0; i < this.contentValues.Count; i++)
                {
                    var property = this.contentValues[i];
                    if (property == luaDynamicConfig)
                    {
                        this.contentValues.RemoveAt(i);
                        this.contentNames.RemoveAt(i);
                        return;
                    }
                }
                for (int i = 0; i < this.contentList.Count; i++)
                {
                    var item = this.contentList[i];
                    if (item == luaDynamicConfig)
                    {
                        this.contentList.RemoveAt(i);
                        return;
                    }
                }
                if (this.simpleValue.GetValue() == luaDynamicConfig.AsValue())
                {
                    this.simpleValue.Clear();
                }
            }
        }

        /// <summary>
        /// 清除所有内容
        /// </summary>
        public void Clear()
        {
            this.contentValues.Clear();
            this.contentNames.Clear();
            this.contentList.Clear();
            this.simpleValue.Clear();
            this.Editor.Clear();
        }

        /// <summary>
        /// 清除编辑器相关数据
        /// </summary>
        public void ClearEditorData()
        {
            this.Editor.Clear();
            foreach (var content in this.contentValues)
            {
                content.ClearEditorData();
            }
            foreach (var content in this.contentList)
            {
                content.ClearEditorData();
            }
        }

        /// <summary>
        /// 获取列表内容
        /// </summary>
        /// <returns>List&lt;CommonContent&gt;.</returns>
        public List<CommonContent> AsList()
        {
            return this.contentList;
        }

        /// <summary>
        /// 获取键值对内容
        /// </summary>
        /// <returns>Dictionary&lt;System.String, CommonContent&gt;.</returns>
        public Dictionary<string, CommonContent> AsDictionary()
        {
            Dictionary <string, CommonContent> dic = new Dictionary<string, CommonContent>();
            for (int i = 0; i < this.contentNames.Count; i++)
            {
                dic[this.contentNames[i]] = this.contentValues[i];
            }
            return dic;
        }

        /// <summary>
        /// 判断是否是一个简单值类型
        /// </summary>
        /// <returns><c>true</c> if this instance is value; otherwise, <c>false</c>.</returns>
        public bool IsValue()
        {
            return this.simpleValue.HasValue();
        }

        public CommonContent FromList(List<CommonContent> children)
        {
            this.contentList.Clear();
            if (children != null)
            {
                foreach (var commonContent in children)
                {
                    this.contentList.Add(commonContent);
                }
            }
            return this;
        }

        public CommonContent FromDictionary(Dictionary<string,CommonContent> children)
        {
            this.contentNames.Clear();
            this.contentValues.Clear();
            if (children != null)
            {
                foreach (var commonContent in children)
                {
                    this.contentNames.Add(commonContent.Key);
                    this.contentValues.Add(commonContent.Value);
                }
            }
            return this;
        }

        public CommonContent FromValue(object value)
        {
            this.SetValue(value);
            return this;
        }
    }
}