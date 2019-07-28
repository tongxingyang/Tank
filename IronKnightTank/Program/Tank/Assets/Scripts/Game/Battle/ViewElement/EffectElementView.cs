namespace Game.Battle.ViewElement
{
    using System;
    using System.Collections;

    using Assets.Tools.Script.Caller;

    using UnityEngine;

    using ParticleSystem = UnityEngine.ParticleSystem;
    [Serializable]
    public class EffectElementView : BaseActionElementView
    {
    
        public GameObject Particle;

        private ParticleSystem[] particleSystemArray;

        public override void Play()
        {
            StartCoroutine(this.play());

        }

        IEnumerator play()
        {
            yield return this.StartTime;
            if (this.particleSystemArray == null)
            {
                this.particleSystemArray = this.Particle.GetComponentsInChildren<ParticleSystem>();
            }
            this.Particle.SetActive(true);
            for (int i = 0; i < this.particleSystemArray.Length; i++)
            {
                this.particleSystemArray[i].Play();
            }
        }

        public override void Stop()
        {

            for (int i = 0; i < this.particleSystemArray.Length; i++)
            {
                this.particleSystemArray[i].Stop();
            }
            this.Particle.SetActive(false);
        }

#if UNITY_EDITOR
        public override void DrawInspector()
        {
            this.StartTime = UnityEditor.EditorGUILayout.FloatField("开始时间", this.StartTime);
            this.Particle = UnityEditor.EditorGUILayout.ObjectField(new GUIContent("粒子"), this.Particle, typeof(GameObject), true) as GameObject;
        }
#endif
    }
}