namespace Game.Battle.ViewElement
{
    using System;
    using UnityEngine;
    using System.Collections;
    using Assets.Tools.Script.Caller;

    [Serializable]
    public class AnimationElementView : BaseActionElementView
    {

        public Animation Animation;

        public AnimationClip AnimationClip;

        public override void Play()
        {
            CoroutineCall.Call(this.play);

        }

        IEnumerator play()
        {
            yield return new WaitForSeconds(this.StartTime);

            this.Animation.clip = this.AnimationClip;
            this.Animation.Play();
        }

        public override void Stop()
        {
            this.Animation.Stop();
        }

#if UNITY_EDITOR
        public override void DrawInspector()
        {
            base.DrawInspector();
            this.StartTime = UnityEditor.EditorGUILayout.FloatField("开始时间", this.StartTime);
            this.Animation = UnityEditor.EditorGUILayout.ObjectField(new GUIContent("动画"), this.Animation, typeof(Animation), true) as Animation;
            this.AnimationClip = UnityEditor.EditorGUILayout.ObjectField(new GUIContent("动画片"), this.AnimationClip, typeof(AnimationClip), true) as AnimationClip;
        }
#endif
    }
}