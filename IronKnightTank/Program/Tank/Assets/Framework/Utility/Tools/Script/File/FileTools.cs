﻿namespace Assets.Tools.Script.File
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// The file tools.
    /// </summary>
    public static class FileTools
    {
        public static void VerifyDirection(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }
        public static void Copy(string fromDir, string toDir, bool overwrite)
        {
            VerifyDirection(toDir);
            File.Copy(fromDir, toDir, overwrite);
        }

        public static object Create(string dir)
        {
            VerifyDirection(dir);
            return File.Create(dir);
        }

        public static void Delete(string dir)
        {
            File.Delete(dir);
        }

        public static bool Exists(string dir)
        {
            return File.Exists(dir);
        }

        public static void Move(string fromdir, string todir)
        {
            VerifyDirection(todir);
            File.Move(fromdir, todir);
        }

        public static FileStream Open(string dir, FileMode mode)
        {
            return File.Open(dir, mode);
        }
        public static byte[] ReadAllBytes(string dir)
        {
            FileInfo fi = new FileInfo(dir);
            long len = fi.Length;
            FileStream fs = new FileStream(dir, FileMode.Open);
            byte[] buffer = new byte[len];
            fs.Read(buffer, 0, (int)len);
            fs.Close();
            return buffer;
        }
        public static string ReadAllText(string dir)
        {
            if (!Exists(dir))
            {
                FileStream fs = Create(dir) as FileStream;
                fs.Close();
            }
            return File.ReadAllText(dir);
        }

        public static void WriteAllText(string dir, string datas)
        {
            VerifyDirection(dir);
            File.WriteAllText(dir, datas, Encoding.UTF8);
        }
    }
}