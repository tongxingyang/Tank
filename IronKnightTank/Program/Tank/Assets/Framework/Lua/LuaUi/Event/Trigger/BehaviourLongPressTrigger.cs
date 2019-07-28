using Assets.Framework.Lua.LuaUi.Event;

using LuaInterface;

using UnityEngine;

namespace Framework.Lua.LuaUi.Event.Trigger
{
    using Assets.Scripts.Game.Tools;

    public class BehaviourLongPressTrigger : MonoBehaviour ,  ILuaEventTrigger
    {
        private static string Signature = "(isEnd)";
        private static string Event = "OnLongPress";

        public const float LongPressTriggerTime = 1.5f;
        private LuaFunction handler;
        private LuaTable self;

        public ILuaEventTrigger Binding(GameObject go, LuaFunction handler, LuaTable self)
        {
            var behaviourLongPressTrigger = go.AddComponent<BehaviourLongPressTrigger>();
            behaviourLongPressTrigger.handler = handler;
            behaviourLongPressTrigger.self = self;
            return behaviourLongPressTrigger;
        }

        public void Dispose()
        {
            this.handler.Dispose();
            this.handler = null;
            this.self = null;
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


        private float pressTime;

        private bool isMouseDown;

        private bool isSuccess;
        private void OnMouseDown()
        {
            if (UGUITools.IsPointerOverUIObject())
            {
                return;
            }
            this.isMouseDown = true;
            this.pressTime = 0;
        }

        private void OnMouseUp()
        {
            this.isMouseDown = false;
            if (this.isSuccess)
            {
                this.handler.Call(this.self, false);
            }
            
        }

        void Update()
        {
            if (this.isMouseDown)
            {
                this.pressTime += Time.deltaTime;
                if (this.pressTime > LongPressTriggerTime)
                {
                    this.handler.Call(this.self ,true);
                    this.isSuccess = true;
                }
            }
        
        }

    }
}
