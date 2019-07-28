// ----------------------------------------------------------------------------
// <copyright file="UnitySceneManager.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>31/05/2016</date>
// ----------------------------------------------------------------------------

namespace Assets.Scripts.Game.Time
{
    using XQFramework.Time;

    public class TankTimeManager : TimeManager
    {
        private static TankTimeManager _instance;

        public static void Initialize()
        {
            _instance = new TankTimeManager();
            //_instance.RegisterAction(new TweenTimeAction());
            _instance.RegisterAction(new UnityTimeAction());
        }

        public static void ChangeTimeScale(float timeScale)
        {
            if (_instance != null)
            {
                _instance.ChangeScale(timeScale);
            }
        }
    }
}