﻿using System;

namespace Assets.Tools.Script.Event
{
    /// <summary>
    /// 带参数事件
    /// </summary>
    /// <typeparam name="T">事件参数类型</typeparam>
    public class Signal<T>
    {
        protected Action<T> handler;
        /// <summary>
        /// 添加事件回调
        /// </summary>
        /// <param name="handler">回调</param>
        public void AddEventListener(Action<T> handler)
        {
            this.handler -= handler;
            this.handler += handler;
        }
        /// <summary>
        /// 移除事件回调
        /// </summary>
        /// <param name="handler">回调</param>
        public void RemoveEventListener(Action<T> handler)
        {
            this.handler -= handler;
        }
        /// <summary>
        /// 清除回调
        /// </summary>
        public void Clear()
        {
            handler = null;
        }
        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="arg">事件参数</param>
        public void Dispatch(T arg)
        {
            if (handler != null)
                handler(arg);
        }
    }

    /// <summary>
    /// 事件
    /// </summary>
    /// <typeparam name="T1">事件参数1类型</typeparam>
    /// <typeparam name="T2">事件参数2类型</typeparam>
    public class Signal<T1, T2>
    {
        protected Action<T1, T2> handler;
        /// <summary>
        /// 添加事件回调
        /// </summary>
        /// <param name="handler">事件回调</param>
        public void AddEventListener(Action<T1, T2> handler)
        {
            this.handler -= handler;
            this.handler += handler;
        }
        /// <summary>
        /// 移除事件回调
        /// </summary>
        /// <param name="handler">事件回调</param>
        public void RemoveEventListener(Action<T1, T2> handler)
        {
            this.handler -= handler;
        }
        /// <summary>
        /// 清除回调
        /// </summary>
        public void Clear()
        {
            handler = null;
        }
        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="arg">参数1</param>
        /// <param name="arg2">参数2</param>
        public void Dispatch(T1 arg, T2 arg2)
        {
            if (handler != null)
                handler(arg, arg2);
        }
    }
}