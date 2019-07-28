namespace Assets.Framework.Lua.LuaUi.Event.Trigger
{
    using LuaInterface;

    using UnityEngine;
    using UnityEngine.EventSystems;

    public class UGUIPressTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ILuaEventTrigger
    {
        private static string Signature = "(isDown,gameObject)";
        private static string Event = "Press";

        private LuaFunction handler;
        private LuaTable self;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            this.handler.Call(this.self, true, this.gameObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.handler.Call(this.self, false, this.gameObject);
        }

        public void Dispose()
        {
            this.handler.Dispose();
            this.handler = null;
            this.self = null;
        }

        public ILuaEventTrigger Binding(GameObject go, LuaFunction handler, LuaTable self)
        {
            var uguiClickTrigger = go.AddComponent<UGUIPressTrigger>();
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
