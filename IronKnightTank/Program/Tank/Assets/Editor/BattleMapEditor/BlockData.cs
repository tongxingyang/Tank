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

    
    public class BlockData
    {
        public GridPos pos;
        public int TerrianId;
        public bool IsBornBlock = false;
        public List<string> Scripts;
        //public int EnemyId;
        public int NPCId;
        [System.NonSerialized]
        public FaceToward Toward;

        public int NpcToward = 0;

        public BlockData(int x, int y)
        {
            pos = new GridPos(x, y);
        }

        public BlockData(GridPos p)
        {
            pos = p;
        }

        public void AfterSeralize()
        {
            this.Toward = (FaceToward)NpcToward;
        }

        public void BeforeSeralize()
        {
            NpcToward = (int)this.Toward;
        }

    }

    
    public enum FaceToward
    {
        Right = 1 ,
        Left =2 ,
        Up = 3 ,
        Down = 4,
        RightUp = 5,
        RightDown = 6,
        LeftUp = 7,
        LeftDown = 8,
    }

}
