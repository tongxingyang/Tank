//----------------------------------------------------------------------------
// <copyright file="fsConvertObjectAttribute.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>Ben</author>
// <date>2016/3/3 17:30:23</date>
//----------------------------------------------------------------------------

using System;

namespace FullSerializer.Extend
{
    /// <summary>
    /// 忽略object类型，直接序列号成原始值，只能用在基本数据类型上（int,string,bool,enum...）
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class fsConvertObjectAttribute : Attribute
    {
        public Type ValueType;

        /// <summary>
        /// 忽略object类型，直接序列号成原始值，只能用在基本数据类型上（int,string,bool,enum...）
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        public fsConvertObjectAttribute(Type baseType)
        {
            ValueType = baseType;
        }
    }
}