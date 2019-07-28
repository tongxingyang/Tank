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
    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Editor.Window;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// 变量选择窗口
    /// </summary>
    public class VariableWindow : PopMenuWindow
    {
        public CommonContent Config;

        public CommonContent Parent;

        public ContentProperty Property;

        private string inputStringValue;
        private float inputFloatValue;
        private bool inputBoolValue;

        private object defaultValue;

        /// <summary>
        /// 当前需求的输入类型
        /// </summary>
        private string inputType;

        public Action<List<CommonContent>> OnSelectHandler;

        private List<CommonContent> selected;

        public VariableWindow(CommonContent config, CommonContent parent, ContentProperty property, Action<List<CommonContent>> onSelected = null)
        {
            this.Config = config;
            this.Parent = parent;
            this.Property = property;
            this.OnSelectHandler = onSelected;
            this.selected = null;

            this.HasSearchBar = true;
            this.Gradable = true;
            this.HasSelectTag = false;
            this.AutoSortItem = true;

            this.defaultValue = this.Config.AsValue();

            //设置默认输入类型
            if (property.PropertyType == ContentType.Boolean)
            {
                this.inputType = ContentType.Boolean;
            }
            else if (property.PropertyType == ContentType.Number)
            {
                this.inputType = ContentType.Number;
            }
            else
            {
                this.inputType = ContentType.String;
            }
            this.ResetInput();

            //添加选单
            this.AddMenuItem();
        }

        private void AddMenuItem()
        {
            //剪切板
            if (ContentRendererUtil.Clipboard != null)
            {
                List<ContentRenderer> variables = new List<ContentRenderer>();
                foreach (var renderer in ContentRendererUtil.Clipboard)
                {
                    bool isValue = false;
                    string valueType = null;
                    if (renderer.Content.IsValue())
                    {
                        isValue = true;
                        valueType = ContentType.GetObjectType(renderer.Content.AsValue());
                    }
                    else if (renderer.Content.GetChildContent(ContentUtil.VariableName))
                    {
                        isValue = true;
                        valueType = this.Property.PropertyType;
                    }
                    
                    //只接受值类型，并且类型符合的
                    if (isValue && ContentType.Is(valueType, renderer.Property.PropertyType))
                    {
                        variables.Add(renderer);
                    }
                }
                if (variables.Count > 0)
                {
                    this.AddItem("粘帖", false,
                        () =>
                        {
                            this.selected = new List<CommonContent>();
                            for (int i = 0; i < variables.Count; i++)
                            {
                                var luaLsConfigRenderer = variables[i];
                                var newVariable = luaLsConfigRenderer.Content.Clone();
                                newVariable.Editor.NewData = true;
                                this.selected.Add(newVariable);
                            }
                        });
                }
            }
            //Lua命令集
            var variableTemplates = LuaCommandAssembly.FindCommandRendererTemplates(template =>
            {
                if (!(template is IVariableRenderer))
                {
                    return false;
                }
                var variableRenderer = template as IVariableRenderer;
                var varType = variableRenderer.GetVarType();
                return varType == ContentType.Any || this.Property.PropertyType == ContentType.Any || varType == this.Property.PropertyType;
            });
            for (int i = 0; i < variableTemplates.Count; i++)
            {
                var template = variableTemplates[i];
                this.AddItem(template.Path,false,
                    () =>
                        {
                            var editorAnyValue = new CommonContent();
                            editorAnyValue.SetChildContent("VariableName", new CommonContent().FromValue(template.CommandName));
                            editorAnyValue.Editor.NewData = true;
                            this.selected = new List<CommonContent>() { editorAnyValue };
                        });
            }
        }

        protected override void DrawOnGUI()
        {
            //标题
            GUILayout.BeginHorizontal(GUITool.GetAreaGUIStyle(new Color(0, 0, 0, 0.2f)));
            GUITool.Button(this.Property.Description, Color.clear);
            GUILayout.EndHorizontal();

            if (VariableType.HasValue(this.Property.VariableType))
            {
                //输入模块
                this.DrawInputBox();
            }
            
            //选单模块
            base.DrawOnGUI();
        }

        private void DrawInputBox()
        {
            GUILayout.BeginHorizontal();

            //需求any类型，增加输入选择按钮
            if (this.Property.PropertyType == ContentType.Any)
            {
                if (GUILayout.Button(ContentType.GetName(this.inputType), GUILayout.Width(50)))
                {
                    var genericMenu = new GenericMenu();
                    string[] menu = new[] { ContentType.String, ContentType.Number, ContentType.Boolean };
                    for (int i = 0; i < menu.Length; i++)
                    {
                        var s = menu[i];
                        genericMenu.AddItem(
                            new GUIContent(ContentType.GetName(s)),
                            false,
                            () =>
                                {
                                    this.inputType = s;
                                    this.ResetInput();
                                });
                    }
                    genericMenu.ShowAsContext();
                }
            }

            //根据类型，渲染输入部分
            if (this.inputType == ContentType.String)
            {
                this.inputStringValue = EditorGUILayout.TextArea(this.inputStringValue, GUILayout.Width(200));
            }
            if (this.inputType == ContentType.Boolean)
            {
                this.inputBoolValue = EditorGUILayout.Toggle(this.inputBoolValue, GUILayout.Width(200));
            }
            if (this.inputType == ContentType.Number)
            {
                this.inputFloatValue = EditorGUILayout.FloatField(this.inputFloatValue, GUILayout.Width(200));
            }

            if (GUILayout.Button("确定", GUILayout.Width(35)))
            {
                
                this.Close();
            }

            GUILayout.EndHorizontal();
            GUITool.Line(4);
        }

        private object GetInputValue()
        {
            if (this.inputType == ContentType.String)
            {
                return this.inputStringValue;
            }
            if (this.inputType == ContentType.Boolean)
            {
                return this.inputBoolValue;
            }
            if (this.inputType == ContentType.Number)
            {
                if (Math.Abs((int)this.inputFloatValue - this.inputFloatValue) < 0.000001)
                {
                    return (int)this.inputFloatValue;
                }
                return this.inputFloatValue;
            }
            return null;
        }

        private void ResetInput()
        {
            if (this.defaultValue == null)
            {
                return;
            }
            try
            {
                if (this.inputType == ContentType.String)
                {
                    this.inputStringValue = (string)this.defaultValue;
                }
                if (this.inputType == ContentType.Boolean)
                {
                    this.inputBoolValue = (bool)this.defaultValue;
                }
                if (this.inputType == ContentType.Number)
                {
                    try
                    {
                        this.inputFloatValue = (float)this.defaultValue;
                    }
                    catch (Exception)
                    {
                    }
                    this.inputFloatValue = (int)this.defaultValue;
                }
            }
            catch (Exception)
            {
            }
        }



        protected override void PreClose()
        {
            //检查输入内容是否变化
            if (VariableType.HasValue(this.Property.VariableType) && this.selected == null && this.IsInputChanged())
            {
                var editorAnyValue = new CommonContent();
                editorAnyValue.Editor.NewData = true;
                editorAnyValue.SetValue(this.GetInputValue());
                this.selected = new List<CommonContent>() { editorAnyValue };
            }

            //处理关闭
            if (this.OnSelectHandler != null && this.selected != null)
            {
                this.OnSelectHandler(this.selected);
            }
            base.PreClose();
        }

        private bool IsInputChanged()
        {
            if (this.defaultValue == null)
            {
                return true;
            }
            var inputValue = this.GetInputValue();
            if (inputValue.GetType() != this.defaultValue.GetType())
            {
                return true;
            }
            if (inputValue is int)
            {
                return (int)inputValue != (int)this.defaultValue;
            }
            if (inputValue is float)
            {
                return Math.Abs((float)inputValue - (float)this.defaultValue) > 0.00001f;
            }
            if (inputValue is bool)
            {
                return (bool)inputValue != (bool)this.defaultValue;
            }
            if (inputValue is string)
            {
                return (string)inputValue != (string)this.defaultValue;
            }
            return true;
        }
    }
}