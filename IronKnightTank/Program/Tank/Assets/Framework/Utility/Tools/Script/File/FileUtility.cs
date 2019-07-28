using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Tools.Script.Core.File
{
    using UnityEngine;

    using File = System.IO.File;

    /// <summary>
    /// 文件工具 win下 读写文件 
    /// </summary>
    public class FileUtility
    {
        /// <summary>
        /// 创建并写入一个文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="info">文件信息</param>
        public static void CreateAndWriteFile(string path, string info)
        {
            File.WriteAllText(path, info, new UTF8Encoding(false));
//            FileStream file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
//            StreamWriter writer = new StreamWriter(file, new UTF8Encoding(false));
//            writer.Write(info);
//            writer.Close();
        }

        /// <summary>
        /// 读取一个文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件内容</returns>
        public static string ReadFile(string path)
        {
            return File.ReadAllText(path, new UTF8Encoding(false));
//            StringBuilder info = new StringBuilder();
//            try
//            {
//                // Create an instance of StreamReader to read from a file.
//                // The using statement also closes the StreamReader.
//                using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false)))
//                {
//                    string line;
//                    // Read and display lines from the file until the end of 
//                    // the file is reached.
//                    while ((line = sr.ReadLine()) != null)
//                    {
//                        info.Append(line).Append("\r\n");
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                // Let the user know what went wrong.
//                Console.WriteLine("The file could not be read:");
//                Console.WriteLine(e.Message);
//            }
//            return info.ToString();
        }

        public static string GetFullPath(string assetsPath)
        {
            if (assetsPath.StartsWith("Assets/"))
            {
                assetsPath = assetsPath.Substring(6, assetsPath.Length - 6);
            }
            else if (!assetsPath.StartsWith("/"))
            {
                assetsPath = string.Format("{0}/", assetsPath);
            }
            return string.Format("{0}{1}", Application.dataPath, assetsPath);
        }

        public static string GetAssetsPath(string fullPath)
        {
            var indexOf = fullPath.IndexOf("Assets/", StringComparison.Ordinal);
            return fullPath.Substring(indexOf, fullPath.Length - indexOf);
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
