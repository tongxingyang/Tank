using Assets.Tools.Script.Event;

namespace Assets.Tools.Script.Net.Downloader
{
    public interface ILoader
    {
        /// <summary>
        /// 加载开始
        /// </summary>
        Signal<ILoader> onLoadStart { get; }
        /// <summary>
        /// 加载成功
        /// </summary>
        Signal<ILoader> onLoadComplete { get; }
        /// <summary>
        /// 加载失败
        /// </summary>
        Signal<ILoader> onLoadError { get; }
        /// <summary>
        /// 开始卸载本地缓存
        /// </summary>
        Signal<ILoader> onUnloadLocalStart { get; }
        /// <summary>
        /// 卸载本地缓存成功
        /// </summary>
        Signal<ILoader> onUnloadLocalComplete { get; }
        /// <summary>
        /// 卸载本地缓存失败
        /// </summary>
        Signal<ILoader> onUnloadLocalError { get; }
        /// <summary>
        /// //加载url资源
        /// </summary>
        /// <param name="catchMemory">是否需要在缓存在内存</param>
        void Load(bool catchMemory=true);
        /// <summary>
        /// 卸载内存数据
        /// </summary>
        void Unload();
        /// <summary>
        /// 卸载本地缓存
        /// </summary>
        void UnloadLocal();
//        /// <summary>
//        /// 本地是否有缓存
//        /// </summary>
//        /// <returns></returns>
        bool HasLocal();
        int GetLocalVersion();
        bool HasNewVersion();
    }
}

