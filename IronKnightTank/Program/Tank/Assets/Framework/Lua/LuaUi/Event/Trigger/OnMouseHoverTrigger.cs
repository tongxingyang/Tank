// ----------------------------------------------------------------------------
// <copyright file="OnMouseHoverTrigger.cs" company="上海序曲网络科技有限公司">
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
    using Assets.Scripts.Game.Tools;
    using LuaInterface;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnMouseHoverTrigger : MonoBehaviour, ILuaEventTrigger
    {
        private static string Signature = "(isHoverIn)";
        private static string Event = "OnMouseHover";

        private LuaFunction handler;
        private LuaTable self;
        private float hoverTime = 0;
        private const float HoverTime = 1f;
        private bool isMouseHover = false;
        private bool isHoverSuccess = false;

        public void OnMouseEnter()
        {
            isMouseHover = true;
            hoverTime = 0;
            isHoverSuccess = false;
        }

        public void OnMouseExit()
        {
            if (isHoverSuccess)
            {
                this.handler.Call(this.self, false);
            }
            isMouseHover = false;
        }

        void Update()
        {
            if (UGUITools.IsPointerOverUIObject()) ///点击在UI上面
            {
                return;
            }
            if (isMouseHover)
            {
                hoverTime += Time.deltaTime;
                if(hoverTime > HoverTime)
                {
                    this.handler.Call(this.self, true);
                    isHoverSuccess = true;
                    isMouseHover = false;
                }
            }
        }

        private void Reset()
        {
            isMouseHover = false;
            hoverTime = 0;
        }

        public void Dispose()
        {
            this.handler.Dispose();
            this.handler = null;
            this.self = null;
        }

        public ILuaEventTrigger Binding(GameObject go, LuaFunction handler, LuaTable self)
        {
            var mouseHoverTrigger = go.AddComponent<OnMouseHoverTrigger>();
            mouseHoverTrigger.handler = handler;
            mouseHoverTrigger.self = self;
            return mouseHoverTrigger;
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
