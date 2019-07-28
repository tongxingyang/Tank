

namespace Game.Tank
{
    using System;
    using Assets.Scripts.Game.Tools;
    using UnityEngine;
    using UnityEngine.UI;

    public class TankMesh : MonoBehaviour
    {
        [SerializeField]
        private Transform turreTransform;
        [SerializeField]
        private Transform bodyTransform;
        [SerializeField]
        private Transform trackTransform;
        [SerializeField]
        private Transform gunTransform;
        private MeshRenderer turretMeshRenderer;

        private MeshRenderer bodyMeshRenderer;

        private MeshRenderer trackMeshRenderer;

        private MeshRenderer gunMeshRenderer;

        private Material defaultMaterial;

        public GameObject MoveEffectObj;


        private void Awake()
        {

        }

        private void Init()
        {
            this.turretMeshRenderer = this.turreTransform.GetComponent<MeshRenderer>();
            this.bodyMeshRenderer = this.bodyTransform.GetComponent<MeshRenderer>();
            this.trackMeshRenderer = this.trackTransform.GetComponent<MeshRenderer>();
            this.gunMeshRenderer = this.gunTransform.GetComponent<MeshRenderer>();
        }

        public void RotateTurret(float angle, float time ,bool timeSplit ,  System.Action callBack = null)
        {
            if (timeSplit)
            {
                var interval = Math.Abs(this.turreTransform.eulerAngles.y - angle) / 180;
                time = time * interval;
            }
            Vector3 vecAngle = new Vector3(this.turreTransform.eulerAngles.x , angle , this.turreTransform.eulerAngles.z);
            this.turreTransform.DTRotate(vecAngle, time, callBack);
        }

        public void RevertTurret( float time , Action callBack = null)
        {
            var angle = this.turreTransform.localEulerAngles;
            angle.y = 0;
            this.turreTransform.DTLocalRotate(angle, time, callBack);
        }

        public void SetMaterial(Material mat)
        {
            if (!this.turretMeshRenderer)
            {
                this.Init();
            }
            this.turretMeshRenderer.material = mat;
            this.trackMeshRenderer.material = mat;
            this.bodyMeshRenderer.material = mat;
            this.gunMeshRenderer.material = mat;
        }

        public void SetDefaultMaterila(Material mat)
        {
            this.defaultMaterial = mat;
            this.SetMaterial(mat);
        }

        public void ResetMaterial()
        {
            this.SetMaterial(this.defaultMaterial);
        }

        public void OpenMoveEffect()
        {
            if(this.MoveEffectObj != null)
            this.MoveEffectObj.SetActive(true);
        }

        public void CloseMoveEffect()
        {
            if (this.MoveEffectObj != null)
                this.MoveEffectObj.SetActive(false);
        }

        private void OnDestroy()
        {
            this.turreTransform = null;
            this.trackTransform = null;
            this.bodyTransform = null;
            this.turretMeshRenderer = null;
            this.trackMeshRenderer = null;
            this.bodyMeshRenderer = null;
            this.defaultMaterial = null;
            this.MoveEffectObj = null;
        }
    }
}
