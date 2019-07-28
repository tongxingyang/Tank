using System.Collections.Generic;
using System.Reflection;
using Assets.Tools.Script.Event.Message;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Tools.Script.Editor
{
    public static class ActionCallEditor
    {
        class Entry
        {
            public MonoBehaviour target;
            public MethodInfo method;
        }

        /// <summary>
        /// Collect a list of usable delegates from the specified target game object.
        /// The delegates must be of type "void Delegate()".
        /// </summary>

        static List<Entry> GetMethods(GameObject target)
        {
            MonoBehaviour[] comps = target.GetComponents<MonoBehaviour>();

            List<Entry> list = new List<Entry>();

            for (int i = 0, imax = comps.Length; i < imax; ++i)
            {
                MonoBehaviour mb = comps[i];
                if (mb == null) continue;

                MethodInfo[] methods = mb.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);

                for (int b = 0; b < methods.Length; ++b)
                {
                    MethodInfo mi = methods[b];

                    if (mi.ReturnType == typeof(void) &&
                        ((mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == typeof(object)||
                          (mi.GetParameters().Length == 0
                              ))))
                    
                    {
                        string name = mi.Name;
                        if (name == "Invoke") continue;
                        if (name == "InvokeRepeating") continue;
                        if (name == "CancelInvoke") continue;
                        if (name == "StopCoroutine") continue;
                        if (name == "StopAllCoroutines") continue;
                        if (name == "BroadcastMessage") continue;
                        if (name.StartsWith("SendMessage")) continue;
                        if (name.StartsWith("set_")) continue;
                    
                        Entry ent = new Entry();
                        ent.target = mb;
                        ent.method = mi;
                        list.Add(ent);
                    
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Convert the specified list of delegate entries into a string array.
        /// </summary>

        static string[] GetMethodNames(List<Entry> list, string choice, out int index)
        {
            index = 0;
            string[] names = new string[list.Count + 1];
            names[0] = string.IsNullOrEmpty(choice) ? "<Choose>" : choice;

            for (int i = 0; i < list.Count; )
            {
                Entry ent = list[i];
                string type = ent.target.GetType().ToString();
                int period = type.LastIndexOf('.');
                if (period > 0) type = type.Substring(period + 1);

                string del = type + "." + ent.method.Name;
                names[++i] = del;

                if (index == 0 && string.Equals(del, choice))
                    index = i;
            }
            return names;
        }

        /// <summary>
        /// Draw an editor field for the Unity Delegate.
        /// </summary>

        static public bool Field(Object undoObject, MessageDelegate del)
        {
            return Field(undoObject, del, true);
        }

        /// <summary>
        /// Draw an editor field for the Unity Delegate.
        /// </summary>

        static public bool Field(Object undoObject, MessageDelegate del, bool removeButton)
        {
            if (del == null) return false;
            bool prev = GUI.changed;
            GUI.changed = false;
            bool retVal = false;
            MonoBehaviour target = del.target;
            bool remove = false;

            if (removeButton && (del.target != null || del.isValid))
            {
                if (del.target == null && del.isValid)
                {
                    EditorGUILayout.LabelField("Notify", del.ToString());
                }
                else
                {
                    target = EditorGUILayout.ObjectField("Notify", del.target, typeof(MonoBehaviour), true) as MonoBehaviour;
                }

                GUILayout.Space(-20f);
                GUILayout.BeginHorizontal();
                GUILayout.Space(64f);

#if UNITY_3_5
			if (GUILayout.Button("X", GUILayout.Width(20f)))
#else
                if (GUILayout.Button("", "ToggleMixed", GUILayout.Width(20f)))
#endif
                {
                    target = null;
                    remove = true;
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                target = EditorGUILayout.ObjectField("Notify", del.target, typeof(MonoBehaviour), true) as MonoBehaviour;
            }

            if (remove)
            {
                NEditorTools.RegisterUndo("Delegate Selection", undoObject);
                del.Clear();
                EditorUtility.SetDirty(undoObject);
            }
            else if (del.target != target)
            {
                NEditorTools.RegisterUndo("Delegate Selection", undoObject);
                del.target = target;
                EditorUtility.SetDirty(undoObject);
            }

            if (del.target != null && del.target.gameObject != null)
            {
                GameObject go = del.target.gameObject;
                List<Entry> list = GetMethods(go);

                int index = 0;
                string[] names = GetMethodNames(list, del.ToString(), out index);
                int choice = 0;

                GUILayout.BeginHorizontal();
                choice = EditorGUILayout.Popup("Method", index, names);
                GUILayout.Space(18f);
                GUILayout.EndHorizontal();

                if (choice > 0)
                {
                    if (choice != index)
                    {
                        Entry entry = list[choice - 1];
                        NEditorTools.RegisterUndo("Delegate Selection", undoObject);
                        del.target = entry.target;
                        del.methodName = entry.method.Name;
                        EditorUtility.SetDirty(undoObject);
                        GUI.changed = prev;
                        return true;
                    }
                }
            }

            retVal = GUI.changed;
            GUI.changed = prev;
            return retVal;
        }

        /// <summary>
        /// Draw a list of fields for the specified list of delegates.
        /// </summary>

        static public void Field(Object undoObject, List<MessageDelegate> list)
        {
            Field(undoObject, list, null, null);
        }

        /// <summary>
        /// Draw a list of fields for the specified list of delegates.
        /// </summary>

        static public void Field(Object undoObject, List<MessageDelegate> list, string noTarget, string notValid)
        {
            bool targetPresent = false;
            bool isValid = false;

            // Draw existing delegates
            for (int i = 0; i < list.Count; )
            {
                MessageDelegate del = list[i];

                if (del == null || (del.target == null && !del.isValid))
                {
                    list.RemoveAt(i);
                    continue;
                }

                Field(undoObject, del);
                EditorGUILayout.Space();

                if (del.target == null && !del.isValid)
                {
                    list.RemoveAt(i);
                    continue;
                }
                else
                {
                    if (del.target != null) targetPresent = true;
                    isValid = true;
                }
                ++i;
            }

            // Draw a new delegate
            MessageDelegate newDel = new MessageDelegate();
            Field(undoObject, newDel);

            if (newDel.target != null)
            {
                targetPresent = true;
                list.Add(newDel);
            }

            if (!targetPresent)
            {
                if (!string.IsNullOrEmpty(noTarget))
                {
                    GUILayout.Space(6f);
                    EditorGUILayout.HelpBox(noTarget, MessageType.Info, true);
                    GUILayout.Space(6f);
                }
            }
            else if (!isValid)
            {
                if (!string.IsNullOrEmpty(notValid))
                {
                    GUILayout.Space(6f);
                    EditorGUILayout.HelpBox(notValid, MessageType.Warning, true);
                    GUILayout.Space(6f);
                }
            }
        }
    }
}
