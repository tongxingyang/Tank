using Assets.Script.Mvc.Pool;
using UnityEngine;

namespace Assets.Script.Mvc
{
    public class SingleUseCommandAgent<T> : ICommand  where T : ICommand, new()
    {
        public ICommandEventDispatcher EventDispatcher { get; set; }
        public MvcContext Context { get; set; }

        private Pool<T> commandPool;

        public void Execute(IEvent e)
        {
            T command;
            if (commandPool == null)
            {
                command = new T();
                if (command is IPoolable)
                {
                    commandPool = new Pool<T>();
                    commandPool.MaxCount = 5;
                    var poolable = command as IPoolable;
                    poolable.Pool = commandPool;
                }
            }
            else
            {
                command = commandPool.GetInstance();
            }

            command.Context = Context;
            command.EventDispatcher = EventDispatcher;

            command.Execute(e);
        }

        public void OnRegister()
        {
        }

        public void OnUnRegister()
        {
        }
    }
}