// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Lua
{
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Renderer.Core;

    /// <summary>
    /// 生成自Lua配置的命令
    /// </summary>
    public abstract class LuaCommandRenderer : ContentRenderer
    {
        /// <summary>
        /// 命令名字
        /// </summary>
        public string CommandName;

        /// <summary>
        /// 命令基类名字
        /// </summary>
        public string SuperName;

        /// <summary>
        /// 命令路径
        /// </summary>
        public string Path;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 命令包含字段及其属性
        /// </summary>
        protected Dictionary<string,ContentProperty> Parameters = new Dictionary<string, ContentProperty>();


        #region 编辑器 runtime 部分

        /// <summary>
        /// 如何生成一个命令实例的接口
        /// </summary>
        /// <returns>LuaCommandRenderer.</returns>
        public abstract LuaCommandRenderer NewInstance();

        protected override void OnInit()
        {
            //默认值生成
            foreach (var propertyName in this.Parameters.Keys)
            {
                var property = this.Parameters[propertyName];
                var propertyValue = this.Content.GetChildContent(propertyName);
                if (propertyValue == null)
                {
                    propertyValue = new CommonContent();
                    if (!string.IsNullOrEmpty(property.DefaultValue))
                    {
                        propertyValue.SetValue(ContentType.FormatValue(property.PropertyType, property.DefaultValue));
                    }
                    this.Content.SetChildContent(property.PropertyName,propertyValue);
                }
            }
        }

        
        public bool Is(LuaCommandRenderer renderer)
        {
            return renderer.Parameters == this.Parameters;
        }



        public override string GetName()
        {
            var strings = this.Path.Split('/');
            return strings[strings.Length - 1];
        }


        #endregion

        #region 模版构建部分


        //初始化命令部分配置
        protected virtual void OnInitializeRenderer(LuaDescription description) { }

        //初始化命令字段部分配置
        protected virtual void OnInitializeParameter(LuaDescription description) { }

        /// <summary>
        /// 根据配置信息，构建该Renderer
        /// </summary>
        /// <param name="commandName">命令名字</param>
        /// <param name="superName">命令基类名字</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="descriptions">其他配置</param>
        public void InitializeRender(string commandName, string superName, List<LuaDescription> descriptions)
        {
            this.CommandName = commandName;
            foreach (var description in descriptions)
            {
                switch (description.Name)
                {
                    case "path":
                        this.Path = description.Description;
                        break;
                    case "description":
                        this.Description = description.Description;
                        break;
                    default:
                        this.OnInitializeRenderer(description);
                        break;
                }
            }
        }

        /// <summary>
        /// 根据配置信息，构建该Renderer的一个字段
        /// </summary>
        /// <param name="propertyName">The parameter description.</param>
        /// <param name="descriptions">The descriptions.</param>
        public void AddProperty(string propertyName, List<LuaDescription> descriptions)
        {
            var luaLsProperty = new ContentProperty();
            
            luaLsProperty.PropertyName = propertyName;
            foreach (var description in descriptions)
            {
                switch (description.Name)
                {
                    case "description":
                        luaLsProperty.Description = description.Description;
                        break;
                    case "default":
                        luaLsProperty.DefaultValue = description.Description;
                        break;
                    case "type":
                        luaLsProperty.PropertyType = description.Description;
                        break;
                    case "color":
                        luaLsProperty.Color = description.Description;
                        break;
                    case "enum":
                        luaLsProperty.Enum = description.Description.Split(',');
                        for (int i = 0; i < luaLsProperty.Enum.Length; i++)
                        {
                            var s = luaLsProperty.Enum[i];
                            luaLsProperty.Enum[i] = s.Trim();
                        }
                        break;
                    default:
                        this.OnInitializeParameter(description);
                        break;
                }
            }

            this.Parameters.Add(propertyName, luaLsProperty);
        }

        #endregion

        public override string ToString()
        {
            if (this.CommandName.IsNOTNullOrEmpty())
            {
                return this.CommandName;
            }
            return string.Format("{0}(null)", this.GetType().Name);
        }


        protected enum StyleType
        {
            Property,
            Text,
            Tab,
        }

        protected class StyleElement
        {
            public string Data;

            public StyleType Type;

            public StyleElement(string data, StyleType type)
            {
                if (type == StyleType.Text && data == "--")
                {
                    type = StyleType.Tab;
                }
                this.Data = data;
                this.Type = type;
            }
        }
    }
}