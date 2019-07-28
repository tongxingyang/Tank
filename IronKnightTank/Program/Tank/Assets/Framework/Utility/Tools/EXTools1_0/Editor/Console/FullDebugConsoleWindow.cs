using System.Reflection;

using Assets.Extends.EXTools.Debug.Console;
using Assets.Tools.Script.Reflec;

using UnityEditor;

using UnityEngine;

namespace Assets.Cosmos.Console
{
    public class FullDebugConsoleWindow : EditorWindow
    {
        [MenuItem("Window/Tools/DebugConsole/FullDebugConsole")]
        public static void Open()
        {
            GetWindow<FullDebugConsoleWindow>("DebugConsole");
        }

        private void OnGUI()
        {
            if (DebugConsole.consoleImpl == null)
            {
                return;
            }
            var debugConsole = DebugConsole.consoleImpl as FullDebugConsole;
            if (debugConsole == null)
            {
                return;
            }
            if (!Application.isPlaying)
            {
                return;
            }
            var defaultHeight = debugConsole.DefaultHeight;
            debugConsole.DefaultHeight = (int)(Screen.height * 1.6f);

            ReflecTool.InvokeMethod(
                DebugConsole.consoleImpl,
                "Window",
                null,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            //还原
            debugConsole.DefaultHeight = defaultHeight;
        }
    }
}
