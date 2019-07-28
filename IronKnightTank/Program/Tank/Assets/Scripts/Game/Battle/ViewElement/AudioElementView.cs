namespace Game.Battle.ViewElement
{
    using UnityEngine;

    public class AudioElementView : BaseActionElementView
    {
        public AudioClip Clip;

        public AudioSource Source;

        public override void Play()
        {
            base.Play();
            
        }
#if UNITY_EDITOR
        public override void DrawInspector()
        {
            GUILayout.Label("目前还米支持");
            base.DrawInspector();
        }
#endif
    }
}