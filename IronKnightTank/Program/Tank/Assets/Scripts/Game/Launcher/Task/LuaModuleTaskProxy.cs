// ----------------------------------------------------------------------------
// <copyright file="LuaModuleTask.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>13/04/2018</date>
// ----------------------------------------------------------------------------


namespace Assets.Scripts.Game.Launcher.Task
{
    using System;

	using XQFramework.Laucher;

    using LuaInterface;

    using UnityEngine;

    public class LuaModuleTaskProxy : ILanucherTask
    {
        private LuaTable luaModule;

        private int weight;


        public int Weight { get
        {
            return weight;
        } }

        public Action<ILanucherTask, float, string> SetTaskProgress { get; set; }

        public void StartTask()
        {
            var luaFunction = this.luaModule.GetLuaFunction("StartTask");
            luaFunction.Call();
            luaFunction.Dispose();
        }

        public LuaModuleTaskProxy(LuaTable luaModule)
        {
            this.luaModule = luaModule;
            
            luaModule["Proxy"] = this;
            var d = (double)luaModule["Weight"];
            this.weight = Convert.ToInt32(d);
        }

        public void Dispose()
        {
            this.luaModule["Proxy"] = null;
            this.luaModule.Dispose();
            this.luaModule = null;
        }

        public void SetProgress(float progress, string msg)
        {
            this.SetTaskProgress(this, progress, msg);
        }
    }
}