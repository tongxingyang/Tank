// ----------------------------------------------------------------------------
// <copyright file="DebugGuard.cs" company="上海序曲网络科技有限公司">
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

    using UnityEngine;

    public class DebugGuard : MonoBehaviour
    {
        public static List<DebugGuard> DebugGuards = new List<DebugGuard>();

        public GuradType Type;

        public GameObject Target;

        private void Awake()
        {
            this.UpdateEnable();
        }

        private void Start()
        {
            DebugGuards.Add(this);
        }

        private void OnDestroy()
        {
            DebugGuards.Remove(this);
        }

        public void UpdateEnable()
        {
#if UNITY_EDITOR
            if (Type != GuradType.NeverUse)
            {
                this.Target.SetActive(true);
            }
#else
            if (Type == GuradType.Always)
            {
                PlayerPrefs.SetInt("gDebugGuard", 1);
                PlayerPrefs.Save();
                this.Target.SetActive(true);
            }
            else if(Type == GuradType.UsePassword)
            {
                this.Target.SetActive(PlayerPrefs.HasKey("gDebugGuard"));
            }
            else if(Type == GuradType.NeverUse)
            {
                this.Target.SetActive(false);
            }
#endif
        }

        public enum GuradType
        {
            Always,
            UsePassword,
            NeverUse,
        }
    }
}