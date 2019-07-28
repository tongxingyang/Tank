// ----------------------------------------------------------------------------
// <copyright file="ExportMonoUiLuaFile.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>21/12/2015</date>
// ----------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Text;

using Assets.Framework.Lua.LuaUi.Event;
using Assets.Tools.Script.Helper;

using UnityEditor;

using UnityEngine;

namespace Assets.Framework.Lua.Editor.LuaUi
{
    using Assets.Framework.Lua.Editor.Util;

    using LuaUi = Assets.Framework.Lua.LuaUi.LuaUi;

    public class ExportMonoUiLuaFile
    {
        public const string UserCode = "--==userCode==--";

        public static void ExportLuaUiFile(LuaUi luaUi)
        {
            var s = EditorPrefs.GetString("ExportMonoUiLuaFilePath");
            if (s == null)
            {
                s = "";
            }
            var saveFilePath = EditorUtility.SaveFilePanel("选择导出目录", s, luaUi.FileName, "lua");
            if (saveFilePath.IsNullOrEmpty())
            {
                return;
            }
            var strings = saveFilePath.Split('/');
            var fileName = strings[strings.Length-1];
            var saveFilePanel = saveFilePath.Replace(fileName, "");

            ExportLuaUiFile(luaUi, saveFilePanel, fileName);

            EditorPrefs.SetString("ExportMonoUiLuaFilePath", saveFilePanel);
        }

        public static void ExportLuaUiFile(LuaUi luaUi,string saveFilePanel,string fileName)
        {
            var exprotNode = ExprotNode(luaUi, fileName.Replace(".lua", ""), false);

            if (File.Exists(saveFilePanel + fileName))
            {
                exprotNode = MergeFile(luaUi, exprotNode, saveFilePanel + fileName);
            }

            File.WriteAllText(string.Format("{0}/{1}", saveFilePanel, fileName), exprotNode);
        }

        public static string PreviewLuaFile(LuaUi luaUi, string saveFilePanel, string fileName)
        {
            string exprotNode;
            if (fileName == null)
            {
                exprotNode = ExprotNode(luaUi, luaUi.FileName, false);
            }
            else
            {
                exprotNode = ExprotNode(luaUi, fileName.Replace(".lua", ""), false);

                if (File.Exists(saveFilePanel + fileName))
                {
                    exprotNode = MergeFile(luaUi, exprotNode, saveFilePanel + fileName);
                }
            }
            return exprotNode;
        }

        private static string MergeFile(LuaUi luaUi, string curr, string luaPath)
        {
            var old = File.ReadAllText(luaPath);
            curr = MergeUserCode(luaUi,curr, old);
            curr = MergeFunction(luaUi, curr, old, luaPath);
            //        curr = MergeInitFunction(luaUi, curr, old, luaPath);
            return curr;
        }

        private static string MergeUserCode(LuaUi luaUi, string curr, string old)
        {
            if (old.Contains(UserCode))
            {
                var oldTexts = old.Split(new[] { UserCode }, StringSplitOptions.None);
                var newTexts = curr.Split(new[] { UserCode }, StringSplitOptions.None);
                for (int i = 0; i < newTexts.Length; i++)
                {
                    if (i % 2 == 1)
                    {
                        newTexts[i] = oldTexts[i];
                    }
                }
                return newTexts.Joint(UserCode);
            }
            return curr;
        }
    
        private static string MergeFunction(LuaUi luaUi, string curr, string old, string luaPath)
        {
            string initFunc =
@"
    
end";
            initFunc = initFunc.Replace("{0}", luaUi.FileName);

            curr = MergeFunction(string.Format("function {0}:Init()", luaUi.FileName), initFunc, curr, old, luaPath, true);
            curr = MergeFunction(string.Format("function {0}:Dispose()", luaUi.FileName), "\r\n\r\nend", curr, old, luaPath, true);
            curr = MergeFunction(string.Format("function {0}:OnEnable()", luaUi.FileName), "\r\n\r\nend", curr, old, luaPath, true);
            curr = MergeFunction(string.Format("function {0}:OnDisable()", luaUi.FileName), "\r\n\r\nend", curr, old, luaPath, true);
            foreach (var luaUiEvent in luaUi.Events)
            {
                string funcName = string.Format("function {0}:{1}", luaUi.FileName, luaUiEvent.FullName);
                string funcName2 = string.Format("{0}\r\n\r\nend", luaUiEvent.GetSignature());
                curr = MergeFunction(funcName, funcName2, curr, old, luaPath, false);
            }
            return curr;
        }

        private static string MergeFunction(string funcName,string funcName2, string curr, string old, string luaPath,bool ignoreIfNotExist)
        {
            var oldLines = File.ReadAllLines(luaPath);
            bool started = false;
            StringBuilder userCode = new StringBuilder();
            for (int i = 0; i < oldLines.Length; i++)
            {
                var line = oldLines[i];
                if (started)
                {
                    if (line.StartsWith("end"))
                    {
                        userCode.Append(line);
                        break;
                    }
                    else
                    {
                        userCode.Append(line);
                        userCode.Append("\r\n");
                    }
                }
                else
                {
                    if (line.StartsWith(funcName))
                    {
                        userCode.Append(line);
                        userCode.Append("\r\n");
                        started = true;
                    }
                }
            }
            if (userCode.Length > 0 || ignoreIfNotExist)
            {
                string func = string.Format("{0}{1}", funcName, funcName2);
                return curr.Replace(func, userCode.ToString());
            }
            return curr;
        }

        private static string ExprotNode(LuaUi luaUi, string fileName, bool itemLike)
        {

            //0:面板名字 1:其他字段 2:panelElement(现在没有使用) 3:初始化字段 4:return itemLike 5:内置监听
            //6:获得luaUi 8:public字段赋值
            //{10}:事件回调
            //{11}:prefab路径
            string template = @"---@class {0}
local {0} = class('{0}',uView)
{0}.ViewPrefabPath = '{11}'

---@type GameObject
{0}.gameObject = nil

---@type Transform
{0}.transform = nil

{1}
{UserCode}

{UserCode}

function {0}:Init()
    
end

function {0}:Binding(object)
    self.gameObject = object
    self.transform = object.transform
    
    local luaUi = {6}
    local luaUiFields = {}
    luaUi:Initialize(luaUiFields,self)    
{3}
end

{10}

{5}
{UserCode}

{UserCode}

return {0}
";

            template = template.Replace("{UserCode}", UserCode);
            template = template.Replace("{0}", fileName);
            template = ReplaceMessageDelegate(template, luaUi, fileName);
            template = ReplaceFields(template, luaUi, fileName);
            template = ReplaceListener(template, luaUi, fileName);
            template = ReplaceFieldInits(template, luaUi, fileName);
            template = ReplacePublicFieldInits(template, luaUi, fileName);
            template = ReplacePrefabPath(template, luaUi, fileName);
            template = template.Replace("{6}", "object:GetComponent(LuaUiClassType)");

            return template;
        }

        private static string ReplacePrefabPath(string template, LuaUi luaUi, string fileName)
        {
            return template.Replace("{11}", luaUi.PrefabPath);
        }
        private static string ReplaceMessageDelegate(string template, LuaUi luaUi, string fileName)
        {
            string func = string.Format(
                @"function {0}:{1}()

end
", fileName,"{0}");

            StringBuilder builder = new StringBuilder();
            var fieldInfos = luaUi.Life.GetType().GetFields(BindingFlags.Public| BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == typeof(bool) && (bool)fieldInfo.GetValue(luaUi.Life))
                {
                    builder.AppendFormat(func, fieldInfo.Name);
                    builder.AppendLine();
                }
            }

            return template.Replace("{10}", builder.ToString());
        }

        private static string ReplaceFieldInits(string template, LuaUi luaUi, string fileName)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < luaUi.Fields.Count; i++)
            {
                var luaUiField = luaUi.Fields[i];
                WriteFieldInit(luaUi, i, luaUiField, builder);
            }
        
            return template.Replace("{3}", builder.ToString());
        }

        private static void WriteFieldInit(LuaUi luaUi, int index, LuaUi.LuaUiField field, StringBuilder builder)
        {
            builder.Append("    self.");
            builder.Append(field.FieldName);
            builder.Append(" = luaUiFields[");
            builder.Append(index + 1);
            builder.Append("]\r\n");
        }

        private static string ReplacePublicFieldInits(string template, LuaUi luaUi, string fileName)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < luaUi.Fields.Count; i++)
            {
                var luaUiField = luaUi.Fields[i];
                if (luaUiField.IsPublic)
                {
                    WritePublicFieldInit(luaUi, i, luaUiField, fileName, builder);
                }
            }

            return template.Replace("{8}", builder.ToString());
        }

        private static void WritePublicFieldInit(LuaUi luaUi, int index, LuaUi.LuaUiField field, string fileName, StringBuilder builder)
        {
            builder.Append("    ");
            builder.Append("self.");
            builder.Append(field.FieldName);
            builder.Append(" = ");
            builder.Append(field.FieldName);
            builder.Append("\r\n");
        }


        private static string ReplaceFields(string template, LuaUi luaUi, string fileName)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var luaUiField in luaUi.Fields)
            {
                WriteField(luaUiField, fileName, builder);
            }
            return template.Replace("{1}", builder.ToString());
        }

        private static void WriteField(LuaUi.LuaUiField field, string fileName, StringBuilder builder)
        {
            builder.Append("---@type ");
            builder.Append(LuaSerializer.GetLuaTypeName(field.FieldType.GetType()));
            builder.Append("\r\n");
            builder.Append(fileName);
            builder.Append(".");
            builder.Append(field.FieldName);
            builder.Append(" = ");
            builder.Append(EmmyLuaPlugin.GetTypeDefaultValue(field.FieldType.GetType()));
            builder.Append("\r\n");
            builder.Append("\r\n");
        }

        private static string ReplaceListener(string template, LuaUi luaUi, string fileName)
        {
            StringBuilder funcBuilder = new StringBuilder();
            for (int i = 0; i < luaUi.Events.Count; i++)
            {
                var luaUiEvent = luaUi.Events[i];
                WriteListener(luaUi, i, luaUiEvent, fileName, funcBuilder);
            }
            template = template.Replace("{5}", funcBuilder.ToString());
            return template;
        }

        private static void WriteListener(LuaUi luaUi, int index, LuaUi.LuaUiEvent uiEvent, string fileName, StringBuilder funcBuilder)
        {
            funcBuilder.Append("function ");
            funcBuilder.Append(fileName);
            funcBuilder.Append(":");
            funcBuilder.Append(uiEvent.FullName);
            funcBuilder.Append(uiEvent.GetSignature());
            funcBuilder.Append("\r\n");
            funcBuilder.Append("\r\n");
            funcBuilder.Append("end");
            funcBuilder.Append("\r\n");
            funcBuilder.Append("\r\n");
        }

        public static string GetPath(GameObject root, GameObject node)
        {
            Transform currNode = node.transform;
            string path = "";
            while (root.transform != currNode)
            {
                if (root.transform == currNode.parent)
                {
                    path = currNode.name + path;
                }
                else
                {
                    path = "/" + currNode.name + path;
                }
                currNode = currNode.parent;
            }
            return path;
        }
    }
}
