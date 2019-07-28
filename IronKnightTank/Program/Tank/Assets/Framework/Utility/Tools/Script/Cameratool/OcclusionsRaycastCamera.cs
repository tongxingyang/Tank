using System.Collections.Generic;
using Assets.Tools.Script.Debug.Console;
using UnityEngine;

namespace Assets.Tools.Script.Cameratool
{
    /// <summary>
    /// 检测摄像机到目标之间是否有Collider遮挡
    /// 如果存在，并且这个collider上有OcclusionsRaycastHandle，则调用OcclusionsRaycastHandle
    /// </summary>
    public class OcclusionsRaycastCamera : MonoBehaviour
    {
        /// <summary>
        /// 检测目标
        /// </summary>
        public GameObject target;
        /// <summary>
        /// 遮挡检测层，如果不填写，则对所有层进行
        /// </summary>
        public List<int> layers;

        private int _layer;
        private Dictionary<Collider, OcclusionsRaycastHandle> _intoLinecastTestHandles = new Dictionary<Collider, OcclusionsRaycastHandle>();
        private Dictionary<Collider, OcclusionsRaycastHandle> _curRaycastHits = new Dictionary<Collider, OcclusionsRaycastHandle>();
        private List<Collider> _clearLis = new List<Collider>();

        void Awake()
        {
            _layer = 0;
            if (layers==null)layers=new List<int>();
            foreach (var layer in layers)
            {
                _layer += (1 << layer);
            }
            DebugConsole.Log("Layers->", _layer);
        }

        void Update()
        {
            RaycastHit[] raycastHits = null;
            if (layers.Count <= 0)
                raycastHits = Physics.RaycastAll(target.transform.position, transform.position, Mathf.Infinity);
            else
                raycastHits = Physics.RaycastAll(target.transform.position, transform.position, Mathf.Infinity, _layer);
            
            //        foreach (var value in _intoLinecastTestHandles.Values)
            //        {
            //            if (value!=null)
            //                value.Linecasted = false;
            //        }


            _curRaycastHits.Clear();
            _clearLis.Clear();

            foreach (var raycastHit in raycastHits)
            {
                OcclusionsRaycastHandle occlusionsRaycastHandle = raycastHit.collider.GetComponent<OcclusionsRaycastHandle>();
                if (occlusionsRaycastHandle != null)
                {
                    _curRaycastHits.Add(raycastHit.collider, occlusionsRaycastHandle);
                }
            }

            foreach (var key in _intoLinecastTestHandles.Keys)
            {
                if (!_curRaycastHits.ContainsKey(key))
                {
                    _clearLis.Add(key);
                }
            }

            foreach (var key in _curRaycastHits.Keys)
            {
                if (!_intoLinecastTestHandles.ContainsKey(key))
                {
                    _curRaycastHits[key].IntoLinecast();
                    _intoLinecastTestHandles.Add(key, _curRaycastHits[key]);
                }
            }

            foreach (var key in _clearLis)
            {
                if (_intoLinecastTestHandles[key] == null) continue;
                _intoLinecastTestHandles[key].OutLinecast();
                _intoLinecastTestHandles.Remove(key);
            }
            //	    foreach (var raycastHit in raycastHits)
            //	    {
            //	        if (!_intoLinecastTestHandles.ContainsKey(raycastHit.collider))
            //	        {
            //	            LinecastTestHandle linecastTestHandle = raycastHit.collider.GetComponent<LinecastTestHandle>();
            //	            if (linecastTestHandle != null)
            //	            {
            //	                linecastTestHandle.Linecasted = true;
            //	                linecastTestHandle.IntoLinecast();
            //                    _intoLinecastTestHandles.Add(raycastHit.collider, linecastTestHandle);
            //	            }
            //	        }
            //	        else
            //	        {
            //	            LinecastTestHandle intoLinecastTestHandle = _intoLinecastTestHandles[raycastHit.collider];
            //                if(intoLinecastTestHandle!=null) intoLinecastTestHandle.Linecasted = true;
            //	        }
            ////	        DebugConsole.Log(raycastHit.collider.gameObject.name);
            //	    }
            //        List<LinecastTestHandle> linecastTestHandles = _intoLinecastTestHandles.Values.ToList();
            //        foreach (var linecastTestHandle in linecastTestHandles)
            //        {
            //            if (linecastTestHandle == null || !linecastTestHandle.enabled ||
            //                !linecastTestHandle.gameObject.activeInHierarchy)
            //            {
            //                
            //            }
            //        }
        }
    }
}