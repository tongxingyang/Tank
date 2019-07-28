// ----------------------------------------------------------------------------
// <copyright file="TextConfig.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>18/10/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editortool
{
    using System;

    using Assets.Tools.Script.Editortool.Reader;

    /// <summary>
    /// 可存储数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <typeparam name="TReader">用于存取数据的工具类型</typeparam>
    public abstract class TextDataConfig<T, TReader>
        where T : class, new()
        where TReader : ConfigReader<T>, new()
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
#if !UNITY_EDITOR
                instance = Reader.GetFromProjectCache();
#else
                try
                {
                    instance = Reader.GetFromLocalCache();
                }
                catch (Exception e)
                {
                    
                }
                
                if (instance == null)
                {
                    instance = Reader.GetFromProjectCache();
                }
#endif
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        /// <summary>
        /// The instance
        /// </summary>
        private static T instance;

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <value>The reader.</value>
        public static ConfigReader<T> Reader
        {
            get
            {
                if (reader == null)
                {
                    reader = new TReader();
                }
                return reader;
            }
        }

        /// <summary>
        /// The reader
        /// </summary>
        private static ConfigReader<T> reader;
    }
}