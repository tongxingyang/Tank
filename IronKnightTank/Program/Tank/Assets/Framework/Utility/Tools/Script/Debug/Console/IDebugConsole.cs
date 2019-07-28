using System;

namespace Assets.Tools.Script.Debug.Console
{
    public interface IDebugConsole
    {
        /// <summary>
        /// 输出内容到默认频道
        /// </summary>
        /// <param name="msg">输出信息</param>
        void Log(string msg);
        /// <summary>
        /// ToString后空格分隔
        /// </summary>
        /// <param name="msgs">输出信息</param>
        void Log(params object[] msgs);
        /// <summary>
        /// 调用栈输出
        /// </summary>
        void LogStackTrace();
        /// <summary>
        /// log到指定频道
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="msg">输出信息</param>
        void LogToChannel(int channel, string msg);
        /// <summary>
        /// log到指定频道
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="msgs">输出信息</param>
        void LogToChannel(int channel, params object[] msgs);
        /// <summary>
        /// 添加调试用按钮
        /// </summary>
        /// <param name="btnName">按钮名</param>
        /// <param name="clickHandler">按钮响应事件</param>
        void AddButton(string btnName, Action clickHandler);
        /// <summary>
        /// 按按钮名移除调试按钮
        /// </summary>
        /// <param name="btnName"></param>
        void RemoveButton(string btnName);
        /// <summary>
        /// 添加一个常驻调试字符
        /// </summary>
        /// <param name="stringName">调试字符名（用于移除）</param>
        /// <param name="content">字符内容</param>
        void AddTopString(string stringName, string content);
        /// <summary>
        /// 移除常驻调试字符
        /// </summary>
        /// <param name="stringName"></param>
        void RemoveTopString(string stringName);
        /// <summary>
        /// 设置调试窗口可见状态
        /// </summary>
        /// <param name="consoleActive">可见性</param>
        void SetConsoleActive(bool consoleActive);
        /// <summary>
        /// Clears the specified level.
        /// </summary>
        /// <param name="level">The level.</param>
        void Clear(int level);

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetText();

    }
}