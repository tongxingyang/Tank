namespace Assets.Tools.Script.Event.Message
{
    /// <summary>
    /// MessageDispather派发的消息执行者
    /// </summary>
    public interface IMessageDelegate
    {
        /// <summary>
        /// 接收到消息后执行
        /// </summary>
        /// <param name="arg">消息所带的参数</param>
        /// <returns>执行结果</returns>
        bool Execute(object arg);
        /// <summary>
        /// 接收到消息后执行
        /// </summary>
        /// <returns>执行结果</returns>
        bool Execute();
        /// <summary>
        /// 消息名
        /// </summary>
        string messageName { get; }
    }
}