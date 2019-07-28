using System;
using System.Reflection;

using Assets.Tools.Script.Attributes;
using Assets.Tools.Script.Editor.Inspector.Field;
using Assets.Tools.Script.Helper;

using UnityEditor;

namespace Assets.Framework.Lua.Editor.LuaUi
{
    using LuaUi = Assets.Framework.Lua.LuaUi.LuaUi;

    public class LuaUiFieldNameInspector : FieldInspectorParser
    {
        public override string Name
        {
            get
            {
                return "LuaUiFieldName";
            }
        }

        public override object ParserFiled(
            InspectorStyle style,
            object value,
            Type t,
            FieldInfo fieldInfo,
            object instance,
            bool withName = true)
        {
            var luaUiField = instance as LuaUi.LuaUiField;
            var o = luaUiField.Source;

            string str = value as string;
            if (str.IsNullOrEmpty())
            {
                if (o != null)
                {
                    str = o.name;
                    if (luaUiField.FieldType != null)
                    {
                        var splitHump = luaUiField.FieldType.GetType().Name.SplitHump();
                        str = o.name + (splitHump.Count > 0 ? splitHump[splitHump.Count - 1] : "");
                    }
                }
            }
            if (withName)
            {
                str = EditorGUILayout.TextField(style.Name, str);
            }
            else
            {
                str = EditorGUILayout.TextField(str);
            }
            return str;
        }
    }
}

