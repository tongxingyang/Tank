// ----------------------------------------------------------------------------
// <copyright file="fsJsonHelper.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>17/11/2015</date>
// ----------------------------------------------------------------------------

using ParadoxNotion.Serialization;

namespace Assets.Tools.Serialization
{
    public static class fsJsonHelper
    {
        public static object JsonClone(this object obj)
        {
            var type = obj.GetType();
            var serialize = JSON.Serialize(type, obj);
            var deserialize = JSON.Deserialize(type, serialize);
            return deserialize;
        }

        public static T JsonClone<T>(this T obj)
        {
            var type = typeof(T);
            var serialize = JSON.Serialize(type, obj);
            var deserialize = JSON.Deserialize<T>(serialize);
            return deserialize;
        }
    }
}