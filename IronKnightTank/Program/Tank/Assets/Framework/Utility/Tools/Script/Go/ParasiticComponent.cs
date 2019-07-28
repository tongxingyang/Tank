using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.Script.Go
{
    
	using XQFramework.Scene;

    /// <summary>
    /// 用于挂工具类MonoBehaviour
    /// 简化工具类需要一个依附的gameobject和AddComponent这些流程
    /// </summary>
    public class ParasiticComponent : MonoBehaviour
    {
        private static GameObject _parasiteHost = null;
        /// <summary>
        /// 根节点
        /// </summary>
        public static GameObject parasiteHost
        {
            get
            {
                if (_parasiteHost == null)
                {
                    _parasiteHost = GlobalScene.CreateRoot("Util_ParasiteHost", new Vector3(6874, 9874, 7514));
                }
                return _parasiteHost;
            }
        }

        /// <summary>
        /// 是否存在二级节点
        /// </summary>
        /// <param name="name">节点名</param>
        /// <returns>System.Boolean.</returns>
        public static bool HasSecondaryHost(string name)
        {
            return _secondaryHosts.ContainsKey(name) && _secondaryHosts[name] != null;
        }

        /// <summary>
        /// 销毁一个二级节点
        /// </summary>
        /// <param name="name">节点名</param>
        public static void DestroySecondaryHost(string name)
        {
            GameObject secondaryHost = null;
            _secondaryHosts.TryGetValue(name, out secondaryHost);
            if (secondaryHost != null)
            {
                Destroy(secondaryHost);
            }
            _secondaryHosts.Remove(name);
        }

        /// <summary>
        /// 获得一个二级节点
        /// </summary>
        /// <param name="name">节点名</param>
        /// <returns>节点GameObject</returns>
        public static GameObject GetSecondaryHost(string name)
        {
            if (!_secondaryHosts.ContainsKey(name) || _secondaryHosts[name] == null)
            {
                GameObject secondaryHost = new GameObject();
                secondaryHost.transform.parent = parasiteHost.transform;
                secondaryHost.transform.localPosition = Vector3.zero;
                secondaryHost.name = name;

                _secondaryHosts[name] = secondaryHost;

            }
            return _secondaryHosts[name];
        }

        /// <summary>
        /// 在一个二级节点上挂一个工具类
        /// </summary>
        /// <typeparam name="T">工具</typeparam>
        /// <param name="name">节点名</param>
        /// <returns>工具</returns>
        public static T GetSecondaryHost<T>(string name) where T : UnityEngine.Component
        {
            GameObject secondaryHost = GetSecondaryHost(name);
            var component = secondaryHost.GetComponent<T>();
            if (component == null) component = secondaryHost.AddComponent<T>();
            return component;
        }

        private static readonly Dictionary<string, GameObject> _secondaryHosts = new Dictionary<string, GameObject>();
    }
}
