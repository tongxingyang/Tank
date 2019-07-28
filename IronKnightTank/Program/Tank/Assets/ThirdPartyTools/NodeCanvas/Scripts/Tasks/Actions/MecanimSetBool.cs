using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Set Mecanim Bool")]
	[ScriptCategory("Mecanim")]
	[AgentType(typeof(Animator))]
	public class MecanimSetBool : ActionTask{

		[RequiredField]
		public string mecanimParameter;
		public BBBool setTo = new BBBool();

		[GetFromAgent]
		private Animator animator;

		protected override string actionInfo{
			get{return "Mec.SetBool '" + mecanimParameter + "' to " + setTo;}
		}

		protected override void OnExecute(){

			animator.SetBool(mecanimParameter, (bool)setTo.value);
			EndAction(true);
		}
	}
}