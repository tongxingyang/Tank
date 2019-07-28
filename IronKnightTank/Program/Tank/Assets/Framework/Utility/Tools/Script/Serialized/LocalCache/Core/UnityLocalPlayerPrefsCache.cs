using UnityEngine;

namespace Assets.Tools.Script.Serialized.LocalCache.Core
{
    /// <summary>
    /// 通过PlayerPrefs保存的缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnityLocalPlayerPrefsCache<T> : UnityLocalCache<T>
    {
        protected UnityLocalPlayerPrefsCache(string name) : base(name)
        {

        }
        override public bool HasCache()
        {
            return PlayerPrefs.HasKey(CacheName);
        }
        override public void DeleteCache()
        {
            PlayerPrefs.DeleteKey(CacheName);
        }
    }
}