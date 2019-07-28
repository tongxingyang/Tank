using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Assets.Tools.Script.Editor.Tool;

using Game.Battle.ActionView;
using Game.Tank;

using JetBrains.Annotations;

using UnityEditor;
using UnityEngine;

using Debug = System.Diagnostics.Debug;

[CustomEditor(typeof(ActionViewPlayer))]
public class ActionViewPlayerEditor : Editor
{
    private ActionViewPlayer player;

    private readonly OnGUIUtility guiUtility = new OnGUIUtility();
    private readonly Color areaColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);

    private void OnEnable()
    {
       this.player = target as ActionViewPlayer;
    }


    private string newActoinViewName = "";
    public override void OnInspectorGUI()
    {
        GUILayout.Space(10);
        var list = this.player.ActionViewDic.ToList();
        for (int i = 0; i < list.Count; i++)
        {
            DrawActionView(list[i]);
            if (GUILayout.Button("×"))
            {
                this.DeleteActionView(list[i].Key);
            }
        }
         
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        {
            this.newActoinViewName = EditorGUILayout.TextField("新增动作名", this.newActoinViewName);
            if (GUILayout.Button("增加View"))
            {
                if (string.IsNullOrEmpty(this.newActoinViewName))
                {
                    EditorApplication.Beep();
                    EditorUtility.DisplayDialog("Tips", "动作名违法", "ok");
                }
                else if (this.player.ActionViewDic.ContainsKey(this.newActoinViewName))
                {
                    EditorApplication.Beep();
                    EditorUtility.DisplayDialog("Tips", "已包含动作名", "ok");
                }
                else
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Once"), false,
                        delegate ()
                            {
                                this.player.ActionViewDic[this.newActoinViewName] = 
                                    BaseActionView.CreateActionView(this.GetActionViewParent() ,  typeof(OnceActionView) , this.newActoinViewName);
                            });
                    menu.AddItem(new GUIContent("Loop"), false,
                        delegate ()
                            {
                                this.player.ActionViewDic[this.newActoinViewName] =
                                    BaseActionView.CreateActionView(this.GetActionViewParent(), typeof(LoopActionView) , this.newActoinViewName);
                            });
                    menu.ShowAsContext();
                    
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        
    }


    private Transform GetActionViewParent()
    {
        var parent = this.player.transform.Find("ActionView");
        if (parent == null)
        {
            parent = new GameObject("ActionView").transform;
            parent.transform.SetParent(this.player.transform);
        }

        return parent;
    }

    public void DrawActionView(KeyValuePair<string , BaseActionView> pairs)
    {
        
        if (this.guiUtility.OpenClose(pairs.Key, "flow overlay box"))
        {
            DrawActionView(pairs.Value);
        }
    }



    private void DeleteActionView(string key)
    {
        this.player.ActionViewDic.Remove(key);
    }

    private void DrawActionView(BaseActionView view)
    {  
        
        GUILayout.BeginVertical(GUITool.GetAreaGUIStyle(this.areaColor));

        GUILayout.Space(5);
        view.DrawInspector();
    }

}
