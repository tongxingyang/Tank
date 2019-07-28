//----------------------------------------------------------------------------
// <copyright file="fsSkipNullAttribute.cs" company="上海序曲网络科技有限公司">
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
    /// 忽略null类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class fsSkipNullAttribute : Attribute
    {
    }
}