using System;
using System.Reflection;

using Assets.Tools.Script.Attributes;
using Assets.Tools.Script.Editor.Inspector.Field;

using UnityEditor;

namespace Assets.Framework.Lua.Editor.LuaUi
{
    public class LuaUiEventInspector : FieldInspectorParser
    {
        public override string Name
        {
            get
            {
                return "LuaUiEvent";
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
            if (value == null)
            {
                value = "";
            }
            string str = value as string;
            if (str.StartsWith("_"))
            {
                str = str.Substring(1, str.Length - 1);
            }
            str = EditorGUILayout.TextField(style.Name, str);
            str = "_" + str;
            return str;
        }
    }
}


