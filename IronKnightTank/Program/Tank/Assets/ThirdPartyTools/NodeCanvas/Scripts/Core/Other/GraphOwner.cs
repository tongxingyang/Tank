using UnityEngine;
using System;

namespace NodeCanvas{

	///The base class where BehaviourTreeOwner and FSMOwner derive from.
	abstract public class GraphOwner : MonoBehaviour {

		public bool executeOnStart = true;

		[SerializeField]
		private Blackboard _blackboard;

		public Blackboard blackboard{
			get {return _blackboard;}
			set {_blackboard = value; if (graph != null) graph.blackboard = value;}
		}

		abstract public NodeGraphContainer graph{ get; set; }
		abstract public System.Type graphType{ get; }

		///Start the graph assigned
		public void StartGraph(){
			if (graph != null)
				graph.StartGraph(this, blackboard);
		}

		///Start the graph assigned providing a callback for when it ends
		public void StartGraph(Action callback){
			if (graph != null)
				graph.StartGraph(this, blackboard, callback);
		}

		///Stop the graph assigned
		public void StopGraph(){
			if (graph != null)
				graph.StopGraph();
		}

		///Send an event through the graph (To be used with CheckEvent for example)
		public void SendEvent(string eventName){
			if (graph != null)
				graph.SendEvent(eventName);
		}

		new public void SendMessage(string name){
			SendMessage(name, null);
		}

		///Sends a message to all tasks in the graph as well as this gameobject as usual.
		new public void SendMessage(string name, System.Object arg){
			if (graph != null)
				graph.SendMessage(name, arg);
		}

		void Reset(){

			blackboard = gameObject.GetComponent<Blackboard>();
			if (blackboard == null)
				blackboard = gameObject.AddComponent<Blackboard>();		
		}

		void Awake(){

			if (graph != null && graph.transform.parent != this.transform){
				graph = (NodeGraphContainer)Instantiate(graph, transform.position, transform.rotation);
				graph.transform.parent = this.transform;
			}

			if (graph != null)
				graph.gameObject.hideFlags = HideFlags.HideInHierarchy;
		}

		void OnEnable(){
			
			if (executeOnStart && !graph.isRunning)
				StartGraph();
		}

		void OnDisable(){

			StopGraph();
		}

		void OnDrawGizmos(){
			Gizmos.DrawIcon(transform.position, "GraphOwner.png", true);
		}
	}
}