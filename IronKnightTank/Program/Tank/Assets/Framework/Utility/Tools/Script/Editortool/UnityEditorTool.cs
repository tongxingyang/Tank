// ----------------------------------------------------------------------------
// <copyright file="UnityEditorTool.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>20/07/2015</date>
// ----------------------------------------------------------------------------

#if UNITY_EDITOR

namespace Assets.Tools.Script.Editor
{
    #region

    using System;
    using System.Reflection;

    using UnityEditor;

    using Object = UnityEngine.Object;

    #endregion

    /// <summary>
    ///     The unity editor tool.
    /// </summary>
    public class UnityEditorTool
    {
        #region Public Methods and Operators

        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// 如果这个字段的类型没有支持
        /// </exception>
        public static object Show(FieldInfo info, object obj)
        {
            if (info.FieldType == typeof(int))
            {
                return EditorGUILayout.IntField(info.Name, (int)info.GetValue(obj));
            }

            if (info.FieldType == typeof(uint))
            {
                return (uint)EditorGUILayout.IntField(info.Name, (int)info.GetValue(obj));
            }

            if (info.FieldType == typeof(float))
            {
                return EditorGUILayout.FloatField(info.Name, (float)info.GetValue(obj));
            }

            if (info.FieldType == typeof(string))
            {
                return EditorGUILayout.TextField(info.Name, (string)info.GetValue(obj));
            }

            if (info.FieldType == typeof(bool))
            {
                return EditorGUILayout.Toggle(info.Name, (bool)info.GetValue(obj));
            }

            if (info.FieldType.IsEnum)
            {
                return EditorGUILayout.EnumPopup(info.Name, (Enum)info.GetValue(obj));
            }

            if (info.FieldType.IsAssignableFrom(typeof(Object)))
            {
                return EditorGUILayout.ObjectField(info.Name, (Object)info.GetValue(obj), info.FieldType);
            }

            throw new NotSupportedException(string.Format("{0} is not support", info.FieldType.FullName));
        }

        public static object ShowWithNoFieldName(FieldInfo info, object obj)
        {
            if (info.FieldType == typeof(int))
            {
                return EditorGUILayout.IntField((int)info.GetValue(obj));
            }

            if (info.FieldType == typeof(uint))
            {
                return (uint)EditorGUILayout.IntField((int)info.GetValue(obj));
            }

            if (info.FieldType == typeof(float))
            {
                return EditorGUILayout.FloatField((float)info.GetValue(obj));
            }

            if (info.FieldType == typeof(string))
            {
                return EditorGUILayout.TextField((string)info.GetValue(obj));
            }

            if (info.FieldType == typeof(bool))
            {
                return EditorGUILayout.Toggle((bool)info.GetValue(obj));
            }

            if (info.FieldType.IsEnum)
            {
                return EditorGUILayout.EnumPopup((Enum)info.GetValue(obj));
            }

            if (info.FieldType.IsAssignableFrom(typeof(Object)))
            {
                return EditorGUILayout.ObjectField((Object)info.GetValue(obj), info.FieldType);
            }

            throw new NotSupportedException(string.Format("{0} is not support", info.FieldType.FullName));
        }

        #endregion
    }
}
#endif