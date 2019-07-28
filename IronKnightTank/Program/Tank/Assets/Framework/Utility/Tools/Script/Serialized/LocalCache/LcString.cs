using Assets.Tools.Script.Serialized.LocalCache.Core;
using UnityEngine;

namespace Assets.Tools.Script.Serialized.LocalCache
{
    /// <summary>
    /// 保存字符串
    /// </summary>
    public class LcString : UnityLocalPlayerPrefsCache<string>
    {
        public LcString(string name)
            : base(name)
        {
        }

        protected override string GetLocalCache()
        {
            return PlayerPrefs.GetString(CacheName);
        }

        protected override void SaveLocalCache(string value)
        {
            PlayerPrefs.SetString(CacheName, value);
            PlayerPrefs.Save();
        }
    }
}