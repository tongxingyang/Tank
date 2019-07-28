// ----------------------------------------------------------------------------
// <copyright file="GlobalObjectTask.cs" company="上海序曲网络科技有限公司">
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
    using XQFramework.Scene;

    using UnityEngine;

    /// <summary>
    /// 加载GlobalObject
    /// </summary>
    public class GlobalObjectTask : ILanucherTask
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
                var gameObject = Resources.Load<GameObject>("GlobalObject");
                var instantiate = GameObject.Instantiate(gameObject);
                instantiate.name = "GlobalObject";
                GlobalScene.Add(instantiate);
                loaded = true;
            }
            this.SetTaskProgress(this, 1, "Global context loaded.");
        }
    }
}