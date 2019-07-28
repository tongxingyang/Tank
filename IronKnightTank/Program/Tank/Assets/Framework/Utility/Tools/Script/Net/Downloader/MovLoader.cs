using System;
using Assets.Tools.Script.File;

namespace Assets.Tools.Script.Net.Downloader
{
    /// <summary>
    /// mov加载器
    /// </summary>
    public class MovLoader : LocalVersionEnableLoader
    {
        public MovLoader(string url, int version) : base(url, version)
        {
        }

        protected override string GetCacheName()
        {
            return "mov/"+base.GetCacheName()+".mov";
        }

        protected override void LoadFromLocal()
        {
            try
            {
                LoadComplete();
            }
            catch (Exception)
            {
                ESFile.Delete(cacheName);
                throw;
            }
        }

        public override bool HasLocal()
        {
            return ESFile.Exists(cacheName);
        }

        protected override void OnSaveToLocal()
        {
            base.OnSaveToLocal();
            ESFile.SaveRaw(www.bytes, cacheName);
        }
        
    }

}