﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class XQFramework_Scene_UnitySceneManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(XQFramework.Scene.UnitySceneManager), typeof(System.Object));
		L.RegFunction("Initialize", Initialize);
		L.RegFunction("GetActiveSceneName", GetActiveSceneName);
		L.RegFunction("SwitchScene", SwitchScene);
		L.RegFunction("ReloadScene", ReloadScene);
		L.RegFunction("GC", GC);
		L.RegFunction("New", _CreateXQFramework_Scene_UnitySceneManager);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateXQFramework_Scene_UnitySceneManager(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				XQFramework.Scene.UnitySceneManager obj = new XQFramework.Scene.UnitySceneManager();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: XQFramework.Scene.UnitySceneManager.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Initialize(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			XQFramework.Scene.UnitySceneManager.Initialize();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetActiveSceneName(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			string o = XQFramework.Scene.UnitySceneManager.GetActiveSceneName();
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SwitchScene(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			System.Action arg1 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 2);
			XQFramework.Scene.UnitySceneManager.SwitchScene(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReloadScene(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			System.Action arg1 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 2);
			XQFramework.Scene.UnitySceneManager.ReloadScene(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GC(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			XQFramework.Scene.UnitySceneManager.GC();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}
