﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class Assets_Scripts_Game_Tools_UIWorldPosFollowerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Assets.Scripts.Game.Tools.UIWorldPosFollower), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("SetPos", SetPos);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("targetPos", get_targetPos, set_targetPos);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPos(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Assets.Scripts.Game.Tools.UIWorldPosFollower obj = (Assets.Scripts.Game.Tools.UIWorldPosFollower)ToLua.CheckObject<Assets.Scripts.Game.Tools.UIWorldPosFollower>(L, 1);
			UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
			obj.SetPos(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_targetPos(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Assets.Scripts.Game.Tools.UIWorldPosFollower obj = (Assets.Scripts.Game.Tools.UIWorldPosFollower)o;
			UnityEngine.Vector3 ret = obj.targetPos;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index targetPos on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_targetPos(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Assets.Scripts.Game.Tools.UIWorldPosFollower obj = (Assets.Scripts.Game.Tools.UIWorldPosFollower)o;
			UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
			obj.targetPos = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index targetPos on a nil value");
		}
	}
}
