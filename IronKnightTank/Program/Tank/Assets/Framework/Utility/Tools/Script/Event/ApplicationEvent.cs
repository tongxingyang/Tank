using UnityEngine;

namespace Assets.Tools.Script.Event
{
    using System;

    /// <summary>
    /// 运行时的一些事件
    /// 使用需要挂挂到某个永远不会被destroy的gameobject上
    /// </summary>
    public class ApplicationEvent : MonoBehaviour
    {
        void Awake()
        {
            if (OnAwake != null)
            {
                OnAwake();
            }
        }

        /// <summary>
        /// 第一次被激活
        /// </summary>
        public static Action OnAwake;
        /// <summary>
        /// 应用重新获得焦点（移动平台）
        /// </summary>
        public static Action OnRestore;
        /// <summary>
        /// 应用失去焦点（移动平台）
        /// </summary>
        public static Action OnPause;
        /// <summary>
        /// 应用退出
        /// </summary>
        public static Action OnQuit;
        /// <summary>
        /// 点击Esc
        /// </summary>
        public static Action OnEsc;

        void OnApplicationQuit()
        {
            if (OnQuit != null)
            {
                OnQuit();
            }
        }

#if UNITY_ANDROID || UNITY_IPHONE
        void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                if (OnPause != null)
                {
                    OnPause();
                }
            }
            else
            {
                if (OnRestore != null)
                {
                    OnRestore();
                }
            }
        }
#endif

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (OnEsc != null)
                {
                    OnEsc();
                }
            }
        }
    }
}
