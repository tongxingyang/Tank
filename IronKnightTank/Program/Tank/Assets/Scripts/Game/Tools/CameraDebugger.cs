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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 摄像机调试  提供给策划和美术调节相机位置和角度
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraDebugger : MonoBehaviour
    {
        private Camera m_cam;
        private bool isShowConfig = false;
        private int screenHeight = 0;
        private int screenWidth = 0;
        private GUIStyle style;
        private GUIStyle thumbStyle;
        private GUIStyle sliderStyle;
        private GUIStyle toggleStyle;
        private float angleX;
        private float angleY;
        private float angleZ;
        private float posX;
        private float posY;
        private float posZ;
        private Transform m_tran;
        private bool isOrthographic;

        private CameraMover move = null;

        int index = 0;
        // Use this for initialization
        void Start()
        {
            m_cam = GetComponent<Camera>();
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            move = GetComponent<CameraMover>();
            m_tran = transform;
        }


        private void OnGUI()
        {
            style = new GUIStyle(GUI.skin.toggle);
            thumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
            thumbStyle.fixedHeight = 80;
            thumbStyle.fixedWidth = 80;
            sliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
            sliderStyle.fixedHeight = 80;
            toggleStyle = new GUIStyle(GUI.skin.toggle);
            toggleStyle.fontSize = 32;
            GUI.skin.button.fontSize = 32;
            GUI.skin.label.fontSize = 32;

            if (GUILayout.Button("是否显示相机配置", GUILayout.Width(500), GUILayout.Height(100)))
            {
                isShowConfig = !isShowConfig;
                if (isShowConfig)
                {
                    angleX = m_tran.localEulerAngles.x;
                    angleY = m_tran.localEulerAngles.y;
                    angleZ = m_tran.localEulerAngles.z;
                    posX = m_tran.position.x;
                    posY = m_tran.position.y;
                    posZ = m_tran.position.z;
                    isOrthographic = m_cam.orthographic;
                    // move.Stop();
                }
                else
                {
                    // move.Restart();
                }
            }
            GUILayout.Space(30);
            if (isShowConfig)
            {


                index = GUILayout.Toolbar(index, new string[] { "投影", "位置", "旋转" }, GUILayout.Width(800), GUILayout.Height(100));
                if (index == 0)
                {
                    DrawnCamOrthographic();
                }
                else if (index == 1)
                {
                    DrawCamPosition();
                }
                else if (index == 2)
                {
                    DrawCamRotation();
                }
                else
                {
                    GUILayout.Label("未知的index");
                }
            }
        }



        private void LateUpdate()
        {
            if (isShowConfig)
            {
                m_tran.localEulerAngles = new Vector3(angleX, angleY, angleZ);
                m_tran.position = new Vector3(posX, posY, posZ);

            }
        }

        private void DrawCamRotation()
        {
            DrawSlider(ref angleX, 0, 360, "X : " + angleX.ToString());
            GUILayout.Space(20);
            DrawSlider(ref angleY, 0, 360, "Y : " + angleY.ToString());
            GUILayout.Space(20);
            DrawSlider(ref angleZ, 0, 360, "Z : " + angleZ.ToString());

        }

       


        private void DrawCamPosition()
        {
            DrawSlider(ref posX, -20, 30, "X : " + posX.ToString());
            GUILayout.Space(20);
            DrawSlider(ref posY, -20, 30, "Y : " + posY.ToString());
            GUILayout.Space(20);
            DrawSlider(ref posZ, -20, 30, "Z : " + posZ.ToString());

        }

        private void DrawnCamOrthographic()
        {
            GUILayout.Space(30);
            if (GUILayout.Button("相机投影方式" + (isOrthographic ? "正交" : "投影"), GUILayout.Width(300), GUILayout.Height(50)))
            {
                isOrthographic = !isOrthographic;
                m_cam.orthographic = isOrthographic;
            }
            if (m_cam.orthographic)
            {
                float x = m_cam.orthographicSize;
                this.DrawSlider(ref x, 1, 100, "Size: " + m_cam.orthographicSize.ToString());
                m_cam.orthographicSize = x;
            }
            else
            {
                float x = m_cam.fieldOfView;
                this.DrawSlider(ref x, 1, 100, "Fov: " + m_cam.fieldOfView.ToString());
                m_cam.fieldOfView = x;
            }
        }

        private void DrawSlider(ref float x, float min, float max, string des)
        {
            GUILayout.Label(des);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                x = GUILayout.HorizontalSlider(x, min, max, sliderStyle, thumbStyle, GUILayout.Width(800));
            }
            GUILayout.EndHorizontal();
        }
    }
}