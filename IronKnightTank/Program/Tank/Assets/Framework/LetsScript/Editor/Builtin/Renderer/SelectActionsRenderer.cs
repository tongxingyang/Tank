// ----------------------------------------------------------------------------
// <copyright file="SelectActionsRenderer.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>05/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Builtin.Renderer
{
    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Renderer;
    using Assets.Framework.LetsScript.Editor.Renderer.Core;
    using Assets.Framework.LetsScript.Editor.Renderer.Lua;
    using Assets.Framework.LetsScript.Editor.Util;
    using Assets.Tools.Script.Editor.Window;

    using UnityEditor;

    using UnityEngine;

    [LuaCommandRenderer("SelectActions")]
    public class SelectActionsRenderer : LuaCommandRenderer,IActionRenderer
    {
        public override LuaCommandRenderer NewInstance()
        {
            var renderer = new SelectActionsRenderer();
            renderer.CommandName = this.CommandName;
            renderer.Path = this.Path;
            renderer.Description = this.Description;
            renderer.Parameters = this.Parameters;
            return renderer;
        }

        public override void Render()
        {
            var options = this.Content.GetChildContent("Options");
            if (options == null)
            {
                options = new CommonContent();
                this.Content.SetChildContent("Options", options);
            }
            var oplist = options.AsList();

            this.BeginLine();
            this.ContentLabel("选择以下选项执行");
            if (LetsScriptGUILayout.EditRegionButton("+", GUILayout.Width(20)))
            {
                string inputValue = "";
                var popEditorWindow = new PopCustomWindow();
                popEditorWindow.DrawGUI = () =>
                    {
                        GUILayout.Label("内容");
                        inputValue = EditorGUILayout.TextField(inputValue);
                        if (GUILayout.Button("确定"))
                        {
                            Undo.RecordObject(this.Content, "add option");

                            oplist.Add(new CommonContent().FromValue(inputValue));
                            this.Content.SetChildContent("Options", new CommonContent().FromList(oplist));
                            this.Content.SetChildContent("Option" + oplist.Count,new CommonContent());
                            popEditorWindow.CloseWindow();
                        }
                        
                    };
                popEditorWindow.PopWindow();
            }
            this.EndLine();


            
            for (int i = 0; i < oplist.Count; i++)
            {
                //title
                this.BeginLine();
                var op = oplist[i];
                this.ContentLabel("     ");
                this.ContentButton(op.AsValue<string>());
                if (LetsScriptGUILayout.EditRegionButton("X", GUILayout.Width(20)))
                {
                    
                }
                this.EndLine();

                //actions
                this.BeginLine();

                this.ContentLabel("          ");
                ContentRendererFactory.GetRenderer(this, this.Content, this.Property, new ContentProperty()
                {
                    PropertyName = "Option" + (i + 1) ,
                    PropertyType = ContentType.List(ContentType.Action)
                }).Render();

                this.EndLine();
            }

            
        }
    }
}