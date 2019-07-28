namespace Assets.Tools.Script.Serialized.LocalCache.Core
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface IUnityLocalCache
    {
        bool HasCache();
        void DeleteCache();
        string CacheName { get; }
    }
}