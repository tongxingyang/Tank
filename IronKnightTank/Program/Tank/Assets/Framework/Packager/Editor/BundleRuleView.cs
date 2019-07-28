﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rule = ABPackRuleConfig.Rule ;
using System.Linq;

public class BundleRuleView  {
	
	private ABPackRuleConfig config;
	private int index = -1;
	private EditorWindow m_hostWin;
	private bool isPull = false ;
    private Vector2 scrollPos = Vector2.zero;
	public BundleRuleView(EditorWindow  hostWindow){
		config = AutoABNamePostprocessor.config;
		m_hostWin = hostWindow;
	}
		
	public void Draw(Rect r){
		GUILayout.BeginHorizontal (TableStyles.Toolbar);
		{
			isPull = GUILayout.Toggle (isPull, "AB规则");
		}
		GUILayout.Space (3f);
		GUILayout.EndHorizontal ();
		if (isPull) {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			for (int i = 0; i < config.rules.Count; i++) {
				OnGUIRule(config.rules[i], index == i);
				if (Event.current.type == EventType.MouseUp && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
				{
					index = i;
					Event.current.Use();
				}
			}
            EditorGUILayout.EndScrollView();
			GUILayout.BeginHorizontal (TableStyles.Toolbar);
			{
				if (GUILayout.Button ("AddRule", TableStyles.ToolbarButton)) {
                    Rule rule = new ABPackRuleConfig.Rule();
                    
                    if (ModifyRule(rule))
                    {
                        config.rules.Add(rule);
                        EditorUtility.SetDirty(config);
                    }
					
                    
                }
				if (GUILayout.Button ("Apply", TableStyles.ToolbarButton)) {
					AutoABNamePostprocessor.PackAll();
                    m_hostWin.ShowNotification(new GUIContent("Apply完成"));
                }
			}
			GUILayout.EndHorizontal ();
		}
	}

	private void OnGUIRule(Rule rule , bool isSelect){
		GUILayout.BeginVertical (  GUI.skin.box );
		{
			GUILayout.BeginHorizontal ();{
				rule.path = EditorGUILayout.TextField ("Path ", rule.path, TableStyles.TextField);
				if (GUILayout.Button("Select", TableStyles.ToolbarButton , GUILayout.MaxWidth(160)))
				{
                    ModifyRule(rule);
				}
				if(GUILayout.Button("Remove" , TableStyles.ToolbarButton , GUILayout.MaxWidth(120))){
					config.rules.Remove (rule);
					EditorUtility.SetDirty (config);
				}
			}
			GUILayout.EndHorizontal ();
			if (!string.IsNullOrEmpty(rule.path))
			{
				rule.typeFilter = EditorGUILayout.TextField("TypeFilter: ", rule.typeFilter , TableStyles.TextField);
				if (Event.current.type == EventType.DragUpdated && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Move;
					DragAndDrop.AcceptDrag();
					Event.current.Use();
				}
				else if (Event.current.type == EventType.DragPerform && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
				{
					rule.typeFilter = string.Join(",", DragAndDrop.objectReferences.Select(x => x.GetType().Name).Distinct().ToArray());
					Event.current.Use();
					EditorUtility.SetDirty(config);
				}
				rule.ruleType = EditorGUILayout.Popup("Rule: ", rule.ruleType, AutoABNamePostprocessor.packRuleNames.ToArray() );
			}
		}
		GUILayout.EndVertical ();
	}

    private bool ModifyRule(Rule rule)
    {
        string result = EditorUtility.OpenFolderPanel("", "选择目录", "");
        if (!string.IsNullOrEmpty(result))
        {
            string path = GetAssetPath(result);
            if (AssertPath(path))
            {
                GUI.FocusControl(null);
                rule.path = path;
                EditorUtility.SetDirty(config);
                return true;
            }
            else
            {
                m_hostWin.ShowNotification(new GUIContent("rule已存在"));
                return false;
            }
        }
        return false;
    }


	private void OnWinClose(){
		AssetDatabase.SaveAssets ();
	}


	private bool AssertPath(string path){
		foreach (var item in config.rules) {
			if (item.path.Equals (path)) {
				return false;
			}
		}
		return true;
	}

	private string GetAssetPath(string result)
	{
		if (result.StartsWith(Application.dataPath))
			return result == Application.dataPath ? "" : result.Substring(Application.dataPath.Length + 1);
		else if (result.StartsWith("Assets"))
			return result == "Assets" ? "" : result.Substring("Assets/".Length);
		return null;
	}

	public static List<object> ToObjectList<T>(List<T> data)
	{
		if (data == null) return null;
		List<object> ret = new List<object>();
		for (int i = 0; i < data.Count; ++i)
		{
			ret.Add(data[i]);
		}
		return ret;
	}
}
public static class TableStyles
{
	public static GUIStyle Toolbar = "Toolbar";
	public static GUIStyle ToolbarButton = "ToolbarButton";

	public static GUIStyle TextField = "TextField";
}