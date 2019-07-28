﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class DebugConsoleWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(DebugConsole), typeof(System.Object));
		L.RegFunction("SetActive", SetActive);
		L.RegFunction("AddTopString", AddTopString);
		L.RegFunction("RemoveTopString", RemoveTopString);
		L.RegFunction("AddButton", AddButton);
		L.RegFunction("RemoveButton", RemoveButton);
		L.RegFunction("LogStackTrace", LogStackTrace);
		L.RegFunction("Log", Log);
		L.RegFunction("LogToChannel", LogToChannel);
		L.RegFunction("DebugBreak", DebugBreak);
		L.RegFunction("Clear", Clear);
		L.RegFunction("GetText", GetText);
		L.RegFunction("New", _CreateDebugConsole);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("consoleImpl", get_consoleImpl, set_consoleImpl);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateDebugConsole(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				DebugConsole obj = new DebugConsole();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: DebugConsole.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetActive(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 1);
			DebugConsole.SetActive(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTopString(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			DebugConsole.AddTopString(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveTopString(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			DebugConsole.RemoveTopString(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddButton(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			System.Action arg1 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 2);
			DebugConsole.AddButton(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveButton(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			DebugConsole.RemoveButton(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogStackTrace(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			DebugConsole.LogStackTrace();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Log(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes<string>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				DebugConsole.Log(arg0);
				return 0;
			}
			else if (TypeChecker.CheckParamsType<object>(L, 1, count))
			{
				object[] arg0 = ToLua.ToParamsObject(L, 1, count);
				DebugConsole.Log(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: DebugConsole.Log");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogToChannel(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<string>(L, 2))
			{
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				DebugConsole.LogToChannel(arg0, arg1);
				return 0;
			}
			else if (TypeChecker.CheckTypes<int>(L, 1) && TypeChecker.CheckParamsType<object>(L, 2, count - 1))
			{
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
				object[] arg1 = ToLua.ToParamsObject(L, 2, count - 1);
				DebugConsole.LogToChannel(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: DebugConsole.LogToChannel");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DebugBreak(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			DebugConsole.DebugBreak();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clear(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
			DebugConsole.Clear(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetText(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			string o = DebugConsole.GetText();
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_consoleImpl(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, DebugConsole.consoleImpl);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_consoleImpl(IntPtr L)
	{
		try
		{
			Assets.Tools.Script.Debug.Console.IDebugConsole arg0 = (Assets.Tools.Script.Debug.Console.IDebugConsole)ToLua.CheckObject<Assets.Tools.Script.Debug.Console.IDebugConsole>(L, 2);
			DebugConsole.consoleImpl = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}
