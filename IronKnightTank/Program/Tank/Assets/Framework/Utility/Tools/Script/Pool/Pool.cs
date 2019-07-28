namespace Assets.Script.Mvc.Pool
{
    public class Pool<T> : PoolBase where T : new()
    {
        new public T GetInstance()
        {
            return (T)base.GetInstance();
        }

        protected override object CreateObject()
        {
            var obj = new T();
            if (obj is IPoolable)
            {
                var poolable = obj as IPoolable;
                poolable.Pool = this;
            }
            return obj;
        }
    }
}