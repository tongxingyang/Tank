using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteFileTools  {

	public static void Write(string path , string content , bool isAppend = false){
		FileMode fileMode = isAppend ? FileMode.Append : FileMode.Create;
		using (FileStream fs = new FileStream (path , fileMode)) {
			using (StreamWriter sw = new StreamWriter (fs)) {
				sw.Write (content);
			}
		}
	}
}
