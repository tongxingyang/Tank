using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class GetGameObjectPosition : ActionTask {

		[RequiredField]
		public BBGameObject targetGameObject;
		public BBVector saveAs = new BBVector{blackboardOnly = true};

		protected override string actionInfo{
			get {return "Get Position from " + targetGameObject + " as " + saveAs;}
		}

		protected override void OnExecute(){

			saveAs.value = targetGameObject.value.transform.position;
			EndAction();
		}
	}
}