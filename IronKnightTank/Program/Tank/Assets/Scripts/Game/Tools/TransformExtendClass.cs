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

namespace Assets.Scripts.Game.Tools
{
    using UnityEngine;
    using DG.Tweening;
    /// <summary>
    /// transform拓展方法
    /// </summary>
    public static class TransformExtendClass
    {
        public static Tweener DTLocalMove(this Transform tran, Vector3 pos, float time, System.Action callBack , bool ignoreTimeScale = false , Ease tweenEase = Ease.Linear)
        {
            Tweener t = tran.DOLocalMove(pos, time);
            SetTween(t, callBack, ignoreTimeScale);
            return t;
        }

        public static Tweener DTMove(this Transform tran, Vector3 pos, float time, System.Action callBack , bool ignoreTimeScale = false, Ease tweenEase = Ease.Linear)
        {
            Tweener t = tran.DOMove(pos, time);
            SetTween(t, callBack, ignoreTimeScale);
            return t;
        }

        public static Tweener DTLocalRotate(this Transform tran, Vector3 angle, float time, System.Action callBack = null , bool ignoreTimeScale = false, Ease tweenEase = Ease.Linear)
        {
            Tweener t = tran.DOLocalRotate(angle, time);
            SetTween(t, callBack, ignoreTimeScale);
            return t;
        }

        public static Tweener DTRotate(this Transform tran, Vector3 angle, float time, System.Action callBack = null, bool ignoreTimeScale = false, Ease tweenEase = Ease.Linear)
        {
            Tweener t = tran.DORotate(angle, time);
            SetTween(t, callBack, ignoreTimeScale);
            return t;
        }

        public static Tweener DTCircle(this Transform tran,Vector3 angle,float time,System.Action callBack = null,bool ignoreTimeScale = false,Ease tweenEase = Ease.Linear)
        {
            Tweener t = tran.DORotate(angle, time , RotateMode.LocalAxisAdd);
            SetTween(t, callBack, ignoreTimeScale);
            return t;
        }

        private static void SetTween(Tweener t   , System.Action callBack = null, bool ignoreTimeScale = false, Ease tweenEase = Ease.Linear)
        {
            t.SetEase(tweenEase);
            if (callBack != null)
            {
                t.OnComplete(() =>
                {
                    callBack();
                });
            }
            if (ignoreTimeScale)
            {
                t.SetUpdate(ignoreTimeScale);
            }
            
            
        }
    }
}