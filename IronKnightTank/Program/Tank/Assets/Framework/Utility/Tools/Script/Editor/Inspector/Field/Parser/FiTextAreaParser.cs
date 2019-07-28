﻿// ----------------------------------------------------------------------------
// <copyright file="TextAreaParser.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>31/07/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Inspector.Field.Parser
{
    using System;
    using System.Reflection;

    using Assets.Tools.Script.Attributes;

    using UnityEditor;

    using UnityEngine;

    public class FiTextAreaParser : FieldInspectorParser
    {
        public override string Name
        {
            get
            {
                return "TextArea";
            }
        }

        public override object ParserFiled(InspectorStyle style, object value, Type t, FieldInfo fieldInfo, object instance, bool withName = true)
        {
            int w = -1;
            int h = 80;

            if (style.ParserAgrs != null)
            {
                if (style.ParserAgrs.Length >= 1)
                {
                    if (style.ParserAgrs[0] is int)
                    {
                        w = (int)style.ParserAgrs[0];
                    }
                }
                if (style.ParserAgrs.Length >= 2)
                {
                    if (style.ParserAgrs[1] is int)
                    {
                        h = (int)style.ParserAgrs[1];
                    }
                }
            }
            GUILayout.BeginHorizontal();
            if (withName)
            {
                GUILayout.Label(style.Name,GUILayout.Width(145));
            }

            string textArea = null;
            if (w < 0)
            {
                textArea = EditorGUILayout.TextArea((string)value, GUILayout.Height(h));
            }
            else
            {
                textArea = EditorGUILayout.TextArea((string)value, GUILayout.Width(w), GUILayout.Height(h));
            }
            
            GUILayout.EndHorizontal();

            return textArea;
        }
    }
}