using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class MD5FileCompareTools  {

	public static List<string>  Compare(string newFilePath , string oldFilePath , string outputFilePath ){
		if (!File.Exists (newFilePath) || !File.Exists (oldFilePath)) {
			Debug.Log ("file not exit newFilePath" + newFilePath + "  oldFilePath" + oldFilePath);
			return null ;
		}
		Dictionary<string , string> newMD5Dic = AnalysisMD5File (newFilePath);
		Dictionary<string , string> oldMD5Dic = AnalysisMD5File (oldFilePath);
		List<string> updateAbList = new List<string> ();
		List<string> deleteAbList = new List<string> ();
		foreach (var pair in newMD5Dic) {
			string filePath = pair.Key;
			string md5 = pair.Value;
			string old;
			bool isSame = false;
			bool isGot = oldMD5Dic.TryGetValue (filePath,out old);
			isSame = (isGot && old.Equals (md5));
			// Debug.Log ("isame" + isSame.ToString () + filePath);
			if (!isSame) {
				updateAbList.Add (filePath);
			}
		}
		foreach (var pair in oldMD5Dic) {
			if(!newMD5Dic.ContainsKey(pair.Key)){
				deleteAbList.Add (pair.Key);
			}
		}
	
		using (FileStream fs = new FileStream (outputFilePath , FileMode.OpenOrCreate)) {
			using (StreamWriter sw = new StreamWriter (fs)) {
				StringBuilder sb = new StringBuilder ();
				sb.AppendLine ("down|" +updateAbList.Count);
				for (int i = 0; i < updateAbList.Count; i++) {
					sb.AppendLine (updateAbList [i]);
				}
				sb.AppendLine ("delete|"+deleteAbList.Count);
				for (int i = 0; i < deleteAbList.Count; i++) {
					sb.AppendLine (deleteAbList [i]);
				}
				sw.Write (sb.ToString ());
			}
		}
		Debug.Log (updateAbList.Count);
		return updateAbList;
	}

	static Dictionary<string,string> AnalysisMD5File(string filePath){
		if (File.Exists (filePath)) {
			Dictionary<string , string> MD5Dic = new Dictionary<string, string> ();
			using (FileStream fs = new FileStream (filePath , FileMode.OpenOrCreate)) {
				using (StreamReader sr = new StreamReader (fs)) {

					string str = sr.ReadLine ();
					while (str != null) {
						string[] strs = str.Split ('|');
						MD5Dic [strs [0]] = strs [1];
						str = sr.ReadLine ();
					}
				}
			}
			return MD5Dic;
		} else {
			Debug.Log ("file not exit" + filePath);
			return null;
		}

	}
}
