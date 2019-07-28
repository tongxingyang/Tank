// ----------------------------------------------------------------------------
// <copyright file="MapEditorWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>09/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Editor.BattleMapEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [System.Serializable]
    public struct GridPos
    {
        public int x;
        public int y;

        public GridPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
