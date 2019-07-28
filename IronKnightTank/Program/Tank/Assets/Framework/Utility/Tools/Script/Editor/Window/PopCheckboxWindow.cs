﻿// ----------------------------------------------------------------------------
// <copyright file="PopCheckboxWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>04/08/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Window
{
    using System;
    using System.Collections.Generic;

    using Assets.Tools.Script.Editor.Tool;

    using UnityEditor;

    using UnityEngine;

    public class PopCheckboxWindow : PopEditorWindow
    {
        public bool HasSearchBar;

        public string Name;

        public Action<string, bool> OnSelectChange;

        private GUISearchBar<SelectItem> searchBar;

        private Vector2 scroll;

        private List<SelectItem> selectItems = new List<SelectItem>();

        public void AddItem(string name, bool on)
        {
            this.selectItems.Add(new SelectItem() {Name = name, On = on});
        }

        protected override void DrawOnGUI()
        {
            if (this.Name.IsNOTNullOrEmpty())
            {
                GUILayout.BeginHorizontal("flow overlay box");
                GUITool.Button(this.Name, Color.clear);
                GUILayout.EndHorizontal();
            }
            var items = this.selectItems;
            if (this.HasSearchBar)
            {
                if (this.searchBar == null)
                {
                    this.searchBar = new GUISearchBar<SelectItem>();
                }
                items = this.searchBar.Draw(this.selectItems, item => item.Name);
            }
            
            for (int i = 0; i < items.Count; i++)
            {
                bool toggle = EditorGUILayout.Toggle(items[i].Name, items[i].On);
                if (toggle != items[i].On)
                {
                    items[i].On = toggle;
                    this.OnSelectChange(items[i].Name, items[i].On);
                }
            }
        }

        private class SelectItem
        {
            public string Name;

            public bool On;
        }
    }
}