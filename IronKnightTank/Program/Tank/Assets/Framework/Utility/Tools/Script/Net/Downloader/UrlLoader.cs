using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.Script.Caller;
using Assets.Tools.Script.Event;
using UnityEngine;

namespace Assets.Tools.Script.Net.Downloader
{
    /// <summary>
    /// 加载器基类
    /// </summary>
    public abstract class UrlLoader:ILoader
    {
        /// <summary>
        /// 默认加载重试次数
        /// </summary>
        public static int defaultAttempts = 1;
        //url对应正在加载的加载器
        protected static Dictionary<string, List<UrlLoader>> loadingLoaders = new Dictionary<string, List<UrlLoader>>();
        protected static Dictionary<string, UrlLoader> cacheMemoryLoaders = new Dictionary<string, UrlLoader>();
        /// <summary>
        /// 加载开始
        /// </summary>
        public Signal<ILoader> onLoadStart { get; private set; }
        /// <summary>
        /// 加载完成
        /// </summary>
        public Signal<ILoader> onLoadComplete { get; private set; }
        /// <summary>
        /// 加载失败
        /// </summary>
        public Signal<ILoader> onLoadError { get; private set; }
        /// <summary>
        /// 卸载本地缓存开始
        /// </summary>
        public Signal<ILoader> onUnloadLocalStart { get; private set; }
        /// <summary>
        /// 卸载本地缓存完成
        /// </summary>
        public Signal<ILoader> onUnloadLocalComplete { get; private set; }
        /// <summary>
        /// 卸载本地缓存错误
        /// </summary>
        public Signal<ILoader> onUnloadLocalError { get; private set; }
        /// <summary>
        /// 加载失败最大重试次数
        /// </summary>
        public int attempts { get;private set; }
        /// <summary>
        /// 设置最大重试次数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public UrlLoader SetAttempts(int value)
        {
            this.attempts = value;
            return this;
        }
        /// <summary>
        /// 加载的url
        /// </summary>
        public string url { get; private set; }
        /// <summary>
        /// 加载的版本号，0和0以下的版本都表示无需缓存本地
        /// </summary>
        public int version { get; private set; }
        /// <summary>
        /// 加载的WWW
        /// </summary>
        public WWW www { get; protected set; }
        /// <summary>
        /// 是否需要缓存在内存
        /// </summary>
        public bool isCatchMemory { get; private set; }
        //当前加载失败的尝试次数
        private int _currAttempts;
        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// 加载器
        /// </summary>
        /// <param name="url">远程地址</param>
        /// <param name="version">如果version>=0则表示要缓存本地，同时如果本地以及存在相应版本或更高版本则从本地加载</param>
        protected UrlLoader(string url, int version)
        {
            this.attempts = defaultAttempts;

            onLoadStart = new Signal<ILoader>();
            onLoadComplete = new Signal<ILoader>();
            onLoadError = new Signal<ILoader>();
            onUnloadLocalStart = new Signal<ILoader>();
            onUnloadLocalComplete = new Signal<ILoader>();
            onUnloadLocalError = new Signal<ILoader>();

            this.url = url;
            this.version = version;
        }
        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// 卸载本地资源
        /// </summary>
        public void UnloadLocal()
        {
            try
            {
                onUnloadLocalStart.Dispatch(this);
                if (HasLocal())
                    OnUnloadLocal();
                onUnloadLocalComplete.Dispatch(this);
            }
            catch (Exception e)
            {
                onUnloadLocalError.Dispatch(this);
            }

        }
        
        /// <summary>
        /// 对比本地是否是更新的版本
        /// </summary>
        /// <returns></returns>
        public bool HasNewVersion()
        {
            return version > GetLocalVersion();
        }
        /// <summary>
        /// 获得本地资源版本号
        /// </summary>
        /// <returns></returns>
        public int GetLocalVersion()
        {
            return OnGetLocalVersion();
        }
        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="catchMemory">是否需要保存在内存，如果不需要，在抛出完成事件后会析构下载的资源</param>
        public void Load(bool catchMemory = true)
        {
            this.isCatchMemory = catchMemory;
            _currAttempts = 0;
//            OnInit();

            lock (loadingLoaders)
            {
                onLoadStart.Dispatch(this);
                if (cacheMemoryLoaders.ContainsKey(url))
                {
                    CopyFrom(cacheMemoryLoaders[url]);
//                    OnPreLoadCompleteHandler();
                    
                    OnLoadCompleteHandler();
                    if (version > 0 && www != null && HasNewVersion())//需要保持到本地
                        SaveToLocal();
                    onLoadComplete.Dispatch(this);
                }
                else if (loadingLoaders.ContainsKey(url))
                {
                    loadingLoaders[url].Add(this);
                }
                else
                {
                    loadingLoaders.Add(url, new List<UrlLoader>());
                    loadingLoaders[url].Add(this);
                    LoadResource();
                }
            }
        }
        /// <summary>
        /// 卸载内存数据
        /// </summary>
        public void Unload()
        {
            try
            {
                OnUnload();
            }
            catch (Exception e)
            {
                DebugConsole.Log(e);
            }
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// 开始加载（远端或者本地）
        /// </summary>
        protected void LoadResource()
        {
            if (version >= 0)
            {
                if (HasLocal())//本地已存在
                {
                    if (HasNewVersion())//需要更新
                    {
//                        DebugConsole.Log("从资源服务器获取新资源");
                        onUnloadLocalComplete.AddEventListener((thisloader) => LoadFromUrl());
                        onUnloadLocalError.AddEventListener((thisloader) => LoadFromUrl());
                        UnloadLocal();
                    }
                    else//版本正确
                    {
//                        DebugConsole.Log("从本地缓存获取新资源");
                        try
                        {
                            LoadFromLocal();
                        }
                        catch(Exception e)
                        {
                            DebugConsole.Log(e);
                        }
                    }
                }
                else//不存在则下载
                {
//                    DebugConsole.Log("从资源服务器获取新资源");
                    LoadFromUrl();
                }
            }
            else//无需缓存本地的下载
            {
//                DebugConsole.Log("从资源服务器获取新资源");
                LoadFromUrl();
            }
        }
        /// <summary>
        /// 从网络下载
        /// </summary>
        private void LoadFromUrl()
        {
            CoroutineCall.Call(StartLoad);
        }

        IEnumerator StartLoad()
        {
            while (_currAttempts < attempts)
            {
                www = new WWW(url);
                yield return www;
                if (!String.IsNullOrEmpty(www.error))//出错
                {
                    _currAttempts++;
                }
                else if (www.isDone)
                {
                    LoadComplete();
                    break;
                }
            }
            if (_currAttempts >= attempts)
                LoadError();
        }
        /// <summary>
        /// 加载完成时
        /// </summary>
        protected void LoadComplete()
        {
            List<UrlLoader> loadingLoader = null;
            lock (loadingLoaders)
            {
                if (loadingLoaders.ContainsKey(url))
                {
                    loadingLoader = loadingLoaders[url];
                    loadingLoaders.Remove(url);
                }
            }
            if (loadingLoader != null)
            {
                bool catchMemory = false;
                foreach (var loader in loadingLoader)//同一批加载的对象
                {
                    catchMemory = catchMemory || loader.isCatchMemory;//只要有一个要求缓存内存
                    loader.CopyFrom(this);
                    
                    loader.OnLoadCompleteHandler();
                }
                if(version>0 && www!=null)//需要保持到本地
                    SaveToLocal();
                if (catchMemory)//要保存在内存的
                {
                    cacheMemoryLoaders.Add(url, this);
                }
                foreach (var loader in loadingLoader)//同一批加载的对象
                {
                    loader.onLoadComplete.Dispatch(loader);
                }
                if (!catchMemory)//全都不要缓存，则从内存移除
                {
                    Unload();
                    Resources.UnloadUnusedAssets();
                    GC.Collect();
                }

                
            }
        }
        /// <summary>
        /// 加载出错时
        /// </summary>
        protected void LoadError(String ErrorString="")
        {
            if (String.IsNullOrEmpty(ErrorString))
            DebugConsole.Log("[LoadError]"+www.url+":"+www.error,GetType().ToString());
            else
            {
                DebugConsole.Log("[LoadError]"+ErrorString);
            }
            List<UrlLoader> loadingLoader=null;
            lock (loadingLoaders)
            {
                if (loadingLoaders.ContainsKey(www.url))
                {
                    loadingLoader = loadingLoaders[www.url];
                    loadingLoaders.Remove(www.url);
                }
            }
            if (loadingLoader != null)
            {
                foreach (var loader in loadingLoader)
                {
                    loader.CopyFrom(this);
                    loader.onLoadError.Dispatch(loader);
                    loader.OnLoadErrorHandler();
                }
            }
        }

        /// <summary>
        /// 保存到本地
        /// </summary>
        private void SaveToLocal()
        {
            try
            {
                OnSaveToLocal();
            }
            catch (Exception e)
            {
                DebugConsole.Log(e);
                UnloadLocal();
            }

        }
        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// 本地是否有缓存
        /// </summary>
        /// <returns></returns>
        public abstract bool HasLocal();
        /// <summary>
        /// 复制UrlLoader内容
        /// </summary>
        /// <param name="loader"></param>
        protected virtual void CopyFrom(UrlLoader loader)
        {
            www = loader.www;
        }
        /// <summary>
        /// 如何卸载内存资源
        /// </summary>
        protected virtual void OnUnload()
        {
            www = null;
        }
        /// <summary>
        /// 完成加载后，做的相应处理
        /// </summary>
        protected virtual void OnLoadCompleteHandler(){}
        /// <summary>
        /// 出错处理
        /// </summary>
        protected virtual void OnLoadErrorHandler(){}
        /// <summary>
        /// 保存到本地磁盘的方法
        /// </summary>
        protected abstract void OnSaveToLocal();
        /// <summary>
        /// 从本地磁盘移除
        /// </summary>
        protected abstract void OnUnloadLocal();
        /// <summary>
        /// 从本地读取，读取完之后要调用LoadComplete()方法
        /// </summary>
        protected abstract void LoadFromLocal();
        /// <summary>
        /// 获得本地版本号
        /// </summary>
        /// <returns></returns>
        protected abstract int OnGetLocalVersion();

    }
}