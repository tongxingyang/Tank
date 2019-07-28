
namespace Framework.Lua.LuaUi.Event.Trigger
{
    using UnityEngine.UI;

    using Assets.Framework.Lua.LuaUi.Event;

    using LuaInterface;

    using UnityEngine;

    public class UGUIInputFieldEndEditTrigger :  ILuaEventTrigger
    {
        private static string Signature = "(content)";
        private static string Event = "InputFieldEndEdit";

        private LuaFunction handler;

        private LuaTable self;

        public ILuaEventTrigger Binding(GameObject go, LuaFunction handler, LuaTable self)
        {
            var endEditTrigger = new UGUIInputFieldEndEditTrigger();
            var inputField = go.GetComponent<InputField>();
            if (inputField != null)
            {
                inputField.onEndEdit.AddListener(
                    endEditTrigger.OnEndEdit);
            }
            else
            {
                Debug.Log("input filed nil wrong");
            }

            endEditTrigger.handler = handler;
            endEditTrigger.self = self;
            return endEditTrigger;
        }


        public void OnEndEdit(string str)
        {
            this.handler.Call(this.self , str);
        }

        public void Dispose()
        {
            this.handler = null;
            this.self = null;
        }
        public string HandlerSignature {
            get
            {
                return Signature;
            }
        }

        public string EventName {
            get
            {
                return Event;
            }
        }
    }
}