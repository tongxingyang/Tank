using Assets.Tools.Script.Serialized.LocalCache;

namespace Assets.Tools.Script.Net.Downloader
{
    /// <summary>
    /// 文本加载器
    /// </summary>
    public class TextLoader:LocalVersionEnableLoader
    {
        //本地保存用
        private LcString _localCache;
        public TextLoader(string url, int version) : base(url, version)
        {
            _localCache = new LcString(cacheName);
        }
        /// <summary>
        /// 加载完成后的文本内容
        /// </summary>
        public string text { get; private set; }

//        protected override void OnInit()
//        {
//            base.OnInit();
//            
//        }

        protected override string GetCacheName()
        {
            return url;
        }

        protected override void LoadFromLocal()
        {
            text = _localCache.Value;
            LoadComplete();
        }

        protected override void OnSaveToLocal()
        {
            base.OnSaveToLocal();
            _localCache.Value = text;
        }

        protected override void OnUnloadLocal()
        {
            base.OnUnloadLocal();
            _localCache.DeleteCache();
        }

//        protected override void OnDeleteLocal(Action onSucceed, Action onError)
//        {
//            _localCache.DeleteCache();
//            onSucceed();
//        }

        protected override void OnLoadCompleteHandler()
        {
            if(www!=null)
                text = www.text;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            text = null;
            _localCache = null;
        }

        public override bool HasLocal()
        {
            return _localCache.HasCache();
        }

        protected override void CopyFrom(UrlLoader loader)
        {
            base.CopyFrom(loader);
            text = (loader as TextLoader).text;
        }
    }
}