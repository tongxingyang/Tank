using System.Collections.Generic;

namespace Assets.Tools.Script.Event.Message
{
    /// <summary>
    /// MessageDispather派发的消息接收者
    /// 接收到的消息将派发给该IMessageReceiver管辖下对应的IMessageDelegate处理
    /// </summary>
    public interface IMessageReceiver
    {
        IEnumerable<IMessageDelegate> GetDelegates();
    }
}