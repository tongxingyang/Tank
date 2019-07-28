using Assets.Tools.Script.Event.Message;
using UnityEditor;

namespace Assets.Tools.Script.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MessageRepeater), true)]
    public class MessageRepeaterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            MessageRepeater messageRepeater = target as MessageRepeater;
            if (messageRepeater.handlers!=null)
            {
                foreach (var o in messageRepeater.handlers)
                {
                    if (!NEditorTools.DrawHeader(o.messageName)) continue;              
                    NEditorTools.BeginContents();
                    ActionCallEditor.Field(messageRepeater, o.handler);
                    NEditorTools.EndContents();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

    }


}