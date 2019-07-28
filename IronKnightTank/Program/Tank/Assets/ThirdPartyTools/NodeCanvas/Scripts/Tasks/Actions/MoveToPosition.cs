
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Move To Position")]
	[ScriptCategory("Movement")]
	[AgentType(typeof(UnityEngine.AI.NavMeshAgent))]
	public class MoveToPosition : ActionTask{

		public BBVector TargetPosition= new BBVector();

		//for faster acccess
		private UnityEngine.AI.NavMeshAgent navAgent{
			get {return (UnityEngine.AI.NavMeshAgent)agent;}
		}

		protected override string actionInfo{
			get {return "GoTo " + TargetPosition.ToString();}
		}

		protected override void OnExecute(){

			if ( (navAgent.transform.position - TargetPosition.value).magnitude < navAgent.stoppingDistance){
				EndAction(true);
				return;
			}

			if ( !navAgent.SetDestination( TargetPosition.value) )
				EndAction(false);
		}

		protected override void OnUpdate(){

			if (navAgent.destination != TargetPosition.value){
				if ( !navAgent.SetDestination( TargetPosition.value) ){
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