// ----------------------------------------------------------------------------
// <copyright file="IntEnumInspectorParser.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>05/01/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Inspector.Field.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Assets.Tools.Script.Attributes;

    using UnityEditor;

    using UnityEngine;

    public class IntEnumInspectorParser : FieldInspectorParser
    {
        public override string Name
        {
            get
            {
                return "IntEnum";
            }
        }

        public override object ParserFiled(
            InspectorStyle style,
            object value,
            Type t,
            FieldInfo fieldInfo,
            object instance,
            bool withName = true)
        {
            var type = style.ParserAgrs[0] as Type;
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            int currValue = value is int ? (int)value : 0;

            List<FieldInfo> enums = new List<FieldInfo>();
            List<GUIContent> enumNames = new List<GUIContent>();
            int currIndex = -1;
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                var info = fieldInfos[i];
                enums.Add(info);
                enumNames.Add(new GUIContent(info.Name));
                var o = info.GetValue(null) is int ? (int)info.GetValue(null) : 0;
                if (o == currValue)
                {
                    currIndex = i;
                }
            }
            if (currIndex < 0)
            {
                currIndex = 0;
            }

            if (withName)
            {
                currIndex = EditorGUILayout.Popup(new GUIContent(style.Name), currIndex, enumNames.ToArray());
            }
            else
            {
                currIndex = EditorGUILayout.Popup(currIndex, enumNames.ToArray());
            }
            currValue = enums[currIndex].GetValue(null) is int ? (int)enums[currIndex].GetValue(null) : 0;
            return currValue;
        }
    }
}