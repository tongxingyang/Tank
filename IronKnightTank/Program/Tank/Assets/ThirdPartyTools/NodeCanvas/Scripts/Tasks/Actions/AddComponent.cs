using UnityEngine;
using System.Collections;

namespace NodeCanvas.Actions{

	[ScriptCategory("GameObject")]
	[AgentType(typeof(Transform))]
	public class AddComponent : ActionTask {

		[RequiredField]
		public string componentName;

		protected override string actionInfo{
			get {return "Add '" + componentName + "' Component";}
		}

		protected override void OnExecute(){

			if (agent.GetComponent(componentName) == null)
				//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(agent.gameObject, "Assets/NodeCanvas/Scripts/Tasks/Actions/AddComponent.cs (20,5)", componentName);

			EndAction();
		}
	}
}