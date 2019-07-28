﻿using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("GameObject")]
	public class GetGameObjectWithName : ActionTask {

		[RequiredField]
		public BBString gameObjectName;
		public BBGameObject saveAs = new BBGameObject(){blackboardOnly = true};

		protected override string actionInfo{
			get {return "Get Object " + gameObjectName + " as " + saveAs;}
		}

		protected override void OnExecute(){

			saveAs.value = GameObject.Find(gameObjectName.value);
			EndAction();
		}
	}
}