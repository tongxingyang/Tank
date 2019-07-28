using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO ;
using System.Linq;
using System.Text;


/// <summary>
/// Bundle打包工具 
/// 功能：自动处理重复资源 自动处理内置资源 （需要提取内置资源）
/// </summary>
public class BundleBuildTools  {

	static Dictionary<string , string[]> abAssetDic  = new Dictionary<string, string[]>() ;
	static Dictionary<string , HashSet<Object>> abDepDic  = new Dictionary<string, HashSet<Object>>();
	static Dictionary<Object , List<string>> depObjDic = new Dictionary<Object, List<string>>() ;
	static List<Object> dupResList = new List<Object>();
	static Dictionary<Object , bool> dupBuiltInResDic = new Dictionary<Object , bool>();
	static Dictionary<string , GUIIDAndFileId> builtInExtraDic = new Dictionary<string, GUIIDAndFileId> ();
	static Dictionary<Object ,List<string>> replacedResAssetDic = new Dictionary<Object, List<string>> ();
	static string builtInResExtractPath = Application.dataPath + "/builtInResExtract";
	static BundleBuildConfig m_config = null ;

	public static void Build(BundleBuildConfig config){
		m_config = config;
		EditorUtility.DisplayProgressBar ("开始打包ab", "开始打包ab", 1f);
		if (config.isCheckDupRes) {
			DetectDupRes ();
			if (config.isReplaceBuiltRes ) {
				if (Directory.Exists (config.builtInResDir)) {
					CopyNeedExtractFile() ;
					AnalysisExtractRes();
					ReplaceBuiltInRes ();
					AssetDatabase.Refresh ();
				} else {
					Debug.LogError ("builtInResDir not exit : " + config.builtInResDir);
				}
			}
		}
    
		if (!Directory.Exists (config.outputPath)) {
			Directory.CreateDirectory (config.outputPath);
		}
        Debug.Log(config.outputPath);
		if (config.isABNameHash) {
			config.options = config.options | BuildAssetBundleOptions.AppendHashToAssetBundleName;
		}
		BuildPipeline.BuildAssetBundles (config.outputPath, config.options , config.target);
	    if (config.isABNameHash)
	    {
	        GetRealAbNameDic();
	    }
        RecordABAssets ();
		
		if (config.isReplaceBuiltRes) {
			ReverReplaceBuiltInRes ();
			DeleteCopyBuiltInRes ();
		}
		if (config.isLog) {
			AnalysisBuildedBundles ();
		}
		AssetDatabase.Refresh ();
		EditorUtility.ClearProgressBar ();
		Debug.Log ("AssetBundle打包完成");
	}

	public static void DetectDupRes(){
		EditorUtility.DisplayProgressBar ("检测重复AB", "检测重复AB", 0.1f);
		abAssetDic.Clear ();
		abDepDic.Clear ();
		depObjDic.Clear ();
		dupResList.Clear ();
		dupBuiltInResDic.Clear ();

		string[] abNames = AssetDatabase.GetAllAssetBundleNames ();
		foreach (var abName in abNames) {
			string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle (abName);
			abAssetDic [abName] = assetPaths;
			List<Object> assetObjList = new List<Object>() ;
			foreach (var assetPath in assetPaths) {
				if (assetPath.EndsWith (".unity")) {
					assetObjList.Add (AssetDatabase.LoadMainAssetAtPath (assetPath));
				} else {
					assetObjList.AddRange (AssetDatabase.LoadAllAssetsAtPath (assetPath));
				}
			}
			HashSet<Object> depObjSet = new HashSet<Object>(EditorUtility.CollectDependencies(assetObjList.ToArray()).Where(x => !(x is MonoScript) && x != null));
			abDepDic [abName] = depObjSet;
		}
		EditorUtility.DisplayProgressBar ("检测重复AB", "检测重复AB", 0.3f);
		foreach (var pair in abDepDic) {
			string abName = pair.Key;
			HashSet<Object> depAsset = pair.Value;
			foreach (var depObj in depAsset) {
				if (depObjDic.ContainsKey (depObj)) {
					depObjDic [depObj].Add (abName); 
				} else {
					depObjDic [depObj] = new List<string> {abName};
				}
			}
		}
		EditorUtility.DisplayProgressBar ("检测重复AB", "检测重复AB", 0.6f);
		foreach (var pair in depObjDic) {
			if (!(pair.Value.Count > 1)) {
				continue;
			}
			string depPath = AssetDatabase.GetAssetPath (pair.Key);
			if (IsBuildIn (depPath)) {
				dupBuiltInResDic.Add (pair.Key , false );
			} else {
				dupResList.Add (pair.Key);
			}
		}
		EditorUtility.DisplayProgressBar ("检测重复AB", "检测重复AB", 0.9f);
		EditorUtility.ClearProgressBar ();
	}

	private static void CopyNeedExtractFile(){
		string shaderNameFilePath = m_config.builtInResDir + "/shaderName.txt";
		if (!File.Exists (shaderNameFilePath)) {
			GenShaderNameFile ();
		}
		Dictionary<string , string > shaderFileNameDic = new Dictionary<string, string> ();
		using (FileStream fs = new FileStream (shaderNameFilePath , FileMode.Open)) {
			using (StreamReader sr = new StreamReader (fs)) {
				string content = sr.ReadLine ();
				while (content != null) {
					string[] strs = content.Split (':');
					shaderFileNameDic [strs [0]] = strs [1];
					content = sr.ReadLine ();
				}
			}
		}
		foreach (var pair in dupBuiltInResDic) {
			Object obj = pair.Key;
			string extName = "";
			string filePath = "";
			if (obj is Shader ) {
				if (shaderFileNameDic.ContainsKey (obj.name)) {
					filePath = m_config.builtInResDir + "/" + shaderFileNameDic [obj.name];
				} else {
					Debug.Log ("not shader " + obj.name);
				}
			} else if (obj is Material) {
				extName = ".mat";
				filePath = m_config.builtInResDir + "/" + obj.name + extName;
			} else if (obj is Mesh) {
				extName = ".asset";
				filePath = m_config.builtInResDir + "/" + obj.name + extName;
			} else {
				extName = ".asset";
				filePath = m_config.builtInResDir + "/" + obj.name + extName;
				Debug.Log ("现在不支持除了shader mesh ， material以外的内置资源");
			}

			if (File.Exists (filePath)) {
				string parentPath =	 builtInResExtractPath;
				if (!Directory.Exists (parentPath)) {
					Directory.CreateDirectory (parentPath);
				}
				File.Copy (filePath, parentPath +"/"+ Path.GetFileName (filePath));
			} else {
				Debug.Log ("没有该文件" + filePath);
			}
		}
		AssetDatabase.Refresh ();
	}

	public static void GenShaderNameFile(){
		DirectoryInfo dirInfo = new DirectoryInfo (m_config.builtInResDir);
		Debug.Log (dirInfo.FullName);
		FileInfo[] files = dirInfo.GetFiles ("*.shader", SearchOption.AllDirectories);
		Dictionary<string , string> shaderFileNameDic = new Dictionary<string, string> ();
		foreach (var file in files) {
			FileStream fs = new FileStream(file.FullName , FileMode.Open);
			StreamReader sr = new StreamReader (fs);
			string content = sr.ReadLine ();
			while (content != null) {
				if (content.Contains ("Shader")) {
					string[] strs = content.Split ('"');
					string name = strs [1];
					shaderFileNameDic [name] = file.Name;
					break;
				} else {
					content = sr.ReadLine ();
				}
			}
			sr.Close ();
		}
		using (FileStream fs = new FileStream (m_config.builtInResDir + "/shaderName.txt" , FileMode.OpenOrCreate)) {
			using (StreamWriter sw = new StreamWriter (fs)) {
				foreach (var pair in shaderFileNameDic) {
					sw.WriteLine (string.Format ("{0}:{1}", pair.Key, pair.Value));
				}
			}
		}
	}

	private static void AnalysisExtractRes(){
		EditorUtility.DisplayProgressBar ("解析内置资源", "解析内置资源", 1f);
		builtInExtraDic.Clear ();
		if (!Directory.Exists (builtInResExtractPath))
			return;
		string[] filePaths = Directory.GetFiles(builtInResExtractPath , "*.*" , SearchOption.AllDirectories) ;
		foreach (var filePath in filePaths) {
			if (filePath.EndsWith (".meta"))
				continue;
			string builtInUnityPath = EditorTools.GetUnityPath (filePath);
			Object builtInObj = AssetDatabase.LoadMainAssetAtPath (builtInUnityPath);
			builtInExtraDic.Add (builtInObj.name, new GUIIDAndFileId{
				guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(builtInObj)),
				fileid = builtInObj.GetFileID()
			});
		}
	}

	private static void ReplaceBuiltInRes(){
		float count = dupBuiltInResDic.Count;
		float i = 0;
		replacedResAssetDic.Clear ();
		foreach (var pair in dupBuiltInResDic) {
			Object builtInRes = pair.Key;
			EditorUtility.DisplayProgressBar ("替换guiid和fileid", "替换guiid和fileid", ++i / count);
			if (!builtInExtraDic.ContainsKey (builtInRes.name)) {
				Debug.Log ("无该资源" + builtInRes.name);
				continue;
			}
			List<string> abNameList = depObjDic [builtInRes];
			bool isPack = false;
			foreach (var abName in abNameList) {
				string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle (abName);
				foreach (var path in assetPaths) {
					List<Object> assetObjectList = new List<Object> ();
					if (path.EndsWith (".unity")) {
						assetObjectList.Add (AssetDatabase.LoadMainAssetAtPath (path));
					} else {
						assetObjectList.AddRange (AssetDatabase.LoadAllAssetsAtPath (path));
					}
					Object[] deps = EditorUtility.CollectDependencies (assetObjectList.ToArray ());
					if (deps.Contains (builtInRes)) {
						ReplaceGUIAndFileId (builtInRes, path);
						RecordReplaceAssetPath (builtInRes, path);
						isPack = true;
					}
				}
			}
			if (isPack) {
				dupBuiltInResDic[builtInRes] = true;
				GUIIDAndFileId ids = builtInExtraDic [builtInRes.name];
				string path = AssetDatabase.GUIDToAssetPath (ids.guid);
				dupResList.AddRange(AssetDatabase.LoadAllAssetsAtPath(path)) ;
			}
		}
		EditorUtility.ClearProgressBar ();
	}

	private static void ReplaceGUIAndFileId(Object builInRes , string targetAssetPath){
		try{
			long defaultFileId = builInRes.GetFileID() ;
			string defaultGUIId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(builInRes)) ;
			GUIIDAndFileId ids = builtInExtraDic[builInRes.name] ;
			StreamReader sr = new StreamReader(EditorTools.GetWindowsPath(targetAssetPath)) ;
			string content = sr.ReadToEnd() ;
			sr.Close() ;
			content = content.Replace(defaultGUIId , ids.guid) ;
			content = content.Replace(defaultFileId.ToString() , ids.fileid.ToString()) ;
			FileStream fs = new FileStream(EditorTools.GetWindowsPath(targetAssetPath) , FileMode.OpenOrCreate) ;
			StreamWriter sw = new StreamWriter(fs) ;
			sw.Write(content) ;
			sw.WriteLine("#修改标记");
			sw.Close();
			fs.Close();
		}catch{
			throw new UnityException ("builnt in extrac dic 数据错误");
		}
	}

	private static void RecordReplaceAssetPath(Object builtInRes , string path){
		if (replacedResAssetDic.ContainsKey (builtInRes)) {
			replacedResAssetDic [builtInRes].Add (path);
		} else {
			replacedResAssetDic [builtInRes] = new List<string>{ path };
		}
	}

	private static void ResetDupResABName(){
		float count = dupResList.Count;
		for (int i = 0; i < dupResList.Count; i++) {
			string path = AssetDatabase.GetAssetPath (dupResList[i]);
			AssetImporter ai = AssetImporter.GetAtPath (path);
			ai.assetBundleName = m_config.commonBundleName;
			EditorUtility.DisplayProgressBar ("修改abName", path + "  "+m_config.commonBundleName, i / count);
		}
	}

	static void SetShaderAssetBundle ()
	{
		HashSet<Object> alldepSet = GetAllDeps() ;
		foreach (var depobj in alldepSet) {
			if (depobj is Shader) {
				string path = AssetDatabase.GetAssetPath (depobj);
				if (IsBuildIn (path)) {
					Debug.Log ("打包内置资源" + path + depobj.name);
					continue;
				}
				AssetImporter ai = AssetImporter.GetAtPath (path);
				if (ai == null) {
					Debug.Log (" ai null exort  " + AssetDatabase.GetAssetPath (depobj) + depobj.name);
				} else {
					ai.assetBundleName = m_config.shaderBundleName;
				}
			}
		}
	}

	private static void RecordABAssets(){
		AssetDatabase.RemoveUnusedAssetBundleNames ();
		string[] abNames = AssetDatabase.GetAllAssetBundleNames ();
		StringBuilder sb = new StringBuilder ();
		Dictionary<string , string> abNameHashDic = GetRealAbNameDic ();

		for (int i = 0; i < abNames.Length; i++) {
			string str = string.Format("abName:{0}" ,abNames [i]);
			if (m_config.isABNameHash) {
				str += ":";
				str += abNameHashDic [abNames [i]];
                Debug.Log("abname" + abNames[i]);
			}
			sb.AppendLine (str);
			string[] assets = AssetDatabase.GetAssetPathsFromAssetBundle (abNames[i]); ;
			foreach (var assetPath in assets) {
				sb.AppendLine (assetPath);
			}
		}
		string path = m_config.outputPath + "/assetInfo.txt";
		WriteFileTools.Write (path, sb.ToString ());

	}

	static Dictionary<string , string> GetRealAbNameDic ()
	{
		Dictionary<string , string> dic = new Dictionary<string, string> ();
		StringBuilder sb = new StringBuilder ();
		string path = Application.streamingAssetsPath;
		string[] files = Directory.GetFiles (path , "*" , SearchOption.AllDirectories).Where(t=>Path.GetExtension(t).Equals("")).ToArray();
		foreach (var file in files) {
			string  newfile = file.Replace ("\\", "/");
			string name = newfile.Replace(path+"/" , "");
			string[] str = name.Split('_');
			sb.AppendLine(string.Format("{0}|{1}" ,str[0] , name ));
			dic[str[0]] = name;
		}
		WriteFileTools.Write (path + "/abHash.txt", sb.ToString ());
		return dic;
	}

	private static void ReverReplaceBuiltInRes(){
		foreach (var pair in replacedResAssetDic) {
			Object defaultObj = pair.Key;
			List<string> replaceAsset = pair.Value;
			string defaultObjPath = AssetDatabase.GetAssetPath (defaultObj);
			GUIIDAndFileId ids = builtInExtraDic [defaultObj.name];
			foreach (var path in replaceAsset) {
				string windowsPath = EditorTools.GetWindowsPath (path);
				StreamReader sr = new StreamReader (windowsPath);
				string content = sr.ReadToEnd ();
				sr.Close ();
				content = content.Replace ("#修改标记", "");
				content = content.Replace (ids.guid, AssetDatabase.AssetPathToGUID (defaultObjPath));
				content = content.Replace (ids.fileid.ToString (), defaultObj.GetFileID ().ToString());
				StreamWriter sw = new StreamWriter (windowsPath);
				sw.Write (content);
				sw.Close ();
			}
		}
	}

	private static void DeleteCopyBuiltInRes(){
		if (Directory.Exists (builtInResExtractPath)) {
			Directory.Delete (builtInResExtractPath, true);
		}
	}

	private static void AnalysisBuildedBundles(){
		string[] abNames = AssetDatabase.GetAllAssetBundleNames ();
		int assetCount = 0, depCount = 0;
		string filePath = Application.dataPath + "/packInfo.txt";
		using (FileStream fs = new FileStream (filePath, FileMode.OpenOrCreate)) {
			using(StreamWriter sw = new StreamWriter (fs)){
				sw.WriteLine ("AB包数量" + abNames.Length);
				sw.WriteLine ("==========AB包包含Asset==========");
				foreach (var pair in abAssetDic) {
					sw.WriteLine ("----------" + pair.Key + "----------");
					foreach (var asset in pair.Value) {
						sw.WriteLine (asset);
						assetCount++;
					}
				}
				sw.WriteLine ("==========AB包依赖==========");
				foreach (var pair in abDepDic) {
					sw.WriteLine ("--------------"+pair.Key+"--------------" );
					foreach (var dep in pair.Value) {
						depCount++;
						sw.WriteLine (AssetDatabase.GetAssetPath (dep) +string.Format("({0})" , dep.GetType()));
					}
				}
				sw.WriteLine ("==========重复引用资源==========(修改为common)");
				foreach (var path in dupResList) {
                    Debug.Log(path.ToString());
					sw.WriteLine (path.ToString());
				}
				sw.WriteLine ("==========重复内置资源==========");

				foreach (var pair in dupBuiltInResDic) {
					sw.WriteLine (pair.Key.name + (pair.Value ? "已替换" : "未替换"));
				}
				sw.WriteLine (string.Format ("AB包：{0}个 ， asset：{1}个 ， dep{2}个 , 重复（包含内置资源）{3}个 ， 重复内置资源{4}个",abNames.Length, assetCount, depCount , dupResList.Count , dupBuiltInResDic.Count));
			}
		}
	}
		


	private static bool IsBuildIn(string path){
		return path.StartsWith("Resources/unity_builtin_extra") || path == "Library/unity default resources";
	}

	private static HashSet<Object> GetAllDeps(){
		string[] abNames = AssetDatabase.GetAllAssetBundleNames();
		HashSet<Object> allDepSet = new HashSet<Object> ();
		foreach (var abName in abNames) {
			string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle (abName);
			List<Object> assetObjList = new List<Object>() ;
			foreach (var assetPath in assetPaths) {
				if (assetPath.EndsWith (".unity")) {
					assetObjList.Add (AssetDatabase.LoadMainAssetAtPath (assetPath));
				} else {
					assetObjList.AddRange (AssetDatabase.LoadAllAssetsAtPath (assetPath));
				}
			}
			HashSet<Object> depObjSet = new HashSet<Object>(EditorUtility.CollectDependencies(assetObjList.ToArray()).Where(x => !(x is MonoScript)));
			allDepSet.UnionWith (depObjSet);
		}
		return allDepSet;
	}
}

public class BundleBuildConfig{
	public BuildTarget target = BuildTarget.StandaloneWindows;
	public string outputPath = "";
	public BuildAssetBundleOptions options = BuildAssetBundleOptions.None;
	public bool isCheckDupRes = false ;
	public bool isReplaceBuiltRes = false;
	public bool isLog = true;
	public bool isABNameHash = true;
	public string builtInResDir = "" ;
	public string commonBundleName = "common";
	public string shaderBundleName = "shader";
}

public class GUIIDAndFileId{
	public  string guid ;
	public long fileid ;
}
