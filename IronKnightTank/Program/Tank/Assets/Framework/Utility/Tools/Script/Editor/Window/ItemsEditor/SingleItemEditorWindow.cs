// ----------------------------------------------------------------------------
// <copyright file="SingleItemEditorWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>31/08/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Window
{
    using System;
    using System.Collections.Generic;

    using Assets.Tools.Script.Reflec;

    using UnityEditor;

    using UnityEngine;

    public abstract class SingleItemEditorWindow<T> : EditorWindow where T : class
    {
        public abstract T Data { get; set; }

        private List<ItemDetailPartInspector<T>> itemDetailPartInspectors = new List<ItemDetailPartInspector<T>>();
        private Vector3 detailScrollView;

        private object inited;
        protected virtual void ShowDetail()
        {
            detailScrollView = GUILayout.BeginScrollView(detailScrollView);
            foreach (var itemDetailPartInspector in itemDetailPartInspectors)
            {
                itemDetailPartInspector.Show(Data);
            }
            GUILayout.EndScrollView();
        }

        private void OnGUI()
        {
            Init();
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;
            GUI.skin.box.richText = true;
            GUI.skin.textArea.richText = true;
            GUI.skin.textField.richText = true;
            GUI.skin.toggle.richText = true;
            GUI.skin.window.richText = true;

            ShowMenu();
            ShowDetail();

            OnGUIEnd();
        }

        protected virtual void OnGUIEnd()
        {
        }

        private void Init()
        {
            if (inited == null)
            {
                List<Type> partInspectors = AssemblyTool.FindTypesInCurrentDomainWhereExtend<ItemDetailPartInspector<T>>();
                foreach (var partInspector in partInspectors)
                {
                    ItemDetailPartInspector<T> itemDetailPartInspector = ReflecTool.Instantiate(partInspector) as ItemDetailPartInspector<T>;
                    itemDetailPartInspectors.Add(itemDetailPartInspector);
                }

                itemDetailPartInspectors.Sort((l, r) => l.Order - r.Order);
                OnInit();
                inited = new object();
            }
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void ShowMenu()
        {
            
        }
    }
}