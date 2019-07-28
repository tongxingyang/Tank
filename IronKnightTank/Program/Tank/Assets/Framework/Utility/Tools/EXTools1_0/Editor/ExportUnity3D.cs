using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Assets.Cosmos.ExportAssertBundle.Editor
{
    /// <summary>
    /// 打包为assertbundle
    /// 3种使用方法
    /// 整体打包 分割打包 打包所选项
    /// </summary>
    public class ExportUnity3D : EditorWindow
    {
        ///
        /// 资源对象名称
        /// 
        public string myTargetPathString = Application.dataPath;
        ///
        /// 创建的文件夹名称
        /// 
        public string mySavePathString = Application.streamingAssetsPath;
        /// <summary>
        /// 要打包的目录
        /// </summary>
        private string TargetPath = "";
        ///
        /// 要保存的文件名称
        /// 
        private string myFileName = "tzEDR";
        ///
        /// 菜单选择的ID
        /// 
        private static int typeFileindex;
        private static string[] strType = { ".prefab", ".FBX", ".mat", ".tga", ".png", ".cs", ".unity" };
        private BuildTarget target = BuildTarget.Android;
        [MenuItem("Window/Export unity3d file")]
        static void Init()
        {
            typeFileindex = 0;
            ExportUnity3D exportWindow = (ExportUnity3D)EditorWindow.GetWindow(typeof(ExportUnity3D));
            exportWindow.Show();
        }
        /// <summary>
        /// true 导出一个文件
        /// false导出整个文件夹中的文件
        /// </summary>
        private bool experFromFile = true;
        /// <summary>
        /// 导出到1个资源包
        /// </summary>
        private bool experInOneAssertBundle = false;
        public int version = 1;
        void OnGUI()
        {
            GUILayout.Label("填写打包为unity3d格式的对象", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            mySavePathString = EditorGUILayout.TextField("创建文件夹名称（路径）：", mySavePathString);
            if (GUILayout.Button("浏览", GUILayout.Width(65)))
            {
                mySavePathString = EditorUtility.OpenFolderPanel("创建文件夹名称（路径）", mySavePathString, "");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("选择[浏览文件]将打包对应的文件 不需要选择后缀名\r\n 如果选择[浏览文件夹] 将会分别打包文件夹下(包括子目录)指定后缀名的资源"
                                    + "\r\n 勾选[单一资源包]则打包到1个资源包内 并命名为[保存名称].assetbundle 不勾选则单独打包命名为对应文件名.assetbundle"
                                    + "\r\n如果不选择打包资源 那么将会打包当前project窗体中选中的物体打包", MessageType.Info, true);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            myTargetPathString = EditorGUILayout.TextField("要打包的对象路径：", myTargetPathString);
            if (GUILayout.Button("浏览文件", GUILayout.Width(65)))//打包单独文件
            {
                experFromFile = true;
                myTargetPathString = EditorUtility.OpenFilePanel("要打包的对象路径）", myTargetPathString, "");
            }
            if (GUILayout.Button("浏览文件夹", GUILayout.Width(65)))//打包整个文件
            {
                experFromFile = false;
                myTargetPathString = EditorUtility.OpenFolderPanel("要打包的对象文件夹", myTargetPathString, "");
            }
            typeFileindex = EditorGUILayout.Popup(typeFileindex, strType, GUILayout.Width(65));
            EditorGUILayout.EndHorizontal();
            experInOneAssertBundle = EditorGUILayout.Toggle("单一资源包", experInOneAssertBundle);
            target = (BuildTarget)EditorGUILayout.EnumPopup(target, GUILayout.Width(65));
            EditorGUILayout.Space();
            myFileName = EditorGUILayout.TextField("保存名称：", myFileName);
            EditorGUILayout.Space();
            if (GUILayout.Button("打包"))
            {
                if (String.IsNullOrEmpty(myTargetPathString))//如果要打包的对象目录为空 即打包当前选择项
                {
                    ExportResource(myFileName);
                }
                else
                {
                    if (experFromFile)
                    {
                        ExportResource(mySavePathString, myTargetPathString, myFileName);//打包目标到单独包中
                    }
                    else
                    {
                        string[] files = GetDir(myTargetPathString, "*" + strType[typeFileindex], SearchOption.AllDirectories);
                        if (experInOneAssertBundle)//是否把所有文件夹中资源打包到同一个包中
                        {
                            ExportResources(mySavePathString, files, myFileName);
                        }
                        else//把所有文件夹中资源分别打包到各自资源包中
                        {
                            foreach (string file in files)
                            {
                                Debug.Log("检测到要打包的文件" + file);
                                int startindex = file.LastIndexOf(@"\");
                                ExportResource(mySavePathString, file, file.Substring(startindex, file.Length - startindex - strType[typeFileindex].Length));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 要设置 
        /// myTargetPath
        /// mySavePath
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void ExportResources(string myTargetPath, string mySavePath, BuildTarget tag)
        {
            this.target = tag;
            string[] files = GetDir(myTargetPath, "*" + strType[typeFileindex], SearchOption.AllDirectories);
            foreach (string file in files)
            {
                Debug.Log("检测到要打包的文件" + file);
#if UNITY_EDITOR_OSX
				int startindex = file.LastIndexOf(@"/");
#else
                int startindex = file.LastIndexOf(@"\");
#endif

                ExportResource(mySavePath, file, file.Substring(startindex, file.Length - startindex - strType[typeFileindex].Length));
            }
        }
        /// <summary>
        /// 获得目录下所有文件名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="seacherPattern">筛选规则</param>
        /// <param name="so">筛选规则</param>
        /// <returns></returns>
        public static string[] GetDir(string path, string seacherPattern, SearchOption so)
        {
            string[] FileList = null;
            if (Directory.Exists(path))
            {
                FileList = Directory.GetFiles(path, seacherPattern, so);
            }
            return FileList;
        }
        /// <summary>
        /// 获得目录下所有文件名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="seacherPattern">筛选规则</param>
        /// <param name="so">筛选规则</param>
        /// <returns></returns>
        public static string[] GetDirectories(string path, string seacherPattern, SearchOption so)
        {
            string[] FileList = null;
            if (Directory.Exists(path))
            {
                FileList = Directory.GetDirectories(path, seacherPattern, so);
            }
            return FileList;
        }
        /// <summary>
        /// 打包资源
        /// </summary>
        /// <param name="path">存放资源包的位置</param>
        /// <param name="filePathName">要打包的资源</param>
        /// <param name="fileName">打包后名字</param>
        void ExportResource(string path, string filePathName, string fileName)
        {
            if (path != "" && filePathName != "" && fileName != "")
            {
                System.IO.Directory.CreateDirectory(path);

                BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
                //BuildPipeline.PushAssetDependencies();

                if (strType[typeFileindex] != ".unity")
                {
                    filePathName = filePathName.Substring(filePathName.IndexOf("Assets"), filePathName.Length - filePathName.IndexOf("Assets"));
                    object asset = AssetDatabase.LoadMainAssetAtPath(filePathName);
                    Debug.Log(asset);
                    if (asset == null || asset == "Null")
                    {
                        this.ShowNotification(new GUIContent(filePathName + "-此资源不存在！"));
                        return;
                    }
                    BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath(filePathName), null, path + "/" + fileName + ".assetbundle", options, target);
                }
                else
                {
                    Debug.Log("Scenec!!!");
                    string[] str = { "Assets/" + filePathName };
                    //string path =  EditorUtility.SaveFilePanel("build bundle", "", "*", "unity3d");
                    BuildPipeline.BuildStreamedSceneAssetBundle(str, path + "/" + fileName + ".unity3d", target);
                }
                this.ShowNotification(new GUIContent(filePathName + " -打包成功！！"));
            }
            else
            {
                this.ShowNotification(new GUIContent("所填写的内容不能为空！！"));
            }
        }
        /// <summary>
        /// 打包多个资源到一个包中
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="filePathNames">资源目录名</param>
        /// <param name="fileName">包名</param>
        void ExportResources(string path, string[] filePathNames, string fileName)
        {
            if (path != "" && filePathNames.Length != 0 && fileName != "")
            {
                System.IO.Directory.CreateDirectory(path);

                BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
                List<UnityEngine.Object> objs = new List<UnityEngine.Object>();

                if (strType[typeFileindex] != ".unity")
                {
                    foreach (string filePathName in filePathNames)
                    {
                        var filePath = filePathName.Substring(filePathName.IndexOf("Assets"), filePathName.Length - filePathName.IndexOf("Assets"));
                        UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(filePath);
                        objs.Add(asset);
                        if (asset == null)
                        {
                            this.ShowNotification(new GUIContent(filePath + "-此资源不存在！"));
                            return;
                        }
                    }
                }
                else
                {
                    Debug.Log("Scenec!!!");
                    this.ShowNotification(new GUIContent("现在还不能打包多个所有场景"));
                }
                if (objs.Count != 0)
                {
                    BuildPipeline.BuildAssetBundle(objs[0], objs.ToArray(), path + "/" + fileName + ".assetbundle", options, target);
                    this.ShowNotification(new GUIContent(" -打包成功！！"));
                }
                else
                {
                    this.ShowNotification(new GUIContent("没有需要打包的东西哦"));
                }
            }
            else
            {
                this.ShowNotification(new GUIContent("所填写的内容不能为空！！"));
            }
        }
        /// <summary>
        /// 打包所选项
        /// </summary>
        /// <param name="fileName"></param>
        public void ExportResource(string fileName)
        { // Bring up save panel 
            string path = mySavePathString;
            if (path.Length != 0)
            {
                // Build the resource file from the active selection. 
                UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
                BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path + @"\" + fileName + ".assetbundle"
                                               , BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets
                                               , target);
                Selection.objects = selection;
            }
        }
    }
}