using UnityEngine;
using System.Collections;

namespace NodeCanvas.Conditions{

	[ScriptName("True Condition")]
	[ScriptCategory("Interop")]
	public class TrueCondition : ConditionTask{

		protected override string conditionInfo{
			get {return "True";}
		}

		protected override bool OnCheck(){
			return true;
		}
	}
}