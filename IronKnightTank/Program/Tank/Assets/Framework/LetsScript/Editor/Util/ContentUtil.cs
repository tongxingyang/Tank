// ----------------------------------------------------------------------------
// <copyright file="ContentUtil.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>02/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Util
{
    using Assets.Framework.LetsScript.Editor.Data;

    public class ContentUtil
    {
        /// <summary>
        /// 标识一个Content为指定的Action命令
        /// </summary>
        public static string ActionName = "ActionName";

        /// <summary>
        /// 标识一个Content为指定的Variable命令
        /// </summary>
        public static string VariableName = "VariableName";

        public static string GetCommandName(CommonContent content)
        {
            string name = GetActionName(content);
            if (name == null)
            {
                return GetVariableName(content);
            }
            return name;
        }

        public static string GetActionName(CommonContent content)
        {
            if (content.GetChildContent("ActionName") != null)
            {
                return content.GetChildContent("ActionName").AsValue() as string;
            }
            return null;
        }

        public static string GetVariableName(CommonContent content)
        {
            if (content.GetChildContent("VariableName") != null)
            {
                return content.GetChildContent("VariableName").AsValue() as string;
            }
            return null;
        }

        public static bool IsCommand(CommonContent content)
        {
            return GetActionName(content) != null || GetVariableName(content) != null;
        }


        public static bool IsAction(CommonContent content)
        {
            return GetActionName(content) != null;
        }

        public static bool IsVariable(CommonContent content)
        {
            return GetVariableName(content) != null;
        }
    }
}