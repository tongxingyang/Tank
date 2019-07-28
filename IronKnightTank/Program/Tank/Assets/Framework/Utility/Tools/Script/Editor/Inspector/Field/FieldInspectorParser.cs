﻿// ----------------------------------------------------------------------------
// <copyright file="InspectorFieldParser.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>31/07/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Inspector.Field
{
    using System;
    using System.Reflection;

    using Assets.Tools.Script.Attributes;

    public abstract class FieldInspectorParser
    {
        /// <summary>
        /// 解析器名字
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 解析显示字段
        /// </summary>
        /// <param name="style">
        /// The style.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <param name="fieldInfo">
        /// The member.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="withName"></param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public abstract object ParserFiled(
            InspectorStyle style,
            object value,
            Type t,
            FieldInfo fieldInfo,
            object instance,
            bool withName = true);
    }
}