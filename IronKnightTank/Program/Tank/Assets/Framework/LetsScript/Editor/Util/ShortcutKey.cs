// ----------------------------------------------------------------------------
// <copyright file="ShortcutKey.cs" company="上海序曲网络科技有限公司">
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
    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Script;
    using Assets.Framework.LetsScript.Editor.Script.Editor;

    using UnityEditor;

    using UnityEngine;

    public class ShortcutKey
    {
        public IScriptEditor Editor;

        public void OnGUI()
        {
            //保存 Ctrl + S
            if (this.Editor != null && Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.S && Event.current.control)
            {
                ScriptSerializer.SerializeFile(this.Editor.GetScriptData());
                EditorWindow.focusedWindow.ShowNotification(new GUIContent("保存成功"));
                Event.current.Use();
            }
            //复制 Ctrl + C
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.C && Event.current.control)
            {
                ContentRendererUtil.CopyRenderers(ContentRendererUtil.Selected);
                Event.current.Use();
            }
            //粘帖 Ctrl + V
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.V && Event.current.control)
            {
                if (ContentRendererUtil.Selected.Count == 1 && ContentType.IsList(ContentRendererUtil.LastSelected.ParentProperty.PropertyType))
                {
                    var index = (int)ContentRendererUtil.LastSelected.Property.PropertyName;
                    ContentRendererUtil.InsertRenderers(ContentRendererUtil.Clipboard, ContentRendererUtil.LastSelected.ParentContent, ContentRendererUtil.LastSelected.ParentProperty, index);
                    Event.current.Use();
                }
            }
            //删除 Ctrl + D
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.D && Event.current.control)
            {
                ContentRendererUtil.DeleteRenderers(ContentRendererUtil.Selected);
                Event.current.Use();
            }
            //剪切 Ctrl + X
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.X && Event.current.control)
            {
                ContentRendererUtil.CopyRenderers(ContentRendererUtil.Selected);
                ContentRendererUtil.DeleteRenderers(ContentRendererUtil.Selected);
                Event.current.Use();
            }
        } 
    }
}