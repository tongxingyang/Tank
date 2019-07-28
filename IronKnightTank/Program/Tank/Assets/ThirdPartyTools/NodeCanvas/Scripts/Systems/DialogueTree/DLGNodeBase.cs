#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.DialogueTree{

	[AddComponentMenu("")]
	abstract public class DLGNodeBase : NodeBase, ITaskDefaults{

		[SerializeField]
		private string _actorName = "_Owner";

		public override string nodeName{
			get{return "#" + ID;}
		}

		public override int maxInConnections{
			get{return -1;}
		}

		public override int maxOutConnections{
			get{return 1;}
		}

		public override System.Type outConnectionType{
			get{return typeof(ConnectionBase);}
		}

		private string actorName{
			get
			{
				return _actorName;
			}
			set
			{
				_actorName = value;
				DLGTree.actorReferences[value] = DialogueActor.FindActorWithName(value);
				foreach (Task task in GetComponentsInChildren<Task>(true))
					task.SetOwnerDefaults(this);
			}
		}

		protected DialogueTreeContainer DLGTree{
			get{return (DialogueTreeContainer)graph;}
		}

		private List<string> actorNames{
			get
			{
				List<string> names = new List<string>(DLGTree.dialogueActorNames);
				names.Insert(0, "_Owner");
				return names;
			}
		}

		protected string finalActorName{
			get
			{
				if (!actorNames.Contains(actorName))
					return "<color=#d63e3e>*" + actorName + "*</color>";
				return actorName;
			}
		}

		protected DialogueActor finalActor{
			get
			{
				if (actorName == "_Owner" || string.IsNullOrEmpty(actorName)){
					
					if (graphAgent == null)
						return null;

					DialogueActor foundActor = graphAgent.GetComponent<DialogueActor>();
					if (foundActor == null)
						Debug.LogError("No DialogueActor Component on Graph Agent found", graphAgent);

					return foundActor;
				}

				if (!DLGTree.actorReferences.ContainsKey(actorName))
					DLGTree.actorReferences[actorName] = DialogueActor.FindActorWithName(actorName);

				return DLGTree.actorReferences[actorName];
			}
		}

		protected Blackboard finalBlackboard{
			get
			{
				if (actorName == "_Owner" || string.IsNullOrEmpty(actorName))
					return graphBlackboard;

				if (finalActor == null)
					return null;
				
				return finalActor.blackboard;
			}
		}

		//implementation
		public Component agent{
			get{return finalActor;}
		}

		//implementation
		public Blackboard blackboard{
			get{return finalBlackboard;}
		}

		//implementation
		public void SendDefaults(){
			foreach (Task task in GetComponentsInChildren<Task>(true))
				task.SetOwnerDefaults(this);
		}

		//implementation. Propts the graph to send event instead.
		public void SendEvent(string eventName){
			graph.SendEvent(eventName);
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

		}

		protected override void OnNodeInspectorGUI(){

			GUI.backgroundColor = EditorUtils.lightBlue;
			actorName = EditorUtils.StringPopup(actorName, actorNames, false, false);
			GUI.backgroundColor = Color.white;		
		}
		
		#endif
	}
}