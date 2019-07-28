//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Tools.Script.Helper
{
    public class GameObjectUtilities
    {
        /// <summary>
        /// �����������Bounds�߿�
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Bounds GetBoundsWithChildren(GameObject obj)
        {
            Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
            if (componentsInChildren.Length == 0)
                throw new Exception("no child in this gameobject");
            Vector3 min = componentsInChildren[0].bounds.min;
            Vector3 max = componentsInChildren[0].bounds.max;
            for (int i = 1; i < componentsInChildren.Length; i++)
            {
                var componentsInChild = componentsInChildren[i];
                if (componentsInChild.bounds.min.x < min.x)
                    min.x = componentsInChild.bounds.min.x;
                if (componentsInChild.bounds.min.y < min.y)
                    min.y = componentsInChild.bounds.min.y;
                if (componentsInChild.bounds.min.z < min.z)
                    min.z = componentsInChild.bounds.min.z;

                if (componentsInChild.bounds.max.x > max.x)
                    max.x = componentsInChild.bounds.max.x;
                if (componentsInChild.bounds.max.y > max.y)
                    max.y = componentsInChild.bounds.max.y;
                if (componentsInChild.bounds.max.z > max.z)
                    max.z = componentsInChild.bounds.max.z;
            }
            Bounds bounds = new Bounds((min + max) / 2, new Vector3(Math.Abs(min.x - max.x), Math.Abs(min.y - max.y), Math.Abs(min.z - max.z)));
            return bounds;
        }
        /// <summary>
        /// �����Ǽ���״̬
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsActive(GameObject obj)
        {
            return obj && obj.activeInHierarchy;
        }
        /// <summary>
        /// ���Parent GameObject
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static GameObject Parent(GameObject obj)
        {
            if (obj == null || obj.transform.parent == null)
                return null;
            return obj.transform.parent.gameObject;
        }
        /// <summary>
        /// �������٣�DestroyImmediate�������ӽڵ�
        /// </summary>
        /// <param name="parent"></param>
        public static void ClearChildrenImmediate(GameObject parent)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in parent.transform)
            {
                children.Add(child.gameObject);
            }
            foreach (GameObject child in children)
            {
                GameObject.DestroyImmediate(child);
            }
        }

        /// <summary>
        /// Destroy�����ӽڵ�
        /// </summary>
        /// <param name="parent"></param>
        public static void ClearChildren(GameObject parent)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in parent.transform)
            {
                children.Add(child.gameObject);
            }
            foreach (GameObject child in children)
            {
                GameObject.Destroy(child);
            }
        }

        /// <summary>
        /// ���һ���յ�GameObject��ָ���ڵ���
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject AddEmptyGameObject(GameObject parent)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = parent.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            obj.layer = parent.layer;
            return obj;
        }
        /// <summary>
        /// ʵ����һ��GameObject��ָ���ڵ���
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="src">��ʵ�����Ķ���</param>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <returns></returns>
        public static GameObject AddInstantiate(GameObject parent, GameObject src, bool active = false)
        {
            return AddInstantiate(parent, src, Vector3.zero, Vector3.one, Quaternion.identity, true, active);
        }
        /// <summary>
        /// ʵ����һ��GameObject��ָ���ڵ���
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="src">��ʵ�����Ķ���</param>
        /// <param name="position">The position.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="quaternion">The quaternion.</param>
        /// <param name="withParentLayer">if set to <c>true</c> [with parent layer].</param>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <returns></returns>
        public static GameObject AddInstantiate(GameObject parent, GameObject src, Vector3 position, Vector3 scale, Quaternion quaternion, 
            bool withParentLayer = true,  bool active = false)
        {
            GameObject obj = GameObject.Instantiate(src) as GameObject;
            obj.transform.parent = parent.transform;
            obj.transform.localPosition = position.IsTNull() ? Vector3.zero : position;
            obj.transform.localScale = scale.IsTNull() ? Vector3.one : scale;
            obj.transform.localRotation = quaternion.IsTNull() ? Quaternion.identity : quaternion;
            if (withParentLayer && obj.layer != parent.layer) SetLayerWidthChildren(obj, parent.layer);
            if (active && !obj.activeSelf) obj.SetActive(true);
            return obj;
        }
        /// <summary>
        /// ���������һ���Ľڵ�
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<GameObject> GetChildren(GameObject parent)
        {
            List<GameObject> list = new List<GameObject>();
            foreach (Transform child in parent.transform)
            {
                list.Add(child.gameObject);
            }
            return list;
        }
        /// <summary>
        /// ����һ���ڵ���Ѱ��
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject GetChildByName(GameObject parent, string name)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.gameObject.name == name)
                    return child.gameObject;
            }
            return null;
        }
        /// <summary>
        /// Ѱ�Ҹ��������ڵ���������ͬ��
        /// </summary>
        /// <param name="child"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject GetParentByName(GameObject child, string name)
        {
            if (child.name == name)
                return child;
            else if (child.transform.parent != null)
                return GetParentByName(child.transform.parent.gameObject, name);
            else
                return null;
        }
        /// <summary>
        /// ������ڵ���ӽڵ�
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool IsChildBy(GameObject child, GameObject parent)
        {
            foreach (Transform componentInChild in parent.GetComponentsInChildren<Transform>())
            {
                if (componentInChild.gameObject == child)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// ������child�Լ�child��child�в��ҵ�һ�����ҵ����ӽڵ�
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform GetChildByNameRecursion(Transform parent, string name)
        {
            Transform t = parent.transform.Find(name);
            if (t == null)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    t = GetChildByNameRecursion(parent.transform.GetChild(i), name);
                    if (t != null)
                        return t;
                }
                return null;
            }
            else
            {
                return t;
            }
        }
        /// <summary>
        /// ���������layer���������е�������)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="layer"></param>
        public static void SetLayerWidthChildren(GameObject parent, int layer)
        {
            parent.layer = layer;

            Transform t = parent.transform;

            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                SetLayerWidthChildren(child.gameObject, layer);
            }
        }
        /// <summary>
        /// ��������Ϊ���������ɼ�
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="camera"></param>
        public static void SetLayerToCameraCullingMask(GameObject obj, Camera camera)
        {
            if (camera.cullingMask == 0) return;
            int currLayer = 1 << obj.layer;
            int uiCameraCullingMask = camera.cullingMask;
            if ((currLayer & uiCameraCullingMask) == 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    if (((1 << i) & uiCameraCullingMask) != 0)
                    {
                        SetLayerWidthChildren(obj,i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Fors the each all children.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="doFunc">The do function.</param>
        public static void ForEachAllChildren(Transform obj, Func<Transform, bool> doFunc)
        {
            TraverseTransform(obj, doFunc);
        }

        private static bool TraverseTransform(Transform obj, Func<Transform, bool> doFunc)
        {
            if (!doFunc(obj))
            {
                return false;
            }
            for (int i = 0; i < obj.childCount; i++)
            {
                var transform = obj.GetChild(i);
                if (!TraverseTransform(transform, doFunc))
                {
                    return false;
                }
            }
            return true;
        }

        public static void NormalizationGameObject(GameObject obj)
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
        }

        public static void NormalizationTransform(Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        public static void SetTransformParentAndNormalization(Transform transform, Transform parent)
        {
            //transform.parent = parent;
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        public static void SetTransformParentAndDisactive(Transform transform, Transform parent)
        {
            //transform.parent = parent;
            transform.SetParent(parent);
            transform.gameObject.SetActive(false);
        }

        public static void SetGameObjectParentAndNormalization(GameObject obj, Transform parent)
        {
            //obj.transform.parent = parent;
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
        }

        public static void SetGameObjectParentSameLayer(GameObject obj, Transform parent)
        {
            //obj.transform.parent = parent;
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            if (obj.layer != parent.gameObject.layer)
            {
                SetLayerWidthChildren(obj, parent.gameObject.layer);
            }
        }

        public static void SetTransformProperty(Transform transform, string name, Transform parent,
            float localPositionX, float localPositionY, float localPositionZ,
            float localScaleX, float localScaleY, float localScaleZ)
        {
            transform.name = name;
            //transform.parent = parent;
            transform.SetParent(parent);
            transform.localPosition = new Vector3(localPositionX, localPositionY, localPositionZ);
            transform.localScale = new Vector3(localScaleX, localScaleY, localScaleZ);
        }

        public static void SetGameObjectActive(bool active, params GameObject[] objs)
        {
            foreach (var gameObject in objs)
            {
                gameObject.SetActive(active);
            }
        }
    }
}
