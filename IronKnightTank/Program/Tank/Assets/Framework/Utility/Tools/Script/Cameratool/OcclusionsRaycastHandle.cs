using UnityEngine;

namespace Assets.Tools.Script.Cameratool
{
    /// <summary>
    /// 配合OcclusionsRaycastCamera用于处理目标进入遮挡物和离开遮挡物时遮挡物的表现
    /// </summary>
    public abstract class OcclusionsRaycastHandle : MonoBehaviour
    {
        public abstract void IntoLinecast();
        public abstract void OutLinecast();
    }
}


