// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Window
{
    using System;
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Renderer.Core;
    using Assets.Framework.LetsScript.Editor.Renderer.Lua;
    using Assets.Framework.LetsScript.Editor.Util;
    using Assets.Tools.Script.Editor.Window;

    /// <summary>
    /// 动作选择窗口
    /// </summary>
    public class ActionWindow : PopMenuWindow
    {
        public CommonContent Config;

        public CommonContent Parent;

        public ContentProperty Property;

        public Action<List<CommonContent>> OnSelectHandler;

        private List<CommonContent> selected;

        public ActionWindow(CommonContent config, CommonContent parent, ContentProperty property, Action<List<CommonContent>> onSelect)
        {
            this.Config = config;
            this.Parent = parent;
            this.Property = property;

            this.HasSearchBar = true;
            this.Gradable = true;
            this.HasSelectTag = false;
            this.AutoSortItem = true;
            this.MenuName = "动作选择";

            this.OnSelectHandler = onSelect;

            this.selected = null;

            this.AddActionItem();
        }

        private void AddActionItem()
        {
            //剪切板
            if (ContentRendererUtil.Clipboard != null)
            {
                List<ContentRenderer> actions = new List<ContentRenderer>();
                foreach (var renderer in ContentRendererUtil.Clipboard)
                {
                    if (ContentUtil.IsAction(renderer.Content))
                    {
                        actions.Add(renderer);
                    }
                }
                if (actions.Count > 0)
                {
                    this.AddItem("粘帖", false,
                        () =>
                        {
                            this.selected = new List<CommonContent>();
                            for (int i = 0; i < actions.Count; i++)
                            {
                                var action = actions[i];
                                this.selected.Add(action.Content.Clone());
                            }
                        });
                }
            }

            //Lua命令集
            var actionTemplates = LuaCommandAssembly.FindCommandRendererTemplates(template =>{ return template is IActionRenderer; });
            for (int i = 0; i < actionTemplates.Count; i++)
            {
                var template = actionTemplates[i];
                this.AddItem(template.Path,false,
                    () =>
                        {
                            var newAction = new CommonContent();
                            newAction.SetChildContent(ContentUtil.ActionName, new CommonContent().FromValue(template.CommandName));
                            this.selected = new List<CommonContent>() { newAction };
                        });
            }
        }
        
        protected override void PreClose()
        {
            if (this.OnSelectHandler != null)
            {
                if (this.selected != null)
                {
                    foreach (var editorAnyValue in this.selected)
                    {
                        editorAnyValue.Editor.NewData = true;
                    }
                }
                this.OnSelectHandler(this.selected);
            }
            base.PreClose();
        }
    }
}