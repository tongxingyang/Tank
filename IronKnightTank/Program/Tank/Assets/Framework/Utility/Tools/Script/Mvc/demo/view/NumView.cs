using UnityEngine;

namespace Assets.Script.Mvc.demo.view
{
    //    using Boo.Lang;

    public class NumView : MonoView
    {
        private string label;

        //  private List<string> a;
        public override void OnRegister()
        {
            EventDispatcher.AddEventListener(TestEvent.NumChange, OnNumChange);
        }

        private void OnNumChange(IEvent obj)
        {
            label = (obj as Event<int>).Data.ToString();
        }

        private void OnGUI()
        {
            GUILayout.Label(label);
            if (GUILayout.Button("Add 1"))
            {
                EventDispatcher.DispatchEvent(TestEvent.AddNum, 1);
            }
            if (GUILayout.Button("Add 5"))
            {
                EventDispatcher.DispatchEvent(TestEvent.AddNum, 5);
            }
        }
    }
}