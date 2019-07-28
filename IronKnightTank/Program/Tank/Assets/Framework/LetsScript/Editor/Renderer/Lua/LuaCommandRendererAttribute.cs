// ----------------------------------------------------------------------------
// <copyright file="LuaCommandRendererAttribute.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>05/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Renderer.Lua
{
    using System;

    /// <summary>
    /// 用该标签为Lua命令指定渲染方式
    /// </summary>
    public class LuaCommandRendererAttribute : Attribute
    {
        public string CommandName;

        public LuaCommandRendererAttribute(string commandName)
        {
            this.CommandName = commandName;
        }
    }
}