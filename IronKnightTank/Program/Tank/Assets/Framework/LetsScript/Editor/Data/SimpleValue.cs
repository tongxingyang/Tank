// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Data
{
    using System;

    using UnityEngine;

    /// <summary>
    /// 简单值类型
    /// </summary>
    [Serializable]
    public class SimpleValue
    {
        [SerializeField]
        private int intValue;
        [SerializeField]
        private float floatValue;
        [SerializeField]
        private string stringValue;
        [SerializeField]
        private bool boolValue;

        [SerializeField]
        private string type;

        public static bool IsValue(object obj)
        {
            if (obj is int)
            {
                return true;
            }
            if (obj is float)
            {
                return true;
            }
            if (obj is string)
            {
                return true;
            }
            if (obj is bool)
            {
                return true;
            }
            return false;
        }

        public void Clear()
        {
            this.type = null;
        }

        public string GetValueType()
        {
            return this.type;
        }

        public bool HasValue()
        {
            return this.type != null && this.type != string.Empty;
        }

        public object GetValue()
        {
            if (this.type == null)
            {
                return null;
            }
            switch (this.type)
            {
                case "int":
                    return this.intValue;
                case "float":
                    return this.floatValue;
                case "string":
                    return this.stringValue;
                case "bool":
                    return this.boolValue;
            }
            return null;
        }

        public void SetValue(object obj)
        {
            if (obj is int)
            {
                this.intValue = (int)obj;
                this.type = "int";
            }
            else if (obj is float)
            {
                this.floatValue = (float)obj;
                this.type = "float";
            }
            else if (obj is string)
            {
                this.stringValue = (string)obj;
                this.type = "string";
            }
            else if (obj is bool)
            {
                this.boolValue = (bool)obj;
                this.type = "bool";
            }
        }
    }
}