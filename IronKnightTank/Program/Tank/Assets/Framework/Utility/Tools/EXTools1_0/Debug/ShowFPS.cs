using System.Collections.Generic;
using Assets.Tools.Script.Debug.Console;
using UnityEngine;

namespace Assets.Extends.EXTools.Debug
{
    public class ShowFPS : MonoBehaviour
    {
        public float f_UpdateInterval = 0.5F;

        private float f_LastInterval;

        private int i_Frames = 0;

        public float CurrFps;

        private Dictionary<int, int> distribution = new Dictionary<int, int>();

        void Start()
        {
            //Application.targetFrameRate=60;

            f_LastInterval = Time.realtimeSinceStartup;

            i_Frames = 0;

            distribution.Add(0, 0);
            distribution.Add(10, 0);
            distribution.Add(30, 0);
            distribution.Add(60, 0);
            distribution.Add(100, 0);
            
        }

        

        void Update()
        {
            ++i_Frames;

            if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
            {
                CurrFps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

                i_Frames = 0;

                f_LastInterval = Time.realtimeSinceStartup;

            }
            
            DebugConsole.AddTopString("FPS", string.Format("FPS:{0}", CurrFps.ToString("f2")));

            if (DebugConsole.consoleImpl is EmptyDebugConsole)
            {
                Destroy(this);
            }
        }
    }
}