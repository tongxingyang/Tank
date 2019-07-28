using System;

namespace Assets.Tools.Script.Event
{
    /// <summary>
    /// 无参数和object参数事件
    /// </summary>
    public class SimpleSignal
    {
        protected Action handler;
        protected Action<object> argHandler;

        protected object cacheArg;
        /// <summary>
        /// 添加一个无参数回调
        /// </summary>
        /// <param name="handler">回调</param>
        public void AddEventListener(Action handler)
        {
            this.handler -= handler;
            this.handler += handler;
        }
        /// <summary>
        /// 添加一个object参数回调（该类型回调只能添加1个）
        /// </summary>
        /// <param name="handler">回调</param>
        /// <param name="cacheArg">回调参数缓存</param>
        public void AddEventListener(Action<object> handler, object cacheArg)
        {
            this.argHandler -= handler;
            this.argHandler += handler;
            this.cacheArg = cacheArg;
        }
        /// <summary>
        /// 移除一个无参数回调
        /// </summary>
        /// <param name="handler">回调</param>
        public void RemoveEventListener(Action handler)
        {
            this.handler -= handler;
        }
        /// <summary>
        /// 清除回调
        /// </summary>
        public void Clear()
        {
            this.handler = null;
            this.argHandler = null;
        }
        /// <summary>
        /// 派发事件
        /// </summary>
        public void Dispatch()
        {
            if (cacheArg != null && argHandler != null)
                argHandler(cacheArg);
            else if (handler != null)
                handler();
        }

    }
}

