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
using XQFramework.Laucher;
using XQFramework.Lua;


namespace Assets.Scripts.Game.Launcher.Task
{
    using System;
    using System.Collections.Generic;

    using XQFramework;

    using LuaInterface;

    using UnityEngine;

    public class LuaHandoverTask : ILanucherTask
    {
        private AppLanucher lanucher;

        private List<LuaModuleTaskProxy> tasks = new List<LuaModuleTaskProxy>();

        public int Weight
        {
            get
            {
                return 30;
            }
        }

        public Action<ILanucherTask, float, string> SetTaskProgress { get; set; }

        public void StartTask()
        {
            this.lanucher = new AppLanucher();
            this.lanucher.AddProgressListener(this.OnProgress);
            this.lanucher.AddFinishListener(this.OnFinish);

            LuaManager.Instance.DoFile("Main.lua");
            var main = LuaManager.Instance.GetFunction("main");

            main.Call(this);
            main.Dispose();
            main = null;

            //this.lanucher.Lanuch();
        }

        private void OnFinish()
        {
            this.SetTaskProgress(this, 1, "初始化完成");
            this.lanucher = null;
            foreach (var luaModuleTaskProxy in this.tasks)
            {
                luaModuleTaskProxy.Dispose();
            }
            this.tasks = null;
        }

        private void OnProgress(float arg1, string arg2)
        {
            this.SetTaskProgress(this, arg1 - 0.01f, arg2);
        }

        public void AddTask(LuaTable table)
        {
            var luaModuleTaskProxy = new LuaModuleTaskProxy(table);
            this.tasks.Add(luaModuleTaskProxy);
            this.lanucher.AddTask(luaModuleTaskProxy);
        }

        public void Lanuch()
        {
            this.lanucher.Lanuch();
        }
    }
}