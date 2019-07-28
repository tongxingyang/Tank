using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace XQFramework.Packager{
	public static class BuildConfig  {
		
		public static BuildTarget BuildTarget
		{
			get { return EditorPrefs.HasKey("BundleBuild.buildTarget") ? (BuildTarget)EditorPrefs.GetInt("BundleBuild.buildTarget") : BuildTarget.StandaloneWindows; }
			set { EditorPrefs.SetInt("BundleBuild.buildTarget", (int)value); }
		}
		
		public static string outputPath
		{
			get { return EditorPrefs.HasKey("BundleBuild.outputPath") ? EditorPrefs.GetString("ABBuild.outputPath") : "AssetBundles"; }
			set { EditorPrefs.SetString("BundleBuild.outputPath", value); }
		}
		
		public static CompressOption BundleCompressOption
		{
			get { return EditorPrefs.HasKey("BundleBuild.compressOption") ? (CompressOption)EditorPrefs.GetInt("BundleBuild.compressOption") : CompressOption.ChunkBasedCompression; }
			set { EditorPrefs.SetInt("BundleBuild.compressOption", (int)value); }
		}
		
		public static bool ForceBuild
		{
			get { return EditorPrefs.GetBool("BundleBuild.forceBuild"); }
			set { EditorPrefs.SetBool("BundleBuild.forceBuild", value); }
		}
		
		public static bool AppendHashToAbName
		{
			get { return EditorPrefs.GetBool("BundleBuild.AppendHashToAbName" , false); }
			set { EditorPrefs.SetBool("BundleBuild.AppendHashToAbName", value); }
		}
		
		public static bool ReUseVersion
		{
			get { return EditorPrefs.GetBool("BundleBuild.ReUseVersion"); }
			set { EditorPrefs.SetBool("BundleBuild.ReUseVersion", value); }
		}
		
		//	public static bool LuaByteMode  在appconst中
		//	{
		//		get { return EditorPrefs.GetBool("BundleBuild.LuaByteMode"); }
		//		set { EditorPrefs.SetBool("BundleBuild.LuaByteMode", value); }
		//	}
		
		public const string CommonBundleName = "common" ;
		public const string ShaderBundleName = "shader" ;
		
		public static bool isReplaceBuiltInRes {
			get{
				return EditorPrefs.GetBool ("BundleBuild.isReplaceBuiltInRes" , true);
			}
			set{
				EditorPrefs.SetBool ("BundleBuild.isReplaceBuiltInRes", value);
			}
		}
		
		public static bool isCheckDupRes {
			get{
				return EditorPrefs.GetBool ("BundleBuild.isCheckDupRes" , true);
			}
			set{
				EditorPrefs.SetBool ("BundleBuild.isCheckDupRes", value);
			}
		}
		
		public const string BuiltExtraFolderName = "builtInExtra";
		
		public static int Version{
			get{ return EditorPrefs.GetInt ("BundleBuild.Version", 1);}
			set{ EditorPrefs.SetInt ("BundleBuild.Version", value);}
		}
	}
	
	public enum CompressOption
	{
		Uncompressed,
		StandardCompression,
		ChunkBasedCompression
	}
}