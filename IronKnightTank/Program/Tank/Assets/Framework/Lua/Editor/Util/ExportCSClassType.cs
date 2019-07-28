// ----------------------------------------------------------------------------
// <copyright file="ExportCSClassType.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>15/03/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.Lua.Editor.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ExportCSClassType
    {
        public static List<Type> GetAllType()
        {
            List<Type> types = new List<Type>();
            types.AddRange(CustomSettings.staticClassTypes);

            var customTypeList = CustomSettings.customTypeList;
            foreach (var bindType in customTypeList)
            {
                types.Add(bindType.type);
            }
            types.AddRange(ToLuaMenu.baseType);
            types.AddRange(ToLuaMenu.dropType);


            Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
            foreach (var type in types)
            {
                dictionary[type] = type;
            }
            return dictionary.Keys.ToList();
        }
    }
}