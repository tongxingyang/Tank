// ----------------------------------------------------------------------------
// <copyright file="ItemDetailPartInspector.cs" company="上海序曲网络科技有限公司">
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
    using Assets.Tools.Script.Editor.Tool;

    using UnityEngine;

    public abstract class ItemDetailPartInspector<T>
    {
        public abstract string Title { get; }

        protected virtual int PartWidth
        {
            get
            {
                return 10;
            } 
        }

        public void Show(T item)
        {
            if (!this.PartEnable)
            {
                return;
            }
            GUILayout.BeginVertical();
            GUILayout.BeginVertical(GUILayout.Width(PartWidth));
            GUILayout.Label(Title.SetSize(28));
            OnShow(item);
            GUILayout.EndVertical();
            GUITool.Line(2);
            GUILayout.EndVertical();
        }

        protected abstract void OnShow(T item);
        public abstract int Order { get; }

        public virtual bool PartEnable
        {
            get
            {
                return true;
            }
        }
    }
}