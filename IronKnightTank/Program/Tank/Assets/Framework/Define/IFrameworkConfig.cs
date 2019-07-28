using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XQFramework{
	public interface IFrameworkConfig{
		bool DebugMode{get;}
		bool ABHashMode{get;}
		string GameResourceRootDir{get;}
		string CustomLuaDir{ get;}
	}
}