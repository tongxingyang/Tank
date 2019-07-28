using System;
using Assets.Tools.Script.Debug.Console;
using Assets.Tools.Script.Go;
using UnityEngine;

namespace Assets.Tools.Script.Caller
{
    using Assets.Script.Mvc.Pool;

    /// <summary>
    /// 和帧有关的调用
    /// </summary>
    public class FrameCall : MonoBehaviour
    {
        private static FrameCallPool pool = new FrameCallPool();

        /// <summary>
        /// 延迟指定帧数后调用
        /// </summary>
        /// <param name="a">回调</param>
        /// <param name="delayFrame">延迟帧数</param>
        public static void DelayFrame(Action a, int delayFrame)
        {
            int currFrame = 0;
            FrameCall addComponent = pool.GetInstance();
            addComponent.enabled = true;
            addComponent.CallAction(() =>
            {
                bool b = ++currFrame < delayFrame;
                if (!b)
                {
                    a();
                }
                return b;
            });
        }
        /// <summary>
        /// 下一帧调用
        /// </summary>
        /// <param name="a">回调</param>
        public static void DelayFrame(Action a)
        {
            FrameCall addComponent = pool.GetInstance();
            addComponent.enabled = true;
            addComponent.CallAction(() =>
            {
                try
                {
                    a();
                }
                catch (Exception e)
                {
                    DebugConsole.Log(e) ;
                }
                return false;
            });
        }

        /// <summary>
        /// 每一帧都调用，直到返回false
        /// </summary>
        /// <param name="a"></param>
        public static void Call(Func<bool> a)
        {
            var frameCall = pool.GetInstance();
            frameCall.enabled = true;
            frameCall.CallAction(a);
        }

        /// <summary>
        /// 每一帧都调用，直到返回false
        /// </summary>
        /// <param name="a"></param>
        public static FrameCall CreateCall(Func<bool> a)
        {
            var frameCall = ParasiticComponent.parasiteHost.AddComponent<FrameCall>();
            frameCall.isCreateInstance = true;
            frameCall.CallAction(a);
            return frameCall;
        }

        private Func<bool> _delayCall;

        /// <summary>
        /// 是一个创建出来的实例
        /// </summary>
        private bool isCreateInstance;

        private void CallAction(Func<bool> a)
        {
            _delayCall = a;
        }
        /// <summary>
        /// 运行一次
        /// </summary>
        public void Run()
        {
            Update();
        }
        
        void Update()
        {
            if (_delayCall != null)
            {
                if (!_delayCall())
                {
                    Dispose();
                }
            }
        }

        private void Dispose()
        {
            if (this.isCreateInstance)
            {
                Destroy(this);
            }
            else
            {
                this.enabled = false;
                this._delayCall = null;
                var returnSucceed = pool.ReturnInstance(this);
                if (!returnSucceed)
                {
                    Destroy(this);
                }
            }
        }

        private class FrameCallPool : Pool<FrameCall>
        {
            public FrameCallPool()
            {
                this.MaxCount = 10;
            }

            protected override object CreateObject()
            {
                return ParasiticComponent.parasiteHost.AddComponent<FrameCall>();
            }
        }
    }
}
