// ----------------------------------------------------------------------------
// <copyright file="PopCustomWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>27/08/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Window
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Helper;

    using UnityEngine;

    public class PopCustomWindow : PopEditorWindow<PopCustomWindow>
    {
        public Action DrawGUI;

        public Action<object> DrawGUIWith;

        public Action OnClose; 

        public object CacheArg;
        public object CacheObject;

        protected override void DrawOnGUI()
        {
            if (CacheArg != null && DrawGUIWith != null)
            {
                DrawGUIWith(CacheArg);
            }
            else
            {
                if (DrawGUI != null)
                {
                    DrawGUI();
                }
            }
        }
        protected override void PreClose()
        {
            if (OnClose != null)
            {
                OnClose();
            }
            base.PreClose();
        }
    }
}