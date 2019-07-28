using UnityEditor;

namespace Assets.Tools.Script.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    
    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Reflec;

    using UnityEngine;
    

    public class ReferenceCounterWindow : EditorWindow
    {
        private const int ColumnWidth = 400;
        private const int ChildSpace = 30;

        [MenuItem("Window/Tools/Reference counter")]
        public static void OpenWindow()
        {
            var referenceCounterWindow = GetWindow<ReferenceCounterWindow>("引用计数器");
            referenceCounterWindow.Repaint();
            referenceCounterWindow.searchBar.HasSort = true;
        }

        public bool AutoClearMemory
        {
            get
            {
                return EditorPrefs.GetBool(Application.dataPath + "ReferenceCounterWindow_AutoClearMemory", true);
            }
            set
            {
                EditorPrefs.SetBool(Application.dataPath + "ReferenceCounterWindow_AutoClearMemory", value);
            }
        }

        private Vector2 scroll;
        private List<ReferenceCountData> referenceDatas = new List<ReferenceCountData>();
        private GUISearchBar<ReferenceCountData> searchBar = new GUISearchBar<ReferenceCountData>() {HasSort = true};

        private Dictionary<string, bool> referenceDetail = new Dictionary<string, bool>();

        private void OnInspectorUpdate()
        {
            this.Repaint();
        }

        public void OnGUI()
        {
            if (AutoClearMemory)
            {
                this.ClearMemory();
            }

            var currMarkTypeCount = ReferenceCounter.GetCurrMarkTypeCount();
            foreach (var typeCount in currMarkTypeCount)
            {
                var referenceCountData = this.referenceDatas.Find(e=>e.Name == typeCount.Key);
                if (referenceCountData == null)
                {
                    referenceDatas.Add(new ReferenceCountData() { Name = typeCount.Key, Count = typeCount.Value });
                }
                else
                {
                    referenceCountData.Count = typeCount.Value;
                }
            }

            GUILayout.BeginHorizontal();
            var referenceCountDatas = this.searchBar.Draw(this.referenceDatas,e => e.Name);
//            if (GUILayout.Button("Snapshoot", (GUIStyle)"toolbarbutton", GUILayout.Width(70)))
//            {
//                LuaReferenceCounter.Snapshoot();
//            }
            if (AutoClearMemory)
            {
                AutoClearMemory = !GUILayout.Button("自动GC", (GUIStyle)"TE toolbarbutton", GUILayout.Width(60));
            }
            else
            {
                AutoClearMemory = GUILayout.Button("自动GC", (GUIStyle)"toolbarbutton", GUILayout.Width(60));
            }
            if (GUILayout.Button("GC", (GUIStyle)"toolbarbutton", GUILayout.Width(60)))
            {
                this.ClearMemory();
            }
            GUILayout.EndHorizontal();

            var detailDic = new Dictionary<string, bool>();
            if (referenceCountDatas != null && referenceCountDatas.Count > 0)
            {
                foreach (var referenceCountData in referenceCountDatas)
                {
                    detailDic[referenceCountData.Name] = false;
                }
            }
            foreach (var key in referenceDetail.Keys)
            {
                if (detailDic.ContainsKey(key))
                {
                    detailDic[key] = referenceDetail[key];
                }
            }
//            string showDetail = null;
            scroll = GUILayout.BeginScrollView(scroll);
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            int i = 0;
            GUILayout.BeginHorizontal(GUITool.GetAreaGUIStyle(new Color(0, 0, 0, 0.8f)));
            GUILayout.Label("Name",GUILayout.Width(ColumnWidth));
            GUILayout.Label("Count");
            GUILayout.EndHorizontal();
            if (referenceCountDatas != null)
            {
                foreach (var data in referenceCountDatas)
                {
                    try
                    {
                        GUILayout.BeginHorizontal(GUITool.GetAreaGUIStyle(new Color(0, 0, 0, i % 2 == 0 ? 0.2f : 0.3f)));
                        if (Button(data.Name, GUILayout.Width(ColumnWidth)))
                        {
                            detailDic[data.Name] = !detailDic[data.Name];
                        }
                        if (Button(ReferenceCounter.GetTypeCount(data.Name).ToString()))
                        {
                            detailDic[data.Name] = !detailDic[data.Name];
                        }
                        GUILayout.EndHorizontal();
                        if (detailDic[data.Name])
                        {
                            GUILayout.BeginHorizontal();
                            var typeWeakReference = ReferenceCounter.GetTypeWeakReference(data.Name);
                            int j = 0;
                            GUILayout.Space(ChildSpace);

                            GUILayout.BeginVertical();
                            foreach (var weakReferenceData in typeWeakReference)
                            {
                                var weakReference = weakReferenceData.Key;
                                var mark = this.GetMark(weakReferenceData.Value, weakReference.Target);

                                GUILayout.BeginHorizontal(GUITool.GetAreaGUIStyle(new Color(0, 0, 0, j % 2 == 0 ? 0f : 0.1f)));

                                bool clickItem;
                                clickItem = Button(weakReference.Target==null?"null": weakReference.Target.ToString(), GUILayout.Width(ColumnWidth));
                                clickItem = Button(mark) || clickItem;
                                if (clickItem)
                                {
                                    if (weakReference.Target!=null && weakReference.Target is IReferenceCounterHandler)
                                    {
                                        var referenceCounterHandler = weakReference.Target as IReferenceCounterHandler;
                                        referenceCounterHandler.HandleClick();
                                    }
                                }
                                GUILayout.EndHorizontal();
                                j++;
                            }
                            GUILayout.EndVertical();

                            GUILayout.EndHorizontal();
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                    
                    i++;
                }
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
//            referenceDatas.Clear();
            referenceDetail = detailDic;
        }

        private string GetMark(string defaultMark,object target)
        {
            if (defaultMark.IsNOTNullOrEmpty())
            {
                return defaultMark;
            }
            if (target!=null)
            {
                return target.ToString();
            }
            return "unknow";
        }

        private void ClearMemory()
        {
            GC.Collect();
//            var findObjectOfType = FindObjectOfType<ResourceManager>();
//            if (findObjectOfType != null)
//            {
//                findObjectOfType.ClearMemory(true);
//            }
//            LuaReferenceCounter.GC();
        }

        private class ReferenceCountData
        {
            public int Count;

            public string Name;
        }

        private static bool Button(string text,params GUILayoutOption[] options)
        {
            TextAnchor textAnchor = GUI.skin.button.alignment;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
            Color backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.clear;
            if (GUILayout.Button(text, options))
            {
                GUI.skin.button.alignment = textAnchor;
                GUI.backgroundColor = backgroundColor;
                return true;
            }
            GUI.skin.button.alignment = textAnchor;
            GUI.backgroundColor = backgroundColor;
            return false;
        }
    }
}
