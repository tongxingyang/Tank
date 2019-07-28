// ----------------------------------------------------------------------------
// <copyright file="LuaTableEnumerator.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>15/03/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Scripts.Tools.Lua
{
    using System.Collections;
    using System.Collections.Generic;

    using LuaInterface;

    public class LuaTableEnumerator : IEnumerator<DictionaryEntry>
    {
        readonly LuaState state;
        DictionaryEntry current;

        public LuaTableEnumerator(LuaTable table)
        {
            state = table.GetLuaState();
            state.PushGeneric(table);
            state.PushGeneric((object)null);
        }

        public DictionaryEntry Current
        {
            get
            {
                return current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public bool MoveNext()
        {
            if (state.LuaNext(-2))
            {
                current = new DictionaryEntry
                {
                    Key = state.ToVariant(-2),
                    Value = state.ToVariant(-1)
                };
                state.LuaPop(1);
                return true;
            }
            current = new DictionaryEntry();
            return false;
        }

        public Dictionary<object, object> ToHashtable()
        {
            Dictionary<object, object> hash = new Dictionary<object, object>();
            var iter = this;

            while (iter.MoveNext())
            {
                hash.Add(iter.Current.Key, iter.Current.Value);
            }

            iter.Dispose();
            return hash;
        }

        public void Reset()
        {
            current = new DictionaryEntry();
        }

        public void Dispose()
        {
            state.LuaPop(1);
        }
    }
}