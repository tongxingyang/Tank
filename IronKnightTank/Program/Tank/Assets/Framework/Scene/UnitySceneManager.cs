// ----------------------------------------------------------------------------
// <copyright file="UnitySceneManager.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>28/04/2016</date>
// ----------------------------------------------------------------------------

namespace XQFramework.Scene
{
    using System;
    using System.Collections;

    using Assets.Tools.Script.Caller;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// 场景加载管理
    /// 加载过程为异步，卸载当前场景->GC->加载新场景
    /// </summary>
    public class UnitySceneManager
    {
        private static string loadTo;
        private static Action onLoadComplete;

        public static void Initialize()
        {
            
        }

        /// <summary>
        /// 获取当前场景名字
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        /// <param name="onComplete">The on complete.</param>
        public static void SwitchScene(string sceneName, Action onComplete)
        {
            loadTo = sceneName;
            onLoadComplete = onComplete;
            CoroutineCall.Call(switchScene);
        }

        /// <summary>
        /// 重新加载当前场景
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        /// <param name="onComplete">The on complete.</param>
        public static void ReloadScene(string sceneName, Action onComplete)
        {
            SwitchScene(GetActiveSceneName(), onComplete);
        }
        
        public static void GC()
        {
            //TODO:how to gc
        }

        private static IEnumerator switchScene()
        {
            //卸载
            var sceneAsync = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            yield return sceneAsync;

            GC();

            //加载
            sceneAsync = SceneManager.LoadSceneAsync(loadTo,LoadSceneMode.Additive);
            yield return sceneAsync;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadTo));

            var loadComplete = onLoadComplete;
            onLoadComplete = null;
            loadTo = null;
            if (loadComplete != null)
            {
                loadComplete();
            }
        }
    }
}