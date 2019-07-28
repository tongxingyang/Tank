using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XQFramework.Packager;
using System.IO;
public class PackWin : EditorWindow {
	private static int version = 0 ;
	private bool ignoreCheckVer= false ;

    private static bool isAutoConnectProfiler = false ;
	[MenuItem("Packager/PackWin" , false , 0)]
	public static void Win(){
		PackWin win = EditorWindow.GetWindow<PackWin> ();
		win.Show ();
        
	}
	

    private static void BuildApp(bool isRepackAssetbundle)
    {
        if (AppConst.DevMode)
        {
            EditorUtility.DisplayDialog("wrong ", "开发者模式下不能打包 ， 请将AppConst.DevMode改成false", "ok");
            return;
        }
        if (isRepackAssetbundle)
        {
            Build();
        }
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = GetBuildScenes();
        options.target = BuildConfig.BuildTarget;
        options.options |= BuildOptions.Development;
        options.options |= BuildOptions.ConnectWithProfiler;
        
        if (isAutoConnectProfiler)
        {
            options.options |= BuildOptions.ConnectWithProfiler;
        }


#if UNITY_EDITOR_OSX
        PlayerSettings.stripEngineCode = false;
        options.options |= BuildOptions.Il2CPP;
#endif
        string platforFolderPath = Path.GetDirectoryName(Application.dataPath.Replace("/Assets", ""));
        string timeSign = System.DateTime.Now.ToLocalTime().ToString();
        timeSign.Trim();
        timeSign = timeSign.Replace("/", "_");
        timeSign = timeSign.Replace(" ", "_");
        timeSign = timeSign.Replace(":", "_");
        platforFolderPath = platforFolderPath + "/Pack/"+options.target.ToString();
        string executableParentPath = platforFolderPath  + "/"  + timeSign + "_tank_" + version.ToString() + "/";
        string executablePath = executableParentPath + "tank"+GetExecutableExtension(options.target);
        options.locationPathName = executablePath;
        BuildPipeline.BuildPlayer(options);
        EditorUtility.DisplayDialog("提示", "打包完成 :" + executablePath, "ok");
        System.Diagnostics.Process.Start(executableParentPath);
    }

    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }

    void OnEnable(){
		version = BuildConfig.Version;
	}

	void OnGUI(){
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(string.Format("当前版本：{0}", BuildConfig.Version), EditorStyles.boldLabel, GUILayout.Width(212));

	    isAutoConnectProfiler = GUILayout.Toggle(isAutoConnectProfiler, "Profiler");
        if (GUILayout.Button ("打包设置"  , GUILayout.Width(100)) ) {
			BuildConfigWin.Open ();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("版本设置：", GUILayout.Width(60));
			version =int.Parse(EditorGUILayout.TextField(version.ToString(), GUILayout.Width(70)).Trim());
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();
		{
			EditorGUILayout.LabelField ("打包平台", GUILayout.Width (60));
			BuildConfig.BuildTarget =  (BuildTarget)EditorGUILayout.EnumPopup ("", BuildConfig.BuildTarget , GUILayout.Width(180));
		}
		EditorGUILayout.EndHorizontal ();
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("发布资源" , GUILayout.MaxWidth(150))){
			Build ();
		}
        if (GUILayout.Button("快速出包(不打AB)", GUILayout.MaxWidth(150)))
        {
            BuildApp(false);
        }
        if (GUILayout.Button("快速出包(打AB)", GUILayout.MaxWidth(150)))
        {
            BuildApp(true);
        }
        


        EditorGUILayout.EndHorizontal();
	}

	public static void Build(){
		Packager.Build (new ThreeKindomPackConfig() );
	}

    private static void CheckBuildSetting()
    {
        if(BuildConfig.BuildTarget != EditorUserBuildSettings.activeBuildTarget)
        {
            if(EditorUtility.DisplayDialog("打包平台与当前平台不一致，是否自动切换平台", "打包平台与当前平台不一致，是否自动切换平台", "ok"))
            {
                
            }
        }
    }

    private static string GetExecutableExtension(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return ".apk";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return ".exe";
            case BuildTarget.iOS:
                return "";
                    default:
                return "";
        }
    }



}

