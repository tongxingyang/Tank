// ----------------------------------------------------------------------------
// <copyright file="GameLanucher.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>13/04/2018</date>
// ----------------------------------------------------------------------------
using XQFramework;
using XQFramework.Laucher;


namespace Assets.Scripts.Game.Launcher
{
    using Assets.Scripts.Game.Launcher.Task;

    using UnityEngine;
    using DG.Tweening;

    public class GameLanucher : MonoBehaviour
    {
        private static AppLanucher appLanucher = new AppLanucher();
        string str = "";
        private bool isShowLog = false;
        public void Awake()
        {
            Application.runInBackground = true;
            Application.targetFrameRate = 60;
            DOTween.defaultEaseType = Ease.Linear;
            isShowLog = true;

            if (!appLanucher.IsLanuchComplete)
            {
                appLanucher.AddTask<GlobalObjectTask>();//GlobalObject初始化
                appLanucher.AddTask<ExtractFileTask>();//解压文件  
                appLanucher.AddTask<InfrastructureTask>();//基础系统启动
                appLanucher.AddTask<SetupLuaVirtualMachineTask>();//启动Lua
                appLanucher.AddTask<LuaHandoverTask>();//Lua方面的启动工作
                appLanucher.AddProgressListener(this.OnProgress);
                appLanucher.AddFinishListener(this.OnFinish);
                appLanucher.Lanuch();
            }
        }

        private void OnFinish()
        {
            isShowLog = false;
        }

        private void OnProgress(float arg1, string arg2)
        {
            string content = "AppLanucher.OnProgress " + arg1 + " " + arg2;
            str = content;
            //Debug.Log("AppLanucher.OnProgress " + arg1+ " " + arg2);
        }

        private void OnGUI()
        {
            if (isShowLog)
            {
                int width = Screen.width;
                int height = Screen.height;
                GUI.TextField(new Rect(width / 2 - 400, height / 2, 800, 200), str, new GUIStyle() { fontSize = 24 });
            }
        }
    }
}