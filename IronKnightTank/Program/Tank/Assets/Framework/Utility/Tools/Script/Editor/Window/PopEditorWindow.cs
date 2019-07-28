// ----------------------------------------------------------------------------
// <copyright file="PopEditorWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>03/08/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Window
{
    using Assets.Tools.Script.Reflec;

    using UnityEditor;

    using UnityEngine;

    public abstract class PopEditorWindow : EditorWindow
    {
        public EditorWindow FromWindow { get; protected set; }

        protected bool AutoAdjustSize = true;

        public Vector2 DefaultSize;

        public Vector2 ScrollViewPosition;

        public void PopWindow()
        {
            if (DefaultSize == Vector2.zero)
            {
                AutoAdjustSize = true;
                DefaultSize = new Vector2(100, 100);
            }
            else
            {
                AutoAdjustSize = false;
            }
            
            this.FromWindow = EditorWindow.focusedWindow;
            Vector2 popPosition = Vector2.zero;
            if (Event.current != null)
            {
                popPosition = Event.current.mousePosition;
            }
            else
            {
                popPosition = EditorWindow.focusedWindow.position.position;
            }
            Vector2 guiToScreenPoint = GUIUtility.GUIToScreenPoint(popPosition);
            this.ShowAsDropDown(new Rect(guiToScreenPoint.x, guiToScreenPoint.y, 0, 0), DefaultSize);

            this.OnOpen();
        }

        private void OnGUI()
        {
            if (AutoAdjustSize)
            {
                GUILayout.BeginVertical(GUILayout.Width(10), GUILayout.Height(10));
                DrawOnGUI();
                GUILayout.EndVertical();
                Rect lastRect = GUILayoutUtility.GetLastRect();

                if (lastRect.size.x > 5 && lastRect.size.y > 5)
                {
                    this.maxSize = lastRect.size;
                    this.minSize = lastRect.size;
                }
            }
            else
            {
                this.ScrollViewPosition = GUILayout.BeginScrollView(this.ScrollViewPosition);
                DrawOnGUI();
                GUILayout.EndScrollView();
            }
        }

        protected virtual void PreClose()
        {
            
        }

        protected virtual void OnOpen()
        {
            
        }

        private void OnDestroy()
        {
            this.PreClose();
            this.FromWindow.Focus();
        }

        public void CloseWindow()
        {
            this.Close();
        }

        protected abstract void DrawOnGUI();
    }

    public abstract class PopEditorWindow<T> : PopEditorWindow
        where T : PopEditorWindow
    {
        public static T LastPopWindow;

        public static T ShowPopWindow()
        {
            T window = ReflecTool.Instantiate<T>();
            window.PopWindow();
            LastPopWindow = window;
            return window;
        }

        public static T ShowPopWindow(Vector2 size)
        {
            T window = ReflecTool.Instantiate<T>();
            window.DefaultSize = size;
            window.PopWindow();
            
            LastPopWindow = window;
            return window;
        }

        protected override void PreClose()
        {
            LastPopWindow = null;
        }
    }
}