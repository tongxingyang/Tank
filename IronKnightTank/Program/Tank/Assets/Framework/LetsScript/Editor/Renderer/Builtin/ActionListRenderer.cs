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
    /// 动作列表渲染器
    /// 渲染ScriptAction[]
    /// </summary>
    public class ActionListRenderer : ContentRenderer
    {
        public override void Render()
        {
            GUILayout.BeginVertical();

            int childrenCount = this.Content.AsList().Count;
            for (int i = 0; i < childrenCount; i++)
            {
                //获取针对子Action的渲染器
                var luaLsProperty = new ContentProperty { PropertyName = i, PropertyType = ContentType.Action };
                var actionItemRenderer = ContentRendererFactory.GetRenderer(this, this.Content, this.Property, luaLsProperty);
                actionItemRenderer.Property.PropertyName = i;

                //渲染他
                GUILayout.BeginVertical(actionItemRenderer.IsSelected? LetsScriptGUILayout .SelectedAreaStyle: LetsScriptGUILayout.GetAreaLineStyle(i % 2 == 0));
                actionItemRenderer.Render();
                if (LetsScriptGUILayout.EditRegionLine())
                {
                    this.InsertNewAction(i);
                }
                GUILayout.EndVertical();
            }

            //没有子Action的时候提示添加
            if (childrenCount == 0 && LetsScriptGUILayout.EditRegionButton("添加动作",GUILayout.Width(100)))
            {
                this.InsertNewAction(childrenCount);
            }

            GUILayout.EndVertical();
        }

        private void InsertNewAction(int index)
        {
            PopMenuWindow menu = CommandWindowTool.ActionWindow(this.Content,this.ParentContent,this.Property,(list =>
                {
                    ContentRendererUtil.InsertRenderers(list, this.Content, this.Property, index);
                }));
            menu.PopWindow();
        }
    }
}