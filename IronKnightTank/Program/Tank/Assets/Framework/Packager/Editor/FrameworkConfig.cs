using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FrameworkConfig  {

	string abOutPutPath{get;}
	string GameResRootDir{ get;}
	string ABExtName{ get;}
	bool ABNameHashMode{get;}
	bool isReplcaceBuildInRes{ get;}
	bool LuaByteMode{ get;}

}
