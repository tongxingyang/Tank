using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptName("Check If In Front")]
	[ScriptCategory("Transform")]
	[AgentType(typeof(Transform))]
	public class CheckInFront : ConditionTask{

		[RequiredField]
		public BBGameObject CheckTarget;
		public float AngleToCheck = 70f;

		protected override string conditionInfo{
			get {return CheckTarget.ToString() + " in front";}
		}

		protected override bool OnCheck(){

			if (CheckTarget.value == null){
				Debug.LogError("CheckTarget value is null on CheckInFront");
				return false;
			}

			float angleNow = Vector3.Angle((CheckTarget.value as GameObject).transform.position - agent.transform.position, agent.transform.forward);
			
			return angleNow < AngleToCheck;
		}
	}
}