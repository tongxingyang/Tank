// ----------------------------------------------------------------------------
// <copyright file="TextConfigTable.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>14/10/2015</date>
// ----------------------------------------------------------------------------
#if !UNITY_EDITOR
#define RELEASE_VERSION
#endif

namespace Assets.Tools.Script.Editortool
{
    using System;
    using System.Collections.Generic;

    using Assets.Tools.Script.Editortool.Reader;

    public interface ITextConfigTable<TData> where TData : INameData
    {
        List<TData> GetDatas();
        void AddData(TData data);
        void RemoveData(TData data);
    }


    public class TextConfigTable<TTable, TData, TReader> : TextDataConfig<TTable, TReader>, ITextConfigTable<TData>
        where TTable : TextConfigTable<TTable, TData, TReader>, new()
        where TData : INameData 
        where TReader : ConfigReader<TTable>, new()
    {
        [NonSerialized]
        public List<TData> Datas = new List<TData>();

        public List<TData> GetDatas()
        {
            return Datas;
        }

        public void AddData(TData data)
        {
            Datas.Add(data);
        }

        public void RemoveData(TData data)
        {
            Datas.Remove(data);
        }
    }
}