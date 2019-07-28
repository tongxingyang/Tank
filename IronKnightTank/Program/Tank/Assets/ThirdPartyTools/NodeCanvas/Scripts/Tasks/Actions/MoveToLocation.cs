using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Move To GameObject")]
	[ScriptCategory("Movement")]
	[AgentType(typeof(UnityEngine.AI.NavMeshAgent))]
	public class MoveToLocation : ActionTask{

		[RequiredField]
		public BBGameObject target = new BBGameObject();
		public float keepDistance = 0.1f;

		private Vector3 targetPos;


		//for faster access
		private UnityEngine.AI.NavMeshAgent navAgent{
			get {return (UnityEngine.AI.NavMeshAgent)agent; }
		}

		protected override string actionInfo{
			get {return "GoTo " + target.ToString();}
		}

		protected override void OnExecute(){

			if (target.value == null){
				Debug.LogError("Target GameObject is not set correctly on Move To GameObject Action", gameObject);
				EndAction(false);
				return;
			}

			targetPos = target.value.transform.position;

			if ( (navAgent.transform.position - targetPos).magnitude < navAgent.stoppingDistance + keepDistance){
				EndAction(true);
				return;
			}

			if (!navAgent.SetDestination(targetPos))
				EndAction(false);
		}

		protected override void OnUpdate(){

			targetPos = target.value.transform.position;

			if (navAgent.destination != targetPos){
				if (!navAgent.SetDestination(targetPos)){
					EndAction(false);
					return;
				}
			}

			if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance + keepDistance)
				EndAction(true);
		}

		protected override void OnStop(){

			if (navAgent.gameObject.activeSelf)
				navAgent.ResetPath();
		}
	}
}