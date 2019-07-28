// ----------------------------------------------------------------------------
// <copyright file="UnitySceneManager.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>30/06/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Scripts.Game.Tools
{
    using UnityEngine;

    /// <summary>
    /// ui固定在世界坐标上面 
    /// 挂在想要固定的ui上并设置目标就行了
    /// </summary>
    public class UIWorldPosFollower : MonoBehaviour
    {

        public Vector3 targetPos;
        private RectTransform m_rectTran;

        private void Start()
        {
            m_rectTran = transform as RectTransform;
        }

        private void Update()
        {
            Vector3 pos = UGUITools.TransferWorldPos2UIWorldPos(targetPos, m_rectTran);
            m_rectTran.position = pos;
        }

        public void SetPos(Vector3 pos)
        {
            this.targetPos = pos;
        }
    }
}