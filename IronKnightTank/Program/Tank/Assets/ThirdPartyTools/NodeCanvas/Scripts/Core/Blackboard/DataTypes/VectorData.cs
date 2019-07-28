using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class VectorData : Data{

		public Vector3 value;

		public override void SetValue(System.Object value){
			this.value = (Vector3)value;
		}

		public override System.Object GetValue(){
			return value;
		}

		public override System.Object GetSerialized(){

			return new float[] {value.x, value.y, value.z};
		}

		public override void SetSerialized(System.Object obj){

			var floatArr = obj as float[];
			value = new Vector3(floatArr[0], floatArr[1], floatArr[2]);
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		public override void ShowDataGUI(){
			value = UnityEditor.EditorGUILayout.Vector3Field("", value, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(18));
		}

		#endif
	}
}