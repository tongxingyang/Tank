// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>01/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Window
{
    using System;
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Tools.Script.Editor.Window;

    /// <summary>
    /// 窗口工具
    /// </summary>
    public class CommandWindowTool
    {
        public static PopMenuWindow VariableWindow(
            CommonContent config,
            CommonContent parent,
            ContentProperty property,
            Action<List<CommonContent>> onSelected = null)
        {
            if (property.Enum != null)
            {
                PopMenuWindow menu = new PopMenuWindow(property.Description);
                menu.HasSelectTag = false;
                foreach (var e in property.Enum)
                {
                    menu.AddItem(e, false,
                        () =>
                        {
                            var formatValue = ContentType.FormatValue(property.PropertyType, e);
                            var newVariable = new CommonContent().FromValue(formatValue);
                            newVariable.Editor.NewData = true;
                            onSelected(new List<CommonContent>() { newVariable });
                        });
                }
                return menu;
            }
            else
            {
                VariableWindow menu = new VariableWindow(config, parent, property, onSelected);
                return menu;
            }
        }

        public static PopMenuWindow ActionWindow(
            CommonContent config,
            CommonContent parent,
            ContentProperty property,
            Action<List<CommonContent>> onSelected = null)
        {
            return new ActionWindow(config, parent, property, onSelected);
        }
    }
}