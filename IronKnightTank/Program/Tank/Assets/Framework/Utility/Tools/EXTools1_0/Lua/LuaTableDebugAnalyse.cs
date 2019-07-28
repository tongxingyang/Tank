using Assets.Extends.EXTools.Debug.Console;

namespace Assets.Scripts.Tools.Lua
{
    using System;
    using System.Collections.Generic;

    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Helper;

	using XQFramework.Lua;

    using LuaInterface;

    using UnityEngine;
    

    public class LuaTableDebugAnalyse : IObjectDebugAnalyse
    {
        public int a = 100;

        private Color blue;

        private Color white;

        private Color green;

        private LuaFunction tostringFunction;

        public LuaTableDebugAnalyse()
        {
            this.blue = ColorTool.GetColorFromRGBHexadecimal("4f9cd6");
            this.white = ColorTool.GetColorFromRGBHexadecimal("dcdcdc");
            this.green = ColorTool.GetColorFromRGBHexadecimal("b5cea8");
        }

        public void Show(object obj, string objName, ObjectAnalyseDisplayer displayer)
        {
            if (this.tostringFunction == null)
            {
				this.tostringFunction = LuaManager.Instance.GetTable("_G").GetLuaFunction("tostring");
            }
            var luaTable = obj as  LuaTable;
            List<object> keys = new List<object>();
            Dictionary<object, object> luaHashtable = new LuaTableEnumerator(luaTable).ToHashtable();

            foreach (var key in luaHashtable.Keys)
            {
                keys.Add(key);
            }

            keys.Sort(
                (l, r) =>
                    {
                        object lvalue = luaHashtable[l];
                        object rvalue = luaHashtable[r];
                        
                        

                        var ltype = (lvalue == null ? "Unknow" : lvalue.GetType().Name);
                        var rtype = (rvalue == null ? "Unknow" : rvalue.GetType().Name);

                        if (ltype != rtype)
                        {
                            return StringComparer.CurrentCulture.Compare(ltype, rtype);
                        }
                        else if (l is Double && r is Double)
                        {
                            var ln = (double)l;
                            var rn = (double)r;
                            if (ln>rn)
                            {
                                return 1;
                            }
                            return -1;
                        }
                        else if (l is Double)
                        {
                            return 1;
                        }
                        else if (r is Double)
                        {
                            return 1;
                        }
                        return StringComparer.CurrentCulture.Compare(l.ToString(), r.ToString());
                    });
            
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                //string keyName = key is LuaTable ? this.tostringFunction.Invoke<object, object>(key).ToString() : key.ToString();
                object value = luaHashtable[key];
                var s = string.Format(
                    "{0} {1} {2}",
                    (value == null ? "Unknow" : value.GetType().Name).SetColor(this.blue),
                    key.ToString().SetColor(this.white),
                    (value == null ? "nil" : value.ToString()).SetColor(this.green));
                if (displayer.GUILayoutBtn(s))
                {
                    if (value != null)
                    {
                        displayer.ShowObjectChild(value, string.Format("{0}.{1}", objName, key));
                    }
                }
            }
        }

        public bool IsActiveBy(object obj)
        {
            return obj is LuaTable;
        }
    }
}
