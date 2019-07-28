namespace Game.Battle.ViewElement
{
    using System;

    using Game.Battle.ActionView;

    using UnityEngine;

    [Serializable]
    public class BaseActionElementView : MonoBehaviour
    {
        public float StartTime = 0;

        public virtual void Play()
        {

        }

        public virtual void Stop()
        {

        }


        public static BaseActionElementView  CreateElement(BaseActionView ownerView, System.Type elementType)
        {
            BaseActionElementView elementView = new GameObject().AddComponent(elementType) as BaseActionElementView;
            elementView.transform.SetParent(ownerView.transform);
            elementView.transform.localPosition = Vector3.zero;
            return elementView;
            // return null;
        }


#if UNITY_EDITOR

        public virtual void DrawInspector()
        {
            
        }

#endif
    }
}