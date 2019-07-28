// ----------------------------------------------------------------------------
// <copyright file="IOXResFileUtil.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>19/10/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.File
{
    using System.IO;

    using Assets.Tools.Script.Core.File;

    using UnityEngine;

    public class ResSafeFileUtil
    {
        public static string ReadFile(string path)
        {
#if UNITY_EDITOR
            return FileUtility.ReadFile(path);
#else
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            return textAsset.text;
#endif
        }

        public static bool DirectoryExists(string path)
        {
#if UNITY_EDITOR
            return Directory.Exists(path);
#else
            return true;
#endif
        }

        public static bool FileExists(string path)
        {
#if UNITY_EDITOR
            return File.Exists(path);
#else
            return true;
#endif
        }


        public static string GetSuffix(string suffix)
        {
#if UNITY_EDITOR
            return suffix;
#else
            return string.Empty;
#endif
        }
    }
}