// ----------------------------------------------------------------------------
// <copyright file="ScriptEditorWindowFactory.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>03/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Script.Editor
{
    using System;
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor.Script;
    using Assets.Tools.Script.Reflec;

    /// <summary>
    /// 脚本编辑器窗口简单工厂类
    /// </summary>
    public class ScriptEditorWindowFactory
    {
        private static Dictionary<string, Type> editors; 

        public static ScriptEditorWindow GetWindow(ScriptData scriptData)
        {
            //初始化编辑器窗口集合
            if (editors == null)
            {
                editors = new Dictionary<string, Type>();
                var editorList = AssemblyTool.FindTypesInCurrentDomainWhereExtend<ScriptEditorWindow>();
                foreach (var type in editorList)
                {
                    var scriptEditorWindowAttribute = type.GetAttribute<ScriptEditorWindowAttribute>();
                    if (scriptEditorWindowAttribute != null)
                    {
                        editors.Add(scriptEditorWindowAttribute.Name, type);
                    }
                }
            }

            //寻找合适的编辑器窗口
            Type windowType = null;
            //脚本指定编辑器字段
            if (scriptData.Contents.GetChildContent("__EditorWindow") != null)
            {
                var windowName = scriptData.Contents.GetChildContent("__EditorWindow").AsValue<string>();
                if (editors.ContainsKey(windowName))
                {
                    windowType = editors[windowName];
                    return ReflecTool.Instantiate(windowType) as ScriptEditorWindow;
                }
            }
            //根据基类全名
            if (editors.ContainsKey(scriptData.SuperClass))
            {
                windowType = editors[scriptData.SuperClass];
                return ReflecTool.Instantiate(windowType) as ScriptEditorWindow;
            }

            //根据基类名
            var baseNames = scriptData.SuperClass.Split('.');
            var baseName = baseNames[baseNames.Length - 1];
            if (editors.ContainsKey(baseName))
            {
                windowType = editors[baseName];
                return ReflecTool.Instantiate(windowType) as ScriptEditorWindow;
            }

            return null;
        }
    }
}