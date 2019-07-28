using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;


public static class EditorTools
{
    public static void CopyDir(string sourceDir, string targetDir, bool isAppend = false, string searchPattern = "")
    {
        if (!Directory.Exists(sourceDir))
        {
            Debug.Log("dir not exit :" + sourceDir + "   please check your input");
            return;
        }
#if UNITY_EDITOR_WIN
        sourceDir = sourceDir.Replace("/", "\\");
#endif
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }
        DirectoryInfo di = new DirectoryInfo(sourceDir);

        if (!isAppend && Directory.Exists(targetDir))
        {
            Directory.Delete(targetDir, true);
        }
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }
        DirectoryInfo[] dirInfos = di.GetDirectories("*.*", SearchOption.AllDirectories);
        foreach (var dirInfo in dirInfos)
        {
            string newSubDirFullPath = dirInfo.FullName.Replace(sourceDir, targetDir);
            if (!Directory.Exists(newSubDirFullPath))
            {
                Directory.CreateDirectory(newSubDirFullPath);
            }
        }

        FileInfo[] fileInfos = di.GetFiles(string.IsNullOrEmpty(searchPattern) ? "*.*" : searchPattern, SearchOption.AllDirectories);
        foreach (var fileInfo in fileInfos)
        {
            if (fileInfo.FullName.EndsWith(".meta") || fileInfo.FullName.EndsWith(".iml"))
            {
                continue;
            }

            string newFileFullPath = fileInfo.FullName.Replace(sourceDir, targetDir);
            File.Copy(fileInfo.FullName, newFileFullPath, true);
        }
    }

    public static string GetWindowsPath(string path)
    {
        return Application.dataPath.Replace("Assets", path);
    }

    public static string GetUnityPath(string path)
    {
        return "Assets" + path.Replace(Application.dataPath, "");
    }

    public static string GetMD5HashFromFile(string filePath)
    {
        try
        {
            FileStream file = new FileStream(filePath, System.IO.FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }
}