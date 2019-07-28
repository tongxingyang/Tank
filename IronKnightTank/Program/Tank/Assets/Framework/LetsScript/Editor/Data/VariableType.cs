// ----------------------------------------------------------------------------
// <copyright file="VariableType.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>06/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Data
{
    public class VariableType
    {
        public const string Dynamic = "dynamic";
        public const string Value = "value";

        public static bool HasDynamic(string type)
        {
            return type == Dynamic || HasBoth(type);
        }

        public static bool HasValue(string type)
        {
            return type == Value || HasBoth(type);
        }

        public static bool HasBoth(string type)
        {
            return string.IsNullOrEmpty(type);
        }
    }
}