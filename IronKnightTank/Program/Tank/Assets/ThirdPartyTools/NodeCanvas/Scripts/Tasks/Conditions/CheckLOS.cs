using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptName("Check Line Of Sight")]
	[ScriptCategory("Transform")]
	[AgentType(typeof(Transform))]
	public class CheckLOS : ConditionTask{

		[RequiredField]
		public BBGameObject LosTarget;
		public Vector3 Offset;

		protected override string conditionInfo{
			get {return "LOS with " + LosTarget.ToString();}
		}

		protected override bool OnCheck(){

			if (LosTarget.value == null){
				Debug.LogError("LOS Target is not set correctly on CheckLOS Condition", gameObject);
				return false;
			}

			Transform t = (LosTarget.value as GameObject).transform;

			RaycastHit hit = new RaycastHit();
			if (Physics.Linecast(agent.transform.position + Offset, t.position + Offset, out hit)){
				if (hit.collider != t.GetComponent<Collider>())
					return false;
			}

			return true;
		}
	}
}