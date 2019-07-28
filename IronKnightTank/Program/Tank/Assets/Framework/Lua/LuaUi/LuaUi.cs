// ----------------------------------------------------------------------------
// <copyright file="LuaUi.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>21/12/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.Lua.LuaUi
{
    using System;
    using System.Collections.Generic;

    using Assets.Framework.Lua.LuaUi.Event;
    using Assets.Tools.Script.Attributes;
    using Assets.Tools.Script.Editor.Tool;

    using LuaInterface;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class LuaUi : MonoBehaviour
    {
        /// <summary>
        /// The file name
        /// </summary>
        public string FileName;

        /// <summary>
        /// The prefab path
        /// </summary>
        public string PrefabPath;

        /// <summary>
        /// The fields
        /// </summary>
        public List<LuaUiField> Fields;

        /// <summary>
        /// The events
        /// </summary>
        public List<LuaUiEvent> Events;

        /// <summary>
        /// The life
        /// </summary>
        public LuaUiLife Life = null;

        /// <summary>
        /// The binding table
        /// </summary>
        public LuaTable BindingTable;

        /// <summary>
        /// Initializes and bind to the lua table
        /// </summary>
        /// <param name="outputFields">The output fields.</param>
        /// <param name="bindingTable">The bound table.</param>
        public void Initialize(LuaTable outputFields, LuaTable bindingTable)
        {
#if UNITY_EDITOR
            ReferenceCounter.Mark("LuaUi", this, this.gameObject.name);
#endif
            this.BindingTable = bindingTable;

            this.GetFields(outputFields);

            this.ManulAddEvent(bindingTable);

            this.InitLife(bindingTable);
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public Object GetField(int index)
        {
            Debug.LogFormat("GetField is obsolete, replace lua file with LuaUi at {0}".SetColor(Color.yellow), this.gameObject.name);
            try
            {
                return this.Fields[index].FieldType;
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("{0} get field({2}) error:\r\n{1}", this.FileName, e, index));
                return null;
            }
        }

        public void GetFields(LuaTable outputFields)
        {
            var luaState = outputFields.GetLuaState();
            int top = luaState.LuaGetTop() + 1;
            for (int i = 0; i < this.Fields.Count; i++)
            {
                luaState.Push(outputFields);
                luaState.PushVariant(this.Fields[i].FieldType);
                luaState.LuaRawSetI(top, i + 1);
                //                outputFields[i + 1] = this.Fields[i].FieldType;
            }
        }

        /// <summary>
        /// Manuls the add event.
        /// </summary>
        /// <param name="bindingTable">The table.</param>
        public void ManulAddEvent(LuaTable bindingTable)
        {
            for (int i = 0; i < this.Events.Count; i++)
            {
                this.Events[i].Binding(bindingTable);
            }
        }

        /// <summary>
        /// Initializes the life.
        /// </summary>
        /// <param name="boundTable">The bound table.</param>
        public void InitLife(LuaTable boundTable)
        {
            if (this.Life.Dispose)
            {
                this.Life.DisposeCallBack = boundTable.RawGetLuaFunction("Dispose");
            }
            if (this.Life.OnDisable)
            {
                this.Life.OnDisableCallBack = boundTable.RawGetLuaFunction("OnDisable");
            }
            if (this.Life.OnEnable)
            {
                this.Life.OnEnableCallBack = boundTable.RawGetLuaFunction("OnEnable");
            }
        }

        /// <summary>
        /// Called when [destroy].
        /// </summary>
        private void OnDestroy()
        {
            if (this.Life.DisposeCallBack != null)
            {
                this.Life.DisposeCallBack.Call();
                this.Life.DisposeCallBack.Dispose();
            }
            if (this.Life.OnEnableCallBack != null)
            {
                this.Life.OnEnableCallBack.Dispose();
            }
            if (this.Life.OnDisableCallBack != null)
            {
                this.Life.OnDisableCallBack.Dispose();
            }
            this.Life = null;

            foreach (var luaUiEvent in this.Events)
            {
                luaUiEvent.Dispose();
            }

            this.Fields = null;
            this.Events = null;
            this.Life = null;

            if (this.BindingTable != null)
            {
                this.BindingTable.Dispose();
                this.BindingTable = null;
            }
        }


        /// <summary>
        /// Called when [enable].
        /// </summary>
        private void OnEnable()
        {
            if (this.Life.OnEnableCallBack != null)
            {
                this.Life.OnEnableCallBack.Call();
            }
        }

        /// <summary>
        /// Called when [disable].
        /// </summary>
        private void OnDisable()
        {
            if (this.Life.OnDisableCallBack != null)
            {
                this.Life.OnDisableCallBack.Call();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class LuaUiField
        {
            /// <summary>
            /// The source
            /// </summary>
            public Object Source;

            /// <summary>
            /// The field name
            /// </summary>
            [InspectorStyle("FieldName", "LuaUiFieldName")]
            public string FieldName;

            /// <summary>
            /// The field type
            /// </summary>
            [InspectorStyle("FieldType", "LuaUiFieldType")]
            public Object FieldType;

            /// <summary>
            /// The is public
            /// </summary>
            public bool IsPublic = false;
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class LuaUiEvent
        {
            /// <summary>
            /// The node
            /// </summary>
            public GameObject Node;

            /// <summary>
            /// The event name
            /// </summary>
            public string EventType;

            /// <summary>
            /// Gets the name of the static.
            /// </summary>
            /// <value>The name of the static.</value>
            public string FullName{get{return string.Format("On{0}{1}", this.EventType, this.Node.name);}}

            /// <summary>
            /// event trigger and transpond
            /// </summary>
            private ILuaEventTrigger eventTrigger;

            public void Binding(LuaTable bindingTable)
            {
                LuaFunction handler = bindingTable.GetLuaFunction(this.FullName);
                this.eventTrigger = this.Create(this.Node, handler, bindingTable);
            }

            public void Dispose()
            {
                if (this.eventTrigger != null)
                {
                    this.eventTrigger.Dispose();
                    this.eventTrigger = null;
                }
            }
        }

        [Serializable]
        public class LuaUiLife
        {
            public bool OnEnable;
            public bool OnDisable;
            public bool Dispose = true;

            [HideInInspector]
            public LuaFunction OnEnableCallBack;
            [HideInInspector]
            public LuaFunction OnDisableCallBack;
            [HideInInspector]
            public LuaFunction DisposeCallBack;
        }
    }
}