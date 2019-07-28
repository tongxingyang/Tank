// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Core
{
    using System;

    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Util;
    using Assets.Tools.Script.Editor.Window;
    using Assets.Tools.Script.Helper;

    using UnityEngine;

    /// <summary>
    /// 脚本内容渲染器
    /// </summary>
    public abstract class ContentRenderer
    {
        /// <summary>
        /// 渲染内容所属的Content
        /// </summary>
        /// <value>The content of the parent.</value>
        public CommonContent ParentContent { get; private set; }

        /// <summary>
        /// ParentContent属性
        /// </summary>
        /// <value>The parent property.</value>
        public ContentProperty ParentProperty { get;private set; }

        /// <summary>
        /// 上一级渲染器
        /// </summary>
        /// <value>The parent renderer.</value>
        public ContentRenderer ParentRenderer { get;private set; }

        /// <summary>
        /// 渲染内容
        /// </summary>
        /// <value>The content.</value>
        public CommonContent Content { get;private set; }

        /// <summary>
        /// Content属性
        /// </summary>
        /// <value>The property.</value>
        public ContentProperty Property { get;private set; }

        /// <summary>
        /// 视图是否被选中
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected { get; set; }


        private DateTime rightClickTime = DateTime.MinValue;

        /// <summary>
        /// 渲染接口
        /// </summary>
        public virtual void Render(){}

        /// <summary>
        /// 初始化接口
        /// </summary>
        protected virtual void OnInit() { }

        public void Init(CommonContent parentContent, ContentProperty parentProperty, ContentRenderer parentRenderer,CommonContent content,ContentProperty property)
        {
            //生成默认值
            if (content == null)
            {
                content = new CommonContent();
                parentContent.SetChildContent(property.PropertyName, content);
            }

            //初始化必要的属性
            this.Content = content;
            this.ParentContent = parentContent;
            this.ParentProperty = parentProperty;
            this.ParentRenderer = parentRenderer;
            this.Property = property;
            this.Content.Editor.Renderer = this;

            //其他初始化
            this.OnInit();
        }

        /// <summary>
        /// Renderer名字
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string GetName()
        {
            return this.Property.PropertyName.ToString();
        }

        protected void BeginLine()
        {
            if (this.IsSelected)
            {
                GUILayout.BeginHorizontal(LetsScriptGUILayout.SelectedAreaStyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
            }
            //新的一行，头上留一点空白
            if (ContentRendererUtil.IsBeginningOfLine(this))
            {
                this.ContentLabel("  ");
            }
        }

        protected void EndLine()
        {
            //新的一行，尾部占位符
            if (ContentRendererUtil.IsBeginningOfLine(this))
            {
                this.ContentFlexibleSpace();
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 内容块占位符
        /// </summary>
        protected void ContentFlexibleSpace()
        {
            this.ContentLabel(LetsScriptGUILayout.VeryLongSpace);
            GUILayout.FlexibleSpace();
        }

        /// <summary>
        /// 内容块按钮
        /// </summary>
        protected void ContentButton(string label)
        {
            if (LetsScriptGUILayout.EditRegionButton(label))
            {
                this.Select();
                ContentRendererUtil.UnselectPreselected();
                var menu = this.CreateLeftMenu();
                menu.OnCloseHandler += ContentRendererUtil.UnselectPreselected;
                menu.PopWindow();
            }
        }

        /// <summary>
        /// 内容块文字
        /// </summary>
        /// <param name="label">The label.</param>
        protected void ContentLabel(string label)
        {
            if (LetsScriptGUILayout.EditRegionLabel(label))
            {
                bool newSelect = !this.IsSelected;
                this.Select();

                //Ctrl+鼠标左键
                if (Event.current.button == 0 && Event.current.control)
                {
                    //重复点击则取消选中
                    if (!newSelect)
                    {
                        this.Unselect(true);
                    }
                }
                //Ctrl+鼠标右键
                else if (Event.current.button == 1 && Event.current.control)
                {
                    //保持选中并打开右键菜单
                    var menu = this.CreateRightMenu();
                    menu.PopWindow();
                }
                //左键单击/双击
                else if (Event.current.button == 0)
                {
                    //单机,除本renderer外取消选择
                    ContentRendererUtil.UnselectPreselected();

                    //双击打开左键菜单
                    if ((DateTime.Now - this.rightClickTime).TotalSeconds < 0.35f)
                    {
                        var menu = this.CreateLeftMenu();
                        menu.PopWindow();
                    }
                    this.rightClickTime = DateTime.Now;
                }
                //右键单击
                else if (Event.current.button == 1)
                {
                    //如果是之前多选对象，不会取消选中
                    if (newSelect)
                    {
                        ContentRendererUtil.UnselectPreselected();
                    }

                    //打开菜单
                    var menu = this.CreateRightMenu();
                    menu.PopWindow();
                }
            }
        }

        /// <summary>
        /// 默认右键菜单
        /// </summary>
        /// <returns>PopMenuWindow.</returns>
        protected virtual PopMenuWindow CreateRightMenu()
        {
            PopMenuWindow menu = new PopMenuWindow();
            menu.MenuName = ContentRendererUtil.Selected.Count<=1?this.GetName():"复选操作";
            menu.MenuName = menu.MenuName.PadBoth(30);

            //不允许在复选情况下插入，不允许在非列表处插入
            if (ContentRendererUtil.Selected.Count > 1 || !(ContentType.IsList(this.ParentProperty.PropertyType)))
            {
                menu.AddDisabledItem("插入");
            }
            else
            {
                var index = (int)this.Property.PropertyName;
                menu.AddItem("插入",false,
                    () =>
                        {
                            ContentRendererUtil.InsertRenderers(ContentRendererUtil.Clipboard, this.ParentContent, this.ParentProperty, index);
                        });
            }
            if (ContentRendererUtil.Selected.Count >= 1)
            {
                ContentRenderer parentRenderer = null;
                bool canCopy = true;
                foreach (var selectedRenderer in ContentRendererUtil.Selected)
                {
                    if (parentRenderer == null)
                    {
                        parentRenderer = selectedRenderer.ParentRenderer;
                    }
                    if (parentRenderer != selectedRenderer.ParentRenderer)
                    {
                        menu.AddDisabledItem("复制");
                        menu.AddDisabledItem("剪切");
                        canCopy = false;
                        break;
                    }
                }
                if (canCopy)
                {
                    menu.AddItem("复制", false,
                    () =>
                    {
                        ContentRendererUtil.CopyRenderers(ContentRendererUtil.Selected);
                    });
                    menu.AddItem("剪切", false,
                    () =>
                    {
                        ContentRendererUtil.CopyRenderers(ContentRendererUtil.Selected);

                        ContentRendererUtil.DeleteRenderers(ContentRendererUtil.Selected);
                    });
                }
            }
            menu.AddItem("删除", false,
                () =>
                {
                    ContentRendererUtil.DeleteRenderers(ContentRendererUtil.Selected);
                });
            return menu;
        }

        protected virtual PopMenuWindow CreateLeftMenu()
        {
            PopMenuWindow menu = new PopMenuWindow();
            menu.AddDisabledItem("-",false);
            return menu;
        }

        public ContentRenderer GetRootRenderer()
        {
            ContentRenderer renderer = this;
            while (renderer.ParentRenderer != null)
            {
                renderer = renderer.ParentRenderer;
            }
            return renderer;
        }
    }
}