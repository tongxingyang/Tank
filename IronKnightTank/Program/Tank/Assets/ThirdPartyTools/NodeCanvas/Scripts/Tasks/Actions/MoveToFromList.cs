using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Move To From List")]
	[ScriptCategory("Movement")]
	[AgentType(typeof(UnityEngine.AI.NavMeshAgent))]
	public class MoveToFromList : ActionTask{

		[RequiredField]
		public BBGameObjectList targetList = new BBGameObjectList(){useBlackboard = true};

		private Vector3 targetPos;
		private int randomIndex;

		//for faster acccess
		private UnityEngine.AI.NavMeshAgent navAgent{
			get {return (UnityEngine.AI.NavMeshAgent)agent;}
		}

		protected override string actionInfo{
			get {return "Random Patrol " + targetList.ToString();}
		}

		protected override void OnExecute(){

			int newValue = Random.Range(0, targetList.value.Count);
			while(newValue == randomIndex)
				newValue = Random.Range(0, targetList.value.Count);

			randomIndex = newValue;
			targetPos = targetList.value[randomIndex].transform.position;

			if ( (navAgent.transform.position - targetPos).magnitude < navAgent.stoppingDistance){
				EndAction(true);
				return;
			}

			if ( !navAgent.SetDestination( targetPos) )
				EndAction(false);
		}

		protected override void OnUpdate(){

			targetPos = targetList.value[randomIndex].transform.position;

			if (navAgent.destination != targetPos){

				if ( !navAgent.SetDestination( targetPos) ){
					EndAction(false);
					return;
				}
			}

			if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
				EndAction(true);
		}

		protected override void OnStop(){

			if (navAgent.gameObject.activeSelf)
				navAgent.ResetPath();
		}
	}
}