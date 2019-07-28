﻿#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	[ScriptName("Probability Selector")]
	[ScriptCategory("Composites")]

	//TODO: Make use of the BBFloats
	public class BTProbabilitySelector : BTNodeBase {

		[SerializeField]
		private List<BBFloat> childWeights = new List<BBFloat>();
		[SerializeField]
		private BBFloat failChance = new BBFloat();

		private float probability;
		private float currentProbability;
		private float total;

		public override string nodeName{
			get {return "<color=#b3ff7f>%SELECTOR</color>";}
		}

		public override string nodeDescription{
			get {return "The Probability Selector will select and execute a child node based on it's chance to be selected. It will then return whatever that child returns. The Probability Selector may immediately return Failure if a 'Failure Chance' is introduced or no childred are connected.";}
		}

		public override void OnPortConnected(int index){
			childWeights.Insert(index, new BBFloat{value = 1, bb = graphBlackboard});
		}

		public override void OnPortDisconnected(int index){
			childWeights.RemoveAt(index);
		}

		//To create a new probability when BT starts
		protected override void OnInit(){
			OnReset();
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			currentProbability = probability;

			for (int i = 0; i < outConnections.Count; i++){

				if (currentProbability >= childWeights[i].value){
					currentProbability -= childWeights[i].value;
					continue;
				}

				nodeState = outConnections[i].Execute(agent, blackboard);

				if (nodeState == NodeStates.Running)
					return nodeState;

				if (nodeState == NodeStates.Success)
					return nodeState;
			}

			return NodeStates.Failure;
		}

		protected override void OnReset(){

			CalcTotal();
			probability = Random.Range(0f, total);
		}


		private void CalcTotal(){
			
			total = failChance.value;
			foreach (BBFloat weight in childWeights)
				total += weight.value;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){
		
			if (outConnections.Count == 0){
				GUILayout.Label("No Connections");
				return;
			}

			CalcTotal();

			if (total == 0){
				GUILayout.Label("100% Failure");
				return;
			}

			string weightsString = string.Empty;
			for (int i = 0; i < childWeights.Count; i++)
				weightsString += Mathf.Round( (childWeights[i].value/total) * 100 ) + "%" + ( (i == childWeights.Count - 1)? " " : ", ");

			GUILayout.Label(weightsString);
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();

			if (outConnections.Count == 0){
				GUILayout.Label("Make some connections first");
				return;
			}

			CalcTotal();

			for (int i = 0; i < childWeights.Count; i++){

				GUILayout.BeginHorizontal();
				childWeights[i].value = EditorGUILayout.Slider("Weight", childWeights[i].value, 0, 1);
				GUILayout.Label( Mathf.Round( (childWeights[i].value/total) * 100 ) + "%", GUILayout.Width(30));
				childWeights[i].value = Mathf.Floor(childWeights[i].value * 10 + 0.1f) / 10;
				GUILayout.EndHorizontal();
			}

			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			failChance.value = EditorGUILayout.Slider("Direct Failure Chance", failChance.value, 0, 1);
			GUILayout.Label( Mathf.Round( (failChance.value/total) * 100 ) + "%", GUILayout.Width(30));
			failChance.value = Mathf.Floor(failChance.value * 10 + 0.1f) / 10;
			GUILayout.EndHorizontal();
		}
		
		#endif
	}
}