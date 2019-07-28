﻿using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Input")]
	public class GetMousePosition : ActionTask {

		public BBVector saveAs = new BBVector{blackboardOnly = true};
		public bool forever;


		protected override void OnExecute(){

			Do();
		}

		protected override void OnUpdate(){

			Do();
		}

		void Do(){

			saveAs.value = Input.mousePosition;
			if (!forever)
				EndAction();
		}
	}
}