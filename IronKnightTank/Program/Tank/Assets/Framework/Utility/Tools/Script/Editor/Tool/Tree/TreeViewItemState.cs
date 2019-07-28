// ----------------------------------------------------------------------------
// <copyright file="TreeViewItemState.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>28/01/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editor.Tool.Tree
{
    public struct TreeViewItemState
    {
        public TreeViewOpenOperation OpenOperation;

        public TreeViewSelectOperation SelectOperation;
    }

    public enum TreeViewOpenOperation
    {
        None,
        Open,
        Close,
    }

    public enum TreeViewSelectOperation
    {
        None,
        Select,
        Deselect,
    }
}