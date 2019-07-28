// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Builtin
{
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor.Renderer.Core;
    using Assets.Framework.LetsScript.Editor.Renderer.Lua;
    using Assets.Framework.LetsScript.Editor.Renderer.Window;
    using Assets.Framework.LetsScript.Editor.Util;
    using Assets.Tools.Script.Editor.Window;

    using UnityEditor;

    /// <summary>
    /// 命令变量渲染
    /// </summary>
    public class LuaStylingVariableRenderer : LuaStylingRenderer,IVariableRenderer
    {
        public string VarType;

        protected override void OnInitializeRenderer(LuaDescription description)
        {
            switch (description.Name)
            {
                case "vartype":
                    this.VarType = description.Description;
                    break;
            }
            base.OnInitializeRenderer(description);
        }

        public override LuaCommandRenderer NewInstance()
        {
            var luaLsVariableRenderer = new LuaStylingVariableRenderer();
            luaLsVariableRenderer.CommandName = this.CommandName;
            luaLsVariableRenderer.StyleLines = this.StyleLines;
            luaLsVariableRenderer.Path = this.Path;
            luaLsVariableRenderer.Description = this.Description;
            luaLsVariableRenderer.Parameters = this.Parameters;
            luaLsVariableRenderer.VarType = this.VarType;
            return luaLsVariableRenderer;
        }

        protected override void RenderCommandLine(List<StyleElement> styleLine)
        {
            this.ContentLabel("(");
            base.RenderCommandLine(styleLine);
            this.ContentLabel(")");
        }

        public override string GetName()
        {
            return this.Content.GetChildContent("VariableName").AsValue() as string;
        }

        protected override PopMenuWindow CreateLeftMenu()
        {
            var menu = CommandWindowTool.VariableWindow(this.Content, this.ParentContent, this.Property,
            list =>
            {
                Undo.RecordObject(this.ParentContent, "select variable");
//                this.ParentContent.RemoveContent(this.Content);
                var selectValue = list[0];
                this.ParentContent.SetChildContent(this.Property.PropertyName, selectValue);
                this.Select();
                ContentRendererUtil.UnselectPreselected();
            });
            return menu;
        }

        public string GetVarType()
        {
            return this.VarType;
        }
    }
}