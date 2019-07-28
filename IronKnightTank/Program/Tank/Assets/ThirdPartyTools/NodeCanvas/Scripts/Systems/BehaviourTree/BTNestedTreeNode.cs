using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	[ScriptName("NestedBT")]
	[ScriptCategory("Nested")]
	public class BTNestedTreeNode : BTNodeBase{

		[SerializeField]
		private BTContainer _nestedTree;
		private bool instanceChecked;

		private BTContainer nestedTree{
			get {return _nestedTree;}
			set
			{
				_nestedTree = value;
				if (_nestedTree != null){
					_nestedTree.agent = graphAgent;
					_nestedTree.blackboard = graphBlackboard;
				}
			}
		}

		public override string nodeName{
			get {return "BEHAVIOUR";}
		}

		public override string nodeDescription{
			get {return "Nested Tree can be assigned an entire Behavior Tree graph. The prime node of that graph will be considered child node of this node and will return whatever the child returns";}
		}

		public override int maxOutConnections{
			get {return 0;}
		}

		public override System.Type outConnectionType{
			get {return typeof(ConnectionBase);}
		}

		/////////
		/////////

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			CheckInstance();

			if (nestedTree && nestedTree.primeNode)
				return nestedTree.primeNode.Execute(agent, blackboard);

			return NodeStates.Success;
		}

		protected override void OnReset(){

			if (nestedTree && nestedTree.primeNode)
				nestedTree.primeNode.ResetNode();
		}

		private void CheckInstance(){

			if (!instanceChecked && nestedTree != null && nestedTree.transform.parent != graph.transform){
				nestedTree = (BTContainer)Instantiate(nestedTree, transform.position, transform.rotation);
				nestedTree.transform.parent = graph.transform;
				instanceChecked = true;	
			}
		}

		////////////////////////////
		//////EDITOR AND GUI////////
		////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){
		    
		    if (nestedTree){

		    	GUILayout.Label("'" + nestedTree.graphName + "'");
		    	if (GUILayout.Button("EDIT"))
		    		graph.nestedGraphView = nestedTree;

			} else {
				
				if (GUILayout.Button("CREATE NEW"))
					nestedTree = CreateNewNestedTree();
			}
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();
		    nestedTree = UnityEditor.EditorGUILayout.ObjectField("Behaviour Tree", nestedTree, typeof(BTContainer), true) as BTContainer;
	    	if (nestedTree == this.graph){
		    	Debug.LogWarning("You can't have a Graph nested to iteself! Please select another");
		    	nestedTree = null;
		    }
		}

		public BTContainer CreateNewNestedTree(){

			BTContainer newTree = new GameObject("NestedBT").AddComponent(typeof(BTContainer)) as BTContainer;
			newTree.transform.parent = graph != null? graph.transform : null;
			newTree.transform.localPosition = Vector3.zero;
			nestedTree = newTree;
			return newTree;
		}

		#endif
	}
}