// ----------------------------------------------------------------------------
// <copyright file="LuaDebugConsole.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>12/01/2016</date>
// ----------------------------------------------------------------------------


using LuaInterface;


namespace Assets.Scripts.Tools.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Assets.Scripts.Tools.Lua;
    using Assets.Tools.Script.Helper;
    using Extends.EXTools.Debug.Console;
	using XQFramework.Lua;
    using UnityEngine;

    public class LuaDebugConsole : FullDebugConsole
    {
        public static LuaManager LuaManager
        {
            get
            {
                return LuaManager.Instance;
            }
        }

        public const int PublicChannel = -1;

        public bool DontDestroy;

        public bool StopEventWhenOpen = false;

        public List<int> NonUnityConsoleChannels = new List<int>();

        public List<LuaDebugConsoleMixConsoleChannel> MixConsoleChannels = new List<LuaDebugConsoleMixConsoleChannel>();

        private GameObject mask;
        

        private readonly Dictionary<int, bool> nonUnityConsoleChannels = new Dictionary<int, bool>();

        private readonly Dictionary<int, List<int>> mixConsoleChannels = new Dictionary<int, List<int>>();

        private int mixChannel = PublicChannel - 1;

        private int mixing;

        private readonly StringBuilder logStringBuilder = new StringBuilder();
        public LuaDebugConsole()
        {
            var analyseDisplayer = this.AnalyseDisplayer as ObjectAnalyseDisplayer;
            analyseDisplayer.RegisterObjectAnalyse(new LuaTableDebugAnalyse());
        }

        public override void LogToChannel(int channel, string msg)
        {
            if (!this.ActiveChannels.ContainsKey(channel))
            {
                AddActiveChannel(channel);
            }
            StringBuilder builder = this.logStringBuilder;
            builder.Clear();
            if (msg.Contains("stack traceback:"))
            {
                var strings = msg.Split(new[] { "stack traceback:" }, StringSplitOptions.None);
                builder.Append(this.ActiveChannels[channel]).AppendFormat("[{0}] ",Time.time.ToString("####.000")).Append(strings[0]);
                this.ActiveChannels[channel] = builder.ToString();
                if (mixing == 0 && !this.nonUnityConsoleChannels.ContainsKey(channel) && channel >= 0 && channel <= 100)
                {
                    Debug.Log(msg);

                }
            }
            else
            {
                builder.Append(this.ActiveChannels[channel]).AppendFormat("[{0}] ", Time.time.ToString("####.000")).Append(msg).Append("\r\n");
                this.ActiveChannels[channel] = builder.ToString();
                if (mixing == 0 && !this.nonUnityConsoleChannels.ContainsKey(channel) && channel >= 0 && channel <= 100)
                {
                    Debug.Log(msg);
                }
            }
            if (channel >= 0)
            {
                if (mixing == 0)
                {
                    this.LogToChannel(PublicChannel, msg);
                }

                List<int> mixTo;
                var tryGetValue = this.mixConsoleChannels.TryGetValue(channel, out mixTo);

                if (tryGetValue)
                {
                    foreach (var i in mixTo)
                    {
                        if (i != channel)
                        {
                            mixing++;
                            this.LogToChannel(i, msg);
                            mixing--;
                        }
                    }
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            DebugConsole.consoleImpl = this;

            if (!this.StopEventWhenOpen)
            {
                return;
            }
            if (this.ViewEnabled)
            {
                if (this.mask != null && !this.mask.activeSelf)
                {
                    this.mask.SetActive(true);
                }
            }
            else
            {
                if (this.mask != null && this.mask.activeSelf)
                {
                    this.mask.SetActive(false);

                }
            }
        }

        protected override void Awake()
        {
            if (Application.isPlaying && LuaManager != null)
            {
                var luaTable = LuaManager.GetTable("DebugConsole");
                if (luaTable != null)
                {
                    var luaFunction = luaTable.GetLuaFunction("GetConsoleEnable");
                    if (luaFunction != null)
                    {
                        var b = luaFunction.Invoke<bool>();
                        if (!b)
                        {
                            Destroy(this);
                            return;
                        }
                    }
                }
            }
            var debugConsole = DebugConsole.consoleImpl as LuaDebugConsole;
            if (debugConsole != null)
            {
                this.DontDestroy = this.DontDestroy || debugConsole.DontDestroy;
                if (debugConsole.gameObject.name == "TestEntry")
                {
                    Destroy(debugConsole.gameObject);
                }
                else
                {
                    Destroy(debugConsole);
                }
            }
            base.Awake();
            if (this.DontDestroy)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            this.nonUnityConsoleChannels.Clear();
            foreach (var nonUnityConsoleChannel in this.NonUnityConsoleChannels)
            {
                this.nonUnityConsoleChannels.Add(nonUnityConsoleChannel, true);
            }

            foreach (var mixConsoleChannel in this.MixConsoleChannels)
            {
                this.mixChannel--;
                foreach (var channel in mixConsoleChannel.MixChannels)
                {
                    List<int> toChannels;
                    this.mixConsoleChannels.TryGetValue(channel, out toChannels);
                    if (toChannels == null)
                    {
                        toChannels = new List<int>();
                        this.mixConsoleChannels.Add(channel, toChannels);
                    }
                    toChannels.Add(mixConsoleChannel.ToChannel > 0 ? mixConsoleChannel.ToChannel : this.mixChannel);
                }
            }
//            this.SetConsoleActive(true);
        }
    }

    [Serializable]
    public class LuaDebugConsoleMixConsoleChannel
    {
        public int ToChannel;

        public List<int> MixChannels = new List<int>();
    }
}