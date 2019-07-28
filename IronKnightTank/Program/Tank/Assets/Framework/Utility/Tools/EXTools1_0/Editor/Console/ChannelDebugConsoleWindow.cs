// ----------------------------------------------------------------------------
// <copyright file="ChannelDebugConsoleWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>23/05/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Cosmos.Console
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Assets.Extends.EXTools.Debug.Console;
    using Assets.Tools.Script.Reflec;

    using ParadoxNotion.Serialization;

    using UnityEditor;

    using UnityEngine;

    public class ChannelDebugConsoleWindow : EditorWindow
    {
        [MenuItem("Window/Tools/DebugConsole/ChannelConsole")]
        public static void Open()
        {
            var channelDebugConsoleWindow = GetWindow<ChannelDebugConsoleWindow>("ChannelConsole");
            channelDebugConsoleWindow.autoRepaintOnSceneChange = true;
        }

        private int currSelect = Int32.MinValue;
        private DateTime lastClickTime = DateTime.MinValue;
        private Vector2 scroll;
        private Dictionary<int, string> channelDatas = new Dictionary<int, string>();

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;
            GUI.skin.box.richText = true;
            GUI.skin.textArea.richText = true;
            GUI.skin.textField.richText = true;
            GUI.skin.toggle.richText = true;
            GUI.skin.window.richText = true;

            //            if (DebugConsole.consoleImpl == null)
            //            {
            //                return;
            //            }
            var debugConsole = DebugConsole.consoleImpl != null ? DebugConsole.consoleImpl as FullDebugConsole : null;
            //            if (debugConsole == null)
            //            {
            //                return;
            //            }
            //            if (!Application.isPlaying)
            //            {
            //                return;
            //            }
            //            var defaultHeight = debugConsole.DefaultHeight;
            //            debugConsole.DefaultHeight = (int)(Screen.height * 1.6f);

            this.Show(debugConsole);

            //还原
            //            debugConsole.DefaultHeight = defaultHeight;
        }

        private void Show(FullDebugConsole console)
        {
            //            var channels = console.ActiveChannels;
            //            var activeChannels = channels.Keys;
            List<int> allChannles = new List<int>();
            var lockChannels = this.GetLockChannels();
            allChannles.AddRange(lockChannels);
            if (console != null)
            {
                this.channelDatas.Clear();
                foreach (var activeChannel in console.ActiveChannels)
                {
                    this.channelDatas.Add(activeChannel.Key, activeChannel.Value);
                }
                foreach (var activeChannel in console.ActiveChannels.Keys)
                {
                    if (!allChannles.Contains(activeChannel))
                    {
                        allChannles.Add(activeChannel);
                    }
                }
            }

            allChannles.Sort();

            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            foreach (var activeChannel in allChannles)
            {
                bool click = false;
                bool locked = lockChannels.Contains(activeChannel);
                string channelName = string.Format("{0}{1}", activeChannel, (locked ? " ★" : ""));
                if (activeChannel == this.currSelect)
                {
                    click = GUILayout.Button(channelName, (GUIStyle)"TE toolbarbutton", GUILayout.Width(50));
                }
                else
                {
                    click = GUILayout.Button(channelName, (GUIStyle)"toolbarbutton", GUILayout.Width(50));
                }
                if (click)
                {
                    if ((DateTime.Now - this.lastClickTime).TotalSeconds < 0.5f && this.currSelect == activeChannel)
                    {
                        this.lastClickTime = DateTime.MinValue;
                        if (locked)
                        {
                            this.RemoveLockChannel(activeChannel);
                        }
                        else
                        {
                            this.AddLockChannel(activeChannel);

                        }
                    }
                    else
                    {
                        this.lastClickTime = DateTime.Now;
                    }
                    GUI.FocusControl("");
                    this.currSelect = activeChannel;
                }
            }
            if (this.currSelect == int.MinValue && lockChannels.Count > 0)
            {
                this.currSelect = lockChannels[0];
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                console.Clear(1);
            }
            GUILayout.EndHorizontal();

            this.ShowLogs();
        }

        private void ShowLogs()
        {
            var containsKey = this.channelDatas.ContainsKey(this.currSelect);
            if (!containsKey)
            {
                return;
            }
            this.scroll = GUILayout.BeginScrollView(this.scroll);
            var channelData = this.channelDatas[this.currSelect];
            EditorGUILayout.TextArea(channelData);
            GUILayout.EndScrollView();
        }

        private List<int> GetLockChannels()
        {
            var s = EditorPrefs.GetString(this.LockChannelKey, null);

            if (s.IsNullOrEmpty())
            {
                return new List<int>();
            }
            return JSON.Deserialize<List<int>>(s);
        }

        private void AddLockChannel(int channel)
        {
            var lockChannels = this.GetLockChannels();
            if (!lockChannels.Contains(channel))
            {
                lockChannels.Add(channel);
            }
            EditorPrefs.SetString(this.LockChannelKey, JSON.Serialize<List<int>>(lockChannels));
        }

        private void RemoveLockChannel(int channel)
        {
            var lockChannels = this.GetLockChannels();
            lockChannels.Remove(channel);
            EditorPrefs.SetString(this.LockChannelKey, JSON.Serialize<List<int>>(lockChannels));
        }

        private string LockChannelKey
        {
            get
            {
                return Application.dataPath + "ChannelDebugConsoleWindowLockChannels";
            }
        }

    }
}