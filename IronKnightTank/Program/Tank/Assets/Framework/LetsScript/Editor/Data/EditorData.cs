// ----------------------------------------------------------------------------
// <copyright file="EditorData.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>02/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Data
{
    using Assets.Framework.LetsScript.Editor.Renderer.Core;

    /// <summary>
    /// 编辑器相关数据
    /// </summary>
    public class EditorData
    {
        public ContentRenderer Renderer;

        public bool NewData = false;

        public void Clear()
        {
            this.Renderer = null;
            this.NewData = false;
        }
    }
}