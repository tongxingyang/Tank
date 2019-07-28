using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug ;
using System.Text;
namespace XQFramework.Packager{
	public class Packager   {
		
		private static int version = 0 ;
		//private bool ignoreCheckVer= false ;
		//private bool fullInstall = false ;
		static string buildPath = "";
		private static IPackConfig m_config ;

		
		public static void Build(IPackConfig config){
			m_config = config;
			buildPath = Application.streamingAssetsPath;
            if (Directory.Exists(buildPath))
            {
			    Directory.Delete (buildPath, true);
            }
			BundleBuildTools.Build (new BundleBuildConfig (){ 
				isCheckDupRes = BuildConfig.isCheckDupRes ,
				outputPath = buildPath,
				target = BuildConfig.BuildTarget,
				isReplaceBuiltRes = BuildConfig.isReplaceBuiltInRes ,
				isABNameHash = config.ABNameHashMode //BuildConfig.AppendHashToAbName 
			});
			PackLuaTools.HandleLuaFile (config.luaPaths , buildPath + "/lua" , config.LuaByteMode);
			GenHashFile (buildPath, buildPath + "/files.txt" , new string[]{".meta" , ".manifest"});
			GenPatch ();
			BuildConfig.Version = version;
		}
		
		public static void GenHashFile(string dirPath , string txtPath  , string[] filterPatterns = null){
			EditorUtility.DisplayProgressBar ("生成MD5文件" + dirPath, "生成MD5文件", 0f);
			string[] files = Directory.GetFiles (dirPath, "*.*", SearchOption.AllDirectories).Where (s => {
				if(filterPatterns != null){
					return !filterPatterns.Contains(Path.GetExtension(s)) ;
				}
				return true ;
			}).ToArray();
			StringBuilder stringBuilder = new StringBuilder ();
			string parentName = dirPath.Substring(dirPath.LastIndexOf("/"));
			for(int i = 0 ; i< files.Length ; i++){
				string filePath = files [i];
				EditorUtility.DisplayProgressBar ("生成MD5文件"+ dirPath, "生成MD5文件", (float)i/(float)files.Length);
				string hash = GetABHash (filePath , dirPath);
#if UNITY_EDITOR_WIN
                string fileName = filePath.Substring (filePath.LastIndexOf( parentName + "\\")+ (parentName + "\\").Length);
				stringBuilder.AppendLine (string.Format ("{0}|{1}", fileName, hash));
#elif UNITY_EDITOR_OSX
                string fileName = filePath.Substring (filePath.LastIndexOf( parentName + "/")+ (parentName + "/").Length);
				stringBuilder.AppendLine (string.Format ("{0}|{1}", fileName, hash));
#endif
            }
            EditorUtility.ClearProgressBar ();
			AssetDatabase.Refresh ();
			Debug.Log ("生成md5 完成 dir :"+dirPath + "  txtPath :" + txtPath);
			WriteFileTools.Write (txtPath, stringBuilder.ToString ());
		}


		/// <summary>
		/// Gets the AB hash.
		/// </summary> 获取abHash 如果是AppendHashToABName模式 采用这个Hash  lua文件和AssetBundleManifest 使用MD5  非AppendHashToABName模式下都是MD5
		/// <returns>The AB hash.</returns>
		/// <param name="filePath">File path.</param>
		public static string GetABHash(string filePath , string dirPath ){
			string hash;
			if (m_config.ABNameHashMode) {
				if (Path.GetExtension (filePath).Equals ("") && Path.GetFileName (filePath) != Path.GetFileName (dirPath)) {
//					Debug.Log (filePath + "filename " + Path.GetFileName (filePath) + Path.GetDirectoryName (dirPath));  
					hash = Path.GetFileNameWithoutExtension (filePath).Split ('_') [1];
				} else {
					hash = EditorTools.GetMD5HashFromFile (filePath);
				}
			} else {
				hash = EditorTools.GetMD5HashFromFile (filePath);
			}
			return hash;
		}
		

		public static void GenPatch(){
			string patchDir = Application.dataPath + "/../Patch/"+BuildConfig.BuildTarget.ToString();
			if (!Directory.Exists (patchDir)) {
				Directory.CreateDirectory (patchDir);
			}
			string versionDir = patchDir + "/Version_" + BuildConfig.Version+"/";
			if (Directory.Exists (versionDir)) {
				Directory.Delete (versionDir , true);
			}
			Directory.CreateDirectory (versionDir);
			string oldFilePath = patchDir + "/files.txt";
			if (File.Exists (oldFilePath)) {
				string outputPath = versionDir+"update.txt";
				List<string> updateList = MD5FileCompareTools.Compare (buildPath + "/files.txt", oldFilePath,outputPath );
				for (int i = 0; i < updateList.Count; i++) {
					string fullPath = Application.streamingAssetsPath + "/" + updateList [i];
					string targetPath = fullPath.Replace(Application.streamingAssetsPath + "/" ,versionDir ) ;
					targetPath = targetPath.Replace ("\\", "/");
					string parentPath = targetPath.Substring(0 , targetPath.LastIndexOf("/")); 
					if (!Directory.Exists (parentPath)) {
						Directory.CreateDirectory (parentPath);
					}
					File.Copy(fullPath , targetPath , true);
					// Debug.Log ("copy need update file " + fullPath + "   " + targetPath);
				}
			} else {
				Debug.Log ("file not exit patch fail" + oldFilePath);
				Directory.CreateDirectory (patchDir);
			}
			if (File.Exists (oldFilePath)) {
				File.Copy (oldFilePath, oldFilePath.Replace ("files", "oldfile") , true);
			}
			File.Copy (buildPath + "/files.txt", oldFilePath , true);
        }
	    

	}
}