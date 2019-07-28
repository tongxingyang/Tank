namespace Game.Camera
{
    using UnityEngine;

    public class EdgeMoveCamera : BaseCameraMovement {

        /// <summary>
        /// 镜头探测宽度
        /// </summary>
        public float senstativeWidth = 20;

        /// <summary>
        /// 镜头移动速度
        /// </summary>
        public float moveSpeed = 10;

        public override void CamUpdate()
        {
            base.CamUpdate();
            Vector3 v = Vector3.zero;
            if ((Input.mousePosition.x - Screen.width) >= (-1 * senstativeWidth) && (Input.mousePosition.x <= Screen.width))
            {
                v = m_tran.TransformVector(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
                //this.m_tran.Translate(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
            }
            else if (Input.mousePosition.x <= senstativeWidth && Input.mousePosition.x >= 0)
            {
                v = m_tran.TransformVector(new Vector3(moveSpeed * -1, 0, 0) * Time.deltaTime);
                //this.m_tran.Translate(new Vector3(moveSpeed * -1, 0, 0) * Time.deltaTime , Space.Self);
            }
            else if (Input.mousePosition.y <= senstativeWidth && Input.mousePosition.y >= 0)
            {
                v = this.m_tran.TransformVector(new Vector3(0, moveSpeed * -1, 0) * Time.deltaTime);
                //this.m_tran.position = this.transform.position + new Vector3(v.x , 0 ,v.z);
            }
            else if ((Input.mousePosition.y - Screen.height) >= (senstativeWidth * -1) && Input.mousePosition.y <= Screen.height)
            {
                v = this.m_tran.TransformVector(new Vector3(0, moveSpeed, 0) * Time.deltaTime);
                //this.m_tran.position = this.transform.position + new Vector3(v.x, 0, v.z);
            }
            this.m_tran.position = this.transform.position + new Vector3(v.x, 0, v.z);
        }

        public override void CamLateUpdate()
        {
            base.CamLateUpdate();
        }


    }
}
