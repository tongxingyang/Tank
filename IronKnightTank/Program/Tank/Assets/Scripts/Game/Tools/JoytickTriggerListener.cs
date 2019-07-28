// ----------------------------------------------------------------------------
// <copyright file="UnitySceneManager.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>28/06/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Scripts.Game.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    /// <summary>
    /// EasyTouch的摇杆监听 主要封装给lua用的 
    /// </summary>
    public class JoytickTriggerListener : MonoBehaviour
    {

        //ETCJoystick m_joystick;
        //private bool _isInit = false;

        //private System.Action<Vector2> _onMoveSpeedCallback = null;
        //// Use this for initialization
        //void Start()
        //{
        //    m_joystick = GetComponent<ETCJoystick>();
        //    if (m_joystick == null)
        //    {
        //        Debug.Log("no ETCJoystick");
        //    }
        //    else
        //    {
        //        _isInit = true;
        //        m_joystick.onMoveSpeed.AddListener(v => {
        //            if (_onMoveSpeedCallback != null)
        //            {
        //                _onMoveSpeedCallback(v);
        //            }
        //        });
        //    }
        
        //}

        //public void AddOnMoveSpeedListener(System.Action<Vector2> moveSpeedCallBack)
        //{
        //    this._onMoveSpeedCallback = moveSpeedCallBack;

        //}
    }

}
