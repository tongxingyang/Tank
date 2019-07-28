// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Builtin
{
    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Renderer;
    using Assets.Framework.LetsScript.Editor.Renderer.Core;
    using Assets.Framework.LetsScript.Editor.Renderer.Window;
    using Assets.Framework.LetsScript.Editor.Util;
    using Assets.Tools.Script.Editor.Window;

    using UnityEngine;

    /// <summary>
    /// 值列表渲染器
    /// 渲染ScriptVariable[]
    /// </summary>
    public class VariableListRenderer : ContentRenderer
    {
        private ContentProperty itemProperty;

        protected override void OnInit()
        {
            this.itemProperty = new ContentProperty()
            {
                PropertyType = ContentType.Unlist(this.Property.PropertyType),
                VariableType = VariableType.Dynamic
            };
            base.OnInit();
        }

        public override void Render()
        {
            GUILayout.BeginVertical();

            int childrenCount = this.Content.AsList().Count;
            for (int i = 0; i < childrenCount; i++)
            {
                //获取针对子Variable的渲染器
                var luaLsProperty = new ContentProperty { PropertyName = i,PropertyType = this.itemProperty.PropertyType, VariableType = VariableType.Dynamic };
                var variableItemRenderer = ContentRendererFactory.GetRenderer(this, this.Content, this.Property, luaLsProperty);
                variableItemRenderer.Property.PropertyName = i;

                //渲染他
                GUILayout.BeginVertical(variableItemRenderer.IsSelected ? LetsScriptGUILayout.SelectedAreaStyle : LetsScriptGUILayout.GetAreaLineStyle(i % 2 == 0));
                variableItemRenderer.Render();
                if (LetsScriptGUILayout.EditRegionLine())
                {
                    this.InsertNewVariable(i);
                }
                GUILayout.EndVertical();
            }

            //没有子Variable的时候提示添加
            if (childrenCount == 0 && LetsScriptGUILayout.EditRegionButton("添加值", GUILayout.Width(100)))
            {
                this.InsertNewVariable(childrenCount);
            }

            GUILayout.EndVertical();
        }

        private void InsertNewVariable(int index)
        {
            PopMenuWindow menu = CommandWindowTool.VariableWindow(this.Content, this.ParentContent, this.itemProperty, (list =>
            {
                ContentRendererUtil.InsertRenderers(list, this.Content, this.Property, index);
            }));
            menu.PopWindow();
        }
    }
}