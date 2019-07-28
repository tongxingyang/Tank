using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public interface IPackConfig  {
	string ABExtName{ get;}//AB后缀名
	bool ABNameHashMode{get;}//AppendHashToAbName
	bool LuaByteMode{ get;}//lua文件使用LuaJit编译
	string[] luaPaths{get;}//lua文件路径
}
