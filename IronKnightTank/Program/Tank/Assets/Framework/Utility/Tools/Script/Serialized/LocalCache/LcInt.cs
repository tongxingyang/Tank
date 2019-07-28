using Assets.Tools.Script.Serialized.LocalCache.Core;
using UnityEngine;

namespace Assets.Tools.Script.Serialized.LocalCache
{
    /// <summary>
    /// 保存int
    /// </summary>
    public class LcInt : UnityLocalPlayerPrefsCache<int>
    {
        public LcInt(string name)
            : base(name)
        {
        }

        protected override int GetLocalCache()
        {
            return PlayerPrefs.GetInt(CacheName);
        }

        protected override void SaveLocalCache(int value)
        {
            PlayerPrefs.SetInt(CacheName, value);
            PlayerPrefs.Save();
        }
    }
}