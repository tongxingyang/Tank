// ----------------------------------------------------------------------------
// <copyright file="OnMouseExitTrigger.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>24/07/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.Lua.LuaUi.Event.Trigger
{
    using Assets.Framework.Lua.LuaUi.Event;
    using Assets.Scripts.Game.Tools;
    using LuaInterface;
    using UnityEngine;

    public class OnMouseExitTrigger : MonoBehaviour, ILuaEventTrigger
    {

        private static string Signature = "()";
        private static string Event = "OnMouseExit";

        private LuaFunction handler;
        private LuaTable self;


        public void OnMouseExit()
        {
            if (UGUITools.IsPointerOverUIObject()) ///点击在UI上面
            {
                this.handler.Call(this.self);
                return;
            }

            this.handler.Call(this.self);
        }

        public void Dispose()
        {
            this.handler.Dispose();
            this.handler = null;
            this.self = null;
        }

        public ILuaEventTrigger Binding(GameObject go, LuaFunction handler, LuaTable self)
        {
            var mouseEnterTrigger = go.AddComponent<OnMouseExitTrigger>();
            mouseEnterTrigger.handler = handler;
            mouseEnterTrigger.self = self;
            return mouseEnterTrigger;
        }

        public string HandlerSignature
        {
            get
            {
                return Signature;
            }
        }

        public string EventName
        {
            get
            {
                return Event;
            }
        }
    }
}