using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
namespace XQFramework{
	public class PackLuaTools  {
		static List<string> paths = new List<string> ();
		static List<string> files = new List<string>() ;
		
		public static void HandleLuaFile(string[] luaPaths , string targetDir , bool luaByteMode ){
			if (Directory.Exists (targetDir)) {
				Directory.Delete (targetDir, true);
			}
			for (int i = 0; i < luaPaths.Length; i++) {
				paths.Clear(); files.Clear();
				string luaDataPath = luaPaths[i].ToLower();
				Recursive(luaDataPath);
				int n = 0;
				foreach (string f in files) {
					if (!f.EndsWith(".lua")) continue;
					string newfile = f.Replace(luaDataPath, "");
					string newpath = targetDir + newfile;
					string path = Path.GetDirectoryName(newpath);
					if (!Directory.Exists(path)) Directory.CreateDirectory(path);
					
					if (File.Exists(newpath)) {
						File.Delete(newpath);
					}
					if (luaByteMode) {
						EncodeLuaFile(f, newpath);
					} else {
						File.Copy(f, newpath, true);
					}
					UpdateProgress(n++, files.Count, newpath);
				} 
			}
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
			UnityEngine.Debug.Log ("handle lua file finish");
		}
		
		
		public static void EncodeLuaFile(string srcFile, string outFile) {
			if (!srcFile.ToLower().EndsWith(".lua")) {
				File.Copy(srcFile, outFile, true);
				return;
			}
			string appDataPath = Application.dataPath.ToLower ();
			bool isWin = true; 
			string luaexe = string.Empty;
			string args = string.Empty;
			string exedir = string.Empty;
			string currDir = Directory.GetCurrentDirectory();
			if (Application.platform == RuntimePlatform.WindowsEditor) {
				isWin = true;
				luaexe = "luajit.exe";
				args = "-b -g " + srcFile + " " + outFile;
				exedir = appDataPath.Replace("assets", "") + "LuaEncoder/luajit/";
			} else if (Application.platform == RuntimePlatform.OSXEditor) {
				isWin = false;
				luaexe = "./luajit";
				args = "-b -g " + srcFile + " " + outFile;
				exedir = appDataPath.Replace("assets", "") + "LuaEncoder/luajit_mac/";
			}
			Directory.SetCurrentDirectory(exedir);
			ProcessStartInfo info = new ProcessStartInfo();
			info.FileName = luaexe;
			info.Arguments = args;
			info.WindowStyle = ProcessWindowStyle.Hidden;
			info.UseShellExecute = isWin;
			info.ErrorDialog = true;
			
			UnityEngine.Debug.Log(info.FileName + " " + info.Arguments);
			
			Process pro = Process.Start(info);
			pro.WaitForExit();
			Directory.SetCurrentDirectory(currDir);
		}
		
		/// <summary>
		/// 遍历目录及其子目录
		/// </summary>
		static void Recursive(string path) {
			string[] names = Directory.GetFiles(path);
			string[] dirs = Directory.GetDirectories(path);
			foreach (string filename in names) {
				string ext = Path.GetExtension(filename);
				if (ext.Equals(".meta")) continue;
				files.Add(filename.Replace('\\', '/'));
			}
			foreach (string dir in dirs) {
				paths.Add(dir.Replace('\\', '/'));
				Recursive(dir);
			}
		}
		
		static void UpdateProgress(int progress, int progressMax, string desc) {
			string title = "Processing...[" + progress + " - " + progressMax + "]";
			float value = (float)progress / (float)progressMax;
			EditorUtility.DisplayProgressBar(title, desc, value);
		}
	}
}