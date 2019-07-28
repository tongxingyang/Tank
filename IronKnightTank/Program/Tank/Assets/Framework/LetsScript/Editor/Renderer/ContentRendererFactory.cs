// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer
{
    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Renderer.Builtin;
    using Assets.Framework.LetsScript.Editor.Renderer.Core;
    using Assets.Framework.LetsScript.Editor.Renderer.Lua;
    using Assets.Framework.LetsScript.Editor.Util;

    /// <summary>
    /// ContentRenderer的简单工厂类
    /// </summary>
    public class ContentRendererFactory
    {
        /// <summary>
        /// 获取指定对象的指定字段的渲染器
        /// </summary>
        /// <param name="parentRenderer">上一级渲染器</param>
        /// <param name="target">指定的对象</param>
        /// <param name="targetProperty">指定的对象的属性</param>
        /// <param name="property">指定的字段</param>
        /// <returns>ContentRenderer.</returns>
        public static ContentRenderer GetRenderer(ContentRenderer parentRenderer, CommonContent target, ContentProperty targetProperty, ContentProperty property)
        {
            //请求渲染的类型
            string rendererType = property.PropertyType;

            //请求渲染的内容
            var content = target.GetChildContent(property.PropertyName);
            if (content != null)
            {
                //基本类型
                if (content.IsValue())
                {
                    var value = content.AsValue();
                    if (value is int || value is float)
                    {
                        rendererType = ContentType.Number;
                    }
                    else if (value is string)
                    {
                        rendererType = ContentType.String;
                    }
                }
                //命令类型
                if (ContentUtil.IsCommand(content))
                {
                    rendererType = ContentUtil.GetCommandName(content);
                }
            }

            //目前已经在为content工作的渲染器
            ContentRenderer activatedRenderer = content != null ? content.Editor.Renderer : null;

            //当前实际需要的渲染器
            ContentRenderer renderer = null;
            switch (rendererType)
            {
                case ContentType.Number:
                    if (activatedRenderer is SimpleValueRenderer)
                    {
                        renderer = activatedRenderer;
                    }
                    else
                    {
                        renderer = new SimpleValueRenderer();
                    }
                    break;
                case ContentType.Boolean:
                    if (activatedRenderer is SimpleValueRenderer)
                    {
                        renderer = activatedRenderer;
                    }
                    else
                    {
                        renderer = new SimpleValueRenderer();
                    }
                    break;
                case ContentType.String:
                    if (activatedRenderer is SimpleValueRenderer)
                    {
                        renderer = activatedRenderer;
                    }
                    else
                    {
                        renderer = new SimpleValueRenderer();
                    }
                    break;
                case ContentType.Any:
                    if (activatedRenderer is SimpleValueRenderer)
                    {
                        renderer = activatedRenderer;
                    }
                    else
                    {
                        renderer = new SimpleValueRenderer();
                    }
                    break;
                default:
                    //复杂对象
                    //列表?
                    if (ContentType.IsList(rendererType))
                    {
                        if (ContentType.Unlist(rendererType) == ContentType.Action)
                        {
                            if (activatedRenderer is ActionListRenderer)
                            {
                                renderer = activatedRenderer;
                            }
                            else
                            {
                                renderer = new ActionListRenderer();
                            }
                        }
                        else
                        {
                            if (activatedRenderer is VariableListRenderer)
                            {
                                renderer = activatedRenderer;
                            }
                            else
                            {
                                renderer = new VariableListRenderer();
                            }
                        }
                    }
                    //指定的某种命令?
                    else
                    {
                        if (activatedRenderer is LuaCommandRenderer && (activatedRenderer as LuaCommandRenderer).Is(LuaCommandAssembly.GetCommandRendererTemplate(rendererType)))
                        {
                            renderer = activatedRenderer;
                        }
                        else
                        {
                            renderer = LuaCommandAssembly.InstanceCommandRenderer(rendererType);
                        }
                    }
                    
                    break;
            }
            
            //更新或者新生成,初始化他
            if (activatedRenderer != renderer)
            {
                renderer.Init(target, targetProperty, parentRenderer, content, property);

                if (renderer.Content.Editor.NewData)
                {
                    renderer.Select();
                    renderer.Content.Editor.NewData = false;
                    ContentRendererUtil.UnselectPreselected();
                }
            }
            
            return renderer;
        }
    }
}