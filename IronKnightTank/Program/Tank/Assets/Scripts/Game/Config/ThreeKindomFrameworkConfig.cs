using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XQFramework;

public class ThreeKindomFrameworkConfig : IFrameworkConfig {
	public bool DebugMode{get{ return AppConst.DevMode;}}
	public bool ABHashMode{get{ return AppConst.ABHashMode;}}
	public string GameResourceRootDir{get{ return AppConst.GameResourceRootDir;}}
	public string CustomLuaDir{get{return AppConst.LuaDir;}} 
}
