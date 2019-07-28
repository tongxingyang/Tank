// ----------------------------------------------------------------------------
// <copyright file="ScriptEditorWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>03/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Script.Editor
{
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Script;
    using Assets.Framework.LetsScript.Editor.Util;
    using Assets.Tools.Script.Core.File;
    using Assets.Tools.Script.Editor.Window;

    using UnityEditor;

    using UnityEngine;

    public abstract class ScriptEditorWindow : SingleItemEditorWindow<ScriptData>, IScriptEditor
    {
        public override ScriptData Data { get; set; }

        /// <summary>
        /// 脚本属性
        /// </summary>
        protected Dictionary<string, ContentProperty> Properties = new Dictionary<string, ContentProperty>(); 

        private ShortcutKey shortcutKey;

        protected override void OnInit()
        {
            this.shortcutKey = new ShortcutKey();
            this.shortcutKey.Editor = this;
        }

        protected override void OnGUIEnd()
        {
            //快捷键
            this.shortcutKey.OnGUI();
        }

        protected override void ShowMenu()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            this.ShowMenuLeft();

            GUILayout.FlexibleSpace();
            this.ShowMenuRight();
            if (GUILayout.Button("保存(Ctrl+S)", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                ScriptSerializer.SerializeFile(this.Data);
                this.ShowNotification(new GUIContent("保存成功"));
            }

            if (GUILayout.Button("重新读取", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                if (!this.Data.FilePath.IsNullOrEmpty())
                {
                    this.Data = ScriptSerializer.DeserializeFile(this.Data.FilePath);
                }
            }
            if (GUILayout.Button("打开Lua", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                if (!this.Data.FilePath.IsNullOrEmpty())
                {
                    var loadAssetAtPath = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(FileUtility.GetAssetsPath(this.Data.FilePath));
                    AssetDatabase.OpenAsset(loadAssetAtPath, 0);
                }
                
            }
            GUILayout.EndHorizontal();
        }

        protected virtual void ShowMenuLeft()
        {

        }

        protected virtual void ShowMenuRight()
        {

        }

        public ScriptData GetScriptData()
        {
            return this.Data;
        }
    }
}