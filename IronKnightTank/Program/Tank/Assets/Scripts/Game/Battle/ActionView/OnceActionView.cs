namespace Game.Battle.ActionView
{
    using System;
    using System.Collections;

    using Assets.Tools.Script.Caller;

    using UnityEngine;

    public class OnceActionView : BaseActionView
    {
        public override  ActionViewType ViewType
        {
            get
            {
                return ActionViewType.Once;
            }
        }
    
        private Action m_back;
        public float Time;

        public override void Play()
        {
            CoroutineCall.Call(this.play());
        }

        public void Play(Action callBack)
        {
            this.OnComplete(callBack);
            this.Play();
        }

        public void OnComplete(Action callBack)
        {
            this.m_back = callBack;
        }

        IEnumerator play()
        {
            for (int i = 0; i < this.ElementViewList.Count; i++)
            {
                this.ElementViewList[i].Play();
            }
            yield return new WaitForSeconds(this.Time);
            if (this.m_back != null)
            {
                this.m_back();
            }
            for (int i = 0; i < this.ElementViewList.Count; i++)
            {
                this.ElementViewList[i].Stop();
            }
        }


#if  UNITY_EDITOR

        public override void DrawInspector()
        {
            this.Time = UnityEditor.EditorGUILayout.FloatField("总时长", this.Time);
            base.DrawInspector();
            if (UnityEditor.EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Play"))
                {
                    this.Play();
                }
            }
            
        }

#endif
    }


    
}