// ----------------------------------------------------------------------------
// <copyright file="PopInputWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>29/01/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Window
{
    using System;

    using UnityEditor;

    using UnityEngine;

    public class PopInputWindow : PopEditorWindow<PopInputWindow>
    {
        public Action<string> OnInput;

        public string InputString;

        protected override void DrawOnGUI()
        {
            this.InputString = EditorGUILayout.TextArea(this.InputString);
            if (GUILayout.Button("确定",GUILayout.Width(300)))
            {
                if (OnInput != null)
                {
                    OnInput(this.InputString);
                }
                this.CloseWindow();
            }
        }
    }
}