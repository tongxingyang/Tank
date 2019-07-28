// ----------------------------------------------------------------------------
// <copyright file="LuaReferenceCounter.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>25/06/2016</date>
// ----------------------------------------------------------------------------
using XQFramework.Lua;


namespace Assets.Scripts.Tools.Lua
{
    using System.Collections.Generic;

    using Assets.Tools.Script.Debug;

	using XQFramework ;

    using LuaInterface;
    
    using UnityEngine;

    public class LuaReferenceCounter
    {
        private static Dictionary<LuaTableRefProxy, string> tables = new Dictionary<LuaTableRefProxy, string>();
        
        private static LuaFunction checkFunc;
        private static LuaFunction printTabReferenceFunc;

        private static List<LuaTableRefProxy> destroyTables = new List<LuaTableRefProxy>();

        private static int snapshootIndex = 0;

        public static void Mark(string typeName, string table, string tableName)
        {
            var luaTableRefProxy = new LuaTableRefProxy() { Id = table };
            tables.Add(luaTableRefProxy, string.Empty);
            ReferenceCounter.Mark(typeName, luaTableRefProxy, tableName);
        }

        public static void GC()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            if (checkFunc == null)
            {
                checkFunc = LuaManager.Instance.GetTable("ReferenceCounter").GetLuaFunction("Has");
            }
            destroyTables.Clear();

            foreach (var table in tables.Keys)
            {
                var o = checkFunc.Invoke<string, bool>(table.Id);
                if (!o)
                {
                    destroyTables.Add(table);
                }
            }

            foreach (var table in destroyTables)
            {
                tables.Remove(table);
            }
        }

        public static void MarkAll()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            var luaFunction = LuaManager.Instance.GetTable("ReferenceCounter").GetLuaFunction("MarkAll");
            luaFunction.Call();
        }

        public static void Snapshoot()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            var snapshootFunc = LuaManager.Instance.GetTable("ReferenceCounter").GetLuaFunction("Snapshoot");
            snapshootFunc.Call();
            var analyzeSnapshootFunc = LuaManager.Instance.GetTable("ReferenceCounter").GetLuaFunction("AnalyzeSnapshoot");
            analyzeSnapshootFunc.Call(snapshootIndex, ++snapshootIndex);
        }

        private class LuaTableRefProxy : IReferenceCounterHandler
        {
            public string Id;

            public override string ToString()
            {
                return Id;
            }

            public void HandleClick()
            {
                if (!Application.isPlaying)
                {
                    return;
                }
                if (printTabReferenceFunc == null)
                {
                    printTabReferenceFunc = LuaManager.Instance.GetTable("ReferenceCounter").GetLuaFunction("PrintTabReferenceWithTabId");
                }
                printTabReferenceFunc.Call(Id);
            }
        }

    }
}