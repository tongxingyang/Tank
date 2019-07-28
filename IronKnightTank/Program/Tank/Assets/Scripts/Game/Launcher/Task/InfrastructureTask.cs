// ----------------------------------------------------------------------------
// <copyright file="InfrastructureTask.cs" company="上海序曲网络科技有限公司">
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
    using System.Collections;
    using XQFramework.Scene;
    using XQFramework.Laucher;
    using XQFramework.Resource;
    using Assets.Scripts.Game.Time;
    using Assets.Tools.Script.Caller;
    using UnityEngine;
    using XQFramework;
    using Assets.Scripts.Game.Input;

    /// <summary>
    /// 启动基础系统
    /// </summary>
    public class InfrastructureTask : ILanucherTask
    {
        private static bool loaded = false;

        public int Weight
        {
            get
            {
                return 1;
            }
        }

        public Action<ILanucherTask, float, string> SetTaskProgress { get; set; }

        public void StartTask()
        {
            if (!loaded)
            {
                CoroutineCall.Call(Init());
            }
            else
            {
                this.SetTaskProgress(this, 1, "");
            }
        }

        IEnumerator Init()
        {
            yield return null;
            FrameworkConst.Initialize(new ThreeKindomFrameworkConfig());
            UnitySceneManager.Initialize();
            TankTimeManager.Initialize();
            GlobalScene.CreateRoot("Input", Vector3.zero).AddComponent<InputManager>();
            GlobalScene.CreateRoot("Test", Vector3.zero).AddComponent<TestMgr>();
            yield return ResourcesManager.Instance.Initialize();
            this.SetTaskProgress(this, 1, "");
            loaded = true;
        }
    }
}