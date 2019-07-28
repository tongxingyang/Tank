using System;
using Assets.Tools.Script.Serialized.LocalCache;
using UnityEngine;

namespace Assets.Tools.Script.Net.Downloader
{
    /// <summary>
    /// 加载png，jpg等图片资源
    /// </summary>
    public class PictureLoader : LocalVersionEnableLoader
    {
        public PictureLoader(string url, int version)
            : base(url, version)
        {
            _localCache = new LcTexture2DFile(cacheName);
        }
        /// <summary>
        /// 加载完之后的Texture
        /// </summary>
        public Texture2D texture { get; private set; }
        private LcTexture2DFile _localCache;
        

//        protected override void OnInit()
//        {
//            base.OnInit();
//            
//        }



        protected override string GetCacheName()
        {
            return "img/"+base.GetCacheName();
        }

        protected override void LoadFromLocal()
        {
//            DebugConsole.Log("LoadFromLocal",Url,Version);
            try
            {
                texture=_localCache.Value;
                LoadComplete();
            }
            catch (Exception)
            {
                _localCache.DeleteCache();
                throw;
            }
        }

        protected override void OnSaveToLocal()
        {
            base.OnSaveToLocal();
            _localCache.Value = texture;
        }

        protected override void OnUnloadLocal()
        {
            base.OnUnloadLocal();
            _localCache.DeleteCache();
        }

        protected override void OnLoadCompleteHandler()
        {
            if (www != null)
            {
                texture = www.texture;
            } 
            base.OnLoadCompleteHandler();
        }

//        protected override void OnPreLoadCompleteHandler()
//        {
//            
//        }

        protected override void OnUnload()
        {
            base.OnUnload();
            texture = null;
            _localCache = null;
        }

        public override bool HasLocal()
        {
            return _localCache.HasCache();
        }

        protected override void CopyFrom(UrlLoader loader)
        {
            base.CopyFrom(loader);
            texture = (loader as PictureLoader).texture;
            _localCache = (loader as PictureLoader)._localCache;
        }
    }

//    [Serializable]
//    public class TextureCache
//    {
//        public byte[] Bytes;
//        public int Width;
//        public int Height;
//        public TextureFormat Format;
//
//        public TextureCache Init(Texture2D texture2D)
//        {
//            Format = texture2D.format;
//            Bytes = texture2D.EncodeToPNG();
//            Width = texture2D.width;
//            Height = texture2D.height;
//            return this;
//        }
//
//        public Texture2D GetTexture2D()
//        {
//            if (Bytes == null) return null;
//            Texture2D texture2D=new Texture2D(Width,Height,Format,false,false);
//            texture2D.LoadImage(Bytes);
//            return texture2D;
//        }
//    }
}