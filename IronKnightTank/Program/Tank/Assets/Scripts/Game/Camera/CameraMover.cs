using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Game.Tools;

using DG.Tweening;

using Game.Camera;

using UnityEngine;

// [RequireComponent(typeof(Camera))]
public class CameraMover : MonoBehaviour {
    
    [SerializeField]
    private Camera m_cam;

    private Transform m_tran;

    public float senstativeWidth = 20;
  
    public int moveSpeed = 10;

    private PanCamera panCam;

    private EdgeMoveCamera edgeMoveCam;

    private FollowCamra followCam;

    private List<BaseCameraMovement> curCameraMovement = new List<BaseCameraMovement>();

    private void Start()
    {
        this.panCam = this.gameObject.AddComponent<PanCamera>();
        this.panCam.Init(this.m_cam , this.transform);
        this.edgeMoveCam = this.gameObject.AddComponent<EdgeMoveCamera>();
        this.edgeMoveCam.Init(this.m_cam , this.transform);
        this.followCam = this.gameObject.AddComponent<FollowCamra>();
        this.followCam.Init(this.m_cam , this.transform);
        this.AddMovement(this.edgeMoveCam);
        this.AddMovement(this.panCam);
        this.edgeMoveCam.senstativeWidth = this.senstativeWidth;
        this.edgeMoveCam.moveSpeed = this.moveSpeed;
        this.m_tran = transform;
    }

    public void Follow(Transform tran )
    {
        this.followCam.Follow(tran);
        this.curCameraMovement.Clear();
        this.AddMovement(this.followCam);
    }

    
    public void StopFollow()
    {
        this.followCam.StopFollow();
        this.curCameraMovement.Clear();
        this.AddMovement(this.panCam);
        this.AddMovement(this.edgeMoveCam);
        
    }

    public void Focus(Vector3 pos , float time , System.Action callback = null)
    {
        Vector3 forward = this.m_tran.transform.forward;
        Vector3 camPos = this.m_tran.transform.position;
        Vector3 camTargetPos = pos + (camPos.y  - pos.y)* forward / forward.y;
        this.curCameraMovement.Clear();
        
        this.m_tran.DTMove(camTargetPos, time,
            () =>
                {
                    callback();
                    this.AddMovement(this.edgeMoveCam);
                    this.AddMovement(this.panCam);
                });
    }

    private void Update()
    {
        for (int i = 0; i < this.curCameraMovement.Count; i++)
        {
            this.curCameraMovement[i].CamUpdate();
        }
        
    }

  
    private void LateUpdate()
    {
        for (int i = 0; i < this.curCameraMovement.Count; i++)
        {
            this.curCameraMovement[i].CamLateUpdate();
        }
    }

    private void AddMovement(BaseCameraMovement movement)
    {
        movement.Reset();
        this.curCameraMovement.Add(movement);
    }

    private void OnDestroy()
    {
        this.panCam = null;
        this.edgeMoveCam = null;
        this.followCam = null;
    }
}
