using Assets.Tools.Script.Core.File;
using Assets.Tools.Script.File;
using Assets.Tools.Script.Serialized.LocalCache.Core;

namespace Assets.Tools.Script.Serialized.LocalCache.xml
{
    /// <summary>
    /// 用xml序列化对象保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LcXMLFile<T> : UnityLocalESFileCache<T> where T : class
    {
        public LcXMLFile(string fileName) : base(fileName)
        {
        }

        protected override T GetLocalCache()
        {
            T xmlDeserializeToObject = ESFile.LoadXMLObject<T>(fileName);
            if (xmlDeserializeToObject is IXMLSerializable)
            {
                (xmlDeserializeToObject as IXMLSerializable).AfterSerialize();
            }
            return xmlDeserializeToObject;
        }

        protected override void SaveLocalCache(T value)
        {
            if (value is IXMLSerializable)
            {
                (value as IXMLSerializable).BeforSerialize();
            }
            ESFile.SaveXMLObject(fileName, value);
        }
    }
}