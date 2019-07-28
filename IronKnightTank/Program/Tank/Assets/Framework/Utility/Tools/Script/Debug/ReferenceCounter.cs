// ----------------------------------------------------------------------------
// <copyright file="ReferenceCounter.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>18/12/2015</date>
// ----------------------------------------------------------------------------

#if UNITY_EDITOR
#define COUNTER_ENABLE
#endif
//#define COUNTER_ENABLE


using System;
using System.Collections.Generic;

public class ReferenceCounter
{
    private static Dictionary<string, Dictionary<WeakReference, string>> counts =
        new Dictionary<string, Dictionary<WeakReference, string>>();

    private static List<WeakReference> keys = new List<WeakReference>();

    private static Dictionary<string, int> typeCounts = new Dictionary<string, int>();

    /// <summary>
    /// 对一个对象进行监视
    /// </summary>
    /// <param name="o">The o.</param>
    public static void Mark(object o)
    {
#if COUNTER_ENABLE
        var type = o.GetType();
        Mark(type.FullName, o, string.Empty);
#endif
    }

    /// <summary>
    /// 对一个对象进行监视
    /// </summary>
    /// <param name="name">标记的名字</param>
    /// <param name="o">标记的对象</param>
    /// <param name="mark">对象关联的识别标签</param>
    public static void Mark(string name,object o,string mark)
    {
#if COUNTER_ENABLE
        if (o == null || o.GetType().IsValueType)
        {
            return;
        }
        lock (counts)
        {
            Dictionary<WeakReference, string> typeDictionary;
            var tryGetValue = counts.TryGetValue(name, out typeDictionary);
            if (!tryGetValue)
            {
                typeDictionary = new Dictionary<WeakReference, string>();
                counts.Add(name, typeDictionary);
                typeCounts.Add(name, 0);
            }
            typeDictionary.Add(new WeakReference(o), mark);
        }
#endif
    }

    /// <summary>
    /// 获得当前记录的类型个数
    /// </summary>
    /// <returns>Dictionary&lt;System.String, System.Int32&gt;.</returns>
    public static Dictionary<string, int> GetCurrMarkTypeCount()
    {
#if COUNTER_ENABLE
        foreach (var type in counts.Keys)
        {
            typeCounts[type] = GetTypeCount(type);
        }
#endif
        return typeCounts;

    }

    /// <summary>
    /// 打印当前记录的类型个数
    /// </summary>
    public static void PrintCurrMarkTypeCount()
    {
        var currMarkTypeCount = GetCurrMarkTypeCount();
        foreach (var i in currMarkTypeCount)
        {
            var format = string.Format("{0}:{1}", i.Key, i.Value);
            DebugConsole.Log(format);
        }
    }

    /// <summary>
    /// 获得某个类型的个数
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>System.Int32.</returns>
    public static int GetTypeCount(string type)
    {
#if COUNTER_ENABLE
        var typeWeakReference = GetTypeWeakReference(type);
        return typeWeakReference.Count;
#endif
        return 0;
    }

    public static Dictionary<WeakReference, string> GetTypeWeakReference(string type)
    {
#if COUNTER_ENABLE
        Dictionary<WeakReference, string> typeDictionary;
        var tryGetValue = counts.TryGetValue(type, out typeDictionary);
        if (tryGetValue)
        {
            foreach (var b in typeDictionary)
            {
                keys.Add(b.Key);
            }

            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                if (!key.IsAlive)
                {
                    typeDictionary.Remove(key);
                }
            }
            keys.Clear();
        }
        return typeDictionary;
#endif
        return null;
    }
}
