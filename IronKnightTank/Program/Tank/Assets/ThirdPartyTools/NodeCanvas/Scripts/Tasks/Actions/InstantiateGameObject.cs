using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("GameObject")]
	public class InstantiateGameObject : ActionTask {

		[RequiredField]
		public BBGameObject original = new BBGameObject();
		public BBGameObject saveCloneAs = new BBGameObject(){blackboardOnly = true};
		public BBVector clonePosition = new BBVector();

		protected override string actionInfo{
			get {return "Instantiate " + original + " at " + clonePosition + " as " + saveCloneAs;}
		}

		protected override void OnExecute(){

			saveCloneAs.value = (GameObject)Instantiate(original.value, clonePosition.value, Quaternion.identity);
			EndAction();
		}
	}
}