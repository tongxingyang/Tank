namespace Game.Camera
{
    using Assets.Scripts.Game.Tools;
    using UnityEngine;
    using Camera = UnityEngine.Camera;

    public class PanCamera : BaseCameraMovement
    {
        private Plane xzPlane;
        public Ray ray;
        private bool inTouch = false;
        private bool dragged = false;
        private Vector3 mousePosPrev = Vector3.zero;
        private Vector3 mousePosStart = Vector3.zero;
        private Vector3 vPickStart = Vector3.zero;
        private Vector3 vPickOld = Vector3.zero;
        private Vector3 vCamRootPosOld = Vector3.zero;
        private float DragCheckMin = 0.1f;
        private Vector3 CameraPanDir = Vector3.zero;


        public override void Init(Camera cam, Transform tran)
        {
            base.Init(cam, tran);
            xzPlane = new Plane(new Vector3(0f, 1f, 0f), 0f);
        }

        public override void CamUpdate()
        {
            Vector3 mousePosition = Input.mousePosition;
            this.ray = this.m_cam.ScreenPointToRay(mousePosition);
            float distance = default(float);
            if (Input.GetMouseButton(0))
            {
                if (UGUITools.IsPointerOverUIObject())
                {
                    return;
                }
                this.xzPlane.Raycast(this.ray, out distance);
                if (!this.inTouch)
                {
                    this.inTouch = true;
                    this.dragged = false;
                    this.mousePosPrev = (this.mousePosStart = mousePosition);
                    this.vPickStart = this.ray.GetPoint(distance) - this.m_tran.position;
                    this.vPickOld = this.vPickStart;
                    this.vCamRootPosOld = this.transform.position;
                }
                else if (Input.touchCount < 2)
                {
                    if (Vector3.Distance(mousePosition, this.mousePosStart) > DragCheckMin)
                    {
                        if (!this.dragged)
                        {
                            this.dragged = true;
                        }
                        if (Vector3.Distance(mousePosition, this.mousePosPrev) > this.DragCheckMin)
                        {
                            Vector3 a = this.ray.GetPoint(distance) - this.transform.position;
                            this.CameraPanDir = a - this.vPickStart;
                            this.SetCameraPosition(this.vCamRootPosOld - this.CameraPanDir);
                            this.vPickOld = a;
                        }
                    }
                    else if (this.dragged)
                    {
                        Vector3 a2 = this.ray.GetPoint(distance) - transform.position;
                        this.vPickOld = a2;
                    }
                    else if (!this.dragged)
                    {

                    }
                    this.mousePosPrev = mousePosition;
                }
            }
            else if (this.inTouch)
            {
                this.inTouch = false;
            }
        }

        public void SetCameraPosition(Vector3 vPos)
        {
            //if (this.borderType == BorderType.Rect)
            //{
            //    vPos.x = Mathf.Clamp(vPos.x, this.XMin, this.XMax);
            //    vPos.z = Mathf.Clamp(vPos.z, this.ZMin, this.ZMax);
            //}
            //else if (this.borderType == BorderType.Circle)
            //{
            //    Vector3 a = vPos;
            //    a.y = 0f;
            //    float magnitude = a.magnitude;
            //    if (magnitude > this.CircleBorderRadius)
            //    {
            //        a.Normalize();
            //        vPos = a * this.CircleBorderRadius;
            //    }
            //}
            this.m_tran.position = vPos;
        }
    }
}
