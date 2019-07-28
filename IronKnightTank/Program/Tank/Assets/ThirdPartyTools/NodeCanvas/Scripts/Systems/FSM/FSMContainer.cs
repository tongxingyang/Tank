using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.FSM{

	[AddComponentMenu("")]
	///A State Machine container
	public class FSMContainer : NodeGraphContainer{

		private FSMNodeBase currentState;
		private List<FSMAnyStateLink> anyStates = new List<FSMAnyStateLink>();

		///The current state name. null if none
		public string currentStateName{
			get
			{
				if (currentState == null)
					return null;
				return currentState.nodeName;
			}
		}

		public override System.Type baseNodeType{
			get {return typeof(FSMNodeBase);}
		}

		protected override void OnGraphStarted(){

			anyStates.Clear();
			foreach(NodeBase node in allNodes){

				if (node.GetType() == typeof(FSMConcurrentState))
					node.Execute(agent, blackboard);

				if (node.GetType() == typeof(FSMAnyStateLink))
					anyStates.Add(node as FSMAnyStateLink);
			}

			EnterState(currentState == null? primeNode as FSMNodeBase : currentState);
		}

		protected override void OnGraphUpdate(){

			foreach(FSMAnyStateLink anyState in anyStates)
				anyState.OnUpdate();

			currentState.OnUpdate();
		}

		protected override void OnGraphStoped(){

			currentState = null;
		}

		public void EnterState(FSMNodeBase state){

			if (!isRunning){
				Debug.LogWarning("Tried to EnterState on an FSM that was not running", gameObject);
				return;
			}

			if (state == currentState)
				return;

			if (currentState != null){
				
				currentState.ResetNode();
				
				//for editor
				foreach (ConnectionBase inConnection in currentState.inConnections)
					inConnection.connectionState = NodeStates.Resting;
				///
			}

			state.Execute(agent, blackboard);
			currentState = state;
		}

		///Trigger a state to enter by name
		public void TriggerState(string stateName){

			foreach (NodeBase node in allNodes){
				if ((node as FSMNodeBase).stateName == stateName ){
					EnterState(node as FSMNodeBase);
					return;
				}
			}

			Debug.LogWarning("No State with name '" + stateName + "' found on FSM '" + graphName + "'");
		}

		///Get all machine State Names
		public List<string> GetStateNames(){

			var names = new List<string>();
			foreach(FSMNodeBase node in allNodes){
				if (!string.IsNullOrEmpty(node.stateName))
					names.Add(node.stateName);
			}
			return names;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		[UnityEditor.MenuItem("GameObject/Create Other/NodeCanvas/FSM")]
		public static void Create(){

			FSMContainer newFSM= new GameObject("FSM").AddComponent(typeof(FSMContainer)) as FSMContainer;
			UnityEditor.Selection.activeObject = newFSM;
		}
		
		#endif
	}
}