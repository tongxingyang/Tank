using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Set Float")]
	[ScriptCategory("Interop")]
	public class SetFloatData : ActionTask{

		public enum SetMode
		{
			SET,
			ADD,
			SUBTRACT,
		}
		public BBFloat valueA = new BBFloat();
		public SetMode Operation = SetMode.SET;
		public BBFloat valueB = new BBFloat();

		protected override string actionInfo{
			get
			{
				if (Operation == SetMode.SET)
					return "Set " + valueA.ToString() + " = " + valueB.ToString();

				if (Operation == SetMode.ADD)
					return "Set " + valueA.ToString() + " += " + valueB.ToString();
				
				if (Operation == SetMode.SUBTRACT)
					return "Set " + valueA.ToString() + " -= " + valueB.ToString();

				return string.Empty;			
			}
		}

		protected override void OnExecute(){

			if (blackboard == null){
				Debug.LogError("No Blackboard Passed to SetFloat Action", gameObject);
				EndAction(false);
				return;
			}

			if (Operation == SetMode.SET){
				valueA.value = valueB.value;
			} else
			if (Operation == SetMode.ADD){
				valueA.value += valueB.value;
			} else
			if (Operation == SetMode.SUBTRACT){
				valueA.value -= valueB.value;
			}

			EndAction(true);
		}
	}
}