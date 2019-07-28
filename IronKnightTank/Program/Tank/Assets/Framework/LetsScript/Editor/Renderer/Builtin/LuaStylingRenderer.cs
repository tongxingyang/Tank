// ----------------------------------------------------------------------------
// <copyright file="LuaDefaultCommandRenderer.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>05/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Builtin
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Assets.Framework.LetsScript.Editor.Renderer;
    using Assets.Framework.LetsScript.Editor.Renderer.Lua;

    public abstract class LuaStylingRenderer : LuaCommandRenderer
    {
        /// <summary>
        /// 显示样式
        /// 行,行元素
        /// </summary>
        protected List<List<StyleElement>> StyleLines = new List<List<StyleElement>>();

        public override void Render()
        {
            //逐行渲染
            for (int i = 0; i < this.StyleLines.Count; i++)
            {
                var styleLine = this.StyleLines[i];

                this.BeginLine();

                //渲染这一行
                this.RenderCommandLine(styleLine);

                this.EndLine();
            }
        }

        protected virtual void RenderCommandLine(List<StyleElement> styleLine)
        {
            //逐元素渲染
            foreach (var styleData in styleLine)
            {
                switch (styleData.Type)
                {
                    case StyleType.Property:
                        ContentRendererFactory.GetRenderer(this, this.Content, this.Property, this.Parameters[styleData.Data]).Render();
                        break;
                    case StyleType.Text:
                        this.ContentLabel(styleData.Data);
                        break;
                    case StyleType.Tab:
                        this.ContentLabel("     ");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected override void OnInitializeRenderer(LuaDescription description)
        {
            switch (description.Name)
            {
                case "style":
                    var styleDatas = new List<StyleElement>();
                    this.StyleLines.Add(styleDatas);
                    string input = description.Description;

                    var regex = new Regex("\\[[^\\]]*\\]");
                    var matchCollection = regex.Matches(input);

                    foreach (var match in matchCollection)
                    {
                        var styleProperty = match.ToString();
                        var propertyIndex = input.IndexOf(styleProperty);
                        var preProperty = input.Substring(0, propertyIndex);
                        if (preProperty.Length > 0)
                        {
                            styleDatas.Add(new StyleElement(input.Substring(0, propertyIndex), StyleType.Text));
                        }
                        styleDatas.Add(new StyleElement(input.Substring(propertyIndex + 1, styleProperty.Length - 2), StyleType.Property));
                        var startIndex = propertyIndex + styleProperty.Length;
                        input = input.Substring(startIndex, input.Length - startIndex);
                    }
                    if (input.Length > 0)
                    {
                        styleDatas.Add(new StyleElement(input, StyleType.Text));
                    }
                    break;
            }
            base.OnInitializeRenderer(description);
        }
    }
}