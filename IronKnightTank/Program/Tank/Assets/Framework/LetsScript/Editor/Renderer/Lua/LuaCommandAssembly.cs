// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Lua
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Assets.Framework.LetsScript.Editor.Renderer.Builtin;
    using Assets.Tools.Script.Reflec;

    using UnityEngine;

    /// <summary>
    /// 来自Lua脚本的命令对象集
    /// </summary>
    public class LuaCommandAssembly
    {
        /// <summary>
        /// 已经完成加载
        /// </summary>
        public static bool Loaded { get; private set; }

        /// <summary>
        /// 指定命令的渲染
        /// </summary>
        private static Dictionary<string, Type> specifiedRenderers;

        /// <summary>
        /// 命令Renderer全集
        /// </summary>
        private static Dictionary<string,LuaCommandRenderer> Commands = new Dictionary<string, LuaCommandRenderer>();

        /// <summary>
        /// 解析缓存
        /// </summary>
        private static List<LuaDescription> descriptions = new List<LuaDescription>();

        /// <summary>
        /// 筛选一组需要的命令
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>List&lt;LuaCommandRenderer&gt;.</returns>
        public static List<LuaCommandRenderer> FindCommandRendererTemplates(Func<LuaCommandRenderer, bool> condition)
        {
            List < LuaCommandRenderer > templates = new List<LuaCommandRenderer>();
            foreach (var value in Commands.Values)
            {
                if (condition(value))
                {
                    templates.Add(value);
                }
            }
            return templates;
        }

        /// <summary>
        /// 获取命令模板
        /// </summary>
        /// <param name="commandName">Name of the command.</param>
        /// <returns>LuaCommandRenderer.</returns>
        public static LuaCommandRenderer GetCommandRendererTemplate(string commandName)
        {
            LuaCommandRenderer template;
            Commands.TryGetValue(commandName, out template);
            return template;
        }

        /// <summary>
        /// 实例化一个Renderer
        /// </summary>
        /// <param name="commandName">Name of the command.</param>
        /// <returns>LuaCommandRenderer.</returns>
        public static LuaCommandRenderer InstanceCommandRenderer(string commandName)
        {
            LuaCommandRenderer template;
            Commands.TryGetValue(commandName, out template);
            if (template != null)
            {
                return template.NewInstance();
            }
            return null;
        }

        /// <summary>
        /// 从Lua文件中读取,并初始化命令集
        /// </summary>
        /// <param name="rootPath">一组命令根目录</param>
        public static void Read(params string[] rootPath)
        {
            Commands.Clear();
            foreach (var path in rootPath)
            {
                var luafiles = Directory.GetFiles(path, "*.lua", SearchOption.AllDirectories);
                foreach (var luafile in luafiles)
                {
                    var luaCommandRenderer = ParseLuaFile(File.ReadAllLines(luafile));
                    if (luaCommandRenderer != null)
                    {
                        Commands.Add(luaCommandRenderer.CommandName, luaCommandRenderer);
                    }
                }
            }

            Loaded = true;
        }

        private static LuaCommandRenderer ParseLuaFile(string[] fileLines)
        {
            LuaCommandRenderer luaCommandRenderer = null;
            string commandType = null;
            foreach (var fileLine in fileLines)
            {
                LuaDescription description = ParseDescription(fileLine);
                if (description == null)
                {
                    descriptions.Clear();
                }
                else if (description.Name == "action" || description.Name == "variable")
                {
                    commandType = description.Name;
                }
                else if (description.Name == "class" && commandType != null)
                {
                    var classDescription = description.Description.Split(':');
                    string className = classDescription[0].Trim();
                    string superClassName = classDescription[1].Trim();
                    try
                    {
                        luaCommandRenderer = CreateTemplate(className, superClassName, commandType);
                        luaCommandRenderer.InitializeRender(className, superClassName, descriptions);
                    }
                    catch (Exception)
                    {
                        Debug.LogErrorFormat("Create {0}'s lua command renderer tempate error", className);
                    }
                }
                else if (description.Name == "parameter")
                {
                    if (luaCommandRenderer != null)
                    {
                        luaCommandRenderer.AddProperty(description.Description, descriptions);
                    }
                }
                else
                {
                    descriptions.Add(description);
                }
            }
            return luaCommandRenderer;
        }

        private static LuaCommandRenderer CreateTemplate(string commandName,string superCommandName, string commandType)
        {
            //初始化使用LuaCommandRendererAttribute标签标注的渲染器类型组
            if (specifiedRenderers == null)
            {
                specifiedRenderers = new Dictionary<string, Type>();
                var specifiedRendererTypes = AssemblyTool.FindTypesInCurrentDomainWhereAttributeIs<LuaCommandRendererAttribute>();
                foreach (var specifiedRendererType in specifiedRendererTypes)
                {
                    var luaCommandRendererAttribute = specifiedRendererType.GetAttribute<LuaCommandRendererAttribute>();
                    if (specifiedRenderers.ContainsKey(luaCommandRendererAttribute.CommandName))
                    {
                        Debug.LogError(luaCommandRendererAttribute.CommandName + " already exist.");
                    }
                    specifiedRenderers[luaCommandRendererAttribute.CommandName] = specifiedRendererType;
                }
            }
            //创建
            if (specifiedRenderers.ContainsKey(commandName))
            {
                //有渲染器指定了该命令
                return ReflecTool.Instantiate(specifiedRenderers[commandName]) as LuaCommandRenderer;
            }
            if (specifiedRenderers.ContainsKey(superCommandName))
            {
                //退回到父类样式
                return ReflecTool.Instantiate(specifiedRenderers[superCommandName]) as LuaCommandRenderer;
            }
            else if (commandType == "action")
            {
                //默认动作
                return new LuaStylingActionRenderer();
            }
            else if (commandType == "variable")
            {
                //默认变量
                return new LuaStylingVariableRenderer();
            }
            return null;
        }

        private static LuaDescription ParseDescription(string description)
        {
            if (!description.StartsWith("---@"))
            {
                return null;
            }
            string type, typeDescription = null;
            var segment = description.IndexOf(' ');
            if (segment >= 0)
            {
                type = description.Substring(4, segment - 4);
                typeDescription = description.Substring(segment + 1, description.Length - segment - 1).Trim();
            }
            else
            {
                type = description.Substring(4, description.Length - 4).Trim();
            }
            return new LuaDescription() { Description = typeDescription ,Name = type};
        }
    }
}