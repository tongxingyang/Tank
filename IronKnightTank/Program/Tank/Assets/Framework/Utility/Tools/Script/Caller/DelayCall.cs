using System;
using Assets.Tools.Script.Go;
using UnityEngine;

namespace Assets.Tools.Script.Caller
{
    using Assets.Script.Mvc.Pool;

    /// <summary>
    /// 延时调用
    /// </summary>
    public class DelayCall : MonoBehaviour
    {
        /// <summary>
        /// 对象池
        /// </summary>
        private static DelayCallPool pool = new DelayCallPool();

        static DelayCall()
        {
            pool.MaxCount = 10;
        }

        /// <summary>
        /// 延时调用
        /// </summary>
        /// <param name="a">回调</param>
        /// <param name="delay">调用延时</param>
        /// <param name="ignoreTimeScale"></param>
        /// <returns></returns>
        public static DelayCall Call(Action a, float delay = 1, bool ignoreTimeScale = false)
        {
            var addComponent = pool.GetInstance();
            addComponent.delay = delay;
            addComponent.fun = a;
            addComponent.ignoreTimeScale = ignoreTimeScale;
            addComponent.enabled = true;
            addComponent.StartDelayCall();
            return addComponent;
        }

        /// <summary>
        /// 产生一个新的实例，并用于延时调用
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="ignoreTimeScale">if set to <c>true</c> [ignore time scale].</param>
        /// <returns>DelayCall.</returns>
        public static DelayCall CreateCall(Action a, float delay = 1, bool ignoreTimeScale = false)
        {
            var addComponent = ParasiticComponent.parasiteHost.AddComponent<DelayCall>();
            addComponent.delay = delay;
            addComponent.fun = a;
            addComponent.ignoreTimeScale = ignoreTimeScale;
            addComponent.StartDelayCall();
            addComponent.isCreateInstance = true;
            return addComponent;
        }

        /// <summary>
        /// 延时
        /// </summary>
        public float delay = 1;

        /// <summary>
        /// 回调
        /// </summary>
        public Action fun;

        /// <summary>
        /// 忽略时间缩放
        /// </summary>
        public bool ignoreTimeScale;

        /// <summary>
        /// 延时调用开始时间
        /// </summary>
        private DateTime startTime;

        /// <summary>
        /// 是一个创建出来的实例
        /// </summary>
        private bool isCreateInstance;

        /// <summary>
        /// 停止这个调用
        /// </summary>
        public void Stop()
        {
            this.Dispose();
        }

        /// <summary>
        /// 执行回调
        /// </summary>
        private void StartDelayCall()
        {
            if (!ignoreTimeScale)
            {
                Invoke("CallBack", delay);
            }
            else
            {
                startTime = DateTime.Now;
            }
        }
        /// <summary>
        /// 回调
        /// </summary>
        private void CallBack()
        {
            if (fun != null)
            {
                var callBack = fun;
                fun = null;
                callBack();
            }
            Dispose();
        }

        private void Dispose()
        {
            this.enabled = false;

            if (!ignoreTimeScale)
            {
                CancelInvoke("CallBack");
            }

            this.delay = 0;
            this.fun = null;
            this.ignoreTimeScale = false;

            if (isCreateInstance)
            {
                Destroy(this);
            }
            else
            {
                var returnSucceed = pool.ReturnInstance(this);
                if (!returnSucceed)
                {
                    Destroy(this);
                }
            }
        }

        private void Update()
        {
            if (ignoreTimeScale)
            {
                if ((DateTime.Now - startTime).TotalSeconds >= delay)
                {
                    CallBack();
                }
            }
        }

        private class DelayCallPool : Pool<DelayCall>
        {
            protected override object CreateObject()
            {
                var addComponent = ParasiticComponent.parasiteHost.AddComponent<DelayCall>();
                return addComponent;
            }
        }
    }
}
