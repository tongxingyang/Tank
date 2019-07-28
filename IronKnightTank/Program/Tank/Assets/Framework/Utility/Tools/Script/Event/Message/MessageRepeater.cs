using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.Script.Event.Message
{
    /// <summary>
    /// 支持编辑器环境指定方法的消息接收者
    /// 实现抄自NGUI
    /// *内部反射实现
    /// </summary>
    public class MessageRepeater : MonoBehaviour,IMessageReceiver
    {
        /// <summary>
        /// 消息执行者
        /// </summary>
        public EventToHandler[] handlers;
        
        readonly List<IMessageDelegate> _messageDelegates = new List<IMessageDelegate>();

        void Start()
        {
            if (handlers != null)
            {
                foreach (var eventToHandler in handlers)
                {
                    foreach (var messageDelegate in eventToHandler.handler)
                    {
                        messageDelegate.messageName = eventToHandler.messageName;
                        _messageDelegates.Add(messageDelegate);
                    }
                }
            }
            MessageDispather.RegisterReceiver(this);
        }

        public IEnumerable<IMessageDelegate> GetDelegates()
        {
            return _messageDelegates;
        }
    }
    /// <summary>
    /// MessageRepeater的消息执行者，本身不是IMessageDelegate
    /// 但是管理了一组IMessageDelegate
    /// </summary>
    [System.Serializable]
    public class EventToHandler
    {
        public string messageName="";
        [HideInInspector]
        public List<MessageDelegate> handler = new List<MessageDelegate>();

        public EventToHandler Init(string msgName, MessageDelegate.MessageCallBack.Callback obj)
        {
            this.messageName = msgName;
            handler.Add(new MessageDelegate(new MessageDelegate.MessageCallBack(obj)));
            return this;
        }
        public EventToHandler Init(string msgName, MessageDelegate.MessageCallBack.CallbackNoArg obj)
        {
            this.messageName = msgName;
            handler.Add(new MessageDelegate(new MessageDelegate.MessageCallBack(obj)));
            return this;
        }
    }
}
