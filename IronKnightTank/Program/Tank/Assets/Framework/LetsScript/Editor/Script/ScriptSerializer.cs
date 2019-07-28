// ----------------------------------------------------------------------------
// <copyright file="ScriptSerializer.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>02/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Script
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Assets.Framework.LetsScript.Editor;
    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Util;
    using Assets.Framework.Lua.Editor.Util;

    using LitJson;

    using LuaInterface;

    using UnityEditor;

    using UnityEngine.Assertions;

    public class ScriptSerializer
    {

        #region Serialize
        private static Dictionary<int, string> depthTab = new Dictionary<int, string>();

        private static string scriptTemplate = @"
---------------------------------------------
--- {scriptname}
--- Generate by LetsScript {version}
--- Author: {username}
--- DateTime: 2018/04/29
---------------------------------------------

---@class {scriptname} : {superclassname}
local {scriptname} = scriptclass('{scriptname}',require('{superclass}'))

{contents}

return {scriptname}
";
        

        public static string Serialize(ScriptData scriptData)
        {
            //
            StringBuilder contentBuilder = new StringBuilder();
            var scriptContents = scriptData.Contents.AsDictionary();
            var scriptContentKeys = scriptContents.Keys.ToList();
            scriptContentKeys.Sort(StringComparer.Ordinal.Compare);
            for (int i = 0; i < scriptContentKeys.Count; i++)
            {
                var childKey = scriptContentKeys[i];
                var childContent = scriptContents[childKey];
                StringBuilder childContentBuilder = new StringBuilder();
                BuildContent(childContentBuilder, childContent, 0);
                contentBuilder.Append(String.Format("{0}.{1} = {2}", scriptData.ScriptName, childKey, childContentBuilder.ToString()));
                contentBuilder.AppendLine();
            }

            //
            var superPath = scriptData.SuperClass.Split('.');
            string superClassName = superPath[superPath.Length - 1];



            //
            string script = scriptTemplate;
            script = script.Replace("{scriptname}", scriptData.ScriptName);
            script = script.Replace("{version}", LetsScriptSetting.Version);
            script = script.Replace("{username}", System.Environment.UserName);
            script = script.Replace("{superclassname}", superClassName);
            script = script.Replace("{superclass}", scriptData.SuperClass);
            script = script.Replace("{contents}", contentBuilder.ToString());

            return script;
        }

        public static void SerializeFile(ScriptData scriptData)
        {
            var serialize = Serialize(scriptData);
            string savePath = scriptData.FilePath;
            if (savePath.IsNullOrEmpty())
            {
                savePath = EditorUtility.SaveFilePanel("保存脚本", null, null, "lua");
            }
            if (savePath.IsNOTNullOrEmpty())
            {
                File.WriteAllText(savePath, serialize);
            }
        }

        private static void BuildContent(StringBuilder builder, CommonContent content, int depth)
        {
            if (content.IsValue())
            {
                var value = content.AsValue();
                if (value is string)
                {
                    value = (value as string).Replace("\r\n", "\\r\\n");
                    value = (value as string).Replace("\n", "\\r\\n");
                    value = (value as string).Replace("\r", "\\r\\n");
                }
                builder.Append(LuaSerializer.Serialize(value));
            }
            else if (IsComment(content))
            {
                var comment = GetComment(content);
                builder.Append((string)comment);
            }
            else
            {
                builder.AppendLine("{");

                
                var childrenContentList = content.AsList();
                for (int i = 0; i < childrenContentList.Count; i++)
                {
                    var childContent = childrenContentList[i];

                    AppendTab(builder, depth + 1);
                    BuildContent(builder, childContent, depth + 1);
                    builder.AppendLine(",");
                }

                var childrenContentMap = content.AsDictionary();
                foreach (var child in childrenContentMap)
                {
                    AppendTab(builder, depth + 1);
                    builder.Append(child.Key);
                    builder.Append(" = ");
                    BuildContent(builder, child.Value, depth + 1);
                    builder.AppendLine(",");
                }

                AppendTab(builder, depth);
                builder.Append("}");
            }
        }

        private static void AppendTab(StringBuilder builder, int depth)
        {
            string tab;
            if (!depthTab.TryGetValue(depth, out tab))
            {
                tab = "";
                for (int i = 0; i < depth; i++)
                {
                    tab += "    ";
                }
                depthTab[depth] = tab;
            }
            builder.Append(tab);
        }
        #endregion

        #region Deserialize


        public static ScriptData Deserialize(string luacode)
        {
            return DeserializeLua(luacode);
        }

        public static ScriptData DeserializeLua(string luacode)
        {
            luacode = RestoreComment(luacode);
            var table = EditorLuaState.lua.DoString<LuaTable>(luacode);
            var json = EditorLuaState.jsonEncode.Invoke<LuaTable, string>(table);
            return DeserializeJson(json);
        }

        public static ScriptData DeserializeFile(string filePath)
        {
            var fileText = File.ReadAllText(filePath);
            
            var deserializeJson = DeserializeLua(fileText);
            deserializeJson.FilePath = filePath;
            return deserializeJson;
        }

        private static ScriptData DeserializeJson(string json)
        {
            JsonData jsonData = JsonMapper.ToObject(json);
            var scriptContent = DeserializeJsonData(jsonData);

            var scriptData = new ScriptData();
            scriptData.Contents = scriptContent;

            scriptData.ScriptName = scriptContent.GetChildContent("__classname").AsValue<string>();
            scriptContent.RemoveContent(scriptContent.GetChildContent("__classname"));
            scriptData.SuperClass = scriptContent.GetChildContent("__supername").AsValue<string>();
            scriptContent.RemoveContent(scriptContent.GetChildContent("__supername"));

            return scriptData;
        }

        public static CommonContent DeserializeJsonData(JsonData jsonData)
        {
            var commonContent = new CommonContent();
            if (jsonData.IsObject)
            {
                foreach (KeyValuePair<string, JsonData> child in jsonData)
                {
                    commonContent.SetChildContent(child.Key, DeserializeJsonData(child.Value));
                }
            }
            else if (jsonData.IsArray)
            {
                for (int i = 0; i < jsonData.Count; i++)
                {
                    var child = jsonData[i];
                    commonContent.SetChildContent(i, DeserializeJsonData(child));
                }
            }
            else if (jsonData.IsString)
            {
                commonContent.SetValue((string)jsonData);
            }
            else if (jsonData.IsInt)
            {
                commonContent.SetValue((int)jsonData);
            }
            else if (jsonData.IsBoolean)
            {
                commonContent.SetValue((bool)jsonData);
            }
            else if (jsonData.IsDouble)
            {
                commonContent.SetValue((float)((double)jsonData));
            }
            else if (jsonData.IsFloat)
            {
                commonContent.SetValue((float)jsonData);
            }
            else if (jsonData.IsLong)
            {
                commonContent.SetValue((float)((long)jsonData));
            }
            return commonContent;
        }

        #endregion

        #region Comments
        public static bool IsComment(CommonContent content)
        {
            return content.GetChildContent(ContentUtil.ActionName) != null && content.GetChildContent(ContentUtil.ActionName).AsValue<string>() == "ActionComments";
        }

        public static string GetComment(CommonContent content)
        {
            var commentContent = content.GetChildContent("Comments");
            Assert.IsTrue(commentContent.IsValue(), "Comments must be a text.");
            var comments = commentContent.AsValue<string>();
            comments = (comments).Replace("\r\n", "\\r\\n");
            comments = (comments).Replace("\n", "\\r\\n");
            comments = (comments).Replace("\r", "\\r\\n");
            return String.Format("--{0}Comments = '{1}'{2}", "{", comments, "}");
        }

        public static string RestoreComment(string text)
        {
            text = text.Replace("--{Comments = ", "{ActionName = \"ActionComments\",Comments = ");
            return text;
        }

        #endregion
    }
}