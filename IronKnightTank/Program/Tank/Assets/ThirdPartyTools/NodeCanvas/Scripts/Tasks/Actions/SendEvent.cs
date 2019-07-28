using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class SendEvent : ActionTask {

		[RequiredField]
		public BBString eventName;
		public BBFloat delay;

		protected override string actionInfo{
			get{ return "Send [<b>" + eventName + "</b>]" + (delay.value > 0? " after " + delay + " sec." : "" );}
		}

		protected override void OnUpdate(){

			if (elapsedTime > delay.value){

				if (!string.IsNullOrEmpty(eventName.value))
					SendEvent(eventName.value);

				EndAction();
			}
		}
	}
}