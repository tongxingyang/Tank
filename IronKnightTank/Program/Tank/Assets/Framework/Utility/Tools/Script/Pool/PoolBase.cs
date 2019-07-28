using System.Collections.Generic;

namespace Assets.Script.Mvc.Pool
{
    public abstract class PoolBase:IPool
    {
        public int MaxCount { get; set; }
        protected Stack<object> AvailableObjects = new Stack<object>();

        public object GetInstance()
        {
            //            AvailableObjects.Count;
            //            AvailableObjects.Clear();
            //            AvailableObjects.Peek();
            //            AvailableObjects.Pop();
//            AvailableObjects.Push();
            if (AvailableObjects.Count > 0)
            {
                return AvailableObjects.Pop();
            }
            return CreateObject();
        }

        /// <summary>
        /// 还回对象，并返回是否还回成功
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if return instance succeed, <c>false</c> otherwise.</returns>
        public virtual bool ReturnInstance(object obj)
        {
            if (AvailableObjects.Count < MaxCount)
            {
                if (obj is IPoolable)
                {
                    (obj as IPoolable).Restore();
                }
                AvailableObjects.Push(obj);
                return true;
            }
            return false;
        }

        public virtual void Clear()
        {
            AvailableObjects.Clear();
        }

        protected abstract object CreateObject();
    }
}