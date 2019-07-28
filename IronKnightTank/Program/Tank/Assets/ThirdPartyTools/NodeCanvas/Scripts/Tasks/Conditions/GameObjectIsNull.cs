using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	public class GameObjectIsNull : ConditionTask {

		public BBGameObject gameObjectVar = new BBGameObject{blackboardOnly = true};

		protected override string conditionInfo{
			get {return gameObjectVar + " == null";}
		}

		protected override bool OnCheck(){
			return gameObjectVar.value == null;
		}
	}
}