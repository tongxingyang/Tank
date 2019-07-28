

namespace Game.Camera
{
    using UnityEngine;

    public class BaseCameraMovement : MonoBehaviour
    {

        protected Transform m_tran;

        protected Camera m_cam;

        public virtual void Init(Camera cam , Transform tran)
        {
            this.m_cam = cam;
            this.m_tran = tran;
        }

        public virtual void CamUpdate()
        {

        }

        public virtual void CamLateUpdate()
        {

        }

        public virtual void Reset()
        {
            
        }
    }
}
