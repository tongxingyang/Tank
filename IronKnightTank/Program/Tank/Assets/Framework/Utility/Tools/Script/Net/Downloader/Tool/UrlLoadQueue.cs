using System;
using System.Collections.Generic;
using Assets.Tools.Script.Caller;
using Assets.Tools.Script.Event;

namespace Assets.Tools.Script.Net.Downloader.Tool
{
    /// <summary>
    /// 加载一批资源
    /// </summary>
    public class UrlLoadQueue
    {
        /// <summary>
        /// 队列总进度
        /// </summary>
        public readonly Signal<float> onTotalScheduleSignal=new Signal<float>();
        /// <summary>
        /// 队列中当前正在加载的单个资源进度
        /// </summary>
        public readonly Signal<float> onCurrLoaderScheduleSignal = new Signal<float>();
        /// <summary>
        /// 完成
        /// </summary>
        public readonly Signal<UrlLoadQueue> onCompleteSignal = new Signal<UrlLoadQueue>();
        /// <summary>
        /// 出错
        /// </summary>
        public readonly Signal<UrlLoadQueue> onErrorSignal = new Signal<UrlLoadQueue>();
        /// <summary>
        /// 是否正在加载
        /// </summary>
        public bool isLoading { get; private set; }
        /// <summary>
        /// 错误后重试次数
        /// </summary>
        public int retry { get; set; }
        /// <summary>
        /// 在单个加载重试Retry次后失败，依然不会终止队列继续加载
        /// </summary>
        public bool ignoreError { get; set; }
        /// <summary>
        /// 加载完成后缓存在内存当中
        /// </summary>
        public bool cacheMemory = true;
        /// <summary>
        /// 当前正在加载的
        /// </summary>
        protected UrlLoadQueueData currLoader;
        /// <summary>
        /// 所有需要加载资源url和他们对应的加载器等数据
        /// </summary>
        private readonly Dictionary<string, UrlLoadQueueData> _loaders = new Dictionary<string, UrlLoadQueueData>();
        private readonly List<string> _urls=new List<string>();
        /// <summary>
        /// 当前加载到的队列的位置
        /// </summary>
        private int _currLoadIndex = 0;
        /// <summary>
        /// 每个资源加载完成所占的总进度比例
        /// </summary>
        private float _progressInOne = 0;

        private FrameCall _progressFrameCall;

        /// <summary>
        /// 添加一个资源到队列
        /// </summary>
        /// <param name="url"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public UrlLoader Add(string url, int version = 0)
        {
            UrlLoader urlLoader = UrlLoaderCreator.GetLoader(url, version);
            if (urlLoader == null) throw new Exception("can't find a loader for this url");
            Add(urlLoader);
            return urlLoader;
        }

        /// <summary>
        /// 添加到队列
        /// </summary>
        /// <param name="loader">使用的加载器</param>
        /// <returns></returns>
        public UrlLoadQueue Add(UrlLoader loader)
        {
            if (isLoading) throw new Exception("can't add on loading");
            string url = loader.url;
            int version = loader.version;

            if (_loaders.ContainsKey(url))
            {
                if (version > _loaders[url].version)//版本更新
                {
                    _loaders[url].version = version;
                }
            }
            else
            {
                _loaders.Add(url, new UrlLoadQueueData(loader, url, version, retry));
                _urls.Add(url);
            }
            return this;
        }
        /// <summary>
        /// 开始对add的资源进行加载
        /// </summary>
        public void Load()
        {
            isLoading = true;

            bool hasNext = LoadNext();
            if (hasNext)
            {
                _progressInOne = 1f / _loaders.Count;
                if (_progressFrameCall != null) _progressFrameCall.Run();
                _progressFrameCall = FrameCall.CreateCall(ScheduleUpdate);
            }
            else
            {
                onTotalScheduleSignal.Dispatch(1);
            }
        }
        /// <summary>
        /// 在队列当中找到url对应的加载器
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public UrlLoader GetLoader(string url)
        {
            foreach (var urlLoadQueueData in _loaders.Values)
            {
                if (urlLoadQueueData.url == url) return urlLoadQueueData.urlLoader;
            }
            return null;
        }

        /// <summary>
        /// Gets the urls.
        /// </summary>
        /// <returns></returns>
        public List<string> GetUrls()
        {
            return _urls;
        }

        /// <summary>
        /// 更新进度（帧调用）
        /// </summary>
        /// <returns></returns>
        private bool ScheduleUpdate()
        {
            try
            {
                int loadIndex = _currLoadIndex < _loaders.Count ? _currLoadIndex : _loaders.Count - 1;
                float currLoaderProgress = currLoader.urlLoader.www != null
                    ? currLoader.urlLoader.www.progress
                    : 0;
                float totalProgress = _progressInOne * loadIndex + _progressInOne * currLoaderProgress;
                onCurrLoaderScheduleSignal.Dispatch(currLoaderProgress);
                onTotalScheduleSignal.Dispatch(totalProgress);
                return isLoading;
            }
            catch (Exception e)
            {
                DebugConsole.Log(e);
                return false;
            }

        }
        /// <summary>
        /// 加载下一个，如果没有下一个，则完成本次队列加载
        /// </summary>
        /// <returns></returns>
        private bool LoadNext()
        {
            if (_currLoadIndex < _loaders.Count)
            {
                currLoader = _loaders[_urls[_currLoadIndex]];
                if (cacheMemory || !currLoader.urlLoader.HasNewVersion()) //需要缓存内存或者还没有加载
                {
                    currLoader.urlLoader.onLoadComplete.AddEventListener(OnOneLoadCompleteHandler);
                    currLoader.urlLoader.onLoadError.AddEventListener(OnOneLoadErrorHandler);
                    FrameCall.DelayFrame(() =>//留一帧时间缓冲
                    {
                        currLoader.urlLoader.Load(cacheMemory);
                    });
                }
                else
                {
                    OnOneLoadCompleteHandler(currLoader.urlLoader);
                }
                return true;
            }
            else
            {
                isLoading = false;
                onCompleteSignal.Dispatch(this);
                return false;
            }
        }
        /// <summary>
        /// 某一个加载出错，如果在重试次数之内，则重新再次加载，否则抛出队列加载出错事件，终止本次加载（除非IgnoreError）
        /// </summary>
        /// <param name="obj"></param>
        private void OnOneLoadErrorHandler(ILoader obj)
        {
            if (currLoader.currRetry < currLoader.maxRetry)
            {
                currLoader.currRetry++;
                LoadNext();
            }
            else if (ignoreError)
            {
                if (!cacheMemory) _loaders[_urls[_currLoadIndex]] = null;
                if (_progressFrameCall != null) _progressFrameCall.Run();
                _progressFrameCall = null;
                _currLoadIndex++;
                LoadNext();
            }
            else
            {
                OnOneEndHandler(obj);
                isLoading = false;
                onErrorSignal.Dispatch(this);
            }

        }
        /// <summary>
        /// 某一个加载完成，尝试加载下一个资源
        /// </summary>
        /// <param name="obj"></param>
        private void OnOneLoadCompleteHandler(ILoader obj)
        {
            if (!cacheMemory) _loaders[_urls[_currLoadIndex]] = null;
            if (_progressFrameCall != null) _progressFrameCall.Run();
            _progressFrameCall = null;
            OnOneEndHandler(obj);
            _currLoadIndex++;
            LoadNext();
        }
        /// <summary>
        /// 任何一个加载完成后，移除这个加载器的事件监听
        /// </summary>
        /// <param name="obj"></param>
        private void OnOneEndHandler(ILoader obj)
        {
            obj.onLoadComplete.RemoveEventListener(OnOneLoadCompleteHandler);
            obj.onLoadError.RemoveEventListener(OnOneLoadErrorHandler);
        }

        protected class UrlLoadQueueData
        {
            public UrlLoader urlLoader;
            public string url;
            public int version;

            public int maxRetry = 0;
            public int currRetry = 0;

            public UrlLoadQueueData(UrlLoader loader, string url, int version = -1, int retry = 0)
            {
                this.urlLoader = loader;
                this.url = url;
                this.version = version;
                this.maxRetry = retry;
            }
        }
    }
}