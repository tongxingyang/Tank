using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Assets.Tools.Script.Debug.Console;
using Assets.Tools.Script.Debug.Log;
using UnityEngine;

namespace Assets.Extends.EXTools.Debug.Console
{
    using Assets.Script.Mvc.Pool;

    using Debug = UnityEngine.Debug;

    public class FullDebugConsole : MonoBehaviour, IDebugConsole
    {
        //默认屏幕分辨率
        public int DefaultHeight = 1080;
        //调试框宽高
        public int ConsoleWidth = 600;
        public int ConsoleHeight = 600;

        //Object显示工具
        public IObjectAnalyseDisplayer AnalyseDisplayer;

        //当前频道
        [HideInInspector]
        public int CurrViewChannel;
        //当前可用频道
        public readonly Dictionary<int, string> ActiveChannels;
        //注册的事件响应按钮
        public readonly Dictionary<string, Action> EventBtns = new Dictionary<string, Action>();
        //常驻字符串
        public readonly Dictionary<string, string> TopStrings = new Dictionary<string, string>();
        //可被查看的object对象
        public readonly List<NameableObject> ObjectList = new List<NameableObject>();

        public readonly Pool<NameableObject> NameableObjectPool = new Pool<NameableObject>();
        //当前调试窗口是否可见
        protected bool ViewEnabled
        {
            get
            {
                return _viewEnabled;
            }
            set
            {
                if (_viewEnabled != value)
                {
                    _viewEnabled = value;
                    OnEnabledChange(_viewEnabled);
                }
            }
        }
        private bool _viewEnabled = false;

        //当前显示模式
        protected int ViewMode = 1;//1:正常Log窗口，2:显示object list

        //调试log窗口滚动条位置
        private Vector2 ScrollPosition;
        //一些视图样式


        protected GUIStyle TextFieldGuiStyle
        {
            get
            {
                if (_textFieldGuiStyle == null)
                {
                    _textFieldGuiStyle = new GUIStyle("textfield");
                    _textFieldGuiStyle.normal.textColor = Color.white;
                }
                _textFieldGuiStyle.fontSize = GetPixelValue(20);
                return _textFieldGuiStyle;
            }
        }
        private GUIStyle _textFieldGuiStyle = null;

        protected GUIStyle BtnGuiStyle
        {
            get
            {
                if (_btnGuiStyle == null)
                {
                    _btnGuiStyle = new GUIStyle("button");
                    _btnGuiStyle.normal.textColor = Color.white;
                }
                _btnGuiStyle.fontSize = GetPixelValue(20);
                return _btnGuiStyle;
            }
        }
        private GUIStyle _btnGuiStyle = null;

        protected GUIStyle HSliderGuiStyle
        {
            get
            {
                if (_hSliderGuiStyle == null)
                {
                    _hSliderGuiStyle = new GUIStyle("horizontalscrollbar");
                }
                _hSliderGuiStyle.fixedHeight = GetPixelValue(48);
                return _hSliderGuiStyle;
            }
        }
        private GUIStyle _hSliderGuiStyle = null;


        protected GUIStyle VSliderGuiStyle
        {
            get
            {
                if (_vSliderGuiStyle == null)
                {
                    _vSliderGuiStyle = new GUIStyle("verticalscrollbar");
                }
                _vSliderGuiStyle.fixedWidth = GetPixelValue(48);
                return _vSliderGuiStyle;
            }
        }
        private GUIStyle _vSliderGuiStyle = null;

        private Rect dragWindowRect;

        protected virtual void Awake()
        {
            DebugConsole.consoleImpl = this;
            dragWindowRect = new Rect(0, 0, GetPixelValue(ConsoleWidth), GetPixelValue(ConsoleHeight));
        }

        public FullDebugConsole()
        {
            this.ActiveChannels = new Dictionary<int, string> { { 0, "" } };
            this.CurrViewChannel = 0;
            this.AnalyseDisplayer = new ObjectAnalyseDisplayer(this);
        }

        protected virtual void OnEnabledChange(bool enable)
        {

        }

        public virtual void AddTopString(string stringName, string content)
        {
            if (this.TopStrings.ContainsKey(stringName))
            {
                this.TopStrings[stringName] = content;
            }
            else
            {
                this.TopStrings.Add(stringName, content);
            }

        }
        public virtual void RemoveTopString(string stringName)
        {
            if (this.TopStrings.ContainsKey(stringName))
                this.TopStrings.Remove(stringName);
        }

        public virtual void SetConsoleActive(bool consoleActive)
        {
            this.ViewEnabled = consoleActive;
        }

        public virtual void Clear(int level)
        {
            if (level == 1)
            {
                this.ClearTextLogs();
            }
            else if (level == 2)
            {
                this.ClearObjectLogs();
            }
            else if (level == 3)
            {
                this.ClearEventButton();
            }
            else if (level == 4)
            {
                if (this.ActiveChannels.ContainsKey(this.CurrViewChannel))
                    this.ActiveChannels[this.CurrViewChannel] = "";
            }
            else
            {
                this.ClearTextLogs();
                this.ClearObjectLogs();
                this.ClearEventButton();
                this.ViewMode = 1;
            }
        }

        public string GetText()
        {
            return this.ActiveChannels[this.CurrViewChannel];
        }

        public virtual void AddButton(string btnName, Action todo)
        {
            if (this.EventBtns.ContainsKey(btnName))
                return;
            this.EventBtns.Add(btnName, todo);
        }
        public virtual void RemoveButton(string btnName)
        {
            if (this.EventBtns.ContainsKey(btnName))
            {
                this.EventBtns.Remove(btnName);
            }
        }
        public virtual void LogStackTrace()
        {
            string stackInfo = new StackTrace().ToString();
            Log(stackInfo);
        }
        public virtual void Log(string msg)
        {
            LogToChannel(0, msg);
        }
        public virtual void Log(params object[] msgs)
        {
            StringBuilder builder = new StringBuilder();
            builder.Capacity = msgs.Length * 8;
            for (int i = 0; i < msgs.Length; i++)
            {
                object str = msgs[i];
                if (str == null)
                {
                    builder.Append("NullObject");
                }
                else
                {
                    if (str is Exception)
                        LogFile.TxtFile(str.ToString(),
                            string.Format("error/{0}_error", DateTime.Now.ToString("yy-MM-dd")));
                    builder.Append(str);
                    builder.Append(" ");
                    CheckObjectAndAddToList(str, i > 0 ? msgs[i - 1] as string : null);
                }
            }
            Log(builder.ToString());
        }

        public virtual void LogToChannel(int channel, string msg)
        {
            if (!this.ActiveChannels.ContainsKey(channel))
            {
                AddActiveChannel(channel);
            }
            StringBuilder builder = new StringBuilder();

            builder.Append(this.ActiveChannels[channel]).Append(msg).Append("\r\n");
            this.ActiveChannels[channel] = builder.ToString();
            Debug.Log(msg);
        }

        public virtual void LogToChannel(int channel, params object[] msgs)
        {
            StringBuilder builder = new StringBuilder();
            builder.Capacity = msgs.Length * 8;
            for (int i = 0; i < msgs.Length; i++)
            {
                object str = msgs[i];
                if (str == null)
                {
                    builder.Append("NullObject");
                }
                else
                {
                    if (str is Exception)
                    {
                        LogFile.TxtFile(str.ToString(),
                            string.Format("error/{0}_error", DateTime.Now.ToString("yy-MM-dd")));
                    }
                    builder.Append(str);
                    builder.Append(" ");
                    CheckObjectAndAddToList(str, i > 0 ? msgs[i - 1] as string : null);
                }
            }
            LogToChannel(channel, builder.ToString());
        }

        protected void AddActiveChannel(int channel)
        {
            if (!this.ActiveChannels.ContainsKey(channel))
            {
                this.ActiveChannels.Add(channel, "");
            }

        }
        public virtual void RemoveActiveChannel(int channel)
        {
            if (this.ActiveChannels.ContainsKey(channel))
                this.ActiveChannels.Remove(channel);
        }

        protected virtual void OnDestroy()
        {
            AnalyseDisplayer = null;
            ActiveChannels.Clear();
            EventBtns.Clear();
            TopStrings.Clear();
            ObjectList.Clear();
            NameableObjectPool.Clear();
        }

        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {
                this.ViewEnabled = !this.ViewEnabled;
            }
        }

        protected virtual void OnGUI()
        {
            if (this.ViewEnabled)
            {
                dragWindowRect = GUILayout.Window(
                    0,
                    dragWindowRect,
                    (id) =>
                        {
                            this.Window();
                            GUI.DragWindow();
                        },
                    "DebugConsole");
            }
        }

        protected virtual void Window()
        {
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;
            GUI.skin.box.richText = true;
            GUI.skin.textArea.richText = true;
            GUI.skin.textField.richText = true;
            GUI.skin.toggle.richText = true;
            GUI.skin.window.richText = true;

            //---------------------show top string-------------------------//
            GUILayout.BeginHorizontal(GUILayout.Width(GetPixelValue(ConsoleWidth)));
            {
                foreach (var topString in this.TopStrings.Values)
                {
                    if (GUILayoutBtn(topString))
                    {

                    }
                }
            }
            GUILayout.EndHorizontal();
            //-------------------------show buttons-------------------------//
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    //内部功能按钮
                    ShowInternalButton();
                    //外部注册按钮
                    //                        foreach (var eventBtnName in _eventBtns.Keys)
                    //                        {
                    //                            if (GUILayoutBtn(eventBtnName))
                    //                            {
                    //                                _eventBtns[eventBtnName]();
                    //                            }
                    //                        }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical();
                {
                    //内部功能按钮
                    //                        ShowInternalButton();
                    //外部注册按钮
                    var strings = this.EventBtns.Keys.ToArray();
                    bool beginHorizontal = false;
                    for (int i = 0; i < strings.Length; i++)
                    {
                        if (i % 4 == 0)
                        {
                            if (beginHorizontal)
                            {
                                GUILayout.EndHorizontal();
                                beginHorizontal = false;
                            }

                            GUILayout.BeginHorizontal();
                            beginHorizontal = true;
                        }
                        var eventBtnName = strings[i];
                        if (GUILayoutBtn(eventBtnName) && this.EventBtns.ContainsKey(eventBtnName))
                        {
                            this.EventBtns[eventBtnName]();
                        }
                    }
                    if (beginHorizontal)
                    {
                        GUILayout.EndHorizontal();
                    }
                    //                    foreach (var eventBtnName in strings)
                    //                    {
                    //                        if (GUILayoutBtn(eventBtnName) && this.EventBtns.ContainsKey(eventBtnName))
                    //                        {
                    //                            this.EventBtns[eventBtnName]();
                    //                        }
                    //                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            //---------------show debug console-----------------------//
            ShowConsole();

        }

        protected virtual void CheckObjectAndAddToList(object obj, string objName)
        {
            if (!(obj is string) && obj.GetType().IsClass && !(obj is Type) && !(obj is Exception))
            {
                var nameableObject = this.NameableObjectPool.GetInstance();
                nameableObject.Name = objName != null ? objName : string.Format("_Elm {0}", this.ObjectList.Count);
                nameableObject.Value = obj;
                this.ObjectList.Add(nameableObject);
            }
        }

        protected void ShowInternalButton()
        {
            if (this.ViewMode == 1)
            {
                if (this.ActiveChannels.ContainsKey(this.CurrViewChannel) && this.ActiveChannels[this.CurrViewChannel].Length == 0)
                {
                    if (GUILayoutBtn("清除所有Log"))
                    {
                        this.ClearTextLogs();
                    }
                }
                else
                {
                    if (GUILayoutBtn("清除Log"))
                    {
                        if (this.ActiveChannels.ContainsKey(this.CurrViewChannel))
                            this.ActiveChannels[this.CurrViewChannel] = "";
                    }
                }

                if (GUILayoutBtn("当前channel" + this.CurrViewChannel))
                {
                    List<int> ids = this.ActiveChannels.Keys.ToList();
                    ids.Sort((l, r) =>
                    {
                        return l - r;
                    });
                    for (int i = 0; i < ids.Count; i++)
                    {
                        int id = ids[i];
                        if (id > this.CurrViewChannel)
                        {
                            this.CurrViewChannel = id;
                            break;
                        }
                        if (i == ids.Count - 1)
                        {
                            this.CurrViewChannel = ids[0];
                        }
                    }
                }

                if (GUILayoutBtn("Object视图"))
                {
                    this.AnalyseDisplayer.ShowNewObject(this.ObjectList, "Debug");
                    this.ViewMode = 2;
                    this.ScrollPosition = Vector2.zero;
                }
            }
            else if (this.ViewMode == 2)
            {
                if (GUILayoutBtn("清除所有Object"))
                {
                    this.ClearObjectLogs();
                }
                if (GUILayoutBtn("返回"))
                {
                    if (this.AnalyseDisplayer.Back())
                    {
                        this.ViewMode = 1;
                    }
                    this.ScrollPosition = Vector2.zero;
                }
            }
        }

        protected void ShowConsole()
        {
            this.ScrollPosition = BeginGUIScrollView(this.ScrollPosition);
            if (this.ViewMode == 1)
            {
                if (this.ActiveChannels.ContainsKey(this.CurrViewChannel))
                    this.ActiveChannels[this.CurrViewChannel] = GUILayoutTextField(this.ActiveChannels[this.CurrViewChannel]);
            }
            else if (this.ViewMode == 2)
            {
                this.AnalyseDisplayer.Show();
            }
            else if (this.ViewMode == 3)
            {

            }
            GUILayout.EndScrollView();
        }

        protected Vector2 BeginGUIScrollView(Vector2 v)
        {
            return GUILayout.BeginScrollView(v, HSliderGuiStyle, VSliderGuiStyle, GUILayout.Height(GetPixelValue(ConsoleHeight)), GUILayout.Width(GetPixelValue(ConsoleWidth)));
        }
        public bool GUILayoutBtn(string str)
        {
            return GUILayout.Button(str, BtnGuiStyle, GUILayout.Height(GetPixelValue(40)), GUILayout.Width(GetPixelValue(150)));
        }

        protected virtual void ClearTextLogs()
        {
            for (int i = 0; i < this.ActiveChannels.Keys.ToList().Count; i++)
            {
                var key = this.ActiveChannels.Keys.ToList()[i];
                if (key < 10000)
                {
                    this.ActiveChannels[key] = "";
                }
            }
        }
        protected virtual void ClearObjectLogs()
        {
            foreach (var nameableObject in this.ObjectList)
            {
                this.NameableObjectPool.ReturnInstance(nameableObject);
            }
            this.ObjectList.Clear();
        }

        protected virtual void ClearEventButton()
        {
            this.EventBtns.Clear();
        }

        public string GUILayoutTextField(string str)
        {
            try
            {
                if (str.Length > 10000)
                    str = str.Substring(str.Length - 10000, 10000);
                return GUILayout.TextField(str, TextFieldGuiStyle);
            }
            catch (Exception)
            {
                return str;
            }
        }

        public int GetPixelValue(int value)
        {
            return Screen.height * value / DefaultHeight;
        }
    }
}