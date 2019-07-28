#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.FSM{

	[AddComponentMenu("")]
	abstract public class FSMNodeBase : NodeBase{

		public string stateName;

		public override int maxInConnections{
			get{return -1;}
		}

		public override int maxOutConnections{
			get{return -1;}
		}

		public override System.Type outConnectionType{
			get{return typeof(FSMConnection);}
		}

		protected FSMContainer fsm{
			get{return (FSMContainer)graph;}
		}

		//Enter...
		protected override NodeStates OnExecute(){

			if (!string.IsNullOrEmpty(stateName))
				graphAgent.SendMessage("OnStateEnter", stateName, SendMessageOptions.DontRequireReceiver);

			return NodeStates.Running;
		}

		//Stay...
		virtual public void OnUpdate(){

			elapsedTime += Time.deltaTime;

			for (int i= 0; i < outConnections.Count; i++){

				FSMConnection connection = outConnections[i] as FSMConnection;
				
				if (connection.CheckCondition(graphAgent, graphBlackboard)){
					if (nodeState != NodeStates.Running || connection.condition != null){
						fsm.EnterState(connection.targetNode as FSMNodeBase);

						//this is done for editor
						connection.connectionState = NodeStates.Success;
						//

						return;
					}
				}
			}

			if (nodeState == NodeStates.Success && outConnections.Count == 0)
				graph.StopGraph();
		}

		//Exit...
		protected override void OnReset(){

			if (!string.IsNullOrEmpty(stateName))
				graphAgent.SendMessage("OnStateExit", stateName, SendMessageOptions.DontRequireReceiver);
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		private static Port clickedPort;

		class Port{

			public FSMNodeBase parent;
			public Vector2 pos;

			public Port(FSMNodeBase parent, Vector2 pos){
				this.parent = parent;
				this.pos = pos;
			}
		}

		sealed protected override void DrawNodeConnections(){

			var e = Event.current;

			if (maxOutConnections == 0){
				if (e.type == EventType.MouseUp && ID == graph.allNodes.Count)
					clickedPort = null;
				return;
			}

			var portRectLeft = new Rect(0,0,20,20);
			var portRectRight = new Rect(0,0,20,20);

			portRectLeft.center = new Vector2(nodeRect.x - 12, nodeRect.yMax - 10);
			portRectRight.center = new Vector2(nodeRect.xMax + 12, nodeRect.yMax - 10);
			GUI.color = new Color(0,0,0,0.5f);
			GUI.Box(portRectLeft, "", "nodeInputRight");
			GUI.Box(portRectRight, "", "nodeInputLeft");
			GUI.color = Color.white;

			if (e.button == 0 && e.type == EventType.MouseDown){
				
				if (portRectLeft.Contains(e.mousePosition))
					clickedPort = new Port(this, portRectLeft.center);
				
				if (portRectRight.Contains(e.mousePosition))
					clickedPort = new Port(this, portRectRight.center);
			}

			if (clickedPort != null && clickedPort.parent == this)
				Handles.DrawBezier(clickedPort.pos, e.mousePosition, clickedPort.pos, e.mousePosition, new Color(0.5f,0.5f,0.8f,0.8f), null, 2);

			if (clickedPort != null && e.type == EventType.MouseUp){
				
				var port = clickedPort;

				if (ID == graph.allNodes.Count)
					clickedPort = null;

				if (nodeRect.Contains(e.mousePosition)){
					
					foreach(FSMConnection connection in inConnections){
						if (connection.sourceNode == port.parent){
							Debug.LogWarning("State is already connected to target state. Consider using ConditionList on the existing transition if you want to check multiple conditions");
							return;
						}
					}

					graph.ConnectNode(port.parent, this);
				}
			}

			for (int i = 0; i < outConnections.Count; i++){
				FSMConnection connection = outConnections[i] as FSMConnection;
				Vector2 targetPos = (connection.targetNode as FSMNodeBase).GetConnectedInPortPosition(connection);
				Vector2 sourcePos = Vector2.zero;

				if (nodeRect.center.x <= targetPos.x)
					sourcePos = portRectRight.center;
				if (nodeRect.center.x > targetPos.x)
					sourcePos = portRectLeft.center;

				connection.DrawConnectionGUI(sourcePos, targetPos);
			}
		}

		private Vector2 GetConnectedInPortPosition(ConnectionBase connection){

			var sourcePos = connection.sourceNode.nodeRect.center;
			var thisPos = nodeRect.center;

			if (sourcePos.x > nodeRect.x && sourcePos.x < nodeRect.xMax)
				return new Vector2(nodeRect.xMax, nodeRect.y + 10);	
			
			if (sourcePos.y > nodeRect.y - 100 && sourcePos.y < nodeRect.yMax){
				if (sourcePos.x <= thisPos.x)
					return new Vector2(nodeRect.x, nodeRect.y + 10);
				if (sourcePos.x > thisPos.x)
					return new Vector2(nodeRect.xMax, nodeRect.y + 10);
			}

			if (sourcePos.y <= thisPos.y)
				return new Vector2(nodeRect.center.x, nodeRect.y);
			if (sourcePos.y > thisPos.y)
				return new Vector2(nodeRect.center.x, nodeRect.yMax);

			return thisPos;
		}
		
		protected override void OnNodeGUI(){

			if (Application.isPlaying)
				GUILayout.Label(elapsedTime.ToString());
		}
		
		protected override void OnNodeInspectorGUI(){

			stateName = EditorGUILayout.TextField("State Name", stateName);

			EditorUtils.Separator();

			GUI.color = Color.yellow;
			GUILayout.Label("Transitions");
			GUI.color = Color.white;

			if (outConnections.Count == 0){
				GUI.backgroundColor = new Color(1,1,1,0.5f);
				GUILayout.BeginHorizontal("box");
				GUILayout.Label("No Transitions");
				GUILayout.EndHorizontal();
				GUI.backgroundColor = Color.white;
			}

			bool onFinishExists = false;
			for (int i= 0; i < outConnections.Count; i++){

				GUI.backgroundColor = new Color(1,1,1,0.5f);
				GUILayout.BeginHorizontal("box");
					FSMConnection connection= outConnections[i] as FSMConnection;

					if (connection.condition){
					
						GUILayout.Label(connection.condition.taskInfo);
					
					} else {
					
						GUILayout.Label("OnFinish" + (onFinishExists? " (only first will execute)" : "" ));
						onFinishExists = true;
					}

					GUILayout.FlexibleSpace();
					GUILayout.Label("--> '" + connection.targetNode.nodeName + "'");
					if (GUILayout.Button(">"))
						NodeGraphContainer.currentSelection = outConnections[i];

				GUILayout.EndHorizontal();
				GUI.backgroundColor = Color.white;
			}

			EditorUtils.Separator();
		}

		#endif
	}
}