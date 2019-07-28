namespace Assets.Tools.Script.Serialized.LocalCache.xml
{
    /// <summary>
    /// LcXMLFile使用时，如果保存的对象实现了IXMLSerializable接口，则会调用
    /// </summary>
    public interface IXMLSerializable
    {
        /// <summary>
        /// 在序列化之前
        /// </summary>
        void BeforSerialize();
        /// <summary>
        /// 在反序列化之后
        /// </summary>
        void AfterSerialize();
    }
}