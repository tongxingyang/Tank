using System;
using System.Collections;
using Assets.Tools.Script.Go;
using UnityEngine;

namespace Assets.Tools.Script.Caller
{
    using Assets.Script.Mvc.Pool;

    public class CoroutineCall : MonoBehaviour
    {
        /// <summary>
        /// The pool
        /// </summary>
        private static CoroutineCallPool pool = new CoroutineCallPool();

        /// <summary>
        /// 可以运行一段协同代码
        /// </summary>
        /// <param name="a">协同方法</param>
		public static void Call(Func<IEnumerator> a)
        {
            CoroutineCall coroutineCall = pool.GetInstance();
            coroutineCall.enabled = true;
            coroutineCall.a = a;
            coroutineCall.StartCoroutine(coroutineCall.CallCoroutine());

        }

        public static Coroutine Call(IEnumerator a)
        {
            CoroutineCall call = pool.GetInstance();
            call.enabled = true;
            call.ienu = a;
            return call.StartCoroutine(call.CallIEnumtor());
        }


        private Func<IEnumerator> a;
        private IEnumerator ienu;

        IEnumerator CallCoroutine()
        {
            yield return StartCoroutine(a());
            var returnSucceed = pool.ReturnInstance(this);
            if (!returnSucceed)
            {
                Destroy(this);
            }
        }

        IEnumerator CallIEnumtor()
        {
            yield return StartCoroutine(ienu);
            var returnSucceed = pool.ReturnInstance(this);
            if (!returnSucceed)
            {
                Destroy(this);
            }
        }

        private class CoroutineCallPool : Pool<CoroutineCall>
        {
            public CoroutineCallPool()
            {
                this.MaxCount = 10;
            }

            protected override object CreateObject()
            {
                return ParasiticComponent.parasiteHost.AddComponent<CoroutineCall>();
            }
        }
    }
}
