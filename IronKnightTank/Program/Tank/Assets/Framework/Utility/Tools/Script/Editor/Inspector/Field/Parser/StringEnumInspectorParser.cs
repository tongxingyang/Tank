﻿// ----------------------------------------------------------------------------
// <copyright file="StringEnumInspectorParser.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>29/12/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Inspector.Field.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Assets.Tools.Script.Attributes;
    using Assets.Tools.Script.Reflec;

    using UnityEditor;

    using UnityEngine;

    public class StringEnumInspectorParser : FieldInspectorParser
    {
        public override string Name { get
        {
            return "StringEnum";
        } }

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
            string currValue = value as string;

            List<FieldInfo> enums = new List<FieldInfo>();
            List<GUIContent> enumNames = new List<GUIContent>();
            int currIndex = -1;
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                var info = fieldInfos[i];
                enums.Add(info);
                enumNames.Add(new GUIContent(info.Name));
                var o = info.GetValue(null) as string;
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

            currValue = enums[currIndex].GetValue(null) as string;
            return currValue;
        }
    }
}