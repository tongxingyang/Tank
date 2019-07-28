using System.Collections.Generic;
using Assets.Script.Mvc.Pool;

namespace Assets.Script.Mvc
{
    public abstract class IEvent
    {
        public string Name;
    }

    public class Event : IEvent, IPoolable
    {
        public IPool Pool { get; set; }
        public void Restore()
        {
            
        }
    }

    public class Event<T> : IEvent
    {
        public T Data;

        public IPool Pool { get; set; }

        public void Restore()
        {
            Data = default(T);
        }
    }

    public class HashDataEvent : IEvent, IPoolable
    {
        public Dictionary<string, object> Datas = new Dictionary<string, object>();

        public T Get<T>(string name)
        {
            object o;
            bool tryGetValue = Datas.TryGetValue(name,out o);
            if (tryGetValue)
            {
                return (T) o;
            }
            return default(T);
        }

        public IPool Pool { get; set; }

        public void Restore()
        {
            Datas.Clear();
        }
    }

    public static class EventHelper
    {
        public static T GetData<T>(this IEvent e)
        {
            Event<T> @event = e as Event<T>;
            return @event.Data;
        }

        public static T GetData<T>(this IEvent e, string name)
        {
            HashDataEvent hashDataEvent = e as HashDataEvent;
            return hashDataEvent.Get<T>(name);
        }

        public static T As<T>(this IEvent e) where T : IEvent
        {
            return e as T;
        }
    }
}