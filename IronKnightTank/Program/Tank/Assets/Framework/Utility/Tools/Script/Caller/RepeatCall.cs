using System;
using Assets.Tools.Script.Go;
using UnityEngine;

namespace Assets.Tools.Script.Caller
{
    using Assets.Script.Mvc.Pool;

    /// <summary>
    /// 定时调用
    /// </summary>
    public class RepeatCall : MonoBehaviour
    {
        private static RepeatCallPool pool = new RepeatCallPool();

        /// <summary>
        /// 重复调用，直到被调用方法返回false
        /// </summary>
        /// <param name="a">被调用方法</param>
        /// <param name="delay">第一次调用延时时间</param>
        /// <param name="repateRate">开始调用后的间隔</param>
        public static void Call(Func<bool> a, float delay, float repateRate)
        {
            var addComponent = pool.GetInstance();
            addComponent.enabled = true;
            addComponent.CallAction(a, delay, repateRate);
        }

        /// <summary>
        /// 重复调用，直到被调用方法返回false
        /// </summary>
        /// <param name="a">被调用方法</param>
        /// <param name="delay">第一次调用延时时间</param>
        /// <param name="repateRate">开始调用后的间隔</param>
        public static RepeatCall CreateCall(Func<bool> a, float delay, float repateRate)
        {
            var addComponent = ParasiticComponent.parasiteHost.AddComponent<RepeatCall>();
            addComponent.CallAction(a, delay, repateRate);
            addComponent.isCreateInstance = true;
            return addComponent;
        }

        private Func<bool> _delayCall;

        /// <summary>
        /// 是一个创建出来的实例
        /// </summary>
        private bool isCreateInstance;

        /// <summary>
        /// 开始执行重复调用，直到被调用方法返回false
        /// </summary>
        /// <param name="a">被调用方法</param>
        /// <param name="delay">第一次调用延时时间</param>
        /// <param name="repateRate">开始调用后的间隔</param>
        private void CallAction(Func<bool> a, float delay, float repateRate)
        {
            _delayCall = a;
            InvokeRepeating("CallBack", delay, repateRate);
        }
        /// <summary>
        /// 停止销毁
        /// </summary>
        public void Stop()
        {
            Dispose();
        }

        private void CallBack()
        {
            if (_delayCall==null || !_delayCall())
            {
                Dispose();
            }
        }

        private void Dispose()
        {
            CancelInvoke("CallBack");
            if (isCreateInstance)
            {
                Destroy(this);
            }
            else
            {
                this._delayCall = null;
                this.enabled = false;
                var returnSucceed = pool.ReturnInstance(this);
                if (!returnSucceed)
                {
                    Destroy(this);
                }
            }
        }

        private class RepeatCallPool : Pool<RepeatCall>
        {
            public RepeatCallPool()
            {
                this.MaxCount = 10;
            }

            protected override object CreateObject()
            {
                return ParasiticComponent.parasiteHost.AddComponent<RepeatCall>();
            }
        }
    }
}
