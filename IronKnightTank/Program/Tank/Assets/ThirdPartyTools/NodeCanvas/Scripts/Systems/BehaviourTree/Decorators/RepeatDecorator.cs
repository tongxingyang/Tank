#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	[ScriptName("Repeater")]
	[ScriptCategory("Decorators")]
	public class RepeatDecorator : BTDecoratorNode{

		public enum RepeatTypes {

			RepeatTimes,
			RepeatUntil
		}

		public enum RepeatUntil {

			Failure = 0,
			Success = 1
		}

		public RepeatTypes repeatType= RepeatTypes.RepeatTimes;
		public RepeatUntil repeatUntil= RepeatUntil.Success;
		public int repeatTimes = 1;

		private int currentIteration = 1;

		public override string nodeName{
			get{return "Repeater";}
		}

		public override string nodeDescription{
			get {return "Repeater will repeat it's child either x times, or until it returns the specified state";}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (!decoratedConnection)
				return NodeStates.Resting;

			nodeState = decoratedConnection.Execute(agent, blackboard);
			repeatTimes = Mathf.Max(repeatTimes, 1);

			if (nodeState == NodeStates.Success || nodeState == NodeStates.Failure)
				return Check();

			return nodeState;
		}

		private NodeStates Check(){

			if (repeatType == RepeatTypes.RepeatTimes){

				if (currentIteration >= repeatTimes)
					return nodeState;

				currentIteration ++;

			} else {

				if ((int)nodeState == (int)repeatUntil)
					return nodeState;
			}

			decoratedConnection.ResetConnection();
			return NodeStates.Running;
		}

		protected override void OnReset(){

			currentIteration = 1;
		}


		/////////////////////////////////////////
		/////////GUI AND EDITOR STUFF////////////
		/////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){

			if (repeatType == RepeatTypes.RepeatTimes){

				GUILayout.Label(repeatTimes + " Times");
				if (Application.isPlaying)
					GUILayout.Label("Itteration: " + currentIteration.ToString());

			} else {

				GUILayout.Label("Until " + repeatUntil);
			}
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();

			repeatType = (RepeatTypes)EditorGUILayout.EnumPopup("Repeat Type", repeatType);

			if (repeatType == RepeatTypes.RepeatTimes){

				repeatTimes = EditorGUILayout.IntField("Times", repeatTimes);

			} else {

				repeatUntil = (RepeatUntil)EditorGUILayout.EnumPopup("Repeat Until", repeatUntil);
			}		
		}

		#endif
	}
}