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
    using Assets.Framework.Lua.Editor.Util;
    using ParadoxNotion.Serialization;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class BattleMap
    {
        public string Name = "";
        public int Width = 20;
        public int Height = 20;
        public float GridWidth = 1;
        public float GridHeight = 1;
        
        public int DefaultToward = 0;

        [NonSerialized]
        public FaceToward Toward = FaceToward.Right;
        [NonSerialized]
        public Dictionary<GridPos, BlockData> BlockDic = new Dictionary<GridPos, BlockData>();
        [SerializeField]
        private List<BlockData> BlockDataList = new List<BlockData>();

        public static BattleMap DeserializeJson(string json)
        {
            BattleMap map = JSON.Deserialize<BattleMap>(json);
            //Debug.Log(map.DefaultToward.ToString());
            map.Toward = (FaceToward)map.DefaultToward;
            foreach (var item in map.BlockDataList)
            {
                map.BlockDic[item.pos] = item;
                item.AfterSeralize();
            }
            map.BlockDataList.Clear();
            return map;
        }

        public string   Serialize()
        {
            return JSON.Serialize<BattleMap>(this);
        }

        public string SerializeLua()
        {
            this.BlockDataList.Clear();
            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Height; j++)
                {
                    GridPos pos = new GridPos(i, j);
                    var item = this.BlockDic[pos];
                    BlockDataList.Add(item);
                    item.BeforeSeralize();
                }
            }
            this.DefaultToward = (int)this.Toward;
            string serialize = LuaSerializer.Serialize(this);
            this.BlockDataList.Clear();
            return string.Format("return {0}", serialize);
        }

        public BlockData GetBlockData(GridPos pos)
        {
            BlockData data = null;
            if (BlockDic.TryGetValue(pos, out data))
            {
                return data;
            }
            return null;
        }

        public void SetBlockData(GridPos pos, BlockData data)
        {
            BlockDic[pos] = data;
        }

        public void FillMap()
        {
            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Height; j++)
                {
                    GridPos p = new GridPos(i, j);
                    if (!BlockDic.ContainsKey(p))
                    {
                        BlockDic[p] = new BlockData(i, j);
                    }
                }
            }
        }

        public void RemoveBlock(GridPos pos)
        {
            BlockDic[pos] = null;   
        }
        
        public List<string> Scripts;


        public BlockData GetBlock(GridPos pos)
        {
            if (BlockDic.ContainsKey(pos))
            {
                return BlockDic[pos];

            }
            else
            {
                return null;
            }
        }
    }

    

}
