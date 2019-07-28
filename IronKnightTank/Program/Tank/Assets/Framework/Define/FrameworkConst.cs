using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XQFramework
{
    public static class FrameworkConst
    {
        public static string FrameworkRootDir = Application.dataPath + "/Framework";
        public static string AppName = "Knight";
        public static string ToLuaDir = FrameworkRootDir + "/ToLua/Lua";
        public static string CustomLuaDir = "";
        public static bool DebugMode = true;
        public static bool ABHashMode = true;  //是否打包ABWithHashName
        public static string GameResourceRootDir = "Assets/GameResource/"; //资源根目录

        public static void Initialize(IFrameworkConfig config)
        {
            ABHashMode = config.ABHashMode;
            DebugMode = config.DebugMode;
            GameResourceRootDir = config.GameResourceRootDir;
            CustomLuaDir = config.CustomLuaDir;
        }
    }
}