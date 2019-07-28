using UnityEngine;
using System.Collections;

namespace NodeCanvas.Actions{

	[ScriptName("Set Visibility")]
	[ScriptCategory("GameObject")]
	[AgentType(typeof(Transform))]
	public class SetObjectVisibility : ActionTask{

		public enum SetMode {Invisible, Visible, Toggle}
		public SetMode SetTo= SetMode.Toggle;

		protected override string actionInfo{
			get {return "Set Visibility To '" + SetTo + "'";}
		}

		protected override void OnExecute(){

			bool value;
			
			if (SetTo == SetMode.Toggle){
			
				value = !agent.gameObject.activeSelf;
			
			} else {

				value = (int)SetTo == 1;
			}

			agent.gameObject.SetActive(value);
			EndAction();
		}
	}
}