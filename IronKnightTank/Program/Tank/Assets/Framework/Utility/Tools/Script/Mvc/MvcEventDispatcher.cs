using System;
using System.Collections.Generic;
using Assets.Script.Mvc.Pool;

namespace Assets.Script.Mvc
{
    public class MvcEventDispatcher : IEventDispatcher
    {
        private static Dictionary<Type, PoolBase> eventPools = new Dictionary<Type, PoolBase>();

        public static void ClearEventPools()
        {
            eventPools.Clear();
        }

        public static Pool<T> GetEventPool<T>() where T : IEvent, new()
        {
            PoolBase pool;
            Type type = typeof(Pool<T>);
            bool tryGetValue = eventPools.TryGetValue(type, out pool);
            if (!tryGetValue)
            {
                pool = new Pool<T>();
                pool.MaxCount = 2;
                eventPools.Add(type, pool);
            }
            return (Pool<T>) pool;
        }

        public static T GetPoolEventInstance<T>() where T : IEvent, new()
        {
            PoolBase pool = GetEventPool<T>();
            return (T)pool.GetInstance();
        }

        public static void ReturnPoolEventInstance(IEvent returnEvent)
        {
            PoolBase pool;
            bool tryGetValue = eventPools.TryGetValue(returnEvent.GetType(), out pool);
            if (tryGetValue)
            {
                pool.ReturnInstance(returnEvent);
            }
        }

        private Dictionary<string, Action<IEvent>> eventHandlers = new Dictionary<string, Action<IEvent>>();

        public void DispatchEvent(string eventName)
        {
            Action<IEvent> handler;
            eventHandlers.TryGetValue(eventName, out handler);
            if (handler != null)
            {
                Pool<Event> eventPool = GetEventPool<Event>();
                var e = eventPool.GetInstance();
                e.Name = eventName;
                handler(e);
                eventPool.ReturnInstance(e);
            }
        }

        public void DispatchEvent(string eventName, IEvent e)
        {
            Action<IEvent> handler;
            eventHandlers.TryGetValue(eventName, out handler);
            if (handler != null)
            {
                e.Name = eventName;
                handler(e);
            }
        }

        public void DispatchEvent<T>(string eventName, T arg)
        {
            Action<IEvent> handler;
            eventHandlers.TryGetValue(eventName, out handler);
            if (handler != null)
            {
                Pool<Event<T>> eventPool = GetEventPool<Event<T>>();
                var e = eventPool.GetInstance();
                e.Data = arg;
                e.Name = eventName;
                handler(e);
                eventPool.ReturnInstance(e);
            }
        }

        public void DispatchEvent(string eventName, params object[] args)
        {
            Action<IEvent> handler;
            eventHandlers.TryGetValue(eventName, out handler);
            if (handler != null)
            {
                Pool<HashDataEvent> eventPool = GetEventPool<HashDataEvent>();
                var e = eventPool.GetInstance();
                e.Name = eventName;
                Dictionary<string, object> datas = e.Datas;

                for (int i = 0; i < args.Length; i += 2)
                {
                    string name = (string)args[i];
                    datas.Add(name, args[i + 1]);
                }

                handler(e);
                eventPool.ReturnInstance(e);
            }
        }

        public void AddEventListener(string eventName, Action<IEvent> eventHandler)
        {
            if (!eventHandlers.ContainsKey(eventName))
            {
                Action<IEvent> handler = null;
                handler += eventHandler;
                eventHandlers.Add(eventName, handler);
            }
            else
            {
                eventHandlers[eventName] += eventHandler;
            }
        }

        public void RemoveEventListener(string eventName, Action<IEvent> eventHandler)
        {
            if (eventHandlers.ContainsKey(eventName))
            {
                eventHandlers[eventName] -= eventHandler;
            }
        } 
    }
}