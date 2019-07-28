using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XQFramework;

public class AppConst
{
    public const bool LuaByteMode = false;                              //Lua打包成luaJit
    public const bool ABHashMode = true;                               //是否打包ABWithHashName
    public static bool DevMode = true;                                 //开发模式  只在Editor模式下有效o
    public static string ExtName = ".u3d";                              //AB扩展名
    public static string GameResourceRootDir = "Assets/GameResource/"; //资源根目录
    public static string LuaDir { get { return Application.dataPath.Replace("Assets", "") + "/" + GameResourceRootDir + "/Lua/Src"; } }
    public static bool isEncrypt = false;


    //	public const string AssetDir = "StreamingAssets";   //素材目录 
    //	public const string AppName = "Knight";//应用程序名称
    //	public const string WebUrl = "http://localhost:6688/";      //测试更新地址
    //	public static string UserId = string.Empty;                 //用户ID
    //	public static int SocketPort = 0;                           //Socket服务器端口
    //	public static string SocketAddress = string.Empty;          //Socket服务器地址
    //	public const string LuaTempDir = "Lua/";                    //临时目录
    //	public const string AppPrefix = AppName + "_";              //应用程序前缀
    //	public const bool DebugMode = true;                       //调试模式-用于内部测试
    //	public const bool LuaByteMode = false;                       //Lua字节码模式-默认关闭 
    //	public const bool LuaBundleMode = false;                    //Lua代码AssetBundle模式
    //	public const int TimerInterval = 1;
    //	public const int GameFrameRate = 30;                        //游戏帧频
}