// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Util
{
    using System.Text;

    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Helper;

    using UnityEngine;

    public static class LetsScriptGUILayout
    {
        private static GUIStyle EditRegionLabelStyle;

        private static GUIStyle EditRegionLineStyle;
        
        public static GUIStyle SelectedAreaStyle;

        public static string VeryLongSpace;

        static LetsScriptGUILayout()
        {
            //选中区域样式
            SelectedAreaStyle = GUITool.GetAreaGUIStyle(ColorTool.GetColorFromRGBHexadecimal("50809f"));

            //编辑区域Line样式
            EditRegionLineStyle = new GUIStyle("TL SelectionBarPreview");
            EditRegionLineStyle.margin = new RectOffset(0, 0, 0, 0);
            EditRegionLineStyle.padding = new RectOffset(0, 0, 0, 0);
            EditRegionLineStyle.contentOffset = new Vector2(0, 0);

            //编辑区域Label样式
            EditRegionLabelStyle = new GUIStyle(GUI.skin.button);
            EditRegionLabelStyle.margin = new RectOffset(0, 0, 0, 0);
            EditRegionLabelStyle.padding = new RectOffset(2, 2, EditRegionLabelStyle.padding.top + 3, EditRegionLabelStyle.padding.bottom + 2);
            EditRegionLabelStyle.contentOffset = new Vector2(0, 0);

            SelectedAreaStyle.richText = true;
            EditRegionLineStyle.richText = true;
            SelectedAreaStyle.richText = true;

            //超长空格占位符
            StringBuilder space = new StringBuilder("    ");
            for (int i = 0; i < 10; i++)
            {
                space.Append(space);
            }
            VeryLongSpace = space.ToString();
        }

        /// <summary>
        /// 获取颜色相间的区域样式
        /// </summary>
        /// <param name="lineStyleStep">if set to <c>true</c> [line style step].</param>
        /// <returns>GUIStyle.</returns>
        public static GUIStyle GetAreaLineStyle(bool lineStyleStep)
        {
            Color c = lineStyleStep ? new Color(0.15f, 0.15f, 0.15f, 0.25f) : new Color(0.3f, 0.3f, 0.3f, 0.5f);
            return GUITool.GetAreaGUIStyle(c);
        }

        /// <summary>
        /// 脚本操作编辑区按钮
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="options">The options.</param>
        /// <returns><c>true</c> if click, <c>false</c> otherwise.</returns>
        public static bool EditRegionButton(string label, params GUILayoutOption[] options)
        {
            return GUILayout.Button(label, EditRegionLabelStyle, options);
        }

        /// <summary>
        /// 脚本操作编辑区文字(可操作)
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns><c>true</c> if click, <c>false</c> otherwise.</returns>
        public static bool EditRegionLabel(string label)
        {
            TextAnchor textAnchor = EditRegionLabelStyle.alignment;
            EditRegionLabelStyle.alignment = (label.Contains("\n")? TextAnchor.MiddleLeft: textAnchor);
            Color backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.clear;
            bool button = GUILayout.Button(label, EditRegionLabelStyle);
            GUI.backgroundColor = backgroundColor;
            EditRegionLabelStyle.alignment = textAnchor;
            return button;
        }

        /// <summary>
        /// 脚本操作编辑区分割线
        /// </summary>
        /// <returns><c>true</c> if click, <c>false</c> otherwise.</returns>
        public static bool EditRegionLine()
        {
            bool button = GUILayout.Button("", EditRegionLineStyle,GUILayout.Height(7));
            return button;
        }
    }
}