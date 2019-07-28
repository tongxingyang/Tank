// ----------------------------------------------------------------------------
// <copyright file="LuaSerializer.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>04/02/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.Lua.Editor.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    using Assets.Tools.Script.Reflec;

    using UnityEngine;

    public class LuaSerializer
    {
        public static object Deserialize(string luaCode)
        {
            throw new System.NotImplementedException();
        }

        public static string Serialize(object value)
        {
            if (value == null)
            {
                return "nil";
            }
            Type type = value.GetType();
            if (type == typeof(float))
            {
                return value.ToString();
            }
            if (type == typeof(int))
            {
                return value.ToString();
            }
            if (type == typeof(double) || type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort))
            {
                return Convert.ToDouble(value).ToString();
            }

            if (type == typeof(bool))
            {
                return value.ToString().ToLower();
            }

            if (type == typeof(string))
            {
                return String.Format("\"{0}\"", value);
            }

          /*  if (type == typeof(List<uint>) || type == typeof(List<int>))
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append('{');
                List<object> list = (List<object>) value;
                list.ForEach(r =>
                {
                    sb.Append(r).Append(',');
                });
                sb.Remove(sb.Length - 1, 1).Append('}');
                return sb.ToString();
            }

            if (type == typeof(List<string>))
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append('{');
                List<object> list = (List<object>) value;
                list.ForEach(r =>
                {
                    sb.Append("'").Append(r).Append("',");
                });
                sb.Remove(sb.Length - 1, 1).Append('}');
                return sb.ToString();
            }*/

            return SerializeClassType(value);
        }

        private static string SerializeClassType(object value)
        {
            if (value == null)
            {
                return "nil";
            }
            Type type = value.GetType();

            //复杂类型序列化到lua代码
            List<FieldInfo> fields = new List<FieldInfo>();
            var publicFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            var nonpublicFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var info in publicFields)
            {
                if (!info.HasAttribute<NonSerializedAttribute>())
                {
                    fields.Add(info);
                }
            }
            foreach (var info in nonpublicFields)
            {
                if (info.HasAttribute<SerializeField>())
                {
                    fields.Add(info);
                }
            }

            List<string> tableFields = new List<string>();


            for (int i = 0; i < fields.Count; i++)
            {
                var fieldInfo = fields[i];
                tableFields.Add(string.Format("{0} = {1}", fieldInfo.Name, Serialize(fieldInfo.GetValue(value))));
            }

            //            if (value is IList)
            //            {
            //                var list = value as IList;
            //                for (int i = 0; i < list.Count; i++)
            //                {
            //                    var listValue = list[i];
            //                    tableFields.Add(Serialize(listValue));
            //                }
            //            }
            if (value is IDictionary)
            {
                var dictionary = value as IDictionary;
                foreach (var key in dictionary.Keys)
                {
                    tableFields.Add(string.Format("{0} = {1}", key, Serialize(dictionary[key])));
                }
            }
            else if (value is IEnumerable)
            {
                var enumerable = value as IEnumerable;
                foreach (var variable in enumerable)
                {
                    tableFields.Add(Serialize(variable));
                }
            }

            StringBuilder tableBuilder = new StringBuilder();
            tableBuilder.Append("{");
            for (int i = 0; i < tableFields.Count; i++)
            {
                var tableField = tableFields[i];
                tableBuilder.Append(tableField);
                if (i < tableFields.Count - 1)
                {
                    tableBuilder.Append(", ");
                }
            }
            tableBuilder.Append("}");

            return tableBuilder.ToString();
        }

        /// <summary>
        /// 获得一个类型在lua代码中是书写字符
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includeNamespace">if set to <c>true</c> [include namespace].</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string GetLuaTypeName(Type type, bool includeNamespace = false)
        {
            if (type == null)
            {
                return "unknow";
            }
            if (type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(uint)
                || type == typeof(ulong) || type == typeof(ushort))
            {
                return "number";
            }

            if (type == typeof(bool))
            {
                return "bool";
            }

            if (type == typeof(string))
            {
                return "string";
            }

            StringBuilder builder = new StringBuilder();

            var indexOf = type.Name.IndexOf("`", StringComparison.Ordinal);
            if (indexOf > 0)
            {
                builder.Append((includeNamespace && type.Namespace.IsNOTNullOrEmpty() ? type.Namespace + "." : "") + type.Name.Substring(0, indexOf));
            }
            else
            {
                builder.Append((includeNamespace && type.Namespace.IsNOTNullOrEmpty() ? type.Namespace + "." : "") + type.Name);
            }


            var genericParameterConstraints = type.GetGenericArguments();
            if (genericParameterConstraints.Length > 0)
            {
                builder.Append("<");
            }
            for (int i = 0; i < genericParameterConstraints.Length; i++)
            {
                var parameterConstraint = genericParameterConstraints[i];
                builder.Append(GetLuaTypeName(parameterConstraint));
                if (i < genericParameterConstraints.Length - 1)
                {
                    builder.Append(",");
                }
            }
            if (genericParameterConstraints.Length > 0)
            {
                builder.Append(">");
            }
            return builder.ToString();
        }
    }
}