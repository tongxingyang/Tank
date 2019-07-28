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
    using UnityEngine.UI;
    /// <summary>
    /// 类似劲舞团那种循环往复滚动的slider 需要的时候挂在slide上面就可以
    /// 计划支持loopType
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class LoopSlider : MonoBehaviour
    {

        public Text timeText;

        private Slider m_slider;
        //移动速度
        private float speed;
        //是否移动
        private bool isMove = false;
        //是否显示时间
        private bool isShowTime = false;
        //时间
        private float time;
        private float startTIme;
        private float passTime;
        //移动类型
        private ESliderMoveType moveType = ESliderMoveType.Up;

        // Use this for initialization
        void Start()
        {
            m_slider = GetComponent<Slider>();
            
        }

        
        private void Update()
        {
            if (isMove && time > 0)
            {
                float n = Time.time - this.startTIme ;
                float content = n * speed;
                passTime += Time.deltaTime;
                float val = (content % 2);
                val = val > 1 ? (2 - val) : val;
                m_slider.value = val;
                if(n > this.time)
                {
                    isMove = false;
                }
                if (isShowTime)
                {
                    timeText.text = (this.time - n).ToString("0.0");
                }
            }
        }

        
        

        public void Restart(float speed, float time, bool isShowTime = false)
        {
            if (m_slider == null)
            {
                m_slider = GetComponent<Slider>();
            }
            if(m_slider == null)
            {
                return;
            }
            m_slider.value = 0;
            isMove = true;
            this.speed = speed;
            this.time = time;
            this.isShowTime = isShowTime;
            this.startTIme = Time.time;
            passTime = 0;
        }

        private enum ESliderMoveType
        {
            Up = 1,
            Down = 2,
        }

        public void End()
        {
            isMove = false;
        }
    }
}
