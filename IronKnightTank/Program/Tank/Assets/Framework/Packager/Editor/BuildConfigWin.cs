using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XQFramework;

namespace XQFramework.Packager{
	public class BuildConfigWin : EditorWindow {
		
		private BundleRuleView m_ruleView ;
		//	BuildAssetBundleOptions options;
		[MenuItem("Packager/Config" , false , 1)]
		public static void Open(){
			BuildConfigWin win = EditorWindow.GetWindow<BuildConfigWin> ();
			//win.maxSize = new Vector2 (600, 500);
			win.Show ();
		}
		
		void OnEnable(){
			m_ruleView = new BundleRuleView (this);
		}
		
		void OnGUI(){
			//TODO 编辑器支持枚举
			//		BuildConfig.BundleCompressOption = (CoappampressOption)EditorGUILayout.EnumPopup ("CompressOption", BuildConfig.BundleCompressOption);
			//		options = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup ("CompressOption", options);
			BuildConfig.isReplaceBuiltInRes = (bool)EditorGUILayout.Toggle ("替换内置资源", BuildConfig.isReplaceBuiltInRes );
			BuildConfig.isCheckDupRes = (bool)EditorGUILayout.Toggle ("处理重复资源", BuildConfig.isCheckDupRes );
			BuildConfig.AppendHashToAbName = (bool)EditorGUILayout.Toggle ("ABWithHash", BuildConfig.AppendHashToAbName );
			AutoABNamePostprocessor.autoPack = (bool)EditorGUILayout.Toggle ("自动根据规则设置AB名", AutoABNamePostprocessor.autoPack );
			//		BuildConfig.LuaByteMode = (bool)EditorGUILayout.Toggle ("LuaByteMode", BuildConfig.LuaByteMode );
			EditorGUILayout.LabelField("AB打包规则", EditorStyles.boldLabel, GUILayout.Width(212));
			m_ruleView.Draw(new Rect(0,0,0,0)) ;
		}
		
		
//		void OnDestroy(){
//			EditorWindow.FocusWindowIfItsOpen<Packager> ();
//		}
		
		private string GetAssetPath(string result)
		{
			if (result.StartsWith(Application.dataPath))
				return result == Application.dataPath ? "" : result.Substring(Application.dataPath.Length + 1);
			else if (result.StartsWith("Assets"))
				return result == "Assets" ? "" : result.Substring("Assets/".Length);
			return null;
		}
		
	}
}