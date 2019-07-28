// ----------------------------------------------------------------------------
// <copyright file="UnitySceneManager.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>19/06/2016</date>
// ----------------------------------------------------------------------------

namespace Assets.Scripts.Game.LuaTools
{
    using Assets.Scripts.Game.Tools;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;
    /// <summary>
    /// lua工具类 
    /// 封装了一些常用方法 方便lua调用
    /// </summary>
    public static class LuaTools
    {
        public static void SliderTween(Slider slider , float val , float time , System.Action onComplete = null)
        {
            // SliderTween sliderTween = slider.GetComponent<SliderTween>();
            // if(sliderTween == null)
            // {
            //     sliderTween = slider.gameObject.AddComponent<SliderTween>();
            // }
            // sliderTween.ChangeSliderValue(val, time, onComplete);
            Tweener tweener = slider.DOSlider(val, time);
            tweener.OnComplete(
                () =>{

                        if (onComplete != null)
                        {
                            onComplete();
                        }

                    });
        }

        public static Vector3 LuaTransferWorldPos2UIWorldPos(Vector3 worldPos , RectTransform rectTran)
        {
            return UGUITools.TransferWorldPos2UIWorldPos(worldPos, rectTran);
        }

        public static Vector2 LuaTransferWorldPos2UILocalPos(Vector3 worldPos , RectTransform rectTran)
        {
            return UGUITools.TransferWorldPos2UILocalPos(worldPos, rectTran);
        }

        public static Vector2 LuaGetUIScreenPos(RectTransform rectTran)
        {
            return UGUITools.GetUIScreenPos(rectTran);
        }

        public static Tweener LuaSpriteDTFade(SpriteRenderer spriteRender , float val , float time , System.Action onComplete = null , System.Action onUpdate = null)
        {
            return spriteRender.DoFade(val, time, onComplete, onUpdate);
        }

        public static Tween LuaSliderDTTween(Slider slider , float val , float time, System.Action onComplete = null, System.Action onUpdate = null)
        {
            return slider.DoSlider(val, time, onComplete, onUpdate);
        }

        public static void SetTranParent(Transform transform , Transform parent)
        {
            transform.parent = parent;
        }

        public static void SetTranParentAndNormalized(Transform transform , Transform parent)
        {
            SetTranParent(transform, parent);
            transform.localScale = Vector3.zero;
            transform.localPosition = Vector3.zero;
        }
    }
}
