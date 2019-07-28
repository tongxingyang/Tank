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
    using UnityEngine.UI;
    using UnityEngine;
   

    public class UGUIButtonClickTrigger : MonoBehaviour , ILuaEventTrigger
    {
        private static string Signature = "()";
        private static string Event = "ButtonClick";

        private LuaFunction handler;
        private LuaTable self;
        private Button button;

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    this.handler.Call(this.self, eventData);
        //}

        public void Dispose()
        {
            this.handler.Dispose();
            this.handler = null;
            this.self = null;
        }

        public ILuaEventTrigger Binding(GameObject go, LuaFunction handler, LuaTable self)
        {
            UGUIButtonClickTrigger uGUIButtonClickTrigger = go.AddComponent<UGUIButtonClickTrigger>();
            uGUIButtonClickTrigger.handler = handler;
            uGUIButtonClickTrigger.self = self;
            button = go.GetComponent<Button>();
            if (!button)
            {
                button = go.AddComponent<Button>();
            }
            button.onClick.AddListener(delegate ()
            {
                uGUIButtonClickTrigger.handler.Call(self);

            });

            return uGUIButtonClickTrigger;
        }

        void OnDestroy()
        {
            if (button)
            {
                button.onClick.RemoveAllListeners();
            }
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
