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

namespace XQFramework.Time
{
    using System.Collections.Generic;
    public class TimeManager
    {
        private List<ITimeChangeAction> _actionList = new List<ITimeChangeAction>();

        public void ChangeScale(float timeScale)
        {
            for (int i = 0; i < _actionList.Count; i++)
            {
                _actionList[i].OnTimeChange(timeScale);
            }
        }

        public void RegisterAction(ITimeChangeAction action)
        {
            if (!_actionList.Contains(action))
            {
                _actionList.Add(action);
            }
        }

        public void RemoveAction(ITimeChangeAction action)
        {
            if (_actionList.Contains(action))
            {
                _actionList.Remove(action);
            }
        }

    }
}