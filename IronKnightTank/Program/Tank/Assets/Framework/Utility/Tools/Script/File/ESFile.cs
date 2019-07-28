using System.IO;
using System.Text;
using Assets.Tools.Script.Core.File;

namespace Assets.Tools.Script.File
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 文件的保存和读取工具
    /// 相对路径是指相对Application.persistentDataPath的路径
    /// </summary>
    public class ESFile
    {
        private static UTF8Encoding utf8Encoding = new UTF8Encoding(false);
        #region save and load

        /// <summary>
        /// 读取一段文本内容
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>文本内容</returns>
        public static string LoadString(string path)
        {
            path = GetAbsolutePath(path);
            string readFile = ReadStringFile(path);
            return readFile;
        }
        /// <summary>
        /// 保存一段文本
        /// </summary>
        /// <param name="info">文本内容</param>
        /// <param name="path">相对路径</param>
        /// <param name="mode">写入模式</param>
        public static void Save(string info, string path, FileMode mode = FileMode.Create)
        {
            path = GetAbsolutePath(path);
            CreateORwriteFile(path, info, mode);
        }

        public static void SaveAsCreate(string info, string path)
        {
            Save(info, path, FileMode.Create);
        }

        public static void SaveAsAppend(string info, string path)
        {
            Save(info, path, FileMode.Append);
        }

        /// <summary>
        /// 保存二进制文件
        /// </summary>
        /// <param name="bytes">内容</param>
        /// <param name="path">相对路径</param>
        public static void SaveRaw(byte[] bytes, string path)
        {
            path = GetAbsolutePath(path);
            CreateDirectoryIfNonexistence(path);
            System.IO.File.WriteAllBytes(path, bytes);
        }
        /// <summary>
        /// 读取二进制文件
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>内容</returns>
        public static byte[] LoadRaw(string path)
        {
            path = GetAbsolutePath(path);
            return ReadBytesFile(path);
        }
        /// <summary>
        /// 序列化为XML保存
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <param name="objToSerialize">序列化对象</param>
        public static void SaveXMLObject(string path, object objToSerialize)
        {
            path = GetAbsolutePath(path);
            objToSerialize.XMLSerialize_AndSaveToPersistentDataPath(Path.GetDirectoryName(path), Path.GetFileName(path));
        }
        /// <summary>
        /// 读取XML文件并反序列化
        /// </summary>
        /// <typeparam name="T">反序列化为T类型</typeparam>
        /// <param name="path">相对路径</param>
        /// <returns>反序列化后的对象</returns>
        public static T LoadXMLObject<T>(string path) where T : class
        {
            path = GetAbsolutePath(path);
            return Path.GetFileName(path).XMLDeserialize_AndLoadFromPersistentDataPath<T>(Path.GetDirectoryName(path));
        }
        #endregion

        #region folder and file
        /// <summary>
        /// 是否存在文件或者文件夹
        /// 当路径是文件夹时，已"/"结尾，如test/aa/
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>是否存在</returns>
        public static bool Exists(string path)
        {
            path = GetAbsolutePath(path);
            if (path.EndsWith("/"))
            {
                return Directory.Exists(path);
            }
            else
            {
                return System.IO.File.Exists(path);
            }

        }
        /// <summary>
        /// 删除文件或者文件夹
        /// 当路径是文件夹时，已"/"结尾，如test/aa/
        /// </summary>
        /// <param name="path">相对路径</param>
        public static void Delete(string path)
        {
            path = GetAbsolutePath(path);
            if (path.EndsWith("/"))
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            else
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

        }
        /// <summary>
        /// 获得文件夹下的所有文件名
        /// </summary>
        /// <param name="folderPath">相对路径</param>
        /// <returns>不带目录路径的文件名</returns>
        public static string[] GetFiles(string folderPath)
        {
            folderPath = GetAbsolutePath(folderPath);
            string[] strings = Directory.GetFiles(folderPath);
            string[] files = new string[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                string substring = strings[i].Substring(folderPath.Length, strings[i].Length - folderPath.Length);
                files[i] = substring;
            }
            return files;
        }
        /// <summary>
        /// 从相对路径获得绝对路径
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>绝对路径</returns>
        private static string GetAbsolutePath(string path)
        {
            return Path.Combine(LoadPath.saveLoadPath, path);
        }
        /// <summary>
        /// 如果路径使用的目录不存在，则创建目录
        /// </summary>
        /// <param name="path">绝对路径</param>
        private static void CreateDirectoryIfNonexistence(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName != null && !Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);
        }
        #endregion

        #region Stream

        private static readonly Dictionary<string, StreamWriter> holdedStreamWriters = new Dictionary<string, StreamWriter>();

        public static void HoldStreamWriter(string path, FileMode mode = FileMode.Create, Encoding encoding = null)
        {
            path = GetAbsolutePath(path);
            if (encoding == null)
            {
                encoding = utf8Encoding;
            }
            bool holded;
            holdedStreamWriters[path] = CreateStreamWriter(path, mode, encoding, out holded);
        }

        public static void ReleaseStreamWriter(string path)
        {
            path = GetAbsolutePath(path);
            StreamWriter streamWriter;
            var tryGetValue = holdedStreamWriters.TryGetValue(path,out streamWriter);
            if (tryGetValue)
            {
                streamWriter.Close();
                streamWriter.Dispose();
                holdedStreamWriters.Remove(path);
            }
        }

        public static void CreateORwriteFile(string path, string info, FileMode mode = FileMode.Create)
        {
            try
            {
//                var fileStream = CreateStreamWriter(path, mode);
                bool holded;
                StreamWriter streamWriter = CreateStreamWriter(path, mode, utf8Encoding,out holded);
                streamWriter.Write(info);
                streamWriter.Flush();
                if (!holded)
                {
                    streamWriter.Close();
                    streamWriter.Dispose();
                }
            }
            catch (Exception e)
            {
                DebugConsole.Log(e);
            }
        }

        public static void CreateORwriteFile(string path, byte[] info, FileMode mode = FileMode.Create)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(path, mode == FileMode.Append);
                streamWriter.Write(info);
                streamWriter.Close();
                streamWriter.Dispose();
            }
            catch (Exception e)
            {
                DebugConsole.Log(e);
            }
        }

        private static StreamWriter CreateStreamWriter(string path, FileMode mode, Encoding encoding, out bool holded)
        {
            StreamWriter streamWriter;
            holded = holdedStreamWriters.TryGetValue(path, out streamWriter);
            if (!holded)
            {
                path.CreateDirectoryIfNotExists();
                streamWriter = new StreamWriter(path, mode == FileMode.Append, encoding);
            }
            return streamWriter;
            //            path.CreateDirectoryIfNotExists();
            //            StreamWriter streamWriter = new StreamWriter(path, mode == FileMode.Append);

            //            FileStream file = new FileStream(path, mode, FileAccess.Write);
            //            return file;
        }

        public static string ReadStringFile(string path)
        {
            StreamReader sr = new StreamReader(path, utf8Encoding);
            var readToEnd = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            return readToEnd;
        }

        public static byte[] ReadBytesFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            fs.Dispose();
            return bytes;
        }
        #endregion
    }
}
