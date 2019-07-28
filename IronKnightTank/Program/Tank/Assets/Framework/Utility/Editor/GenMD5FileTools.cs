using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class GenMD5FileTools
{

    /// <summary>
    /// 生成文件夹下面所有的MD5文件
    /// </summary>
    /// <param name="dirPath">Dir path.</param>
    /// <param name="fileName">File name.</param>
    public static void GenMD5File(string dirPath, string txtPath, string[] filterPatterns = null)
    {
        Dictionary<string, string> md5Dic = new Dictionary<string, string>();
        EditorUtility.DisplayProgressBar("生成MD5文件" + dirPath, "生成MD5文件", 0f);
        string[] files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories).Where(s =>
        {
            if (filterPatterns != null)
            {
                return !filterPatterns.Contains(Path.GetExtension(s));
            }
            return true;
        }).ToArray();
        StringBuilder stringBuilder = new StringBuilder();
        string parentName = dirPath.Substring(dirPath.LastIndexOf("/"));
        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            EditorUtility.DisplayProgressBar("生成MD5文件" + dirPath, "生成MD5文件", (float)i / (float)files.Length);
            string md5 = EditorTools.GetMD5HashFromFile(filePath);
            string fileName = filePath.Substring(filePath.LastIndexOf(parentName + "\\") + (parentName + "\\").Length);
            md5Dic[fileName] = md5;
            stringBuilder.AppendLine(string.Format("{0}|{1}", fileName, md5));
        }

        using (FileStream fs = new FileStream(txtPath, FileMode.OpenOrCreate))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                string str = stringBuilder.ToString();
                str = str.Replace("\\", "/");
                sw.Write(str);
            }
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        Debug.Log("生成md5 完成 dir :" + dirPath + "  txtPath :" + txtPath);
    }
}
