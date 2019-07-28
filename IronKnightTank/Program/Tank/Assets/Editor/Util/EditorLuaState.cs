// ----------------------------------------------------------------------------
// <copyright file="EditorLuaState.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>09/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.Lua.Editor.Util
{
    using LuaInterface;

    public class EditorLuaState
    {
        #region LuaState

        public static readonly LuaState lua;

        public static readonly LuaFunction jsonEncode;
        public static readonly LuaFunction load2json;

        /// <summary>
        /// 重载class和require
        /// </summary>
        private static string luaclass = @"
luarequire = require
jsonutil = luarequire 'cjson'

function require(name)
    return name
end

function scriptclass(name,base)
    local t = {}
    t.__classname = name
    t.__supername = base
    return t
end

function load2json(script)
    return jsonutil.encode(luarequire(script))
end
";
        static EditorLuaState()
        {
            if (lua == null)
            {
                lua = new LuaState();
                lua.Start();
                //lua
                lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
                lua.OpenLibs(LuaDLL.luaopen_cjson);
                lua.LuaSetField(-2, "cjson");

                lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
                lua.LuaSetField(-2, "cjson.safe");

                //loader
                lua.DoString(luaclass);

                load2json = lua.GetFunction("load2json");
                jsonEncode = lua.GetTable("jsonutil").GetLuaFunction("encode");
            }
        }
        #endregion
    }
}