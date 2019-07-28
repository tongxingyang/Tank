#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptName("Check Distance")]
	[ScriptCategory("Transform")]
	[AgentType(typeof(Transform))]
	public class CheckDistance : ConditionTask{

		[RequiredField]
		public BBGameObject CheckTarget;
		public BBFloat distance;

		protected override string conditionInfo{
			get {return "Distance < " + distance.ToString() + " to " + CheckTarget.ToString();}
		}

		protected override bool OnCheck(){

			if (CheckTarget.value == null){
				Debug.LogError("CheckTarget value is null on CheckDistance Condition");
				return false;
			}

			if (Vector3.Distance(agent.transform.position, (CheckTarget.value as GameObject).transform.position) < distance.value)
				return true;

			return false;
		}
	}
}