namespace Game.Battle.ActionView
{
    using System;

    using UnityEngine;

    [Serializable]
    public class LoopActionView : BaseActionView
    {
        // public ActionViewType ViewType = ActionViewType.Loop;
        public override ActionViewType ViewType
        {
            get
            {
                return  ActionViewType.Loop;
            }
        }

        public override void Play()
        {
            for (int i = 0; i < this.ElementViewList.Count; i++)
            {
                this.ElementViewList[i].Play();
            }
        }


        public override void Stop()
        {
            for (int i = 0; i < this.ElementViewList.Count; i++)
            {
                this.ElementViewList[i].Stop();
            }
        }

#if UNITY_EDITOR
        public override void DrawInspector()
        {
            base.DrawInspector();
            if (UnityEditor.EditorApplication.isPlaying)
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Play"))
                    {
                        this.Play();
                    }
                    if (GUILayout.Button("Stop"))
                    {
                        this.Stop();
                    }
                }
                GUILayout.EndHorizontal();
            }
             
        }
#endif
   }
}