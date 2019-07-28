using System;
using System.Collections.Generic;

namespace Assets.Script.Mvc
{
    public class MvcContext
    {
        public IEventDispatcher EventDispatcher = new MvcEventDispatcher();

        private Dictionary<string, IView> registeredView = new Dictionary<string, IView>();
        private Dictionary<string, IModel> registeredModel = new Dictionary<string, IModel>();

        public void RegisterView(string name, IView view)
        {
            registeredView.Add(name, view);
            view.Context = this;
            view.EventDispatcher = this.EventDispatcher;
            view.OnRegister();
        }

        public void RegisterView<T>(IView view) where T : IView
        {
            registeredView.Add(typeof(T).FullName, view);
            view.Context = this;
            view.EventDispatcher = this.EventDispatcher;
            view.OnRegister();
        }

        public IView GetView(string name)
        {
            IView view;
            registeredView.TryGetValue(name, out view);
            return view;
        }

        public T GetView<T>() where T : class, IView
        {
            IView view;
            registeredView.TryGetValue(typeof(T).FullName, out view);
            return view as T;
        }

        public void UnregisterView(string name)
        {
            IView view;
            bool tryGetValue = registeredView.TryGetValue(name, out view);
            if (tryGetValue)
            {
                view.Context = null;
                view.EventDispatcher = null;
                registeredView.Remove(name);
                view.OnUnRegister();
            }
        }

        public void UnregisterView<T>()
        {
            string name = typeof(T).FullName;
            IView view;
            bool tryGetValue = registeredView.TryGetValue(name, out view);
            if (tryGetValue)
            {
                view.Context = null;
                view.EventDispatcher = null;
                registeredView.Remove(name);
                view.OnUnRegister();
            }
        }

        public void RegisterModel(string name, IModel model)
        {
            registeredModel.Add(name, model);
            model.Context = this;
            model.EventDispatcher = this.EventDispatcher;
            model.OnRegister();
        }

        public void RegisterModel<T>(IModel model) where T : IModel
        {
            registeredModel.Add(typeof(T).FullName, model);
            model.Context = this;
            model.EventDispatcher = this.EventDispatcher;
            model.OnRegister();
        }

        public IModel GetModel(string name)
        {
            IModel model;
            registeredModel.TryGetValue(name, out model);
            return model;
        }

        public T GetModel<T>() where T : class, IModel
        {
            IModel model;
            registeredModel.TryGetValue(typeof(T).FullName, out model);
            return model as T;
        }

        public void UnregisterModel(string name)
        {
            IModel model;
            bool tryGetValue = registeredModel.TryGetValue(name, out model);
            if (tryGetValue)
            {
                model.Context = null;
                model.EventDispatcher = null;
                registeredModel.Remove(name);
                model.OnUnRegister();
            }
        }

        public void UnregisterModel<T>()
        {
            string name = typeof(T).FullName;
            IModel model;
            bool tryGetValue = registeredModel.TryGetValue(name, out model);
            if (tryGetValue)
            {
                model.Context = null;
                model.EventDispatcher = null;
                registeredModel.Remove(name);
                model.OnUnRegister();
            }
        }

        public void RegisterCommand(string eventName, ICommand command)
        {
            command.Context = this;
            command.EventDispatcher = this.EventDispatcher;
            EventDispatcher.AddEventListener(eventName, command.Execute);
            command.OnRegister();
        }

        public void UnregisterCommand(string eventName, ICommand command)
        {
            EventDispatcher.RemoveEventListener(eventName, command.Execute);
            command.OnUnRegister();
        }

        private Dictionary<Type, ICommand> singleUseCommandAgents = new Dictionary<Type, ICommand>();

        public void RegisterSingleUseCommand<T>(string eventName) where T : ICommand, new()
        {
            SingleUseCommandAgent<T> command = new SingleUseCommandAgent<T>();
            singleUseCommandAgents.Add(typeof(T), command);
            RegisterCommand(eventName, command);
        }

        public void UnregisterSingleUseCommand<T>(string eventName) where T : ICommand, new()
        {
            Type type = typeof(T);
            ICommand singleUseCommandAgent = singleUseCommandAgents[typeof(T)];
            singleUseCommandAgents.Remove(type);
            UnregisterCommand(eventName, singleUseCommandAgent);
        }
    }
}