using System;
using Assets.Tools.Script.Core.File;
using Assets.Tools.Script.File;
using Assets.Tools.Script.Serialized.LocalCache.Core;
using Assets.Tools.Script.Serialized.LocalCache.xml;
using UnityEngine;

namespace Assets.Tools.Script.Serialized.LocalCache
{
    /// <summary>
    /// 保存图片
    /// </summary>
    public class LcTexture2DFile : UnityLocalCache<Texture2D>
    {
        private readonly LcXMLFile<TextureCacheConfig> _cacheDataConfig;

        public LcTexture2DFile(string fileName)
            : base(fileName)
        {
            _cacheDataConfig = new LcXMLFile<TextureCacheConfig>(fileName);
        }
        protected override Texture2D GetLocalCache()
        {
            byte[] bytes = ESFile.LoadRaw(CacheName + ".png");
            TextureCache cache = new TextureCache();
            cache.Init(_cacheDataConfig.Value, bytes);

            return cache.GetTexture2D();
        }

        protected override void SaveLocalCache(Texture2D value)
        {
            TextureCache cache = new TextureCache();
            cache.Init(value);
            _cacheDataConfig.Value = cache.Config;
            
            ESFile.SaveRaw(cache.Bytes, CacheName + ".png");
        }

        public override bool HasCache()
        {
            return _cacheDataConfig.HasCache();
        }

        public override void DeleteCache()
        {
            _cacheDataConfig.DeleteCache();
            ESFile.Delete(CacheName + ".png");
        }

        private class TextureCache
        {
            public byte[] Bytes;
            public TextureCacheConfig Config;

            public TextureCache Init(Texture2D texture2D)
            {
                Bytes = texture2D.EncodeToPNG();
                Config = new TextureCacheConfig();
                Config.Format = texture2D.format;
                Config.Width = texture2D.width;
                Config.Height = texture2D.height;
                return this;
            }

            public TextureCache Init(TextureCacheConfig config, byte[] bytes)
            {
                Bytes = bytes;
                Config = config;
                return this;
            }

            public Texture2D GetTexture2D()
            {
                if (Bytes == null || Config == null) return null;
                Texture2D texture2D = new Texture2D(Config.Width, Config.Height, Config.Format, false, false);
                texture2D.LoadImage(Bytes);
                return texture2D;
            }

        }
        
    }
    [Serializable]
    public class TextureCacheConfig
    {
        public int Width;
        public int Height;
        public TextureFormat Format;
    }
}