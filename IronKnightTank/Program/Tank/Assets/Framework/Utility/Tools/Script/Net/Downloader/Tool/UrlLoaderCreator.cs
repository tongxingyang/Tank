namespace Assets.Tools.Script.Net.Downloader.Tool
{
    public class UrlLoaderCreator
    {
        /// <summary>
        /// 根据url指定的资源类型，获得UrlLoader
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        public static UrlLoader GetLoader(string url,int ver)
        {
            string[] strings = url.Split('.');
            string s = strings[strings.Length - 1];
            s = s.ToLower();
            switch (s)
            {
                case "unity3d": return new AssetBundleLoader(url, ver);
                case "assetbundle": return new AssetBundleLoader(url, ver);
                case "png": return new PictureLoader(url, ver);
                case "jpg": return new PictureLoader(url, ver);
                case "txt": return new TextLoader(url, ver);
                case "xml": return new TextLoader(url, ver);
            }
            return null;
        }
    }
}