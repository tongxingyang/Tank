using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Tools.Script.Helper
{
    public static class SeralizeHelper
    {
        /// <summary>
        /// 序列化byte数组
        /// </summary>
        /// <param name="obj">Object.</param>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null) return new byte[] { };
            BinaryFormatter serializer = new BinaryFormatter();
            using (MemoryStream memStream = new MemoryStream())
            {
                serializer.Serialize(memStream, obj);
                return memStream.ToArray();
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <returns>The byte array.</returns>
        /// <param name="barray">Barray.</param>
        public static object FromByteArray(this byte[] barray)
        {
            if (barray.Length == 0)
                return null;
            using (MemoryStream memStream = new MemoryStream(barray))
            {
                BinaryFormatter deserializer = new BinaryFormatter();
                return deserializer.Deserialize(memStream);
            }
        }
        /// <summary>
        /// 序列化后base64
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToByteString(this object obj)
        {
            byte[] bytes = obj.ToByteArray();
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// base64字符串反序列化
        /// </summary>
        /// <param name="bytestring"></param>
        /// <returns></returns>
        public static T FromByteString<T>(this string bytestring) where T : class
        {
            byte[] bytes= Convert.FromBase64String(bytestring);
            return bytes.FromByteArray() as T;
        }
    }
}