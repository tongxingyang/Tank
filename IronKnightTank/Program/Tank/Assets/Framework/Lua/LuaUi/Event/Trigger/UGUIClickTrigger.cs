// ----------------------------------------------------------------------------
// <copyright file="UGUIClickTrigger.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>23/07/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.Lua.LuaUi.Event.Trigger
{
    using LuaInterface;

    using UnityEngine;
    using UnityEngine.EventSystems;

    public class UGUIClickTrigger : MonoBehaviour, IPointerClickHandler, ILuaEventTrigger
    {
        private static string Signature = "(eventData)";
        private static string Event = "Click";

        private LuaFunction handler;
        private LuaTable self;
        

        public void OnPointerClick(PointerEventData eventData)
        {
            this.handler.Call(this.self, eventData);
        }

        public void Dispose()
        {
            this.handler.Dispose();
            this.handler = null;
            this.self = null;
        }

        public ILuaEventTrigger Binding(GameObject go, LuaFunction handler, LuaTable self)
        {
            var uguiClickTrigger = go.AddComponent<UGUIClickTrigger>();
            uguiClickTrigger.handler = handler;
            uguiClickTrigger.self = self;
            return uguiClickTrigger;
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
