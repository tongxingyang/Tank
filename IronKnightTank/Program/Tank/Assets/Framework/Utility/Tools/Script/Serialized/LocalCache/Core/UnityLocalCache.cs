using System;

namespace Assets.Tools.Script.Serialized.LocalCache.Core
{
    /// <summary>
    /// 能够缓存到本地的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnityLocalCache<T> : IUnityLocalCache
    {
        /// <summary>
        /// 保存key的后缀，如果没有,无需赋值
        /// </summary>
        public Func<string> suffixGetter = null;

        private readonly string _baseName;

        protected UnityLocalCache(string name)
        {
            _baseName = name;
        }
        //保存key后缀
        protected string Suffix
        {
            get
            {
                if (suffixGetter != null)
                    return suffixGetter();
                return "";
            }
        }
        /// <summary>
        /// 保存的key,(baseName_Suffix)
        /// </summary>
        public string CacheName
        {
            get
            {
                if (!String.IsNullOrEmpty(Suffix))
                {
                    return _baseName + Suffix;
                }
                return _baseName;
            }
        }

        /// <summary>
        /// 保存值，在设置值时会缓存到本地
        /// 如果从未设置，读取类型的默认值
        /// </summary>
        public T Value
        {
            get
            {
                if (HasCache())
                    return GetLocalCache();
                return default(T);
            }
            set
            {
                SaveLocalCache(value);
            }
        }
        /// <summary>
        /// 本地外存是否已经保存过这个值
        /// </summary>
        /// <returns></returns>
        public abstract bool HasCache();
        /// <summary>
        /// 删除缓存
        /// </summary>
        public abstract void DeleteCache();

        //--------------------------------------------------------------
        //保存和读取方法
        protected abstract T GetLocalCache();
        protected abstract void SaveLocalCache(T value);
    }
}