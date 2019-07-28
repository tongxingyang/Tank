// ----------------------------------------------------------------------------
// <copyright file="ActionListRenderer.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>03/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Builtin.Window.Part
{
    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Renderer;
    using Assets.Framework.LetsScript.Editor.Script;
    using Assets.Tools.Script.Editor.Window;

    public class CommandRendererPart : ItemDetailPartInspector<ScriptData>
    {
        public ContentProperty Property;

        private string title;
        private int order;

        public CommandRendererPart(string title, int order, ContentProperty property)
        {
            this.Property = property;
            this.title = title;
            this.order = order;
        }

        public override string Title
        {
            get
            {
                return this.title;
            }
        }

        public override int Order
        {
            get
            {
                return this.order;
            }
        }

        protected override int PartWidth
        {
            get
            {
                return 3000;
            }
        }

        protected override void OnShow(ScriptData item)
        {
            var renderer = ContentRendererFactory.GetRenderer(null, item.Contents, null, this.Property);
            renderer.Render();
        }
    }
}