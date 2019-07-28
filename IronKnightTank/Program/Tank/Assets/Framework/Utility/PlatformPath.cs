using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPath
{

    /// <summary>
    /// assetBundle解压目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            string game = XQFramework.FrameworkConst.AppName.ToLower();
            if (Application.isMobilePlatform || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return Application.persistentDataPath + "/" + game + "/";
            }

            //                if (AppConst.DebugMode) {
            //                    return Application.dataPath + "/" + AppConst.AssetDir + "/";
            //                }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                int i = Application.dataPath.LastIndexOf('/');
                return Application.dataPath.Substring(0, i + 1) + game + "/";
            }
            return "c:/" + game + "/";
        }
    }

    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.streamingAssetsPath + "/";
                break;
        }
        return path;
    }
}
