#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptName("Check Mouse Input")]
	[ScriptCategory("Input")]
	public class CheckMouseInput : ConditionTask{

		public enum ButtonKeys{
			Left = 0,
			Right = 1,
			Middle = 2
		}
		
		public ButtonKeys buttonKey;
		[LayerField]
		public int layer;

		public BBVector savePosAs = new BBVector(){blackboardOnly = true};
		public BBGameObject saveGoAs = new BBGameObject(){blackboardOnly = true};

		private int buttonID;
		private RaycastHit hit;

		protected override string conditionInfo{
			get
			{
				string finalString= "Click ";
				if (!string.IsNullOrEmpty(savePosAs.dataName))
					finalString += "& SaveAs " + savePosAs;
				if (!string.IsNullOrEmpty(saveGoAs.dataName))
					finalString += "& SaveAs " + saveGoAs;
				return finalString;
			}
		}

		protected override bool OnCheck(){

			buttonID = (int)buttonKey;
			if (Input.GetMouseButtonDown(buttonID)){
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1<<layer)){

					savePosAs.value = hit.point;
					saveGoAs.value = hit.collider.gameObject;

					return true;
				}
			}
			return false;
		}
	}
}