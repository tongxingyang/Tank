using System;
using System.Collections.Generic;
using System.Reflection;

using Assets.Tools.Script.Attributes;
using Assets.Tools.Script.Editor.Inspector.Field;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Assets.Framework.Lua.Editor.LuaUi
{
    using LuaUi = Assets.Framework.Lua.LuaUi.LuaUi;

    public class LuaUiFieldInspector : FieldInspectorParser
    {
        public static void GetSelectableObjects(Object o,out Object[] objects, out string[] typeNames)
        {
            Component[] addComponents = null;
            objects = null;
            if (o is GameObject)
            {
                addComponents = (o as GameObject).GetComponents<Component>();
                objects = new Object[addComponents.Length + 1];
                objects[0] = o;
                for (int i = 0; i < addComponents.Length; i++)
                {
                    var addComponent = addComponents[i];
                    objects[i + 1] = addComponent;
                }
            }
            else
            {
                objects = new[] { o };
            }

            Dictionary<string, int> nameCount = new Dictionary<string, int>();
            typeNames = new string[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                var type = objects[i];
                var name = type.GetType().Name;
                if (nameCount.ContainsKey(name))
                {
                    typeNames[i] = name + "_" + (++nameCount[name]);
                }
                else
                {
                    typeNames[i] = name;
                    nameCount.Add(name, 1);
                }
            }
        }

        public override string Name { get
        {
            return "LuaUiFieldType";
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

            if (o == null)
            {
                return value;
            }
            var fieldType = value as Object;
            if (fieldType == null)
            {
                fieldType = o;
            }

            string[] typeNames;
            Object[] objects;
            GetSelectableObjects(o,out objects,out typeNames);

            int currSelect = objects.ToList().FindIndex(e => e == fieldType);
            if (currSelect < 0)
            {
                currSelect = 0;
            }
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            var popup = EditorGUILayout.Popup("Component", currSelect, typeNames);
            fieldType = objects[popup];

            GUILayout.EndHorizontal();
            //GUILayout.Label(o == null?"null": o.ToString());
            return fieldType;
        }
    }
}
