using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Set IK")]
	[ScriptCategory("Mecanim")]
	[AgentType(typeof(Animator))]
	public class MecanimIKActions : ActionTask{

		public AvatarIKGoal IKGoal;
		[RequiredField]
		public BBGameObject goal = new BBGameObject();
		public BBFloat weight = new BBFloat();

		[GetFromAgent]
		private Animator animator;

		protected override string actionInfo{
			get{return "Set '" + IKGoal + "' " + goal;}
		}

		protected override void OnExecute(){

			animator.SetIKPositionWeight(IKGoal, weight.value);
			animator.SetIKPosition(IKGoal, goal.value.transform.position);
			EndAction();
		}
	}
}