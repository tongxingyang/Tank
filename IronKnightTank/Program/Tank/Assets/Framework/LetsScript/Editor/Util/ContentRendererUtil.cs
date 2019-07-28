// ----------------------------------------------------------------------------
// <copyright file="ContentRendererUtil.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>02/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Util
{
    using System;
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Renderer.Core;

    using UnityEditor;

    public static class ContentRendererUtil
    {
        /// <summary>
        /// 当前剪切板 
        /// </summary>
        public static List<ContentRenderer> Clipboard = new List<ContentRenderer>();

        /// <summary>
        /// 当前选中的Renderer
        /// </summary>
        public static List<ContentRenderer> Selected = new List<ContentRenderer>();

        /// <summary>
        /// 最后一个被操作的Renderer
        /// </summary>
        /// <value>The last selected.</value>
        public static ContentRenderer LastSelected { get; private set; }

        public static void Select(this ContentRenderer renderer)
        {
            if (!renderer.IsSelected)
            {
                Selected.Add(renderer);
                renderer.IsSelected = true;
            }
            else
            {
                Selected.Remove(renderer);
                Selected.Add(renderer);
            }
            LastSelected = renderer;
        }

        public static void Unselect(this ContentRenderer renderer, bool fallback = false)
        {
            if (fallback && Selected.Count > 1)
            {
                var back = Selected[Selected.Count - 2];
                back.Select();
            }
            if (LastSelected == renderer)
            {
                return;
            }
            Selected.Remove(renderer);
            renderer.IsSelected = false;
        }

        /// <summary>
        /// 除了最后选中的，取消选中状态
        /// </summary>
        public static void UnselectPreselected()
        {
            foreach (var renderer in Selected.ToList())
            {
                renderer.Unselect();
            }
        }

        /// <summary>
        /// 判断是否是一行的起始
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <returns><c>true</c> if [is beginning of line] [the specified renderer]; otherwise, <c>false</c>.</returns>
        public static bool IsBeginningOfLine(ContentRenderer renderer)
        {
            //TODO:判断是新的一行，用其他方式，代替该判断
            return renderer.Property.PropertyName is int;
        }

        /// <summary>
        /// 复制到剪切板
        /// </summary>
        /// <param name="renderers">The renderers.</param>
        public static void CopyRenderers(List<ContentRenderer> renderers)
        {
            if (renderers == null || renderers.Count == 0)
            {
                return;
            }

            //按原本在视图中的先后排序
            renderers.Sort(
                        (a, b) =>
                        {
                            var an = a.Property.PropertyName.ToString().PadLeft(10);
                            var bn = b.Property.PropertyName.ToString().PadLeft(10);
                            return StringComparer.Ordinal.Compare(an, bn);
                        });
            Clipboard.Clear();
            Clipboard.AddRange(renderers);

        }

        /// <summary>
        /// 删除一组渲染器
        /// </summary>
        /// <param name="renderers">The renderers.</param>
        public static void DeleteRenderers(List<ContentRenderer> renderers)
        {
            if (renderers == null || renderers.Count == 0)
            {
                return;
            }
            UnityEngine.Object[] deleteRoot = new UnityEngine.Object[renderers.Count];
            for (int i = 0; i < renderers.Count; i++)
            {
                deleteRoot[i] = (renderers[i].ParentContent);
            }
            Undo.RecordObjects(deleteRoot, "delete");
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].ParentContent.RemoveContent(renderers[i].Content);
            }
        }

        /// <summary>
        /// 在指定位置插入一组
        /// </summary>
        /// <param name="renderers">The renderers.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <param name="index">The index.</param>
        public static void InsertRenderers(List<ContentRenderer> renderers, CommonContent target, ContentProperty targetProperty, int index)
        {
            if (renderers == null || renderers.Count == 0)
            {
                return;
            }
            List<CommonContent> commands = new List<CommonContent>();
            for (int i = 0; i < renderers.Count; i++)
            {
                commands.Add(renderers[i].Content);
            }
            InsertRenderers(commands, target, targetProperty, index);
        }

        public static void InsertRenderers(List<CommonContent> commands, CommonContent target, ContentProperty targetProperty, int index)
        {
            if (commands == null || commands.Count == 0)
            {
                return;
            }

            //只接受在列表中插入
            string name = null;
            if (ContentType.IsList(targetProperty.PropertyType))
            {
                if (ContentType.Unlist(targetProperty.PropertyType) == ContentType.Action)
                {
                    name = ContentUtil.ActionName;
                }
                else
                {
                    name = ContentUtil.VariableName;
                }
            }
            if (name == null)
            {
                return;
            }

            //选取指定的Action或者Variable类型
            List<ContentRenderer> list = new List<ContentRenderer>();
            foreach (var renderer in Clipboard)
            {
                if (renderer.Content.GetChildContent(name) != null)
                {
                    list.Add(renderer);
                }
            }
            Undo.RecordObject(target, "add command");
            for (int i = 0; i < commands.Count; i++)
            {
                var newCommand = commands[i].Clone();
                newCommand.Editor.NewData = true;
                target.InsertChildContent(newCommand, index + 1 + i);
            }
        }
    }
}