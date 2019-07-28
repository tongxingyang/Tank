// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Builtin
{
    using System;

    using Assets.Framework.LetsScript.Editor.Renderer.Core;
    using Assets.Framework.LetsScript.Editor.Renderer.Lua;
    using Assets.Framework.LetsScript.Editor.Renderer.Window;
    using Assets.Tools.Script.Editor.Window;

    using UnityEditor;

    /// <summary>
    /// 动作内容渲染
    /// </summary>
    public class LuaStylingActionRenderer : LuaStylingRenderer,IActionRenderer
    {
        public override LuaCommandRenderer NewInstance()
        {
            var luaLsActionRenderer = new LuaStylingActionRenderer();
            luaLsActionRenderer.CommandName = this.CommandName;
            luaLsActionRenderer.StyleLines = this.StyleLines;
            luaLsActionRenderer.Path = this.Path;
            luaLsActionRenderer.Description = this.Description;
            luaLsActionRenderer.Parameters = this.Parameters;
            return luaLsActionRenderer;
        }

        protected override PopMenuWindow CreateLeftMenu()
        {
            PopMenuWindow menu = CommandWindowTool.ActionWindow(this.Content, this.ParentContent, this.Property, (list =>
            {
                if (list == null || list.Count == 0)
                {
                    return;
                }
                Undo.RecordObject(this.ParentContent, "add action");
                
                if (this.Property.PropertyName is int)
                {
                    var index = Convert.ToInt32(this.Property.PropertyName);
                    this.ParentContent.RemoveContent(this.Content);
                    for (int i = 0; i < list.Count; i++)
                    {
                        this.ParentContent.InsertChildContent(list[i], index + i);
                    }
                }
                else
                {
                    this.ParentContent.RemoveContent(this.Content);
                    this.ParentContent.SetChildContent(this.Property.PropertyName, list[0]);
                }
            }));
            return menu;
        }
    }
}