// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Builtin
{
    using Assets.Framework.LetsScript.Editor.Renderer.Core;
    using Assets.Framework.LetsScript.Editor.Renderer.Window;
    using Assets.Framework.LetsScript.Editor.Util;
    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Editor.Window;

    using UnityEditor;

    /// <summary>
    /// 简单值渲染
    /// </summary>
    public class SimpleValueRenderer : ContentRenderer,IVariableRenderer
    {
        public override void Render()
        {
            this.BeginLine();

            //显示内容
            if (this.Content.IsValue())
            {
                this.ContentLabel(this.Content.AsValue().ToString().SetColor(this.Property.Color));
            }
            else
            {
                this.ContentButton(this.Property.Description);
            }

            this.EndLine();
        }

        protected override PopMenuWindow CreateLeftMenu()
        {
            var menu = CommandWindowTool.VariableWindow(this.Content, this.ParentContent, this.Property,
            list =>
            {
                Undo.RecordObject(this.ParentContent, "select variable");
                this.ParentContent.RemoveContent(this.Content);
                var selectValue = list[0];
                this.ParentContent.SetChildContent(this.Property.PropertyName,selectValue);
                
                this.Select();
                ContentRendererUtil.UnselectPreselected();
            });
            return menu;
        }

        public string GetVarType()
        {
            if (this.Content.IsValue())
            {
                return this.Content.GetValueType();
            }
            return this.Property.PropertyType;
        }
    }
}