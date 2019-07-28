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
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// UGUI的一些工具类  目前包含坐标转换 
    /// </summary>
    public class UGUITools
    {

        private static Camera _uiCamera;
        private static Camera _battleFieldCamera;
        public static Vector3 TransferWorldPos2UIWorldPos(Vector3 worldPos, RectTransform rectTran)
        {
            try
            {
                if (_uiCamera == null)
                {
                    _uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                }
                if (_battleFieldCamera == null)
                {
                    _battleFieldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                }
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(_battleFieldCamera, worldPos);
                Vector3 pos = Vector3.zero;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTran, screenPos, _uiCamera, out pos);
                return pos;
            }
            catch
            {
                throw new System.Exception("找不到UI相机或主相机，请添加UI相机tag为UICamera ， 主相机Tag为MainCamera");
            }
        }

        public static Vector2 TransferWorldPos2UILocalPos(Vector3 worldPos, RectTransform rectTran)
        {
            try
            {
                if (_uiCamera == null)
                {
                    _uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                }
                if (_battleFieldCamera == null)
                {
                    _battleFieldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                }
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(_battleFieldCamera, worldPos);
                Vector2 pos = Vector2.zero;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTran, screenPos, _uiCamera, out pos);
                return pos;
            }
            catch
            {
                throw new System.Exception("找不到UI相机或主相机，请添加UI相机tag为UICamera ， 主相机Tag为MainCamera");
            }
        }

        public static Vector2 GetUIScreenPos(RectTransform rectTran)
        {
            try
            {
                if (_uiCamera == null)
                {
                    _uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                }

                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(_uiCamera, rectTran.position);
                return screenPos;
            }
            catch
            {
                throw new System.Exception("找不到UI相机或主相机 ，请添加UI相机tag为UICamera ");
            }
        }

        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
