using UnityEditor;
using UnityEngine;

namespace Assets.Tools.Script.Editor
{
    public class NEditorTools : MonoBehaviour {

        static public void RegisterUndo(string name, params Object[] objects)
        {
            if (objects != null && objects.Length > 0)
            {
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
			UnityEditor.Undo.RegisterUndo(objects, name);
#else
                UnityEditor.Undo.RecordObjects(objects, name);
#endif
                foreach (Object obj in objects)
                {
                    if (obj == null) continue;
                    EditorUtility.SetDirty(obj);
                }
            }
        }

        static public void BeginContents()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(4f);
            EditorGUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(10f));
            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        static public void EndContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
            GUILayout.Space(3f);
        }

        static public bool DrawHeader(string text) { return DrawHeader(text, text, false); }

        static public bool DrawHeader(string text, string key, bool forceOn)
        {
            bool state = EditorPrefs.GetBool(key, true);

            GUILayout.Space(3f);
            if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(3f);

            GUI.changed = false;
#if UNITY_3_5
		if (state) text = "\u25B2 " + text;
		else text = "\u25BC " + text;
		if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
#else
            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25B2 " + text;
            else text = "\u25BC " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
#endif
            if (GUI.changed) EditorPrefs.SetBool(key, state);

            GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!forceOn && !state) GUILayout.Space(3f);
            return state;
        }
    }
}
