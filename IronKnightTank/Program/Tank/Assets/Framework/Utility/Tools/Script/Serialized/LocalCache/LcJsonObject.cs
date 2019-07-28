using System;
using Assets.Tools.Script.Debug.Console;
using Assets.Tools.Script.Serialized.LocalCache.Core;
using LitJson;
using UnityEngine;

namespace Assets.Tools.Script.Serialized.LocalCache
{
    /// <summary>
    /// 用json序列化保存对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LcJsonObject<T> : UnityLocalPlayerPrefsCache<T> where T : class
    {
        public LcJsonObject(string name)
            : base(name)
        {
        }

        protected override T GetLocalCache()
        {
            string saveData = PlayerPrefs.GetString(CacheName);
            try
            {
                T o = JsonMapper.ToObject<T>(saveData);
                return o;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        protected override void SaveLocalCache(T value)
        {
            try
            {
                string json = JsonMapper.ToJson(value);
                PlayerPrefs.SetString(CacheName, json);
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                DebugConsole.Log(e);
            }
            
        }
    }
}