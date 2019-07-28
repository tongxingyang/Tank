using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeKindomPackConfig : IPackConfig{
	public string ABExtName{ get{ return AppConst.ExtName;}}//AB后缀名
	//	//	string BuiltInResDir{get;}//提取出的内置资源路径
	public bool ABNameHashMode{get{return AppConst.ABHashMode;}}//AppendHashToAbName
	public bool LuaByteMode{ get{return AppConst.LuaByteMode;}}//lua文件使用LuaJit编译
	public string[] luaPaths{get { return new string[]{ XQFramework.FrameworkConst.ToLuaDir, AppConst.LuaDir };	}}
}
