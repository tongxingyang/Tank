namespace Assets.Framework.Lua.Editor.Util
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Assets.Framework.Lua.Editor.LuaUi;

    using UnityEditor;

    using UnityEngine;

    public class EmmyLuaPlugin
    {
        [MenuItem("Lua/EmmyLua Plugin", false, 30)]
        public static void Export()
        {
            var s = EditorPrefs.GetString(savePathKey,"");
            var openFolderPanel = EditorUtility.OpenFolderPanel("选择导出目录", s, "");
            if (openFolderPanel.IsNullOrEmpty())
            {
                return;
            }
            EditorPrefs.SetString(savePathKey, openFolderPanel);

            Export(openFolderPanel);
        }

        public static void Export(string folderPanel)
        {
            string root = folderPanel + "/stubs";
            if (Directory.Exists(root))
            {
                FileUtil.DeleteFileOrDirectory(root + "/api");
            }
            else
            {
                Directory.CreateDirectory(root);
            }

            Debug.Log(string.Format("Export api plugin to {0}", root));
            Directory.CreateDirectory(root + "/api");

            ExportApi(root + "/api");
        }

        public static void ExportAndSaveToLastSelect()
        {
            var s = EditorPrefs.GetString(savePathKey, "");
            if (s.IsNullOrEmpty())
            {
                Export();
            }
            else
            {
                Export(s);
            }
        }

        private static string savePathKey
        {
            get
            {
                return Application.dataPath + "ExportLuaGliderPluginPath";
            }
        }
        //-----------------------------------------------API---------------------------------------------------//

        private static void ExportApi(string root)
        {
            foreach (var staticClassType in ExportCSClassType.GetAllType())
            {
                ExportApi(staticClassType, root);
            }
        }

        private static void ExportApi(Type type, string root)
        {
            string fileName = string.Format("{0}/{1}.lua", root, type.Name);

            File.WriteAllText(fileName, BuildClassApi(type));
        }

        private static string BuildClassApi(Type type)
        {
            string typeName = type.Name;
            StringBuilder builder = new StringBuilder();
            builder.Append("---@class ");
            builder.Append(typeName);
            builder.Append("\r\n");
            builder.Append("local ");
            builder.Append(typeName);
            builder.Append("\r\n");
            builder.Append("\r\n");
            builder.Append("\r\n");

            builder.Append("----------------------------static fields---------------------------------");
            builder.Append("\r\n");
            var fieldInfos = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fieldInfo in fieldInfos)
            {
                BuildField(type, typeName, fieldInfo, builder, true);
            }

            builder.Append("\r\n");
            builder.Append("----------------------------static properties---------------------------------");
            builder.Append("\r\n");
            var propertyInfos = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var propertyInfo in propertyInfos)
            {
                BuildProperty(type, typeName, propertyInfo, builder, true);
            }

            builder.Append("\r\n");
            builder.Append("----------------------------static functions---------------------------------");
            builder.Append("\r\n");
            var methodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var methodInfo in methodInfos)
            {
                BuildMethod(type, typeName, methodInfo, builder, true);
            }

            builder.Append("\r\n");
            builder.Append("----------------------------properties---------------------------------");
            builder.Append("\r\n");
            fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var fieldInfo in fieldInfos)
            {
                BuildField(type, typeName, fieldInfo, builder, false);
            }

            builder.Append("\r\n");
            builder.Append("----------------------------properties---------------------------------");
            builder.Append("\r\n");
            propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var propertyInfo in propertyInfos)
            {
                BuildProperty(type, typeName, propertyInfo, builder, false);
            }

            builder.Append("\r\n");
            builder.Append("----------------------------functions---------------------------------");
            builder.Append("\r\n");
            methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var methodInfo in methodInfos)
            {
                BuildMethod(type, typeName, methodInfo, builder, false);
            }

            builder.Append("\r\n");
            builder.Append("return ");
            builder.Append(typeName);
            return builder.ToString();
        }

        private static void BuildField(Type type, string typeName, FieldInfo fieldInfo, StringBuilder builder, bool isStatic)
        {
            if (fieldInfo.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
            {
                return;
            }
            builder.Append("---@type ");
            builder.Append(LuaSerializer.GetLuaTypeName(fieldInfo.FieldType));
            builder.Append("\r\n");
            builder.Append(typeName);
            if (isStatic) builder.Append("."); else builder.Append("def:");
            builder.Append(fieldInfo.Name);
            builder.Append("\r\n");
            builder.Append("\r\n");
        }

        private static void BuildProperty(Type type, string typeName, PropertyInfo propertyInfo, StringBuilder builder, bool isStatic)
        {
            if (propertyInfo.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
            {
                return;
            }
            builder.Append("---@type ");
            builder.Append(LuaSerializer.GetLuaTypeName(propertyInfo.PropertyType));
            builder.Append("\r\n");
            builder.Append(typeName);
            if (isStatic) builder.Append("."); else builder.Append("def:");
            builder.Append(propertyInfo.Name);
            builder.Append("\r\n");
            builder.Append("\r\n");
        }

        private static void BuildMethod(Type type, string typeName, MethodInfo methodInfo, StringBuilder builder, bool isStatic)
        {
            if (methodInfo.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
            {
                return;
            }
            //泛型方法不要
            var genericArguments = methodInfo.GetGenericArguments();
            if (genericArguments != null && genericArguments.Length > 0)
            {
                return;
            }
            //存取器方法不要
            if (methodInfo.Name.StartsWith("get_") || methodInfo.Name.StartsWith("set_"))
            {
                return;
            }

            var parameterInfos = methodInfo.GetParameters();
            builder.Append("---");
            builder.Append("\r\n");
            foreach (var parameterInfo in parameterInfos)
            {
                builder.Append("---@param ");
                builder.Append(parameterInfo.Name);
                builder.Append(" ");
                builder.Append(LuaSerializer.GetLuaTypeName(parameterInfo.ParameterType));
                builder.Append("\r\n");
            }
        

            if (methodInfo.ReturnType != typeof(void))
            {
                builder.Append("---@return ");
                builder.Append(LuaSerializer.GetLuaTypeName(methodInfo.ReturnType));
            }
            builder.Append("\r\n");
            builder.Append("--");
            builder.Append("\r\n");
            builder.Append("function ");
            builder.Append(typeName);
            if (isStatic)
                builder.Append(".");
            else
                builder.Append(":");

            builder.Append(methodInfo.Name);
            builder.Append("(");
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                var parameterInfo = parameterInfos[i];
                builder.Append(parameterInfo.Name);
                if (i < parameterInfos.Length - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(") ");
            builder.Append("end");
            builder.Append("\r\n");
            builder.Append("\r\n");
        }

        public static string GetTypeDefaultValue(Type type)
        {
            var typeName = LuaSerializer.GetLuaTypeName(type);
            switch (typeName)
            {
                case "number":
                    return "0";
                case "bool":
                    return "false";
                case "string":
                    return "nil";
            }
            return "nil";
        }
    }
}
