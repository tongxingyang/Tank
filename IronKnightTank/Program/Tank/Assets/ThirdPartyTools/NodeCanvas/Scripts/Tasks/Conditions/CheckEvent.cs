using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	[EventListener("OnCustomEvent")]
	public class CheckEvent : ConditionTask {

		[RequiredField]
		public BBString eventName;

		private bool isReceived = false;

		protected override string conditionInfo{
			get {return "[<b>" + eventName + "</b>]"; }
		}

		protected override bool OnCheck(){

			if (isReceived){
				isReceived = false;
				return true;
			}

			return false;
		}

		void OnCustomEvent(string receivedEvent){

			if (receivedEvent == eventName.value)
				isReceived = true;
		}
	}
}