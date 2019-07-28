using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.Script.Event.Message
{
    /// <summary>
    /// 消息接收者
    /// 实现使用系统自带的SendMessage
    /// 对带有MessageReceiver组件调用SendMessage
    /// </summary>
	public class MessageReceiver : MonoBehaviour,IMessageReceiver
	{
        /// <summary>
        /// 尚未注册到MessageDispather的消息
        /// </summary>
        private static readonly Dictionary<GameObject,string[]> _dynamicMessageReceiverArg=new Dictionary<GameObject, string[]>();
        /// <summary>
        /// 添加一个MessageReceiver组件
        /// </summary>
        /// <param name="obj">添加在该GameObject</param>
        /// <param name="registerMessage">希望处理的消息名</param>
		public static void AddMessageReceiver(GameObject obj,params string[] registerMessage)
		{
		    if (_dynamicMessageReceiverArg.ContainsKey(obj))
		    {
                string[] existentMsg = _dynamicMessageReceiverArg[obj];
                string[] allMsg = new string[existentMsg.Length + registerMessage.Length];
                for (int i = 0; i < existentMsg.Length; i++)
                {
                    allMsg[i] = existentMsg[i];
                }
                for (int i = 0; i < registerMessage.Length; i++)
                {
                    allMsg[i + existentMsg.Length] = registerMessage[i];
                }
		    }
		    else
		    {
                _dynamicMessageReceiverArg.Add(obj, registerMessage);
                obj.AddComponent<MessageReceiver>();
		    }
		}
        
        [SerializeField]
        protected string[] registerMessage;

        private readonly Dictionary<string, IMessageDelegate> _registeredMessage = new Dictionary<string, IMessageDelegate>();

		void Awake()
		{
		    if (_dynamicMessageReceiverArg.ContainsKey(gameObject))
		    {
		        registerMessage = _dynamicMessageReceiverArg[gameObject];
		        _dynamicMessageReceiverArg.Remove(gameObject);
		    }
			if(registerMessage!=null)
			{
				foreach(string msg in registerMessage)
				{
                    if (!_registeredMessage.ContainsKey(msg))
                    {
                        _registeredMessage.Add(msg, new UnityMessageDelegate(this, msg));
                    }
				}
			}
            MessageDispather.RegisterReceiver(this);
		}

        public IEnumerable<IMessageDelegate> GetDelegates()
	    {
	        return _registeredMessage.Values;
	    }
	}

    class UnityMessageDelegate:IMessageDelegate
    {
        private readonly MessageReceiver _messageReceiver;
        public string messageName { get; private set; }

        public UnityMessageDelegate(MessageReceiver messageReceiver, string methodName)
        {
            _messageReceiver = messageReceiver;
            messageName = methodName;
        }

        public bool Execute(object arg)
        {
            if (_messageReceiver != null)
            {
                _messageReceiver.SendMessage(messageName, arg, SendMessageOptions.DontRequireReceiver);
                return true;
            }
            return false;
        }
    

        public bool Execute()
        {
            if (_messageReceiver != null)
            {
                _messageReceiver.SendMessage(messageName, SendMessageOptions.DontRequireReceiver);
                return true;
            }
            return false;
        }

        
    }
}
