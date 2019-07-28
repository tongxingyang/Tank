using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Look At")]
	[ScriptCategory("Transform")]
	[AgentType(typeof(Transform))]
	public class LookAt : ActionTask{

		[RequiredField]
		public BBGameObject lookTarget = new BBGameObject();
		public bool forever = false;

		protected override string actionInfo{
			get {return "LookAt " + lookTarget;}
		}

		protected override void OnExecute(){

			if (lookTarget.value == null){
				EndAction(false);
				return;
			}

			DoLook();
			
			if (!forever)
				EndAction(true);
		}

		protected override void OnUpdate(){

			DoLook();
		}

		void DoLook(){

			Vector3 lookPos = lookTarget.value.transform.position;
			lookPos.y = agent.transform.position.y;
			agent.transform.LookAt(lookPos);
		}
	}
}