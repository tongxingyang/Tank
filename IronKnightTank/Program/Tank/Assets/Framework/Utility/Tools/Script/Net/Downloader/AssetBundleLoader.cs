using System;
using System.Collections;
using Assets.Tools.Script.Caller;
using Assets.Tools.Script.Core.File;
using Assets.Tools.Script.File;
using UnityEngine;

namespace Assets.Tools.Script.Net.Downloader
{
    /// <summary>
    /// 加载一个AssetBundle
    /// </summary>
    public class AssetBundleLoader : LocalVersionEnableLoader
    {
        /// <summary>
        /// 加载到的资源
        /// </summary>
        public AssetBundle assetBundle;

        public AssetBundleLoader(string url, int version) : base(url, version)
        {

        }

        protected override void LoadFromLocal()
        {
            CoroutineCall.Call(StartLoadLocal);
        }

        private IEnumerator StartLoadLocal()
        {
            www = new WWW(LoadPath.readLoadPath + cacheName);
            yield return www;
            if (!String.IsNullOrEmpty(www.error))//出错
            {
                LoadError();
            }
            else if (www.isDone)
            {
                assetBundle = www.assetBundle;
                LoadComplete();
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            assetBundle = null;
        }

        protected override string GetCacheName()
        {
            return LoadPath.replace("bundle/" + base.GetCacheName() + ".assetbundle");
        }

        protected override void OnSaveToLocal()
        {
            base.OnSaveToLocal();
            if (www!=null)
            {
                ESFile.SaveRaw(www.bytes,cacheName);
            }     
        }

        protected override void OnUnloadLocal()
        {
            base.OnUnloadLocal();
            ESFile.Delete(cacheName);
        }

        protected override void OnLoadCompleteHandler()
        {
            base.OnLoadCompleteHandler();
            assetBundle = www.assetBundle;
        }

        public override bool HasLocal()
        {
            return ESFile.Exists(cacheName);
        }

        protected override void CopyFrom(UrlLoader loader)
        {
            base.CopyFrom(loader);
            var assetBundleLoader = loader as AssetBundleLoader;
            if (assetBundleLoader != null) assetBundle = assetBundleLoader.assetBundle;
        }

//        protected override void OnSaveToLocal()
//        {
//            throw new NotImplementedException();
//        }
//
//        protected override void OnDeleteLocal(Action onSucceed, Action onError)
//        {
//            throw new NotImplementedException();
//        }
    }
}