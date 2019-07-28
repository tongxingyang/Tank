// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Data
{
    /// <summary>
    /// 内容属性
    /// </summary>
    public class ContentProperty
    {
        /// <summary>
        /// 名字
        /// </summary>
        public object PropertyName;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue;

        /// <summary>
        /// 值的类型(ContentType)
        /// </summary>
        public string PropertyType;

        /// <summary>
        /// 枚举选项
        /// </summary>
        public string[] Enum;

        /// <summary>
        /// 变量输入类型
        /// </summary>
        public string VariableType;

        /// <summary>
        /// 颜色
        /// </summary>
        public string Color;
    }
}