namespace Game.Battle.ActionView
{
    using System;
    using System.Collections.Generic;

    using Assets.Tools.Script.Reflec;

    using Game.Battle.ViewElement;

    using UnityEngine;

    [Serializable]
    public class BaseActionView  : MonoBehaviour
    {

        public List<BaseActionElementView> ElementViewList = new List<BaseActionElementView>();

        public virtual ActionViewType ViewType { get; private set; }

        public virtual void Play()
        {
            
        }

        public virtual void Stop()
        {

        }

        public static BaseActionView CreateActionView(Transform parent , Type actionViewType , string name)
        {
            BaseActionView view = new GameObject(name).AddComponent(actionViewType) as BaseActionView;
            view.transform.SetParent(parent);
            view.transform.localPosition = Vector3.zero;
            return  view;
        }

        public enum ActionViewType
        {
            Once =1 ,
            Loop = 2 ,
        }


#if UNITY_EDITOR
        
        
        public virtual void DrawInspector()
        {
            UnityEditor.EditorGUILayout.EnumPopup("表现类型", this.ViewType);
            GUILayout.Space(5);
            List<BaseActionElementView> removeList = new List<BaseActionElementView>();
            for (int i = 0; i < this.ElementViewList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    {
                        this.ElementViewList[i].DrawInspector();
                    }
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    {
                        if (GUILayout.Button("×" , GUILayout.Width(20)))
                        {
                            removeList.Add(this.ElementViewList[i]);
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
               
                
            }
            GUILayout.EndVertical();
            for (int i = 0; i < removeList.Count; i++)
            {
                this.ElementViewList.Remove(removeList[i]);
            }

            if (GUILayout.Button("增加"))
            {
                UnityEditor.GenericMenu menu = new UnityEditor.GenericMenu();
                List<Type> list = AssemblyTool.FindTypesInCurrentDomainWhereExtend<BaseActionElementView>();
                for (int i = 0; i < list.Count; i++)
                {
                    Type t = list[i];
                    string name = list[i].ToString().Replace("ElementView", "");
                    name = name.Substring(name.LastIndexOf(".") + 1) ;
                    if (list[i] == typeof(BaseActionElementView))
                    {
                        continue;
                    }
                    menu.AddItem(new GUIContent(name),false , ()=>this.ElementViewList.Add(BaseActionElementView.CreateElement(this , t)));
                }
                menu.ShowAsContext();
            }
        }
#endif

    }
}
