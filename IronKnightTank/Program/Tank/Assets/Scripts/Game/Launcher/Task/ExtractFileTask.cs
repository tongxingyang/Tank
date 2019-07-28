// ----------------------------------------------------------------------------
// <copyright file="LuaHandoverTask.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>13/04/2018</date>
// ----------------------------------------------------------------------------
using System;
using XQFramework.Laucher;
using Assets.Scripts.Game.Tools;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;
using Assets.Tools.Script.Caller;

namespace Assets.Scripts.Game.Launcher.Task
{
    public class ExtractFileTask : ILanucherTask
    {
        public int Weight
        {
            get
            {
                return 50;
            }
        }
        Dictionary<string, string> m_oldHashDic = null;
        public Action<ILanucherTask, float, string> SetTaskProgress { get; set; }
        public void StartTask()
        {
            if (AppConst.DevMode)
            {
                SetTaskProgress(this, 1, "");
            }
            else
            {
                CoroutineCall.Call(CopyNeedFile());
            }
        }

        IEnumerator CopyNeedFile()
        {
            string dataPath = PlatformPath.DataPath;  //数据目录 目标目录
            string resPath = PlatformPath.AppContentPath(); //游戏包资源目录
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            string infile = resPath + "files.txt";
            string outfile = dataPath + "files.txt";
            if (File.Exists(outfile))
            {
                m_oldHashDic = new Dictionary<string, string>();
                string[] oldFiles = File.ReadAllLines(outfile);
                foreach (var file in oldFiles)
                {
                    string[] fs = file.Split('|');
                    if (m_oldHashDic.ContainsKey(fs[0]))
                    {
                        m_oldHashDic[fs[0]] = m_oldHashDic[fs[1]];
                    }
                    else
                    {
                        m_oldHashDic.Add(fs[0], fs[1]);
                    }

                }
                File.Delete(outfile);
            }
            string message = "正在解包文件:>files.txt";
            //Debug.Log(infile);
            //Debug.Log(outfile);

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else File.Copy(infile, outfile, true);
            yield return new WaitForEndOfFrame();

            //释放所有文件到数据目录
            string[] files = File.ReadAllLines(outfile);
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string[] fs = file.Split('|');
                infile = resPath + fs[0];  //
                outfile = dataPath + fs[0];
                infile = infile.Replace("\\", "/");
                outfile = outfile.Replace("\\", "/");
                message = "正在解包文件:>" + fs[0];
                Debug.Log("正在解包文件:>" + infile);

                bool b = CheckNeedCopy(outfile, fs[0], fs[1]);
                //Debug.Log ("need copy  path " + outfile + b.ToString ());
                //if (!b)
                //{
                //    //Debug.Log("file alread exit " + outfile);
                //}
                if (b)
                {
                    //Debug.Log("copy  file " + outfile);
                    string dir = Path.GetDirectoryName(outfile);
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    if (Application.platform == RuntimePlatform.Android)
                    {
                        WWW www = new WWW(infile);
                        yield return www;
                        if (!string.IsNullOrEmpty(www.error))
                        {
                            Debug.LogError(www.error);
                        }
                        else
                        {
                            if (www.isDone)
                            {
                                File.WriteAllBytes(outfile, www.bytes);
                            }
                            yield return 0;
                        }
                    }
                    else
                    {
                        if (File.Exists(outfile))
                        {
                            File.Delete(outfile);
                        }
                        File.Copy(infile, outfile, true);
                    }
                }
                if (SetTaskProgress != null)
                {
                    SetTaskProgress.Invoke(this, (float)(i + 1) / (float)files.Length, message);
                }
            }
            yield return new WaitForEndOfFrame();
        }

        private bool CheckNeedCopy(string path, string name, string newHash)
        {
            if (m_oldHashDic == null)
            {
                return true;
            }
            string hash = null;
            if (m_oldHashDic.TryGetValue(name, out hash))
            {
                if (newHash == hash)
                {
                    return !File.Exists(path);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}