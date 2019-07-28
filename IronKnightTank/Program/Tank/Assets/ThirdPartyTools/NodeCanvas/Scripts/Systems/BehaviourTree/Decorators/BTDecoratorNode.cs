using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	abstract public class BTDecoratorNode : BTNodeBase{

		public override int maxOutConnections{
			get{return 1;}
		}

		public override int maxInConnections{
			get{return 1;}
		}

		public override System.Type outConnectionType{
			get{return typeof(ConnectionBase);}
		}

		protected ConnectionBase decoratedConnection{
			get
			{
				if (outConnections.Count != 0)
					return outConnections[0];
				return null;			
			}
		}

		protected NodeBase decoratedNode{
			get
			{
				if (outConnections.Count != 0)
					return outConnections[0].targetNode;
				return null;			
			}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (decoratedConnection)
				return decoratedConnection.Execute(agent, blackboard);

			return NodeStates.Failure;
		}
	}
}