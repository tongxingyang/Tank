// ----------------------------------------------------------------------------
// <copyright file="ScriptEditorWindowAttribute.cs" company="上海序曲网络科技有限公司">
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
    using System;

    public class ScriptEditorWindowAttribute : Attribute
    {
        public string Name;

        public ScriptEditorWindowAttribute(string name)
        {
            this.Name = name;
        }
    }
}