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

using Assets.Scripts.Game.Tools;

namespace Assets.Scripts.Game.LuaTools
{
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.UI;

    public static class DTLuaTools
    {
        /// <summary>
        /// lua调用Material fade  
        /// onUpdate会每帧调用造成很大开销请慎用
        /// </summary> 
        /// <param name="sprite"></param>
        /// <param name="endVal"></param>
        /// <param name="time"></param>
        /// <param name="onComplete"></param>
        /// <param name="onUpdate"></param>
        /// <returns></returns> Tweener
        public static Tweener DTFade(this Material mat, float endVal, float time, System.Action onComplete = null, System.Action onUpdate = null)
        {
            var tweener = mat.DOFade(endVal, time);
            tweener.OnComplete(() =>
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            });
            tweener.OnUpdate(() =>
            {
                if (onUpdate != null)
                {
                    onUpdate();
                }
            });
            return tweener;
        }

        /// <summary>
        /// lua调用sprite fade  
        /// onUpdate会每帧调用造成很大开销请慎用
        /// </summary> 
        /// <param name="sprite"></param>
        /// <param name="endVal"></param>
        /// <param name="time"></param>
        /// <param name="onComplete"></param>
        /// <param name="onUpdate"></param>
        /// <returns></returns> Tweener
        public static Tweener DoFade(this SpriteRenderer sprite, float endVal, float time, System.Action onComplete = null, System.Action onUpdate = null)
        {
            var tweener = sprite.DOFade(endVal, time);
            tweener.OnComplete(() =>
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            });

            tweener.OnUpdate(() =>
            {
                if (onUpdate != null)
                {
                    onUpdate();
                }
            });
            return tweener;
        }

        /// <summary>
        /// lua 调用 slider 差值缓动改变数值
        /// </summary>
        /// <param name="slider"></param>
        /// <param name="endVal"></param>
        /// <param name="time"></param>
        /// <param name="onComplete"></param>
        /// <param name="onUpdate"></param>
        /// <returns></returns>
        public static Tweener DoSlider(this Slider slider, float endVal, float time, System.Action onComplete = null, System.Action onUpdate = null)
        {
            var tweener = slider.DOSlider(endVal, time);
            tweener.OnComplete(() =>
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            });

            tweener.OnUpdate(() =>
            {
                if (onUpdate != null)
                {
                    onUpdate();
                }
            });
            return tweener;
        }
    }
}