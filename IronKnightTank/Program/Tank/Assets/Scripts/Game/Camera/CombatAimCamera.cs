// ----------------------------------------------------------------------------
// <copyright file="UnitySceneManager.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>26/06/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Scripts.Game.Camera
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CombatAimCamera : MonoBehaviour
    {

        private Camera m_cam;
        // Use this for initialization
        void Start()
        {
            m_cam = GetComponent<Camera>();
        }

        public int RaycastTank(Vector2 screenPos)
        {
            Ray r = GetComponent<Camera>().ScreenPointToRay(screenPos);
            RayCastTankResult res = RayCastTankResult.None;
            RaycastHit[] hits = Physics.RaycastAll(r, 15);
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.CompareTag("TankBody"))
                    {
                        res = RayCastTankResult.HitBody;
                    }else if (hits[i].collider.CompareTag("TankTurret"))
                    {
                        res = RayCastTankResult.HitTurret;
                    }
                    else if (hits[i].collider.CompareTag("TankBodyWeakness"))
                    {
                        res = RayCastTankResult.HitWeakBody;
                    }
                    else if (hits[i].collider.CompareTag("TankTurretWeakness"))
                    {
                        res = RayCastTankResult.HitWeakTurret;
                    }
                }
            }
            else
            {
                res = RayCastTankResult.Miss;
            }
            return (int)res;
        }

        public enum RayCastTankResult
        {
            None = 0,
            HitTurret = 1,
            HitBody = 2,
            HitWeakTurret = 3,
            HitWeakBody = 4,
            Miss = 5,
        }
    }

}
