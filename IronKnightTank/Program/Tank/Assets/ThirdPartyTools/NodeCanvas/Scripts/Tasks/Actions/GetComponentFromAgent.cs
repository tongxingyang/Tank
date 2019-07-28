using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Get Component")]
	[ScriptCategory("GameObject")]
	[AgentType(typeof(Transform))]
	public class GetComponentFromAgent : ActionTask{

		[RequiredField]
		public string TypeToGet = "Transform";

		[RequiredField]
		public BBComponent saveAs = new BBComponent(){blackboardOnly = true};

		protected override string actionInfo{
			get{return "Get '" + TypeToGet + "' as " + saveAs;}
		}

		protected override void OnExecute(){

			if (!blackboard){
				EndAction(false);
				return;
			}

			var foundCompo = agent.GetComponent(TypeToGet);

			saveAs.value = foundCompo;

			if (foundCompo != null){

				EndAction(true);

			} else {

				Debug.LogWarning("No Component named '" + TypeToGet + "' found on agent. Does a Type with such name exist?");
				EndAction(false);
			}
		}
	}
}