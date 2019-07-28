using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class BoolData : Data{

		public bool value;

		override public void SetValue(System.Object value){
			this.value = (bool)value;
		}

		override public System.Object GetValue(){
			return value;
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		override public void ShowDataGUI(){
			value = UnityEditor.EditorGUILayout.Toggle(value, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));
		}

		#endif
	}
}