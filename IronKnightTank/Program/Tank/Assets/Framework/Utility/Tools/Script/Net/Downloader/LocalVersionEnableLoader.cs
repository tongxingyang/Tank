using Assets.Tools.Script.Serialized.LocalCache;

namespace Assets.Tools.Script.Net.Downloader
{
    /// <summary>
    /// 带有本地版本管理功能的加载器
    /// </summary>
    public abstract class LocalVersionEnableLoader:UrlLoader
    {
        protected LocalVersionEnableLoader(string url, int version)
            : base(url, version)
        {
            _localVer = new LcInt(cacheName + "_Ver");
        }
        //本地资源版本号
        private LcInt _localVer;
        /// <summary>
        /// 本地保存的名字
        /// </summary>
        public string cacheName
        {
            get { return _cacheName ?? (_cacheName = GetCacheName()); }
            set { _cacheName = value; }
        }
        private string _cacheName;

        protected override void OnSaveToLocal()
        {
            _localVer.Value = version;
        }

        protected override void OnUnloadLocal()
        {
            _localVer.DeleteCache();
        }

        protected override void CopyFrom(UrlLoader loader)
        {
            base.CopyFrom(loader);
            var commonResourceLoader = loader as LocalVersionEnableLoader;
            _localVer = commonResourceLoader._localVer;
            cacheName = commonResourceLoader.cacheName;
        }
        
        protected sealed override int OnGetLocalVersion()
        {
            if (!HasLocal()) return 0;
            return _localVer.Value;
        }
        /// <summary>
        /// 创建LocalName
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCacheName()
        {
            string localname = url.Replace("http://", "");
            localname = url.Replace(":", "");
            localname = localname.Replace(".", "_");
            localname = localname.Replace("/", "_");
//            localname = localname.Replace("!", "");
//            localname = localname.Replace("&", "");
//            localname = localname.Replace("=", "");
            localname = localname.Replace("*", "");
            localname = localname.Replace("?", "");
            //            localName = "sdjic32";
            return localname;
        }
    }
}