using System;
using Assets.Tools.Script.Core;
using Assets.Tools.Script.Serialized.LocalCache;
using UnityEngine;

namespace Assets.Tools.Script.Debug.Log
{
    public class LogFile
    {
        /// <summary>
        /// 在主线程的下一帧追加内容到文件
        /// </summary>
        /// <param name="content">追加的内容</param>
        /// <param name="fileName">文件名，无需后缀名</param>
        public static void TxtFile(string content, string fileName = "log")
        {
            try
            {
                Loom.QueueOnMainThread(() =>
                {
                    try
                    {
                        LcStringFile cache = new LcStringFile(fileName);
                        content = "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + "\r\n" + content;
                        DebugConsole.Log(content);
                        if (cache.HasCache())
                        {
                            string value = cache.Value;
                            cache.Value = value + content;
                        }
                        else
                        {
                            cache.Value = content;
                        }
                    }
                    catch (Exception e)
                    {
                    }

                });
            }
            catch (Exception) { }
        }
        /// <summary>
        /// 获得文件内容
        /// </summary>
        /// <param name="fileName">文件名，无需后缀名</param>
        /// <returns></returns>
        public static string GetFile(string fileName)
        {
            LcStringFile cache = new LcStringFile(fileName);
            return cache.Value;
        }
        /// <summary>
        /// 保存png截图
        /// </summary>
        /// <param name="fileName">文件名，无需后缀名</param>
        public static void Screenshot(string fileName)
        {
            try
            {
                Loom.QueueOnMainThread(() =>
                {
                    try
                    {
                        ScreenCapture.CaptureScreenshot("Log_" + fileName + "_" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") +
                                                ".png");
                    }
                    catch (Exception e)
                    {
                    }

                });
            }
            catch (Exception) { }
        }
    }
}