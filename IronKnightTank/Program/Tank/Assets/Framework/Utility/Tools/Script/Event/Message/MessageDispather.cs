using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.Script.Event.Message
{
    /// <summary>
    /// 字符串消息派发者，旨在解除消息耦合
    /// </summary>
	public class MessageDispather 
	{
        /// <summary>
        /// 所有注册的消息和对应的处理者
        /// </summary>
        private static readonly Dictionary<string,List<IMessageDelegate>> _messageDic=new Dictionary<string, List<IMessageDelegate>>(); 
        /// <summary>
        /// 派发一个消息
        /// </summary>
        /// <param name="methodName">消息名</param>
		public static void SendMessage(string methodName)
		{
            if (_messageDic.ContainsKey(methodName))
            {
                List<IMessageDelegate> messageDelegates = _messageDic[methodName];
                for (int i=0;i< messageDelegates.Count;i++)
                {
                    if (!messageDelegates[i].Execute())
                    {
                        if(RemoveMessageDelegate(messageDelegates[i], methodName))
                        {
                            i--;
                        }
                    }
                }
            }
		}
        /// <summary>
        /// 派发一个消息
        /// </summary>
        /// <param name="methodName">消息名</param>
        /// <param name="value">消息参数</param>
		public static void SendMessage(string methodName,object value)
		{
            if (_messageDic.ContainsKey(methodName))
            {
                List<IMessageDelegate> messageDelegates = _messageDic[methodName];
                for (int i=0;i< messageDelegates.Count;i++)
                {
                    if (!messageDelegates[i].Execute(value))
                    {
                        if(RemoveMessageDelegate(messageDelegates[i], methodName))
                        {
                            i--;
                        }
                    }
                }

            }
		}
        /// <summary>
        /// 注册一个消息接收者，使其管理的消息执行者能响应消息
        /// </summary>
        /// <param name="receiver">消息接收者</param>
        public static void RegisterReceiver(IMessageReceiver receiver)
        {
            IEnumerable<IMessageDelegate> messageDelegates = receiver.GetDelegates();
            foreach (var msgDelegate in messageDelegates)
            {
                var message = msgDelegate.messageName;
                if (!_messageDic.ContainsKey(message))
                {
                    _messageDic.Add(message, new List<IMessageDelegate>());
                }
                _messageDic[message].Add(msgDelegate);
            }
        }
        /// <summary>
        /// 移除一个消息处理者
        /// </summary>
        /// <param name="messageDelegate">消息处理者</param>
        /// <param name="message">消息名</param>
        /// <returns>是否成功</returns>
	    private static bool RemoveMessageDelegate(IMessageDelegate messageDelegate,string message)
	    {
	        bool succeed = false;
	        if (_messageDic.ContainsKey(message))
	        {
	            List<IMessageDelegate> messageDelegates = _messageDic[message];
                succeed=messageDelegates.Remove(messageDelegate);
	            if (messageDelegates.Count == 0)
	                _messageDic.Remove(message);
	        }
	        return succeed;
	    }
	}
}
