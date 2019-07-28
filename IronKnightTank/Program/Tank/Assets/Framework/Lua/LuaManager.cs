using UnityEngine;
using System.Collections;
using LuaInterface;

namespace XQFramework.Lua
{
    public class LuaManager : MonoBehaviour
    {
        public static LuaManager Instance;

        private LuaState lua;
        //        private LuaLoader loader;  暂时不需要AB加载
        private LuaLooper loop = null;

        // Use this for initialization
        void Awake()
        {
            Instance = this;
            //            loader = new LuaLoader();
            lua = new LuaState();
            this.OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, this);
            InitStart();
        }

        public void InitStart()
        {
            InitLuaPath();
            this.lua.Start();    //启动LUAVM
            this.StartLooper();
        }

        void StartLooper()
        {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson()
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");
            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        void StartMain()
        {
            lua.DoFile("Main.lua");
            LuaFunction main = lua.GetFunction("Main");
            main.Call();
            main.Dispose();
            main = null;
        }

        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs()
        {
            lua.OpenLibs(LuaDLL.luaopen_pb);
            //            lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            //            lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.OpenLibs(LuaDLL.luaopen_socket_core);

            this.OpenCJson();
        }

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath()
        {
#if UNITY_EDITOR
            if (FrameworkConst.DebugMode)
            {
                lua.AddSearchPath(FrameworkConst.CustomLuaDir);
                lua.AddSearchPath(FrameworkConst.ToLuaDir);
                return;
            }
#endif
            lua.AddSearchPath(PlatformPath.DataPath + "lua");
            Debug.Log("InitLuaPath");
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        void InitLuaBundle()
        {
            //            if (loader.beZip) {
            //                loader.AddBundle("lua/lua.unity3d");
            //                loader.AddBundle("lua/lua_math.unity3d");
            //                loader.AddBundle("lua/lua_system.unity3d");
            //                loader.AddBundle("lua/lua_system_reflection.unity3d");
            //                loader.AddBundle("lua/lua_unityengine.unity3d");
            //                loader.AddBundle("lua/lua_common.unity3d");
            //                loader.AddBundle("lua/lua_logic.unity3d");
            //                loader.AddBundle("lua/lua_view.unity3d");
            //                loader.AddBundle("lua/lua_controller.unity3d");
            //                loader.AddBundle("lua/lua_misc.unity3d");
            //
            //                loader.AddBundle("lua/lua_protobuf.unity3d");
            //                loader.AddBundle("lua/lua_3rd_cjson.unity3d");
            //                loader.AddBundle("lua/lua_3rd_luabitop.unity3d");
            //                loader.AddBundle("lua/lua_3rd_pbc.unity3d");
            //                loader.AddBundle("lua/lua_3rd_pblua.unity3d");
            //                loader.AddBundle("lua/lua_3rd_sproto.unity3d");
            //            }
        }

        public void DoFile(string filename)
        {
            lua.DoFile(filename);
        }

        public T DoFile<T>(string filename)
        {
            return lua.DoFile<T>(filename);
        }

        public LuaFunction GetFunction(string funcname)
        {
            return lua.GetFunction(funcname);
        }

        public LuaTable GetTable(string tablename)
        {
            return lua.GetTable(tablename);
        }

        // Update is called once per frame
        public object[] CallFunction(string funcName, params object[] args)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                return func.LazyCall(args);
            }
            return null;
        }

        public void LuaGC()
        {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close()
        {
            loop.Destroy();
            loop = null;

            lua.Dispose();
            lua = null;
            //            loader = null;
        }

        private void OnDestroy()
        {
            Close();
        }
    }
}