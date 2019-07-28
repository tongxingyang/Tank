// ----------------------------------------------------------------------------
// <copyright file="InputManager.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>23/07/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Scripts.Game.Input
{
    using LuaInterface;
    using System.Collections.Generic;
    using UnityEngine;

    public class InputManager : MonoBehaviour
    {
        private List<LuaFuncInfo> onEscActionList = new List<LuaFuncInfo>();
        private List<LuaFuncInfo> onRightMouseClickActionList = new List<LuaFuncInfo>();
        private List<LuaFuncInfo> onLeftMouseClickActionList = new List<LuaFuncInfo>();

        public static InputManager Instance { get { return _instance; } }
        private static InputManager _instance;
        private void Start()
        {
            _instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                for (int i = 0; i < this.onEscActionList.Count; i++)
                {
                    this.onEscActionList[i].Invoke();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                for (int i = 0; i < this.onRightMouseClickActionList.Count; i++)
                {
                    this.onRightMouseClickActionList[i].Invoke();
                }
            }
            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < this.onLeftMouseClickActionList.Count; i++)
                {
                    this.onLeftMouseClickActionList[i].Invoke();
                }
            }
            
        }

        public void AddOnEscListener(LuaFunction fuc , LuaTable table = null)
        {
            for (int i = 0; i < this.onEscActionList.Count; i++)
            {
                if(onEscActionList[i].Equals(fuc , table))
                {
                    return;
                }
            }
            onEscActionList.Add(new LuaFuncInfo(fuc, table));
        }

        public void RemoveOnEscListener(LuaFunction fuc , LuaTable table = null)
        {
            for (int i = 0; i < this.onEscActionList.Count; i++)
            {
                if(onEscActionList[i].Equals(fuc , table))
                {
                    onEscActionList.RemoveAt(i);
                    return;
                }
            }
        }

        public void AddOnRightMouseClickListener(LuaFunction fuc, LuaTable table = null)
        {
            for (int i = 0; i < this.onRightMouseClickActionList.Count; i++)
            {
                if (onRightMouseClickActionList[i].Equals(fuc, table))
                {
                    return;
                }
            }
            onRightMouseClickActionList.Add(new LuaFuncInfo(fuc, table));
        }

        public void RemoveOnRightMouseClickListener(LuaFunction fuc, LuaTable table = null)
        {
            for (int i = 0; i < this.onRightMouseClickActionList.Count; i++)
            {
                if (onRightMouseClickActionList[i].Equals(fuc, table))
                {
                    onRightMouseClickActionList.RemoveAt(i);
                }
            }
        }

        public void AddOnLeftMouseClickListener(LuaFunction fuc , LuaTable table = null)
        {
            for (int i = 0; i < this.onLeftMouseClickActionList.Count; i++)
            {
                if (onRightMouseClickActionList[i].Equals(fuc, table))
                {
                    return;
                }
            }
            onRightMouseClickActionList.Add(new LuaFuncInfo(fuc, table));
        }

        public void RemoveLeftMouseClickListener(LuaFunction fuc , LuaTable table = null)
        {
            for (int i = 0; i < this.onLeftMouseClickActionList.Count; i++)
            {
                if (onLeftMouseClickActionList[i].Equals(fuc, table))
                {
                    onLeftMouseClickActionList.RemoveAt(i);
                }
            }
        }

        private void OnDestroy()
        {
            this.onEscActionList = null;
            this.onRightMouseClickActionList = null;
        }

        private class LuaFuncInfo
        {
            public LuaFunction func;
            public LuaTable self;
            public LuaFuncInfo(LuaFunction luaFunction ) : this(luaFunction , null)
            {
               
            }

            public LuaFuncInfo(LuaFunction luaFunction , LuaTable luaTable)
            {
                this.func = luaFunction;
                this.self = luaTable;
            }

            public void Invoke()
            {
                if (self != null)
                {
                    func.Call(self);
                }
                else
                {
                    
                    func.Call();
                }
            }

            public bool Equals(LuaFunction luaFunction , LuaTable table)
            {
                return (this.func == luaFunction && this.self == table);
            }
            
        }
    }
}