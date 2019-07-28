// ----------------------------------------------------------------------------
// <copyright file="DebugGuradPassword.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>14/12/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Scripts.Tools.Debug
{
    using System.Collections.Generic;

    using Assets.Scripts.Game;

    using UnityEngine;

    public class DebugGuradPassword : MonoBehaviour
    {
        /// <summary>
        /// 密码序列，true代表长按，false代表短按
        /// </summary>
        public bool[] Password;

        private List<bool> currPassword = new List<bool>();

        private float pressTime;

        private void Start()
        {
            DebugConsole.AddButton("去密码",
                () =>
                {
                    PlayerPrefs.DeleteKey("gDebugGuard");
                    PlayerPrefs.Save();
                });
        }

        private void OnDestroy()
        {
            DebugConsole.RemoveButton("去密码");
        }

        public void OnPress(bool isdown)
        {
            if (isdown)
            {
                this.pressTime = Time.time;
            }
            else
            {
                this.currPassword.Add((Time.time - this.pressTime) > 0.5f);
            }
        }

        private void Update()
        {
            var b2 = Input.GetMouseButton(2);

            if (b2)
            {
                bool pass = true;
                if (Password != null && Password.Length == currPassword.Count)
                {
                    for (int i = 0; i < this.Password.Length; i++)
                    {
                        if (this.Password[i] != this.currPassword[i])
                        {
                            pass = false;
                            break;
                        }
                    }
                }
                else
                {
                    pass = false;
                }
                if (pass)
                {
                    PlayerPrefs.SetInt("gDebugGuard", 1);
                    PlayerPrefs.Save();
                    foreach (var debugGuard in DebugGuard.DebugGuards)
                    {
                        if (debugGuard != null)
                        {
                            debugGuard.UpdateEnable();
                        }
                    }

                    this.gameObject.SetActive(false);
                }

                this.currPassword.Clear();
            }
        }

    }
}