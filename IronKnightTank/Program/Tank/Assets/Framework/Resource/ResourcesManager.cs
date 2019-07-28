// ----------------------------------------------------------------------------
// <copyright file="UnitySceneManager.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>12/07/2018</date>
// ----------------------------------------------------------------------------

using Assets.Tools.Script.File;

namespace XQFramework.Resource
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using UObject = UnityEngine.Object;
    using System.IO;
    using Assets.Tools.Script.Caller;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    public class ResourcesManager
    {
        private static ResourcesManager _instance;

        public static ResourcesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ResourcesManager();
                }

                return _instance;
            }
        }


        private string m_abBasePath = string.Empty;
        private Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
        private Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();

        private Dictionary<string, List<LoadAssetRequest>> m_LoadRequests =  new Dictionary<string, List<LoadAssetRequest>>();

        private Dictionary<string, List<Action<UObject[]>>> m_loadAllRequests =  new Dictionary<string, List<Action<UObject[]>>>();

        private Dictionary<string, string> m_assetAbNameDic = new Dictionary<string, string>();

        private Dictionary<string, string> m_abHashNameDic = null;

        //private Dictionary<string, AssetABPathInfo> m_assetPathInfoDic = new Dictionary<string, AssetABPathInfo>();
        private AssetBundleManifest m_AssetBundleManifest = null;
        private string ResRootDir = string.Empty;

        public IEnumerator Initialize()
        {
            ResRootDir = FrameworkConst.GameResourceRootDir;
            if (!FrameworkConst.DebugMode)
            {
                m_abBasePath = PlatformPath.DataPath;
                AnalysisAssetInfo();
                yield return InitManifest();
            }
        }

        public void LoadAsset<T>(string assetPath, Action<UObject> action = null) where T : UObject
        {
            assetPath = ResRootDir + assetPath;
#if UNITY_EDITOR
            if (FrameworkConst.DebugMode)
            {
                UObject obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (obj != null)
                {
                    action(obj);
                }
                else
                {
                    action(null);
                    Debug.Log("asset not exit " + assetPath);
                }
                return;
            }
#endif

            string abName = GetAbName(assetPath);
            if (string.IsNullOrEmpty(abName))
            {
                return;
            }
            string assetName = assetPath.Substring(assetPath.LastIndexOf("/") + 1);
            LoadAsset<T>(abName, assetName, action);
        }

        public void LoadObject(string path, Action<UObject> action = null)
        {
            LoadAsset<UObject>(path, action);
        }

        public void LoadScene(string scenePath, Action<UObject> action = null)
        {
            LoadAsset<UObject>(scenePath, action);
            //TODO  scenebundle 应该有指定的规则 现在还不确定  规则明确了再去做
        }

        //public void LoadAssetForLua(string path , Action<UObject> action = null){
        //	LoadAsset<UObject> (path, action);
        //}

        public void LoadText(string textPath, Action<UObject> action)
        {
            LoadAsset<TextAsset>(textPath, action);
        }

   public void LoadTexture(string path, Action<Texture> action = null)
        {
            LoadAsset<Texture>(path, delegate (UObject obj)
            {
                Texture t = obj as Texture;
                if (t != null)
                {
                    action(t);
                }
                else
                {
                    Debug.Log("texture not exit " + path);
                }
            });
        }

        public void LoadPrefab(string path, Action<GameObject> action = null)
        {
            LoadAsset<GameObject>(path, delegate (UObject obj)
            {
                GameObject t = obj as GameObject;
                if (t)
                {

                }
                if (t != null)
                {
                    action(t);
                }
                else
                {
                    Debug.Log("prefab not exit " + path);
                }
            });
        }

        public void LoadSprite(string path, Action<Sprite> action = null)
        {
            LoadAsset<Sprite>(path, delegate (UObject obj)
            {
                Sprite t = obj as Sprite;
                if (t != null)
                {
                    action(t);
                }
                else
                {
                    Debug.Log("Sprite not exit " + path);
                }
            });
        }

        public void LoadMaterial(string path, Action<Material> action = null)
        {
            LoadAsset<Material>(path, delegate (UObject obj)
            {
                Material t = obj as Material;
                if (t != null)
                {
                    action(t);
                }
                else
                {
                    Debug.Log("Material not exit " + path);
                }
            });
        }

        public void LoadAudioClip(string path, Action<AudioClip> action = null)
        {
            LoadAsset<AudioClip>(path, delegate (UObject obj)
            {
                AudioClip t = obj as AudioClip;
                if (t != null)
                {
                    action(t);
                }
                else
                {
                    Debug.Log("AudioClip not exit " + path);
                }
            });
        }

        public void LoadAllAB(string abName, Action<UObject[]> action = null)
        {
#if UNITY_EDITOR
            if (FrameworkConst.DebugMode)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(abName);
                if (assetPaths.Length > 0)
                {
                    List<UObject> list = new List<UObject>();
                    foreach (var path in assetPaths)
                    {
                        if (path.EndsWith(".unity"))
                        {
                            list.Add(AssetDatabase.LoadMainAssetAtPath(path));
                        }
                        else
                        {
                            list.Add(AssetDatabase.LoadMainAssetAtPath(path));
                        }
                    }
                    action(list.ToArray());
                }
                return;
            }
#endif
            abName = GetRealAbName(abName);
            List<Action<UObject[]>> callBackList = null;
            if (m_loadAllRequests.TryGetValue(abName, out callBackList))
            {
                callBackList.Add(action);
            }
            else
            {
                callBackList = new List<Action<UObject[]>>() { action };
                m_loadAllRequests[abName] = callBackList;
                CoroutineCall.Call(OnLoadAsset<UObject>(abName));
            }

        }

        private void LoadAsset<T>(string abName, string assetName, Action<UObject> action = null) where T : UObject
        {
#if UNITY_EDITOR
            if (FrameworkConst.DebugMode)
            {
                string[] assetPath = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, assetName);
                if (assetPath.Length > 0)
                {
                    action(AssetDatabase.LoadMainAssetAtPath(assetPath[0]));
                }
                return;
            }
#endif
            LoadAssetRequest request = new LoadAssetRequest();
            request.assetType = typeof(T);
            request.assetName = assetName;
            request.callBack = action;
            abName = GetRealAbName(abName);
            List<LoadAssetRequest> requests = null;
            if (!m_LoadRequests.TryGetValue(abName, out requests))
            {
                requests = new List<LoadAssetRequest>();
                requests.Add(request);
                m_LoadRequests.Add(abName, requests);
                CoroutineCall.Call(OnLoadAsset<T>(abName));
            }
            else
            {
                requests.Add(request);
            }
        }

        IEnumerator InitManifest()
        {
            string mainfestPath = GetRealAssetPath("StreamingAssets");
            AssetBundleCreateRequest acr = AssetBundle.LoadFromFileAsync(mainfestPath);
            yield return acr;
            AssetBundle ab = acr.assetBundle;
            UObject obj = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            if (obj != null)
            {
                m_AssetBundleManifest = obj as AssetBundleManifest;
                if (m_AssetBundleManifest != null)
                {
                    Debug.Log(" init successs");
                }
                else
                {
                    Debug.Log(" init fail");
                }
            }

        }

        /// <summary>
        /// 解析ab文件信息
        /// </summary>
        private void AnalysisAssetInfo()
        {
            string path = m_abBasePath + "assetInfo.txt";
            if (!File.Exists(path))
            {
                Debug.LogError("path not eixt 会发生未知错误 , 请重新更新" + path);
                return;
            }
            Dictionary<string, string> abHashDic = new Dictionary<string, string>();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string abName = string.Empty;
                    string str = sr.ReadLine();
                    while (str != null)
                    {
                        // ab名行   格式：    abName:gameresource/material:gameresource/material_5b8de38671a2c0ba8db57600264335c6  abName：ab名:abHash名
                        if (str.Contains("abName:"))
                        {
                            string[] strs = str.Split(':');
                            abName = strs[1];
                            if (FrameworkConst.ABHashMode && strs.Length > 1)
                            {
                                abHashDic[abName] = strs[2];
                            }
                        }
                        // 资源路径行  Assets/GameResource/Material/Block/building.mat
                        else
                        {
                            m_assetAbNameDic[str] = abName;
                        }
                        str = sr.ReadLine();
                    }
                }
            }
            if (abHashDic.Count > 0)
            {
                m_abHashNameDic = abHashDic;
                foreach (var pair in abHashDic)
                {
                    Debug.Log(string.Format("key :{0}   value:{1}", pair.Key, pair.Value));
                }
            }
        }


        private IEnumerator OnLoadAsset<T>(string abName) where T : UObject
        {

            AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null)
            {
                yield return CoroutineCall.Call(OnLoadAssetBundle(abName, typeof(T)));

                bundleInfo = GetLoadedAssetBundle(abName);
                if (bundleInfo == null)
                {
                    m_LoadRequests.Remove(abName);
                    Debug.LogError("OnLoadAsset--->>>" + abName);
                    yield break;
                }
            }

            List<LoadAssetRequest> loadSingleList = null;
            List<Action<UObject[]>> loadAllList = null;
            m_LoadRequests.TryGetValue(abName, out loadSingleList);
            m_loadAllRequests.TryGetValue(abName, out loadAllList);
            //		if (!m_LoadRequests.TryGetValue(abName, out loadSingleList)) {
            //			m_LoadRequests.Remove(abName);
            //			yield break;
            //		}
            if (loadSingleList != null)
            {
                for (int i = 0; i < loadSingleList.Count; i++)
                {
                    LoadAssetRequest req = loadSingleList[i];
                    AssetBundle ab = bundleInfo.m_AssetBundle;
                    AssetBundleRequest request = ab.LoadAssetAsync(req.assetName, loadSingleList[i].assetType);
                    yield return request;
                    if (request.asset == null)
                    {
                        req.callBack(null);
                        Debug.LogError("requset error: asset not exit " + abName + "/" + req.assetName);
                    }
                    else
                    {
                        if (req.callBack != null)
                        {
                            req.callBack(request.asset);
                        }
                        bundleInfo.m_ReferencedCount++;
                    }
                }
            }
            if (loadAllList != null)
            {
                for (int i = 0; i < loadAllList.Count; i++)
                {
                    AssetBundle ab = bundleInfo.m_AssetBundle;
                    AssetBundleRequest req = ab.LoadAllAssetsAsync();
                    yield return req;
                    if (req.allAssets != null)
                    {
                        for (int j = 0; j < loadAllList.Count; j++)
                        {
                            loadAllList[j](req.allAssets);
                        }
                    }
                    else
                    {
                        Debug.Log("ab all asset null" + abName);
                    }
                }
            }
            m_LoadRequests.Remove(abName);
            m_loadAllRequests.Remove(abName);
        }

        private AssetBundleInfo GetLoadedAssetBundle(string abName)
        {
            AssetBundleInfo bundle = null;
            m_LoadedAssetBundles.TryGetValue(abName, out bundle);
            if (bundle == null) return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return bundle;

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies)
            {
                AssetBundleInfo dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null) return null;
            }
            return bundle;
        }

        private IEnumerator OnLoadAssetBundle(string abName, Type type)
        {
            string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);
            if (dependencies.Length > 0)
            {
                m_Dependencies.Add(abName, dependencies);
                for (int i = 0; i < dependencies.Length; i++)
                {
                    string depName = dependencies[i];
                    AssetBundleInfo bundleInfo = null;
                    if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo))
                    {
                        bundleInfo.m_ReferencedCount++;
                    }
                    else if (!m_LoadRequests.ContainsKey(depName))
                    {
                        yield return CoroutineCall.Call(OnLoadAssetBundle(depName, type));
                    }
                }
            }
            AssetBundleCreateRequest abCreateRequest = AssetBundle.LoadFromFileAsync(GetRealAssetPath(abName));
            yield return abCreateRequest;
            AssetBundle assetObj = abCreateRequest.assetBundle;
            if (assetObj != null)
            {
                m_LoadedAssetBundles.Add(abName, new AssetBundleInfo(assetObj));
            }
        }

        public void UnLoadAllAssetBundle()
        {
            foreach (var pair in m_LoadedAssetBundles)
            {
                AssetBundleInfo abInfo = pair.Value;
                if (abInfo.m_ReferencedCount > 0)
                {
                }
                else
                {
                    abInfo.m_AssetBundle.Unload(true);
                }

            }
            Resources.UnloadUnusedAssets();
        }

        private string GetRealAssetPath(string abName)
        {
            return m_abBasePath + abName;
        }

        private string GetRealAbName(string abName)
        {
            if (m_abHashNameDic == null)
            {
                Debug.Log("no hash text");
                return abName;
            }
            string realName = null;
            if (m_abHashNameDic.TryGetValue(abName, out realName))
            {
                return realName;
            }
            else
            {
                Debug.Log("no abname hash" + abName);
                return abName;
            }

        }

        private string GetAbName(string assetPath)
        {
            if (m_assetAbNameDic.ContainsKey(assetPath))
            {
                return m_assetAbNameDic[assetPath];
            }
            else
            {
                Debug.LogWarning("not contain assetPath" + assetPath);
                return null;
            }
        }

        class LoadAssetRequest
        {
            public Type assetType;
            public string assetName;
            public Action<UObject> callBack;
        }

        struct AssetABPathInfo
        {
            public string AssetName;
            public string ABName;

            public AssetABPathInfo(string assetName, string abName)
            {
                this.AssetName = assetName;
                this.ABName = abName;
            }
        }
    }

    public class AssetBundleInfo
    {
        public AssetBundle m_AssetBundle;
        public int m_ReferencedCount;

        public AssetBundleInfo(AssetBundle assetBundle)
        {
            m_AssetBundle = assetBundle;
            m_ReferencedCount = 0;
        }
    }
}