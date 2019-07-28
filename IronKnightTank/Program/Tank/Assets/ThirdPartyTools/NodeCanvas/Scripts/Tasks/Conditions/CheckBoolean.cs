using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptName("Check Boolean")]
	[ScriptCategory("Interop")]
	public class CheckBoolean : ConditionTask{

		public BBBool valueA = new BBBool();
		public BBBool valueB = new BBBool();

		protected override string conditionInfo{
			get {return valueA.ToString() + " == " + valueB.ToString();}
		}

		protected override bool OnCheck(){

			if (blackboard == null){
				Debug.LogError("No Blackboard Passed to CheckBoolean Condition", gameObject);
				return false;
			}

			return valueA.value == valueB.value;
		}
	}
}