

namespace Game.Camera
{
    using UnityEngine;

    public class FollowCamra : BaseCameraMovement
    {

        private Transform targetTransform;

        private Vector3 nextStepPos;

        public override void CamUpdate()
        {
            Vector3 forward = this.m_tran.transform.forward;
            Vector3 camPos = this.m_tran.transform.position;
            
            this.nextStepPos  = this.targetTransform.position + (camPos.y - this.targetTransform.position.y) * forward / forward.y ;
            // Debug.Log("pos " + nextStepPos.ToString()+ forward.ToString());
        }

        public override void CamLateUpdate()
        {
            this.m_tran.position = this.nextStepPos;
        }
    

        public void Follow(Transform target)
        {
            this.nextStepPos = this.m_tran.position;
            this.targetTransform = target;
        }

        public void StopFollow()
        {
            this.targetTransform = null;
        }

        private void OnDestroy()
        {
            this.targetTransform = null;
        }
    }
}
