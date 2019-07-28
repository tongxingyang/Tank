using System.Text;
using Assets.Tools.Script.Debug.Log;
using Assets.Tools.Script.File;
using UnityEngine;

namespace Assets.Extends.EXTools.Debug
{
    public class ApplicationErrorLog : MonoBehaviour
    {
        public const string ErrorFolder = "error/";
        void Start()
        {
            Application.logMessageReceivedThreaded += OnErrorHandler;
        }

        public void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= OnErrorHandler;
        }

        public string GetAllErrors()
        {
            string[] strings = ESFile.GetFiles(ErrorFolder);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var s in strings)
            {
                string path = s.Substring(0, s.Length - 4);
                stringBuilder.Append("\r\n");
                stringBuilder.Append(ErrorFolder);
                stringBuilder.Append(path);
                stringBuilder.Append("\r\n");
                string file = LogFile.GetFile(ErrorFolder + path);
                if (file != null)
                {
                    stringBuilder.Append("c");
                    stringBuilder.Append(file);
                }
            }
            return stringBuilder.ToString();
        }

        public void DeleteAllErrors()
        {
            ESFile.Delete(ErrorFolder);
        }

        private void OnErrorHandler(string condition, string stacktrace, LogType type)
        {
            //编辑器下错误有打印，这里不需要再次打印
#if !UNITY_EDITOR
            if (type == LogType.Error || type == LogType.Exception)
            {
                string s = "ERROR: " + condition + "\r\n" + "stacktrace:\r\n" + stacktrace;
                //                LogFile.TxtFile(s, ErrorFolder + DateTime.Now.ToString("yy-MM-dd") + "_uncatch_error");
                DebugConsole.Log(s);
        }
#endif
        }
    }
}
