// ----------------------------------------------------------------------------
// <copyright file="UnitySceneManager.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>06/07/2016</date>
// ----------------------------------------------------------------------------

namespace Assets.Scripts.Game.Tools
{
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using DG.Tweening.Core;
    using DG.Tweening.Plugins.Options;

    /// <summary>
    /// DT的api有点老了，而且不是很全，所以自己封装了一些拓展方法
    /// </summary>
    public static class DTExtendClass
    {
        /// <summary>
        /// sprite 缓动透明
        /// </summary>
        /// <param name="target"></param>
        /// <param name="endVal"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Tweener DOFade(this SpriteRenderer target , float endVal , float time )
        {

            return DOTween.ToAlpha(() => target.color, delegate (Color x)
           {
               target.color = x;
           }, endVal, time);
        }

        /// <summary>
        /// slider 缓动改变val
        /// </summary>
        /// <param name="target"></param>
        /// <param name="endVal"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Tweener DOSlider(this Slider target , float endVal , float time)
        {
            return DOTween.To(() => target.value, delegate (float x)
             {
                 target.value = x;
             }, endVal, time);
        }
        
    }
}