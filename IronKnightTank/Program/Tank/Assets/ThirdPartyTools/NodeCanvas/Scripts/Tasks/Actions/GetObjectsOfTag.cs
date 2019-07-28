using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Get Objects Of Tag")]
	[ScriptCategory("GameObject")]
	public class GetObjectsOfTag : ActionTask{

		[RequiredField] [TagField]
		public string searchTag = "Untagged";
		
		[RequiredField]
		public BBGameObjectList saveAs = new BBGameObjectList(){blackboardOnly = true};

		protected override string actionInfo{
			get{return "GetObjects '" + searchTag + "' as " + saveAs.ToString();}
		}

		protected override void OnExecute(){

			if (!blackboard){
				Debug.LogError("No blackboard for action 'GetObjectsOfTag'");
				EndAction(false);
				return;
			}

			saveAs.value = GameObject.FindGameObjectsWithTag(searchTag).ToList();
			EndAction(true);
		}
	}
}