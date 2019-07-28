using Assets.Tools.Script.Serialized.LocalCache.Core;
using UnityEngine;

namespace Assets.Tools.Script.Serialized.LocalCache
{
    /// <summary>
    /// 保存bool
    /// </summary>
    public class LcBool : UnityLocalPlayerPrefsCache<bool>
    {
        public LcBool(string name)
            : base(name)
        {
        }

        protected override bool GetLocalCache()
        {
            return PlayerPrefs.GetInt(CacheName)!=0;
        }

        protected override void SaveLocalCache(bool value)
        {
            PlayerPrefs.SetInt(CacheName, value?1:0);
            PlayerPrefs.Save();
        }
    }
}